namespace Prog6212Poe.Models
{
    using System;
    using System.Collections.Generic;

    public partial class StudyDays
    {
        public int StudyDaysId { get; set; }
        public string Day { get; set; }
        public int Module_Id { get; set; }

        public virtual ModuleTable Module { get; set; } = null!;
    }
}