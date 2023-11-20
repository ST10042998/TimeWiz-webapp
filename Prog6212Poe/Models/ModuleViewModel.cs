using Microsoft.AspNetCore.Mvc.Rendering;

namespace Prog6212Poe.Models
{
    public class ModuleViewModel
    {
        public ModuleTable Module { get; set; }
        public Semester Semester { get; set; }
        public int SelectedItemId { get; set; }
        public List<SelectListItem> ItemsList { get; set; }
        public ModuleViewModel()
        {
            // Initialize the properties if needed
            Module = new ModuleTable();
            Semester = new Semester();
        }   
    }
}
