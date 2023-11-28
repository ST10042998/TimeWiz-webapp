using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Prog6212Poe.Models;

namespace Prog6212Poe.ModelHelper
{
    public class StudDy
    {
        // initialise the database
        private TimeWizContext db;
        private StudyDays day = new StudyDays();
        
        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="db"></param>
        public StudDy(TimeWizContext db)
        {
            this.db = db;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Mehod to add study days or update if exits
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public StudyDays AddStudyDays(int moduleId, string day)
        {
            try
            {
                var existingStudyDay = db.StudyDays.FirstOrDefault(s => s.Module_Id == moduleId);

                if (existingStudyDay != null)
                {
                    // Update existing record
                    existingStudyDay.Day = day;
                    db.SaveChanges();
                    return existingStudyDay;
                }
                else
                {
                    var newStudyDay = new StudyDays
                    {
                        Module_Id = moduleId,
                        Day = day
                    };

                    db.StudyDays.Add(newStudyDay);
                    db.SaveChanges();

                    return newStudyDay;
                }
            }
            catch (Exception e)
            {
                // Log or handle the specific exception type
                Console.Error.WriteLine($"Error in AddStudyDays: {e.Message}");
                return null;
            }
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Method to get the study days for the current day of the week
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public string GetStudyDaysForCurrentDay(int studentId)
        {
            try
            {
                // Get the current day of the week
                DayOfWeek currentDayOfWeek = DateTime.Now.DayOfWeek;

                var studyDay = db.StudyDays
                    .Join(db.ModuleTables,
                        s => s.Module_Id,
                        m => m.ModuleId,
                        (s, m) => new { s, m })
                    .Join(db.Semesters,
                        sm => sm.m.SemesterId,
                        se => se.SemesterId,
                        (sm, se) => new { sm, se })
                    .Where(Semesters => Semesters.se.StudentId == studentId &&
                                       Semesters.sm.s.Day == currentDayOfWeek.ToString())
                    .FirstOrDefault();

                if (studyDay != null)
                {
                    // Format the string with the desired information
                    return $"Module: {studyDay.sm.m.Name}";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                // Log the exception or handle it appropriately
                return null;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public List<StudyDays> GetStudyDays(int moduleId)
        {
            try
            {
                var studyDays = db.StudyDays
              .Join(
                  db.ModuleTables,
                  studyDay => studyDay.Module_Id,
                  module => module.ModuleId,
                  (studyDay, module) => new { StudyDays = studyDay, Module = module }
              )
              .Where(j => j.Module.ModuleId == moduleId)
              .Select(j => new StudyDays { Day = j.StudyDays.Day, Module = j.Module })
              .ToList();

                if (studyDays.Any())
                {
                    return studyDays;
                
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                // Log the exception or handle it appropriately
                Console.Error.WriteLine($"Error in GetStudyDays: {e.Message}");
                return null;
            }
        }


    }

}
//---------------------------------------------------------------------------------------------------------------------------------**Eugene*End..