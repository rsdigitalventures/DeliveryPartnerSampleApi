using DeliveryPartnerSampleApi.Data;
using DeliveryPartnerSampleApi.Models;
using DeliveryPartnerSampleApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryPartnerSampleApi.Controllers
{
    public class ApiController : ControllerBase
    {
        public ApiController(ApplicationDbContext applicationDbContext,
            IDeliveryService deliveryService,
            ILogger<ApiController> logger,
            IPumiService pumiService,
            IGeoJsonService geoJsonService)
        {
            ApplicationDbContext = applicationDbContext;
            DeliveryService = deliveryService;
            Logger = logger;
            PumiService = pumiService;
            GeoJsonService = geoJsonService;
        }

        public ApplicationDbContext ApplicationDbContext { get; }
        public IDeliveryService DeliveryService { get; }
        public ILogger<ApiController> Logger { get; }
        public IPumiService PumiService { get; }
        public IGeoJsonService GeoJsonService { get; }

        [HttpGet]
        [Route("/api/install")]
        [SwaggerOperation(summary: "Install",
        description: "Installs the database and adds sample data",
        Tags = new string[] { "Delivery" })]
        [SwaggerResponse(200, "Installed", typeof(ApiResponse))]
        [SwaggerResponse(500, "Could not install.", typeof(ApiResponse))]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Install()
        {
            try
            {
                await ApplicationDbContext.Database.EnsureDeletedAsync();
                await ApplicationDbContext.Database.EnsureCreatedAsync();

                for (int i = 1; i < 6; i++)
                {
                    var deliveryRequest = new CreateDeliveryRequest
                    {
                        RecipientAddress = $"#{i} Street 10{i}, Phnom Penh",
                        RecipientLatitude = 11.572478f,
                        RecipientLongitude = 104.923720f,
                        RecipientName = "Shaun",
                        RecipientPhone = "+855964918397",
                        RecipientPostcode = "120200",
                        SenderAddress = $"#{i} Street 10{i}, Phnom Penh",
                        SenderLatitude = 11.511500f,
                        SenderLongitude = 104.877795f,
                        SenderName = "Stuart",
                        SenderPhone = "+85585332462",
                        SenderPostcode = "120500"
                    };

                    await DeliveryService.CreateDelivery(deliveryRequest);
                     
                }

                return Ok(new ApiResponse { Success = true });
            }
            catch (Exception ex)
            {
                Logger.LogError("There was a problem installing", ex);
                return Ok(new ApiResponse { Success = false, Message = ex.Message });
            }
        }


        [HttpGet]
        [Route("/api/deliveries/{id}")]
        [SwaggerOperation(summary: "Get Delivery",
        description: "Gets the Delivery by ID.",
        Tags = new string[] { "Delivery" })]
        [SwaggerResponse(200, "Returns the Delivery", typeof(ApiResponse<Delivery>))]
        [SwaggerResponse(500, "Could not get delivery.", typeof(ApiResponse))]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetDelivery([FromRoute] long id)
        {
            try
            {
                return Ok(new ApiResponse<Delivery> { Data = await DeliveryService.GetDelivery(id), Success = true });
            }
            catch (Exception ex)
            {
                Logger.LogError("There was a problem getting the delivery", ex);
                return Ok(new ApiResponse<Delivery> { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("/api/deliveries/{id}/cancel")]
        [SwaggerOperation(summary: "Cancel Delivery",
        description: "Cancels the Delivery by ID.",
        Tags = new string[] { "Delivery" })]
        [SwaggerResponse(200, "The Delivery has been cancelled", typeof(ApiResponse))]
        [SwaggerResponse(500, "There was a problem cancelling the Delivery.", typeof(ApiResponse))]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> CancelDelivery([FromRoute] long id)
        {
            try
            {
                await DeliveryService.CancelDelivery(id);
                return Ok(new ApiResponse<Delivery> { Success = true });
            }
            catch (Exception ex)
            {
                Logger.LogError("There was a problem cancelling the delivery", ex);
                return Ok(new ApiResponse<Delivery> { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("/api/deliveries")]
        [SwaggerOperation(summary: "Create Delivery",
        description: "Creates a Delivery.",
        Tags = new string[] { "Delivery" })]
        [SwaggerResponse(200, "The Delivery has been created", typeof(ApiResponse<Delivery>))]
        [SwaggerResponse(500, "There was a problem creating the Delivery.", typeof(ApiResponse))]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> CreateDelivery([FromRoute] CreateDeliveryRequest createDeliveryRequest)
        {
            try
            {
                var delivery = await DeliveryService.CreateDelivery(createDeliveryRequest);
                return Ok(new ApiResponse<Delivery> { Success = true, Data = delivery });
            }
            catch (Exception ex)
            {
                Logger.LogError("There was a problem creating the delivery", ex);
                return Ok(new ApiResponse<Delivery> { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("/api/reverse")]
        [SwaggerOperation(summary: "Reverse to Address",
       description: "Reverses a latitude and longitude and returns the Address.",
       Tags = new string[] { "Delivery" })]
        [SwaggerResponse(200, "The reversed address", typeof(ApiResponse<Address>))]
        [SwaggerResponse(500, "There was a problem creating the Delivery.", typeof(ApiResponse))]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> ReverseToAddress([FromBody] ReverseRequest reverseRequest)
        {
            try
            {
                var province = await GeoJsonService.GetName(reverseRequest.Latitude, reverseRequest.Longitude, 1);
                var district = await GeoJsonService.GetName(reverseRequest.Latitude, reverseRequest.Longitude, 2);
                var commune = await GeoJsonService.GetName(reverseRequest.Latitude, reverseRequest.Longitude, 3);

                var allDistricts = await PumiService.GetAllItems(PumiItemType.District);
                var selectedDistrict = allDistricts.FirstOrDefault(e => e.Value.Name.Latin == district);

                var address = new Address
                {
                    Province = province,
                    Commune = commune,
                    District = district,
                    Latitude = reverseRequest.Latitude,
                    Longitude = reverseRequest.Longitude,
                    Postcode = $"{selectedDistrict.Key}00"
                };

                return Ok(new ApiResponse<Address> { Success = true, Data = address });
            }
            catch (Exception ex)
            {
                Logger.LogError("There was a problem reversing the latitude and longitude", ex);
                return Ok(new ApiResponse<Delivery> { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("/api/check-coverage")]
        [SwaggerOperation(summary: "Check Coverage",
       description: "Checks if Coverage Area is in Cambodia.",
       Tags = new string[] { "Delivery" })]
        [SwaggerResponse(200, "Returns if the latitude/longitude is in the coverage area or not.", typeof(ApiResponse<bool>))]
        [SwaggerResponse(500, "There was a problem checking the coverage area.", typeof(ApiResponse))]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> CheckCoverage([FromBody] ReverseRequest reverseRequest)
        {
            try
            {
                var isInCoverage = await GeoJsonService.CheckCoverage(reverseRequest.Latitude, reverseRequest.Longitude);
                return Ok(new ApiResponse<bool> { Success = true, Data = isInCoverage });
            }
            catch (Exception ex)
            {
                Logger.LogError("There was a problem reversing the latitude and longitude", ex);
                return Ok(new ApiResponse<Delivery> { Success = false, Message = ex.Message });
            }
        }
    }
}
