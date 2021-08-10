using Newtonsoft.Json;

namespace DeliveryPartnerSampleApi.Models
{
    public class PumiProperty
    {
        [JsonProperty("km")]
        public string Khmer { get; set; }

        [JsonProperty("latin")]
        public string Latin { get; set; }
    }


}
