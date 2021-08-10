using Newtonsoft.Json;
using System.Collections.Generic;

namespace DeliveryPartnerSampleApi.Models
{
    public class PumiVillages
    {
        [JsonProperty("villages")]
        public Dictionary<string, PumiItem> Villages { get; set; }
    }
}
