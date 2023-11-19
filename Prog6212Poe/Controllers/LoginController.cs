using Microsoft.AspNetCore.Mvc;
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
        public LoginController(TimeWizContext context)
        {
            _context = context;
            loginHelper = new LoginHelper(context);
        }

        public IActionResult RegisterView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterView(RegisterViewModel register)
        {
            
                if (loginHelper.AddStudent(register.Student.Name, register.Student.Surname, register.Student.Email, register.Student.Gender, register.Login.UserName, register.Login.Password))
                {
                ViewBag.Message = loginHelper.loginId;
                return View("RegisterView", register);
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
           if(loginHelper.Login(log.UserName, log.Password))
            {
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
