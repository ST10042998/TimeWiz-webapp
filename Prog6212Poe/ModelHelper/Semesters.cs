﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Prog6212Poe.Models;
using System.Reflection;

namespace Prog6212Poe.ModelHelper
{
    public class Semesters
    {
        //initializing variables
        private TimeWizContext db ;

        public Semester semeseter = new Semester();

        public DbSet<Semester> Sem { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Semesters(TimeWizContext _db)
        {
            db = _db;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get all semesters using entity framework
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Semester> GetAllSemesterEF(int id)
        {
            List<Semester> semesters = new List<Semester>();

            try
            {
                
                    // Query the database using Entity Framework
                    semesters = db.Semesters
                        .Where(s => s.StudentId == id)
                        .ToList();
                
            }
            catch (Exception e)
            {
             
            }

            return semesters;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// adding semester using entity framework
        /// </summary>
        /// <param name="semesterNum"></param>
        /// <param name="numOfWeeks"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="student_id"></param>
        /// <returns></returns>
        public Semester AddSemester(int semesterNum, int numOfWeeks, DateTime startDate, DateTime endDate, int student_id)
        {
            try
            { 

            var semester = new Semester
            {
                SemesterNum = semesterNum,
                NumOfWeeks = numOfWeeks,
                StartDate = startDate,
                EndDate = endDate,
                StudentId = student_id
            };

            
                
                    db.Semesters.Add(semester);
                    db.SaveChanges();
                return semester;
                               
            }
            catch (Exception e)
            {
                return null;
                
            }
        } 

    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// update semester using entity
    /// </summary>
    /// <param name="id"></param>
    /// <param name="semesterNum"></param>
    /// <param name="numOfWeeks"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public Semester UpdateSemester(int id, int semesterNum, int numOfWeeks, string startDate, string endDate)
        {
            using (db = new TimeWizContext())
            {
                var semester = db.Semesters.Where(s => s.SemesterId == id).SingleOrDefault();
                if (semester != null)
                {
                    semester.SemesterNum = semesterNum;
                    semester.NumOfWeeks = numOfWeeks;
                    semester.StartDate = Convert.ToDateTime(startDate);
                    semester.EndDate = Convert.ToDateTime(endDate);
                    db.SaveChanges();
                    return semester;
                }
                else
                {
                    return null;
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get semester using entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Semester> GetSemester(int id)
        {
            
                var semester = db.Semesters.Where(s => s.SemesterId == id).ToList();
            if (semester != null)
            {
                return semester;
            }
            else
            {
                return null;
            }
        
            
        }
        public List<ModuleTable> GetAllModules(int semesterId)
        {
            // Assuming there is a navigation property from Semester to Module in your data model
            var modules = db.Semesters
                .Where(s => s.SemesterId == semesterId)
                .SelectMany(s => s.ModuleTables) // Assuming ModuleTables is the navigation property to modules
                .ToList();

            return modules;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get all semesters using entity
        /// </summary>
        /// <returns></returns>
        public List<Semester> GetAllSemesters()
        {
            using (db = new TimeWizContext())
            {

                return db.Semesters.ToList();
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// delete semester using entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Semester DeleteSemesterEntity(int id)
        {
            Semester deletedSemester = null;

           
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Retrieve the semester to delete
                    deletedSemester = db.Semesters.Find(id);

                    if (deletedSemester != null)
                    {
                        // Delete modules that reference the semester
                        
                        var studyDaysToDelete = db.StudyDays.Join(db.ModuleTables,
                                s => s.Module_Id,
                                m => m.ModuleId,
                                (s, m) => new { s, m })
                            .Join(db.Semesters,
                                sm => sm.m.SemesterId,
                                se => se.SemesterId,
                                (sm, se) => new { sm, se })
                            .Where(st=> st.se.SemesterId == id);
                        db.StudyDays.RemoveRange(studyDaysToDelete.Select(s => s.sm.s));

                        var modulesToDelete = db.ModuleTables.Where(m => m.SemesterId == id);
                        db.ModuleTables.RemoveRange(modulesToDelete);

                        // Delete the semester
                        db.Semesters.Remove(deletedSemester);

                        // Commit the transaction if everything is successful
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        // Roll back the transaction if the semester was not found
                        transaction.Rollback();
                    }
                }
                catch (Exception e)
                {
                   
                    // Roll back the transaction in case of an exception
                    transaction.Rollback();
                }
            }

            return deletedSemester;
        }


    }
}
//---------------------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*end
