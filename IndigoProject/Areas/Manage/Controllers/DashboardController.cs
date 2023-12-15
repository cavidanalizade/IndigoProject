using Microsoft.AspNetCore.Mvc;

namespace IndigoProject.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
