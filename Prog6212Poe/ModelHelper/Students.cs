using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Prog6212Poe.Models;

public class Students
{
    private readonly TimeWizContext _context;

    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*start

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="context"></param>
    public Students(TimeWizContext context)
    {
        _context = context;
    }
       
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*start
    
    /// <summary>
    /// add student using entity framework
    /// </summary>
    /// <param name="name"></param>
    /// <param name="surname"></param>
    /// <param name="email"></param>
    /// <param name="gender"></param>
    /// <param name="login_id"></param>
    public void AddStudentUsingEF(string name, string surname, string email, string gender, int login_id)
    {
        try
        {
            var student = new Student
            {
                Name = name,
                Surname = surname,
                Email = email,
                Gender = gender,
                LoginId = login_id
            };

            _context.Students.Add(student);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
         
        }
    }


}

//----------------------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*end
