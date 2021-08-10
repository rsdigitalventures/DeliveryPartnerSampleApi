namespace DeliveryPartnerSampleApi.Models
{
    public class CreateDeliveryRequest
    {
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string SenderPostcode { get; set; }
        public double SenderLatitude { get; set; }
        public double SenderLongitude { get; set; }
        public string SenderPhone { get; set; }

        public string RecipientName { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientPostcode { get; set; }
        public double RecipientLatitude { get; set; }
        public double RecipientLongitude { get; set; }

        public string RecipientPhone { get; set; }
    }
}
