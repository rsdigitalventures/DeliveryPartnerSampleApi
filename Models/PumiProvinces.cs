using Newtonsoft.Json;
using System.Collections.Generic;

namespace DeliveryPartnerSampleApi.Models
{
    public class PumiProvinces
    {
        [JsonProperty("provinces")]
        public Dictionary<string, PumiItem> Provinces { get; set; }
    }
}
