using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using knawels.obverts.Notifiers;
using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.MessageBuilders;
using Newtonsoft.Json.Linq;

namespace Nml.Refactor.Me.Notifiers
{
	public class EmailNotifier : Notifier<EmailNotifier, IMailMessageBuilder>
	{
		public EmailNotifier(IMailMessageBuilder messageBuilder, IOptions options):base(messageBuilder, options)
		{
		}
		
		public async override Task Notify(NotificationMessage message)
		{
			await NotificationEngine<IMailMessageBuilder>.Build(nameof(EmailNotifier), _messageBuilder, _options, _logger, message);
		}
	}
}
