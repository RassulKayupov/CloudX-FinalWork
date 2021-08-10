using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace OrderItemsReserver
{
    public class OrderItemsReserverFunction
    {
        private const string queueName = "order-items";
        public string containerName = "orders";
        private readonly HttpClient _httpClient;

        public OrderItemsReserverFunction(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [FunctionName("OrderItemsReserver")]
        public async Task Run([ServiceBusTrigger(queueName, Connection = "ServiceBusConnection")]string myQueueItem, ILogger log)
        {
            try
            {
                var container = new BlobContainerClient(Environment.GetEnvironmentVariable("blobConnectionString"), containerName);
                await container.CreateIfNotExistsAsync();
                var numberOfOrders = container.GetBlobs().Count();
                var blobName = $"order_{numberOfOrders}.json";
                var byteArray = Encoding.UTF8.GetBytes(myQueueItem);
                var stream = new MemoryStream(byteArray);
                await container.UploadBlobAsync(blobName, stream);
            }
            catch (Exception e)
            {
                var jsonData = JsonSerializer.Serialize(new
                {
                    Exception = e.Message,
                });

                await _httpClient.PostAsync(Environment.GetEnvironmentVariable("LogicAppEmailSenderUrl"),
                    new StringContent(jsonData, Encoding.UTF8, "application/json"));
            }

        }
    }
}
