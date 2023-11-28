using Microsoft.AspNetCore.Components;
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
        //initialize variables
        private StudyHelper studyHelper;
        private Semesters semester;
        private ModuleTables module;
        private ModuleViewModel moduleViewModel;
        private CalculationClass cal = new CalculationClass();
       
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor for the DashboardController
        /// </summary>
        /// <param name="db"></param>
        public DashboardController(TimeWizContext db)
        {
            studyHelper = new StudyHelper(db);
            semester = new Semesters(db);
            moduleViewModel = new ModuleViewModel();
            module = new ModuleTables(db);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Method to return the DashboardView and populate semester dropdown
        /// </summary>
        /// <returns></returns>
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

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// method that updates the semestertable
        /// </summary>
        /// <param name="selectedSemesterId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DropDownSemester(int selectedSemesterId)
        {
          
            var semesterData = SemTables(selectedSemesterId);
            
           
            return semesterData;

        }
       
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
      
        /// <summary>
        /// method that updates the moduletable
        /// </summary>
        /// <param name="selectedSemesterId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Table(int selectedSemesterId)
        {
            var moduleData = ModTables(selectedSemesterId);
            return moduleData;

        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method that returns the semesterdata
        /// </summary>
        /// <param name="selectedID"></param>
        /// <returns></returns>
        private JsonResult SemTables(int selectedID)
        {
            // Get the data for the selected semester
            var semesterData = semester.GetSemester(selectedID);
            HttpContext.Session.SetInt32("SemesterID", semesterData.Single().SemesterId);
            // Return data as JSON
            return Json(new { semesterData });
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method that returns the moduledata
        /// </summary>
        /// <param name="selectedID"></param>
        /// <returns></returns>
        private JsonResult ModTables(int selectedID)
        {
            // Get the data for the selected semester
            var moduleData = semester.GetAllModules(selectedID);


            // Return data as JSON
            return Json(new { moduleData });
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
      
        [HttpPost]
        /// <summary>
        /// method that deletes the selected semester
        /// </summary>
        /// <returns></returns>
        public IActionResult Delete()
        {
           var id = HttpContext.Session.GetInt32("SemesterID");
            Semester deletedSemester = semester.DeleteSemesterEntity(id.Value);
            if (deletedSemester != null)
            {
                return RedirectToAction("DashboardView");
            }
            else
            {
                return RedirectToAction("DashboardView");
            }
   
        }
       
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------*Eugene*end.