using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Remibit.Models.SupportObj;

namespace Remibit.Utility.RabitMQ
{
    public class ConsumeRabbitMQHostedService : BackgroundService
    {
        SubscriberIhub _subscriber;
        IHubContext<PriceHub, IPriceHub> _pricehub;
        public ConsumeRabbitMQHostedService(IHubContext<PriceHub, IPriceHub> pricehub)
        {
            _pricehub = pricehub;
            _subscriber = new SubscriberIhub(pricehub);
            _subscriber.ExchangeName = "getData";
            _subscriber.QueueName = "usdtbtc";
            _subscriber.getStartedUse();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            InitialRabbitMQRun();
            return Task.CompletedTask;
        }

        private void InitialRabbitMQRun()
        {
            _subscriber.SubcribeAChannel();
        }

        public override void Dispose()
        {
            _subscriber.CloseChannel();
            base.Dispose();
        }
    }
}
