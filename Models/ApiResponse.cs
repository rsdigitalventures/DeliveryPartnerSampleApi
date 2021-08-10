namespace DeliveryPartnerSampleApi.Models
{
    public class ApiResponse
    {
        public string Message { get; set; } = "OK";
        public bool Success { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }
    }
}
