using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Customers;

namespace IndDev.Domain.ViewModels
{
    public class CustomerViewModel
    {
        public User User { get; set; }
        public Customer Customer { get; set; } 
    }

    public class CustMenuItems
    {
        public string Title { get; set; }
        public string MenuLink { get; set; }
        public bool IsActive { get; set; }
    }

    public class EditCustomer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Adress { get; set; }
        public DateTime Register { get; set; }
        public Details Details { get; set; }
        public int Status { get; set; }
        public IEnumerable<SelectListItem> CustStatus { get; set; }

    }

    public class Feedback
    {
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string MailMessage { get; set; }
    }
}