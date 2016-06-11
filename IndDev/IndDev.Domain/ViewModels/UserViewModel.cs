using System.Collections.Generic;
using System.Web.Mvc;
using IndDev.Domain.Entity.Auth;

namespace IndDev.Domain.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        public IEnumerable<SelectListItem> Status { get; set; } 

    }
}