using Newtonsoft.Json;

namespace DeliveryPartnerSampleApi.Models
{
    public class Address
    {
        public string Province { get; set; }

        public string Commune { get; set; }

        public string District { get; set; }

        public string Postcode { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }

    public class ReverseRequest
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}
