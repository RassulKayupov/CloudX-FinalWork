
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.Pages.Basket;
using Microsoft.Extensions.Configuration;

namespace Microsoft.eShopWeb.Web.Services
{
    public class BusService : IBusService
    {
        private const string QueueName = "order-items";
        private readonly string _serviceBusConnection;

        public BusService(IConfiguration configuration)
        {
            _serviceBusConnection = configuration.GetValue<string>("ServiceBusConnection");
        }

        public async Task SendOrderInQueue(BasketViewModel basket)
        {
            IQueueClient queueClient = new QueueClient(_serviceBusConnection, QueueName);
            await queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(basket.Items))));
        }
    }
}
