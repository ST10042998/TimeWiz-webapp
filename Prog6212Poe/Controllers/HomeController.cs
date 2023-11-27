using Microsoft.AspNetCore.Mvc;
using Prog6212Poe.ModelHelper;
using Prog6212Poe.Models;
using System.Diagnostics;

namespace Prog6212Poe.Controllers
{
    public class HomeController : Controller
    {
        //initialize variables
        private readonly ILogger<HomeController> _logger;
        private readonly TimeWizContext _context;
        private StudDy day;
        private LoginInfos loginInfo;
        private ModuleTables module;

        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor for the home controller
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public HomeController(ILogger<HomeController> logger, TimeWizContext context)
        {
            _logger = logger;
            _context = context;
            day = new StudDy(context);
            loginInfo = new LoginInfos(context);
            module = new ModuleTables(context);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------
       
        /// <summary>
        /// Method to return the index view and display the study planner messaeg
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var loginId = loginInfo.GetLastAdded();
            var studentId = module.GetStudentId(loginId);
        
            var studyDayInfo = day.GetStudyDaysForCurrentDay(studentId);

            if (studyDayInfo != null)
            {
                TempData["StudyDayInfo"] = studyDayInfo;
            }

            return View("Index");
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method that returns privacy view
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method that returns error view
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
//--------------------------------------------------------------------------------------------------------------------------------------------------------*Eugene*End..