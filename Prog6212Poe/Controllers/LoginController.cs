using Microsoft.AspNetCore.Mvc;
using Prog6212Poe.ModelHelper;
using Prog6212Poe.Models;
using System.Runtime.CompilerServices;
using System.Text;
using TimeWizWebApp.HelperClasses;

namespace Prog6212Poe.Controllers
{
    public class LoginController : Controller
    {
        //initialize variables
        private readonly TimeWizContext _context;
        private LoginHelper loginHelper;
        private Logins logins;
        private LoginInfos loginInfo;

        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor for the login controller
        /// </summary>
        /// <param name="context"></param>
        public LoginController(TimeWizContext context)
        {
            _context = context;
            loginHelper = new LoginHelper(context);
            logins = new Logins(context);
            loginInfo = new LoginInfos(context);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// returns the register view
        /// </summary>
        /// <returns></returns>
        public IActionResult RegisterView()
        {
            return View();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// register method that returns the login view if the registration is successful
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RegisterView(RegisterViewModel register)
        {
            try
            {

                if (register != null && loginHelper.checkInput(register.Student.Name, register.Student.Surname, register.Student.Email, register.Student.Gender, register.Login.UserName, register.Login.Password))
                {
                    loginHelper.AddStudent(register.Student.Name, register.Student.Surname, register.Student.Email, register.Student.Gender, register.Login.UserName, register.Login.Password);
                    ViewBag.Message = loginHelper.mess;
                    return View("LoginView");
                }

                else
                {
                    ViewBag.Message = "not added";
                    return View("RegisterView", register);
                }
            }
            catch
            {
                ViewBag.Message = "Username already exists";
                return View("RegisterView", register);
            }

        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------
       
        /// <summary>
        /// returns the login view
        /// </summary>
        /// <returns></returns>
        public IActionResult LoginView()
        {
            return View();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// login method that returns the index view if the login is successful
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LoginView(Login log)
        {
            loginInfo.DeleteLoginInfoEF();

            bool isAuthenticated = await Task.Run(() => loginHelper.Login(log.UserName, log.Password));

            if (isAuthenticated)
            {
                var Login_Id = logins.GetLoginId(log.UserName);
                loginInfo.AddLoginInfoEF(Login_Id);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Username or password is incorrect";
                return View("LoginView", log);
            }
        }

    }
}
//--------------------------------------------------------------------------------------------------------------------------------------------------------*Eugene*end.