using System;
using System.Collections.Generic;

namespace Prog6212Poe.Models
{
    public partial class Semester
    {
        public Semester()
        {
            ModuleTables = new HashSet<ModuleTable>();
        }

        public int SemesterId { get; set; }
        public int SemesterNum { get; set; }
        public int NumOfWeeks { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StudentId { get; set; }

        public virtual Student Student { get; set; } = null!;
        public virtual ICollection<ModuleTable> ModuleTables { get; set; }
    }
}
