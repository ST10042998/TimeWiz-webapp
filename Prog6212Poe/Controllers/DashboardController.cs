using Microsoft.AspNetCore.Mvc;

namespace Prog6212Poe.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult DashboardView()
        {
            return View();
        }
    }
}
