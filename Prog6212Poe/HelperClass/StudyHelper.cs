﻿using Prog6212Poe.ModelHelper;
using System.ComponentModel;
using Prog6212Poe.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Prog6212Poe.HelperClass
{
    public class StudyHelper
    {
        //initialize the classes
        private Semesters semester;
        private ModuleTables module;
        private TimeWizContext db;
        LoginInfos loginInfo;
        
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        public StudyHelper(TimeWizContext db)
        {
            this.db = db;
            semester = new Semesters(db);
            module = new ModuleTables(db);
            loginInfo = new LoginInfos(db);

        }
      
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// sort and return the list of semerster
        /// </summary>
        /// <returns></returns>
        public List<Semester> SemesterData()
        {
           var loginId = loginInfo.GetLastAdded();
            var studentId = module.GetStudentId(loginId);

            // Sort the semesters in alphabetical order
            var sortedSemester = semester.GetAllSemesterEF(studentId)
                .OrderBy(s => s.SemesterNum)
                .ToList();


            return sortedSemester ;
        }



        //----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Finding module by using modulecode
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ModuleTable FindModuleByCode(int id)
        {
           
                var module = db.ModuleTables.Where(m => m.ModuleId == id).FirstOrDefault();
                return module;
            
        }

    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------------------------*Eugene*end.
