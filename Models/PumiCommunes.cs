using Newtonsoft.Json;
using System.Collections.Generic;

namespace DeliveryPartnerSampleApi.Models
{
    public class PumiCommunes
    {
        [JsonProperty("communes")]
        public Dictionary<string, PumiItem> Communes { get; set; }
    }
}
