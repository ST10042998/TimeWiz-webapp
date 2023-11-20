using Prog6212Poe.ModelHelper;
using System.ComponentModel;
using Prog6212Poe.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Prog6212Poe.HelperClass
{
    public class StudyHelper
    {
        private Semesters semester;
        private ModuleTables module = new ModuleTables();
        int loginId = 1;
        public StudyHelper(TimeWizContext db)
        {
            semester = new Semesters(db);
            
        }
      

        public List<SelectListItem> SemesterData()
        {
            // Assuming loginId is a variable in your class
            var studentId = module.GetStudentId(loginId);

            // Sort the semesters in alphabetical order
            var sortedSemester = semester.GetAllSemesterEF(studentId)
                .OrderBy(s => s.SemesterNum)
                .ToList();

            // Convert Semester objects to SelectListItem objects
            var selectListItems = sortedSemester.Select(s => new SelectListItem
            {
                Value = s.SemesterId.ToString(),
                Text = $"{s.SemesterNum} " // Adjust as per your Semester class properties
            }).ToList();

            return selectListItems;
        }






    }
}
