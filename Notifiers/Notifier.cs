using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.Notifiers;
using System;
using System.Threading.Tasks;

namespace knawels.obverts.Notifiers
{
    public abstract class Notifier<T,U> : INotifier
    {
        protected readonly U _messageBuilder;
        protected readonly IOptions _options;
        protected readonly ILogger _logger;
        protected Notifier(U messageBuilder, IOptions options)
        {
            _messageBuilder = messageBuilder ?? throw new ArgumentNullException(nameof(messageBuilder));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = LogManager.For<T>();
        }
        public abstract Task Notify(NotificationMessage message);
    }
}
