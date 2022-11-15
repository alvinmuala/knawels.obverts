using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.MessageBuilders;
using Nml.Refactor.Me.Notifiers;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace knawels.obverts.Notifiers
{
    public static class NotificationEngine<U>
        where U : IMessageBuilder<object>
    {
        public static async Task Build(string notificationType, U messageBuilder, IOptions options, ILogger logger, NotificationMessage message)
        {
            switch (notificationType)
            {
                case nameof(SlackNotifier):
                    await SendViaHttpClientAsync(messageBuilder, options.Slack.WebhookUri, logger, message);
                    break;

                case nameof(TeamsNotifier):
                    await SendViaHttpClientAsync(messageBuilder, options.Teams.WebhookUri, logger, message);
                    break;

                case nameof(EmailNotifier):
                    var smtp = new SmtpClient(options.Email.SmtpServer);
                    smtp.Credentials = new NetworkCredential(options.Email.UserName, options.Email.Password);
                    var mailMessage = messageBuilder.CreateMessage(message);

                    try
                    {
                        await smtp.SendMailAsync((MailMessage)mailMessage);
                        LogSuccessMessage(logger);
                    }
                    catch (Exception e)
                    {
                        LogErrorMessage(logger, e);
                        throw;
                    }
                    break;

                case nameof(SmsNotifier):
                    var smsClient = new SmsApiClient(options.Sms.ApiUri, options.Sms.ApiKey);
                    var sms = messageBuilder.CreateMessage(message);

                    try
                    {
                        await smsClient.SendAsync(message.To, sms.ToString() ?? string.Empty);
                        LogSuccessMessage(logger);
                    }
                    catch (Exception e)
                    {
                        LogErrorMessage(logger, e);
                        throw;
                    }
                    break;

                default:
                    throw new ArgumentException($"Invalid notification type {nameof(notificationType)}");
            }

        }

        private static async Task SendViaHttpClientAsync(U messageBuilder, string webHookUri, ILogger logger, NotificationMessage message)
        {
            var serviceEndPoint = new Uri(webHookUri);
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, serviceEndPoint);
            request.Content = new StringContent(
                messageBuilder.CreateMessage(message).ToString(),
                Encoding.UTF8,
                "application/json");
            try
            {
                var response = await client.SendAsync(request);
                logger.LogTrace($"Message sent. {response.StatusCode} -> {response.Content}");
            }
            catch (AggregateException e)
            {
                foreach (var exception in e.Flatten().InnerExceptions)
                    logger.LogError(exception, $"Failed to send message. {exception.Message}");

                throw;
            }
        }

        private static void LogSuccessMessage(ILogger logger)
        {
            logger.LogTrace($"Message sent.");
        }

        private static void LogErrorMessage(ILogger logger, Exception e)
        {
            logger.LogError(e, $"Failed to send message. {e.Message}");
        }
    }
}
