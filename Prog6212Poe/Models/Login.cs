using System;
using System.Collections.Generic;

namespace Prog6212Poe.Models
{
    public partial class Login
    {
        public Login()
        {
            Students = new HashSet<Student>();
        }

        public int LoginId { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<Student> Students { get; set; }
    }
}
