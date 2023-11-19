using Microsoft.Data.SqlClient;
using Prog6212Poe.Models;

public class Logins
{
    private readonly TimeWizContext _context;
   
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

    
}
