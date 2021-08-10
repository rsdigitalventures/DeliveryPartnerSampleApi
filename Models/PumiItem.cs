using Newtonsoft.Json;

namespace DeliveryPartnerSampleApi.Models
{
    public class PumiItem
    {
        [JsonProperty("name")]
        public PumiProperty Name { get; set; }
        [JsonProperty("administrative_unit")]
        public PumiProperty AdministrativeUnit { get; set; }
    }


}
