using System.Collections.Generic;

namespace IndDev.Models
{
    public class CatViewModel
    {
        public IEnumerable<string> Categories { get; set; }
        public string SelectedCategory { get; set; } 
    }
}