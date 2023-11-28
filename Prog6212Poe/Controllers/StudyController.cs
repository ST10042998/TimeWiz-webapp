using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTimeWizClassLib;
using NuGet.Protocol;
using Prog6212Poe.HelperClass;
using Prog6212Poe.ModelHelper;
using Prog6212Poe.Models;

namespace Prog6212Poe.Controllers
{
    public class StudyController : Controller
    {
        //initialize variables
        private StudyHelper studyHelper;
        private Semesters semester; 
        private ModuleTables module;
        private ModuleViewModel moduleViewModel;
        private CalculationClass cal = new CalculationClass();
        private StudDy days;

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// dictionary property to store the recorded hours for this module
        /// </summary>
        public Dictionary<DateTime, int> StudiedHoursPerDate { get; } = new Dictionary<DateTime, int>();

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// constructor for study controller
        /// </summary>
        /// <param name="db"></param>
        public StudyController(TimeWizContext db)
        {
            studyHelper = new StudyHelper(db);
            semester = new Semesters(db);
            moduleViewModel = new ModuleViewModel();
            module = new ModuleTables(db);
            days = new StudDy(db);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method to display the study view and add semester data to the dropdown list
        /// </summary>
        /// <returns></returns>
        public IActionResult StudyView()
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

            return View("StudyView");
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method that uses ajax to populate the semester table with the selected semester data
        /// </summary>
        /// <param name="selectedSemesterId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DropDownSemester(int selectedSemesterId)
        {
            var semesterData = SemTables(selectedSemesterId);
            return semesterData;

        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method that uses ajax to populate the module table with the selected semester data
        /// </summary>
        /// <param name="selectedSemesterId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Table(int selectedSemesterId)
        {
            var moduleData = ModTables(selectedSemesterId);
            return moduleData;

        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method that uses the selected semester id to get the semester data and returns the data as json
        /// </summary>
        /// <param name="selectedID"></param>
        /// <returns></returns>
        private JsonResult SemTables(int selectedID)
        {
            // Get the data for the selected semester
            var semesterData = semester.GetSemester(selectedID);
          

            // Return data as JSON
            return Json(new { semesterData });
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method that uses the selected semester id to get the module data and returns the data as json
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

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets the ModuleId from the selected module and stores it in a session variable
        /// </summary>
        /// <param name="selectedItemId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult IdModule(int selectedItemId)
        {
            var moduleData = studyHelper.FindModuleByCode(selectedItemId);
            HttpContext.Session.SetInt32("ModuleId", moduleData.ModuleId);
            // Return data as JSON
            return Json(new { moduleData });
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Method that saves the amount of hours studied for the selected module
        /// </summary>
        /// <param name="moduleViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveStudy(ModuleViewModel moduleViewModel)
        {
           int  selectedModuleId = HttpContext.Session.GetInt32("ModuleId").Value;
            try
            {
              
                int StudiedHours;


                if (selectedModuleId == null)
                {
                   ViewBag.Error = "Please select a module";
                   
                }

                var moduleData = studyHelper.FindModuleByCode(selectedModuleId);

                if (char.IsDigit(Convert.ToChar(moduleViewModel.Studiedhrs)))
                {
                    ViewBag.Error = "Please enter the amount of hours studied and make sure its a valid number";
                }
                
                try
                {
                    DateTime currentDate = DateTime.Now.Date;
                    var hours = 0;


                    hours += moduleViewModel.Studiedhrs;

                    // theres only 24 hours in a day so you cant add more hours than that 
                    if (hours <= 24)
                    {

                        if (moduleData.StudiedHours != null)
                        {
                             StudiedHours = moduleData.StudiedHours.Value;
                        }
                        else
                        {
                            StudiedHours = 0;
                        }

                        StudiedHours += moduleViewModel.Studiedhrs;



                        if (StudiedHours <= moduleData.SelfStudyHours)

                        {
                            moduleData.StudiedHours = StudiedHours;

                            StudiedHoursPerDate.Add(currentDate, moduleData.StudiedHours.Value);
                            // Calculate ProgressBarPercentage 
                            double Progressbar = cal.ProgressBarCal(StudiedHoursPerDate, moduleData.SelfStudyHours.Value);

                            int RemainingWeekHours = this.cal.CalculateRemainingHoursForCurrentWeek(StudiedHoursPerDate, moduleData.SelfStudyHours.Value);

                            // Save changes to the database
                            module.UpdateStudyModule(moduleData.ModuleId, RemainingWeekHours, Convert.ToInt32(Progressbar), DateTime.Now, moduleData.StudiedHours.Value);

                            ViewBag.Message($"Studied {StudiedHours} hours for {moduleData.Name} on {currentDate.ToShortDateString()}");
                           
                        }
                        else
                        {
                            StudiedHours -= moduleViewModel.Studiedhrs;
                            moduleData.StudiedHours  -= moduleViewModel.Studiedhrs;
                            ViewBag.Message($"The amount of self study hours is more than assigned \n All self-study hours for the week has been completed if {moduleViewModel.Module.StudiedHours}: {moduleViewModel.Module.RemainingWeekHours}");
                           

                        }
                    }


                    else
                    {
                        hours -= moduleViewModel.Studiedhrs;

                        ViewBag.Message($"The amount of self study hours is more than assigned \n All self-study hours for the week has been completed if (0): {moduleData.RemainingWeekHours}");
                        
                    }


                }
                catch
                {
                    throw;
                }
            }
            catch (Exception ex)
            {

               
            }

            return RedirectToAction("StudyView");

        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method that displays the study days view and adds the days of the week to the dropdown list
        /// </summary>
        /// <returns></returns>
       public IActionResult StudyDaysView()
        {
        
                // Populate the StudyDayList with days of the week
                ViewBag.StudyDayList = GetDaysOfWeek();

            var semesterData = studyHelper.SemesterData();

            var selectListItems = semesterData
                .Select(s => new SelectListItem
                {
                    Value = s.SemesterId.ToString(),
                    Text = $"{s.SemesterNum}"
                })
                .ToList();

            ViewBag.SemesterList = new SelectList(selectListItems, "Value", "Text");

          

            return View("StudyDaysView");
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method to populate the study days dropdown list with
        /// </summary>
        /// <returns></returns>
            private IEnumerable<SelectListItem> GetDaysOfWeek()
            {
                var daysOfWeek = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Monday", Value = "Monday" },
                    new SelectListItem { Text = "Tuesday", Value = "Tuesday" },
                    new SelectListItem { Text = "Wednesday", Value = "Wednesday" },
                    new SelectListItem { Text = "Thursday", Value = "Thursday" },
                    new SelectListItem { Text = "Friday", Value = "Friday" },
                    new SelectListItem { Text = "Saturday", Value = "Saturday" }
                };

                return daysOfWeek;
            }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Method to save the selected day for the specified module
        /// </summary>
        /// <param name="selectedDay"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveDays(string selectedDay)
        {
            int selectedModuleId = HttpContext.Session.GetInt32("ModuleId").Value;

            try
            {
                // Save the selected day for the specified module
                var studyDay = days.AddStudyDays(selectedModuleId, selectedDay);
              
                return View("StudyDaysView");
            }
            catch (Exception ex)
            {
                return Json(new { error = "Error saving study day" });
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        /// <summary>
        /// get table data for the study days table
        /// </summary>
        /// <param name="selectedSemesterId"></param>
        /// <returns></returns>
        public IActionResult GetModuleName(int selectedItemId)
        {
           var studyDaysData = this.StudyTables(selectedItemId);
            return studyDaysData;   

        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// returns the study days data as json
        /// </summary>
        /// <param name="selectedItemId"></param>
        /// <returns></returns>
        private JsonResult StudyTables(int selectedItemId)
        {
            var studyDaysData = days.GetStudyDays(selectedItemId);

            // Return data as JSON
            return Json(new { studyDaysData });
        }

    }
}
//--------------------------------------------------------------------------------------------------------------------------------------------------------------*Eugene*End..

