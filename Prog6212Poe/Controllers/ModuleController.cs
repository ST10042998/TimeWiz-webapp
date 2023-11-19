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
        private int numweeks;
        private List<ModuleViewModel> moduleData = new List<ModuleViewModel>();
        public ModuleController(TimeWizContext context)
        {
            _context = context;
           semester = new Semesters(context);
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
                semester.AddSemester(sem.Semester.SemesterNum, sem.Semester.NumOfWeeks, Convert.ToDateTime(sem.Semester.StartDate), EndDate, studId);
                numweeks = sem.Semester.NumOfWeeks;
                moduleData.Add(sem);
                ViewBag.Message = "Semester  added";
                return View("ModuleView");
            }

            // Redirect to the form or another action
            else
            {
                ViewBag.Message = "Semester not added";
                return View("ModuleView");
            }
        }

        // Action method for handling module data submission
        [HttpPost]
        public IActionResult SaveModule(ModuleViewModel viewModel)
        {
            int selfStudyHours = 0;
            if (viewModel.Module.Credits != 0)
            {
                moduleData.Add(viewModel);

                foreach (var moduleData in this.moduleData)
                {
                    int numberOfWeeks = numweeks;
                    int classHoursPerWeek = moduleData.Module.ClassHoursPerWeek.Value;
                    int credit = moduleData.Module.Credits.Value;
                    string code = moduleData.Module.Code;

                    // Calculate the SelfStudyHours for the current module
                     selfStudyHours = cal.CalculateSelfStudyHours(code, numberOfWeeks, classHoursPerWeek, credit);
                
                }
                var studId = 1;
                module.AddModule(viewModel.Module.Name, viewModel.Module.Code, viewModel.Module.Credits.Value ,studId , viewModel.Module.ClassHoursPerWeek.Value, selfStudyHours);
               ViewBag.Message = "Module added";
                return View("ModuleView");
                // Redirect to the form or another action
                }
            ViewBag.Message = "Module not added";
                // If the model state is not valid, return to the form with validation errors
                return View("ModuleView", viewModel);
            }
        }
    }

