using System;
using System.Threading.Tasks;
using knawels.obverts.Notifiers;
using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.MessageBuilders;

namespace Nml.Refactor.Me.Notifiers
{
	public class SmsNotifier : Notifier<SmsNotifier, IStringMessageBuilder>
	{
		
		public SmsNotifier(IStringMessageBuilder messageBuilder, IOptions options) : base(messageBuilder, options)
		{
		}
		
		public async override Task Notify(NotificationMessage message)
		{
			await NotificationEngine<IStringMessageBuilder>.Build(nameof(SmsNotifier), _messageBuilder, _options, _logger, message);
		}
	}
}
