using Microsoft.Data.SqlClient;
using Prog6212Poe.Models;

public class Logins
{
    private readonly TimeWizContext _context;
    private readonly string connectionString = "Server=tcp:st10042998.database.windows.net,1433;Initial Catalog=TimeWiz;Persist Security Info=False;User ID=st10042998;Password=Yukio.187;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
  
public Logins(TimeWizContext context)
    {
        _context = context;
    }

   

    public void AddLoginUsingEF(string username, string password)
    {
        try
        {
            var login = new Login
            {
                UserName = username,
                Password = password
            };

            _context.Logins.Add(login);
            _context.SaveChanges(); // This will insert the login and get the ID
        }
        catch (Exception e)
        {
            // Handle exceptions
        }
    }

    public int GetLoginId(string username)
    {
        int id = 0;
        string query = "SELECT Login_Id FROM Login WHERE UserName = @UserName";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@UserName", username);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        id = reader.GetInt32(0);
                    }
                }
            }
        }

        return id;
    }
    // Other methods...
}
