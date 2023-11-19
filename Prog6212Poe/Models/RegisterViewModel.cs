using Prog6212Poe.Models;

namespace Prog6212Poe.Models
{
    public class RegisterViewModel
    {
        public Student Student { get; set; }
        public Login Login { get; set; }
        

        public RegisterViewModel()
        {
            // Initialize the properties if needed
            Student = new Student();
            Login = new Login();
        }
    }
}