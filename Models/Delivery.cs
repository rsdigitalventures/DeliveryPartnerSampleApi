using System;

namespace DeliveryPartnerSampleApi.Models
{
    public class Delivery : CreateDeliveryRequest
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? StatusUpdated { get; set; }
        public DeliveryStatus Status { get; set; }
    }
}
