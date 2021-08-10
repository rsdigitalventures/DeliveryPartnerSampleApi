using DeliveryPartnerSampleApi.Data;
using DeliveryPartnerSampleApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DeliveryPartnerSampleApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ApplicationDbContext DatabaseContext { get; }

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext databaseContext)
        {
            _logger = logger;
            DatabaseContext = databaseContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await DatabaseContext.Deliveries.AsNoTracking().ToListAsync());
        }

        public IActionResult ApiDocs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
