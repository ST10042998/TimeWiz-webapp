using System;
using System.Collections.Generic;

namespace Prog6212Poe.Models
{
    public partial class ModuleTable
    {
        public ModuleTable()
        {
            StudyDay = new HashSet<StudyDays>();
        }

        public int ModuleId { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int? Credits { get; set; }
        public int SemesterId { get; set; }
        public int? ClassHoursPerWeek { get; set; }
        public int? SelfStudyHours { get; set; }
        public int? RemainingWeekHours { get; set; }
        public int? ProgressBarPercentage { get; set; }
        public DateTime? StudyDate { get; set; }
        public int? StudiedHours { get; set; }

        public virtual Semester Semester { get; set; } = null!;
        public virtual ICollection<StudyDays> StudyDay { get; set; }
    }
}
