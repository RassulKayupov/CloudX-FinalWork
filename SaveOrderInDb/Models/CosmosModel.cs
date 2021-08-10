using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaveOrderInDb.Models
{
    public class CosmosModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public Address Address { get; set; }

        public List<BasketItem> Items { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
