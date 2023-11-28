using Microsoft.Data.SqlClient;
using Prog6212Poe.Models;

namespace Prog6212Poe.ModelHelper
{
    public class ModuleTables
    {
        //initializing variables
        private TimeWizContext db;

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*start

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="_context"></param>
        public ModuleTables(TimeWizContext _context)
        {
         db = _context;  
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*end

        /// <summary>
        /// add module to database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="credits"></param>
        /// <param name="semesterId"></param>
        /// <param name="classhours"></param>
        /// <param name="selfstudy"></param>
        /// <returns></returns>
        public ModuleTable AddModule(string name, string code, int credits, int semesterId, int classhours, int selfstudy)
        {

            var module = new ModuleTable
            {
                Name = name,
                Code = code,
                Credits = credits,
                SemesterId = semesterId,
                ClassHoursPerWeek = classhours,
                SelfStudyHours = selfstudy
            };
            try

            {
                
                    db.ModuleTables.Add(module);
                    db.SaveChanges();
                

                return module;
            }
            catch (Exception e)
            {

                return null;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// update module using entity framework
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="credits"></param>
        /// <param name="semester_id"></param>
        /// <param name="classHours"></param>
        /// <param name="selfHours"></param>
        /// <param name="remainingHours"></param>
        /// <param name="Progressbar"></param>
        /// <param name="date"></param>
        /// <param name="studiedHrs"></param>
        /// <returns></returns>
        public ModuleTable UpdateModule(int id, string name, string code, int credits, int semester_id, int classHours, int selfHours, int remainingHours, int Progressbar, DateTime date, int studiedHrs)
        {
         


                var module = db.ModuleTables.Where(m => m.ModuleId == id).SingleOrDefault();
                if (module != null)
                {
                    module.Name = name;
                    module.Code = code;
                    module.Credits = credits;
                    module.SemesterId = semester_id;
                    module.ClassHoursPerWeek = classHours;
                    module.SelfStudyHours = selfHours;
                    module.RemainingWeekHours = remainingHours;
                    module.ProgressBarPercentage = Progressbar;
                    module.StudyDate = date;
                    module.StudiedHours = studiedHrs;

                    db.SaveChanges();
                    return module;
                }
                else
                {
                    return null;
                }
            
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// updating module ,study hours part using entity framework
        /// </summary>
        /// <param name="id"></param>
        /// <param name="remainingHours"></param>
        /// <param name="Progressbar"></param>
        /// <param name="date"></param>
        /// <param name="studiedHrs"></param>
        /// <returns></returns>
        public ModuleTable UpdateStudyModule(int id, int remainingHours, int Progressbar, DateTime date, int studiedHrs)
        {
            


                var module = db.ModuleTables.Where(m => m.ModuleId == id).SingleOrDefault();
                if (module != null)
                {

                    module.RemainingWeekHours = remainingHours;
                    module.ProgressBarPercentage = Progressbar;
                    module.StudyDate = date;
                    module.StudiedHours = studiedHrs;

                    db.SaveChanges();
                    return module;
                }
                else
                {
                    return null;
                }
            
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get study hours 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetStudiedHours(int id)
        {
            using (db = new TimeWizContext())
            {
                var module = db.ModuleTables.Where(m => m.ModuleId == id).SingleOrDefault();
                if (module != null)
                {
                    id = module.StudiedHours.Value;
                    return id;
                }
                else
                {
                    return 0;
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get module by modulecode
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<ModuleTable> GetModuleByCode(string code)
        {
            using (db = new TimeWizContext())
            {
                var module = db.ModuleTables.Where(m => m.Code == code).ToList();
                if (module != null)
                {
                    return module;
                }
                else
                {
                    return null;
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get all modules from database that are in the selected semester
        /// </summary>
        /// <param name="semester"></param>
        /// <returns></returns>
        public List<ModuleTable> GetAllModules(int semester)
        {
           
                var modules = db.ModuleTables.Where(m => m.SemesterId == semester).ToList();
                return modules;
            
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// delete module by semester id using entity framework
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModuleTable> DeleteModuleBySemesterId(int id)
        {
            using (db = new TimeWizContext())
            {
                var module = db.ModuleTables.Where(m => m.SemesterId == id).ToList();
                if (module.Count > 0)
                {
                    db.ModuleTables.RemoveRange(module);
                    db.SaveChanges();
                    return module;
                }
                else
                {
                    return module;
                }
            }

        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get student id using login id
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public int GetStudentId(int login)
        {
            int id = 0;

           
                var student = db.Students
                    .Where(s => s.LoginId == login)
                    .FirstOrDefault();

                if (student != null)
                {
                    id = student.StudentId;
                }
            
            

            return id;
        }
    }
}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*end

  
