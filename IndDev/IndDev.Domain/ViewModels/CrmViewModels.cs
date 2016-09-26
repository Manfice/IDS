using System;
using System.Diagnostics;

namespace IndDev.Domain.ViewModels
{
    public class CrmViewModels
    {
    }

    public class DetailsTitle
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Inn { get; set; }
        public string Offer { get; set; }//Сайт предприятия
        public string Region { get; set; }
        
    }

    public class Phone
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Title { get; set; }
        public int Details { get; set; }
    }

    public class Person
    {
        public int Id { get; set; }
        public string PersonName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Details { get; set; }
    }

    public class EventData
    {
        public int Id { get; set; }
        public DateTime EventDate { get; set; }
        public bool EventInit { get; set; }
        public string EventType { get; set; }
        public int Priority { get; set; }
        public bool RemindMe { get; set; }
        public string Descr { get; set; }
        public int Details { get; set; }
        public DetailsTitle Company { get; set; }
        public string Meneger { get; set; }
    }
}