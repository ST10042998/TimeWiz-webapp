using Microsoft.Data.SqlClient;
using Prog6212Poe.Models;

public class Logins
{
    private readonly TimeWizContext _context;
   
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*start

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="context"></param>
    public Logins(TimeWizContext context)
    {
        _context = context;
    }

    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*start

    /// <summary>
    /// add login using entity framework
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
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
            _context.SaveChanges(); 

            return login;
        }
        catch (Exception e)
        {
            return null;
            
        }
    }

    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*end

        /// <summary>
        /// gets login id using username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
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
               
            }
        }
}
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*end

