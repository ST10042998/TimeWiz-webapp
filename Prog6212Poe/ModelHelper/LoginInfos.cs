using Microsoft.Data.SqlClient;
using Prog6212Poe.Models;

namespace Prog6212Poe.ModelHelper
{
    public class LoginInfos
    {
        private TimeWizContext db;

        private LoginInfo logInfo = new LoginInfo();

        public LoginInfos(TimeWizContext db)
        {
            this.db = db;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// adding login info 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       public LoginInfo AddLoginInfoEF(int id)
        {
            try
            {
                logInfo.LoginId = id;
                db.LoginInfos.Add(logInfo);
                db.SaveChanges();
                return logInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// delete login info
        /// </summary>
        public void DeleteLoginInfoEF()
        {
            try
            {
                db.LoginInfos.Remove(logInfo);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return;
            }
        }   
        //---------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get last added login info
        /// </summary>
        /// <returns></returns>
        public int GetLastAdded()
        {
          
            
                var lastAdded = db.LoginInfos.OrderByDescending(l => l.LoginId).FirstOrDefault();
                if (lastAdded != null)
                {
                    return lastAdded.LoginId;
                }
                else
                {
                    return 0;
                }
            
        }

        public LoginInfo GetLoginInfoEF()
        {
            try
            {
                var LoginInfo = db.LoginInfos.FirstOrDefault();
                return LoginInfo;
            }
            catch (Exception ex)
            {
               return null;
            }
        }

    }
}
//----------------------------------------------------------------------------------------------------------------------------------------Eugene*end