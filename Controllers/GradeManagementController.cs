using Microsoft.AspNetCore.Mvc;

namespace BookDB.Controllers
{
    public class GradeManagementController : Controller
    {
        private readonly ILogger<GradeManagementController> _logger;

        public GradeManagementController(ILogger<GradeManagementController> logger)
        {
            _logger = logger;
        }

        // GET: GradeManagement
        public IActionResult Index()
        {
            _logger.LogInformation("Accessed Grade Management Index");
            return View();
        }

        // GET: GradeManagement/Audit
        public IActionResult Audit()
        {
            _logger.LogInformation("Accessed Grade Management Audit");
            return View();
        }
    }
}
