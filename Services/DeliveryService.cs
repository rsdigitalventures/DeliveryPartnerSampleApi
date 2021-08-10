using DeliveryPartnerSampleApi.Data;
using DeliveryPartnerSampleApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DeliveryPartnerSampleApi.Services
{

    public interface IDeliveryService
    {
        Task<Delivery> CreateDelivery(CreateDeliveryRequest createDeliveryRequest);
        Task CancelDelivery(long id);
        Task<Delivery> GetDelivery(long id);
    }

    public class DeliveryService : IDeliveryService
    {
        public DeliveryService(ApplicationDbContext databaseContext, ILogger<DeliveryService> logger)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
        }

        public ApplicationDbContext DatabaseContext { get; }
        public ILogger<DeliveryService> Logger { get; }

        public async Task CancelDelivery(long id)
        {
            var delivery = await GetDelivery(id);
            delivery.Status = DeliveryStatus.Cancelled;
            delivery.StatusUpdated = DateTime.UtcNow;
            DatabaseContext.Update(delivery);
            await DatabaseContext.SaveChangesAsync();
        }

        public async Task<Delivery> CreateDelivery(CreateDeliveryRequest createDeliveryRequest)
        {
            var delivery = new Delivery
            {
                Created = DateTime.UtcNow,
                RecipientAddress = createDeliveryRequest.RecipientAddress,
                RecipientLatitude = createDeliveryRequest.RecipientLatitude,
                Status = DeliveryStatus.Assigning,
                RecipientLongitude = createDeliveryRequest.RecipientLongitude,
                RecipientName = createDeliveryRequest.RecipientName,
                RecipientPhone = createDeliveryRequest.RecipientPhone,
                RecipientPostcode = createDeliveryRequest.RecipientPostcode,
                SenderAddress = createDeliveryRequest.SenderAddress,
                SenderLatitude = createDeliveryRequest.SenderLatitude,
                SenderLongitude = createDeliveryRequest.SenderLongitude,
                SenderName = createDeliveryRequest.SenderName,
                SenderPhone = createDeliveryRequest.SenderPhone,
                SenderPostcode = createDeliveryRequest.SenderPostcode
            };

            DatabaseContext.Add(delivery);
            await DatabaseContext.SaveChangesAsync();
            return delivery;
        }

        public async Task<Delivery> GetDelivery(long id)
        {
            return await DatabaseContext.Deliveries
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
