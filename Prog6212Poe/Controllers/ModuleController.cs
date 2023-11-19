using Microsoft.AspNetCore.Mvc;
using Prog6212Poe.ModelHelper;
using Prog6212Poe.Models;
using MyTimeWizClassLib;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Prog6212Poe.Controllers
{
    public class ModuleController : Controller
    {
        private readonly TimeWizContext _context;
        private Semesters semester;
        private ModuleTables module = new ModuleTables();
        private CalculationClass cal = new CalculationClass();
        private int numweeks { get; set; }

        private List<ModuleViewModel> moduleData;
        public ModuleController(TimeWizContext context)
        {
            _context = context;
           semester = new Semesters(context);
            moduleData = new List<ModuleViewModel>();
        }

        public IActionResult ModuleView()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveSemester(ModuleViewModel sem)
        {
            if (sem.Semester.SemesterNum != 0)
            {
                var EndDate = cal.CalculateEndOfSemester(Convert.ToDateTime(sem.Semester.StartDate), sem.Semester.NumOfWeeks);
                var studId = 1;
              var semest = semester.AddSemester(sem.Semester.SemesterNum, sem.Semester.NumOfWeeks, Convert.ToDateTime(sem.Semester.StartDate), EndDate, studId);
                numweeks = sem.Semester.NumOfWeeks;
                TempData["NumWeeks"] = numweeks;
                TempData["SemesterId"] = semest.SemesterId;
                moduleData.Add(sem);
                ViewBag.Message = "Semester  added";
               
            }

            
                ViewBag.Message = "Semester not added";
            return View("ModuleView");

        }

        // Action method for handling module data submission
        [HttpPost]
        public IActionResult SaveModule(ModuleViewModel viewModel)
        {
            int selfStudyHours = 0;
            if (viewModel.Module.Credits != 0)
            {
                moduleData.Add(viewModel);

                if (TempData.TryGetValue("NumWeeks", out object tempNumWeeks))
                {
                    int numberOfWeeks = (int)tempNumWeeks;
                    int classHoursPerWeek = viewModel.Module.ClassHoursPerWeek.Value;
                    int credit = viewModel.Module.Credits.Value;
                    string code = viewModel.Module.Code;

                    // Calculate the SelfStudyHours for the current module
                    selfStudyHours = cal.CalculateSelfStudyHours(code, numberOfWeeks, classHoursPerWeek, credit);
                }
                TempData.TryGetValue("SemesterId", out object tempId);
                int id = (int)tempId;
               
                module.AddModule(viewModel.Module.Name, viewModel.Module.Code, viewModel.Module.Credits.Value, id, viewModel.Module.ClassHoursPerWeek.Value, selfStudyHours);
                ViewBag.Message = "Module added";
                //ViewBag.SaveSuccess = true;
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

