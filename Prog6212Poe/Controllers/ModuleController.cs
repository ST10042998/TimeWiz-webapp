using Microsoft.AspNetCore.Mvc;
using Prog6212Poe.ModelHelper;
using Prog6212Poe.Models;
using MyTimeWizClassLib;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Prog6212Poe.Controllers
{
    public class ModuleController : Controller
    {
        //initializing variables
        private readonly TimeWizContext _context;
        private Semesters semester;
        private ModuleTables module;
        private CalculationClass cal = new CalculationClass();
        private List<int> numweeks = new List<int>();
        private List<int> semId = new List<int>();
        private LoginInfos loginInfo;

        //-------------------------------------------------------------------------------------------------------------------------------------------------

        //creating a list of module view model
        private List<ModuleViewModel> moduleData;

        //-------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// constructor for the module controller
        /// </summary>
        /// <param name="context"></param>
        public ModuleController(TimeWizContext context)
        {
           _context = context;
           semester = new Semesters(context);
           moduleData = new List<ModuleViewModel>();
           module = new ModuleTables(context);
           loginInfo = new LoginInfos(context);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// displaying the module view
        /// </summary>
        /// <returns></returns>
        public IActionResult ModuleView()
        {
            return View();
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// saving semester data
        /// </summary>
        /// <param name="sem"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveSemester(ModuleViewModel sem)
        {
            numweeks.Clear();
            semId.Clear();
            if (sem.Semester.SemesterNum != 0)
            {
                var loginId = loginInfo.GetLastAdded();
                var studentId = module.GetStudentId(loginId);

                var EndDate = cal.CalculateEndOfSemester(Convert.ToDateTime(sem.Semester.StartDate), sem.Semester.NumOfWeeks);
             
              var semest = semester.AddSemester(sem.Semester.SemesterNum, sem.Semester.NumOfWeeks, Convert.ToDateTime(sem.Semester.StartDate), EndDate, studentId);
            
                TempData["NumWeeks"] = semest.NumOfWeeks;
                TempData["SemesterId"] = semest.SemesterId;
                HttpContext.Session.SetInt32("SemesterId", semest.SemesterId);
                HttpContext.Session.SetInt32("NumOfWeeks", semest.NumOfWeeks);

                moduleData.Add(sem);
                ViewBag.Message = "Semester  added";
               
            }

            else 
            {
                ViewBag.Message = "Semester not added";
            }
         
            return View("ModuleView");

        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------
       
        /// <summary>
        /// saving module data
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveModule(ModuleViewModel viewModel)
        {
            int selfStudyHours = 0;
            if (viewModel.Module.Credits != 0)
            {
                moduleData.Add(viewModel);
                int? semesterId = HttpContext.Session.GetInt32("SemesterId");
                int? numOfWeeks = HttpContext.Session.GetInt32("NumOfWeeks");

                foreach (var moduleData in moduleData)
                {
                    int numberOfWeeks = numOfWeeks.Value;
                    int classHoursPerWeek = moduleData.Module.ClassHoursPerWeek.Value;
                    int credit = moduleData.Module.Credits.Value;
                    string code = moduleData.Module.Code;

                    // Calculate the SelfStudyHours for the current module
                    int StudyHours = cal.CalculateSelfStudyHours(code, numberOfWeeks, classHoursPerWeek, credit);

                    // Update the SelfStudyHours property of the current module
                    if (StudyHours != 0)
                    {
                       selfStudyHours = StudyHours;
                    }
                    else
                    {
                        ViewBag.Message = "Class Hours cant be more than number of weeks";
                                       
                    }
                }

                TempData.TryGetValue("SemesterId", out object tempId);
               
                int Id = semesterId.Value;
               
                var  newmodule = module.AddModule(viewModel.Module.Name, viewModel.Module.Code, viewModel.Module.Credits.Value, Id, viewModel.Module.ClassHoursPerWeek.Value, selfStudyHours);
                ViewBag.Message = "Module added";
              
                return View("ModuleView");
                // Redirect to the form or another action
            }
            else
            {
                ViewBag.SaveSuccess = false;
                ViewBag.Message = "Module not added";
                ViewBag.Error = "Please add correct values";
                return View("ModuleView");
            }
        }
    }
}

//-------------------------------------------------------------------------------------------------------------------------------------------------*Eugene*End..