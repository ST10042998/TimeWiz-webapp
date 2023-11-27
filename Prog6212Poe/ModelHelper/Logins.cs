using Microsoft.Data.SqlClient;
using Prog6212Poe.Models;

public class Logins
{
    private readonly TimeWizContext _context;
   
public Logins(TimeWizContext context)
    {
        _context = context;
    }



    public Login AddLoginUsingEF(string username, string password)
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

            return login;
        }
        catch (Exception e)
        {
            return null;
            // Handle exceptions
        }
    }
        public int GetLoginId(string username)
        {
            try
            {
                var login = _context.Logins.FirstOrDefault(x => x.UserName == username);
                return login.LoginId;
            }
            catch (Exception e)
            {
                return 0;
                // Handle exceptions
            }
        }
    }

    

