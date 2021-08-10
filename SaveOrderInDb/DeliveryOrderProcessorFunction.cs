using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SaveOrderInDb.Models;

namespace SaveOrderInDb
{
    public class DeliveryOrderProcessorFunction
    {
        private string databaseId = "cosmosDatabase";
        private string containerId = "orderList";

        [FunctionName("DeliveryOrderProcessorFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("CosmoUrl"), Environment.GetEnvironmentVariable("CosmoKey"), new CosmosClientOptions() { ApplicationName = "CosmosDBDotnetQuickstart" });
            var databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(containerId, "/id");
            var json = await req.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<CosmosModel>(json);
            model.Id = Guid.NewGuid().ToString();
            await containerResponse.Container.CreateItemAsync(model);

            return new OkObjectResult("ok");
        }
    }
}
