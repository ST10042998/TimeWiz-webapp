using System.Text;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using Prog6212Poe.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace TimeWizWebApp.HelperClasses
{
    public class LoginHelper
    {
        private readonly TimeWizContext _context;
        private Logins login;
        private Students student;
        public int loginId { get; set; }
        public string mess { get; set; }
        public LoginHelper(TimeWizContext context)
        {
            _context = context;
            login = new Logins(context);
            student = new Students(context);
        }

        string pass = string.Empty;
   
        public void AddStudent(string name, string surname, string email, string gender, string username, string password)
        {
            if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password) && this.CheckPassword(password))
            {
                
               login.AddLoginUsingEF(username, this.HashPassword(password));
               loginId = login.GetLoginId(username);
               

            }

            if (!String.IsNullOrWhiteSpace(name) && !String.IsNullOrWhiteSpace(surname) && !String.IsNullOrWhiteSpace(email) && !String.IsNullOrWhiteSpace(gender) && this.CheckEmail(email))
            {
              
                student.AddStudentUsingEF(name, surname, email, gender,loginId);
               
            }

            else
            {
                return;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Hash the password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string HashPassword(string password)
        {
          
                byte[] data = Encoding.ASCII.GetBytes(password);
                data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                return  Encoding.ASCII.GetString(data);
            
        }

        public int GetLoginId(string username)
        {
            int id = 0;

            try
            {
                var login = _context.Logins
                    .Where(l => l.UserName == username)
                    .FirstOrDefault();

                if (login != null)
                {
                    id = login.LoginId;
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                mess = ex.Message;
            }

            return id;
        }



        //----------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// CHECK PASSWORD
        /// </summary>
        /// <param name="passwd"></param>
        /// <returns></returns>
        public bool CheckPassword(string passwd)
        {
            bool hasUpperCase = false;
            bool hasLowerCase = false;
            bool hasSpecialChar = false;

            // Check the password length
            if (passwd.Length < 8 || passwd.Length > 14)
            {
                return false;
            }

            // Check for uppercase, lowercase, and special characters
            foreach (char ch in passwd)
            {
                if (char.IsUpper(ch))
                {
                    hasUpperCase = true;
                }
                else if (char.IsLower(ch))
                {
                    hasLowerCase = true;
                }
                else if ("%!@#$%^&*()?/>.<,:;'\\|}]{[_~`+=-\"".Contains(ch))
                {
                    hasSpecialChar = true;
                }

                // If all criteria are met, return true
                if (hasUpperCase && hasLowerCase && hasSpecialChar)
                {
                    return true;
                }
            }
            return false;

        }

        //----------------------------------------------------------------------------------------------------------------------------------

     /// <summary>
     /// bool to check if the user can login
     /// </summary>
     /// <param name="username"></param>
     /// <param name="password"></param>
     /// <returns></returns>
        public bool Login(string username, string password)
        {
            if (String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            bool isAuthenticated = false;

            var user = _context.Logins.FirstOrDefault(u => u.UserName == username);

            if (user != null)
            {
                // Compare the stored hashed password with the hashed input password
                if (user.Password == this.HashPassword(password))
                {
                    isAuthenticated = true;
                }
            }

            return isAuthenticated;
        }

        //----------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// check if the email is valid
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool CheckEmail(string email)
        {
            if (email.Contains("@"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
//----------------------------------------------------------------------------------------------------------------------------------Eugene*End
