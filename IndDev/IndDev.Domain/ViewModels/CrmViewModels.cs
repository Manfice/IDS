﻿namespace IndDev.Domain.ViewModels
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
}