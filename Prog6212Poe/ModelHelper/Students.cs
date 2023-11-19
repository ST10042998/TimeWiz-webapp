using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Prog6212Poe.Models;

public class Students
{
    private readonly TimeWizContext _context;

    public Students(TimeWizContext context)
    {
        _context = context;
    }

   

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
            // Handle exceptions
        }
    }


}

//----------------------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*end
