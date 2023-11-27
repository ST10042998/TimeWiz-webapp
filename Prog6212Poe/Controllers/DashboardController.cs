using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTimeWizClassLib;
using NuGet.Protocol;
using Prog6212Poe.HelperClass;
using Prog6212Poe.ModelHelper;
using Prog6212Poe.Models;

namespace Prog6212Poe.Controllers
{
    public class DashboardController : Controller
    {
        private StudyHelper studyHelper;
        private Semesters semester;
        private ModuleTables module;
        private ModuleViewModel moduleViewModel;
        private CalculationClass cal = new CalculationClass();
       
        public DashboardController(TimeWizContext db)
        {
            studyHelper = new StudyHelper(db);
            semester = new Semesters(db);
            moduleViewModel = new ModuleViewModel();
            module = new ModuleTables(db);
        }

        public IActionResult DashboardView()
        {
            var semesterData = studyHelper.SemesterData();

            var selectListItems = semesterData
                .Select(s => new SelectListItem
                {
                    Value = s.SemesterId.ToString(),
                    Text = $"{s.SemesterNum}"
                })
                .ToList();

            ViewBag.SemesterList = new SelectList(selectListItems, "Value", "Text");
          
            return View("DashboardView");
        }


        [HttpPost]
        public IActionResult DropDownSemester(int selectedSemesterId)
        {
          
            var semesterData = SemTables(selectedSemesterId);
           
            return semesterData;

        }
        [HttpPost]
      

        [HttpPost]
        public IActionResult Table(int selectedSemesterId)
        {
            var moduleData = ModTables(selectedSemesterId);
            return moduleData;

        }

        private JsonResult SemTables(int selectedID)
        {
            // Get the data for the selected semester
            var semesterData = semester.GetSemester(selectedID);

            // Return data as JSON
            return Json(new { semesterData });
        }

        private JsonResult ModTables(int selectedID)
        {
            // Get the data for the selected semester
            var moduleData = semester.GetAllModules(selectedID);


            // Return data as JSON
            return Json(new { moduleData });
        }
    }
}
