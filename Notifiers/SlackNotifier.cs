﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using knawels.obverts.Notifiers;
using Newtonsoft.Json.Linq;
using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.MessageBuilders;

namespace Nml.Refactor.Me.Notifiers
{
	public class SlackNotifier : Notifier<SlackNotifier, IWebhookMessageBuilder>
	{
		public SlackNotifier(IWebhookMessageBuilder messageBuilder, IOptions options) : base(messageBuilder, options)
		{
		}

		public async override Task Notify(NotificationMessage message)
		{
			await NotificationEngine<IWebhookMessageBuilder>.Build(nameof(EmailNotifier), _messageBuilder, _options, _logger, message);
		}
	}
}
