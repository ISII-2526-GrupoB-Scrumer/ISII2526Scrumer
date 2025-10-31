using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<MaintenanceController> _logger;


        public MaintenanceController(ApplicationDbContext context, ILogger<MaintenanceController> logger)
        {
            _context = context;
            _logger = logger;
        }


       
    }
}
