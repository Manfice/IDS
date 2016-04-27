using System;
using System.Collections.Generic;
using IndDev.Domain.Entity.Auth;

namespace IndDev.Domain.Entity.Customers
{
    public class CustomerModel
    {
         
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Adress { get; set; }
        public DateTime Register { get; set; }
        public virtual CustomerLogo Photo { get; set; }
        public virtual Details Details { get; set; }
    }

    public class Details
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Inn { get; set; }
        public string Kpp { get; set; }
        public string Ogrn { get; set; }    
        public string UrAdress { get; set; }
        public string RealAdress { get; set; }
        public virtual ICollection<Telephone> Telephones { get; set; }
        public virtual ICollection<Bank> Banks { get; set; }
    }

    public class Telephone
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Numder { get; set; }
        public virtual Details DetailsOf { get; set; }
    }

    public class Bank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Korr { get; set; }
        public string Bik { get; set; }
        public virtual Details DetailsOf { get; set; }
    }

    public class CustomerLogo
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string AltText { get; set; }
        public string FullPath { get; set; }

    }
}