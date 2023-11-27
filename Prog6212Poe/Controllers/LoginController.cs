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
        private readonly TimeWizContext _context;
        private LoginHelper loginHelper;
        private Logins logins;
        private LoginInfos loginInfo;
        public LoginController(TimeWizContext context)
        {
            _context = context;
            loginHelper = new LoginHelper(context);
            logins = new Logins(context);
            loginInfo = new LoginInfos(context);
        }

        public IActionResult RegisterView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterView(RegisterViewModel register)
        {
            
            if (register != null)
            {
                loginHelper.AddStudent(register.Student.Name, register.Student.Surname, register.Student.Email, register.Student.Gender, register.Login.UserName, register.Login.Password);
                ViewBag.Message = loginHelper.mess;
                return View("LoginView");
            }
            
            else 
            {  
                ViewBag.Message = "Username already exists";
                return View("RegisterView", register);
            }

        }

        public IActionResult LoginView()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginView(Login log)
        {
           loginInfo.DeleteLoginInfoEF();

           if(loginHelper.Login(log.UserName, log.Password))
            {
                var Login_Id = 0;

                Login_Id = logins.GetLoginId(log.UserName);
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
