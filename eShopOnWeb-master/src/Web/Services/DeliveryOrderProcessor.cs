using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.Pages.Basket;
using Microsoft.Extensions.Configuration;

namespace Microsoft.eShopWeb.Web.Services
{
    public class DeliveryOrderProcessor : IDeliveryOrderProcessor
    {
        private readonly HttpClient _httpClient;
        private readonly string _funcUrl;

        public DeliveryOrderProcessor(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _funcUrl = configuration.GetValue<string>("DeliveryOrderProcessorUrl");
        }

        public async Task Process(BasketViewModel basketModel)
        {
            var cosmoModel = new
            {
                Address = new Address("123 Main St.", "Kent", "OH", "United States", "44240"),
                Items = basketModel.Items,
                FinalPrice = basketModel.Total()
            };
            var stringContent = new StringContent(
                JsonSerializer.Serialize(cosmoModel),
                Encoding.UTF8,
                "application/json");

            await _httpClient.PostAsync(_funcUrl, stringContent);
        }
    }
}
