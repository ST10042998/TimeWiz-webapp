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

    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// getting student id using ADO
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public int GetLoginId(string username)
    {
        int id = 0;

        using (SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            connection.Open();

            string query = "SELECT Login_Id FROM Login WHERE UserName = @UserName";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@UserName", username);

                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        return id;
    }

}

//----------------------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*end
