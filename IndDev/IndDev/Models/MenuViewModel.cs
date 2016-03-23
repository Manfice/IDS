using System.Collections.Generic;
using IndDev.Domain.Entity.Menu;

namespace IndDev.Models
{
    public class MenuViewModel
    {
        public int ParentId { get; set; }
        public IEnumerable<Menu> Menus { get; set; }  
    }
}