using System;
using System.Collections.Generic;

namespace Prog6212Poe.Models
{
    public partial class Student
    {
        public Student()
        {
            Semesters = new HashSet<Semester>();
        }

        public int StudentId { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public int? LoginId { get; set; }

        public virtual Login? Login { get; set; }
        public virtual ICollection<Semester> Semesters { get; set; }
    }
}
