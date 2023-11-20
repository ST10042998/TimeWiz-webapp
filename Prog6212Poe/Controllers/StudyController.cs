using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Prog6212Poe.HelperClass;
using Prog6212Poe.ModelHelper;
using Prog6212Poe.Models;

namespace Prog6212Poe.Controllers
{
    public class StudyController : Controller
    {
        private StudyHelper studyHelper;
        public StudyController(TimeWizContext db)
        {
            studyHelper = new StudyHelper(db);
        }
        public IActionResult StudyView()
        {
            return View("StudyView");
        }

        public IActionResult DropDownSemester()
        {
            
            return View();
        }


        public IActionResult SaveStudy()
        {
            return View();
        }

        public IActionResult DropDownModule()
        {
            return View();
        }

        public List<SelectListItem> GetSemester()
        {
            var semesterData = studyHelper.SemesterData();
            return semesterData;
        }

    }
}

