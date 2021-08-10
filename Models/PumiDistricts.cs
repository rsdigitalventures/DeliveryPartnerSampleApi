using Newtonsoft.Json;
using System.Collections.Generic;

namespace DeliveryPartnerSampleApi.Models
{
    public class PumiDistricts
    {
        [JsonProperty("districts")]
        public Dictionary<string, PumiItem> Districts { get; set; }
    }


}
