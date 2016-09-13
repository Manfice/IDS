using System;
using System.Collections.Generic;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Products;

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
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PhoneCell { get; set; }
        public string PhoneWork { get; set; }
        public string Email { get; set; }
        public int Ranck { get; set; }
        public DateTime Register { get; set; }
        public virtual CustomerLogo Logo { get; set; }
        public virtual Details Details { get; set; }
        public virtual CustomerStatus CustomerStatus { get; set; }
        public virtual ICollection<UserPhoto> Photos { get; set; }
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
        public string Director { get; set; }
        public string Buh { get; set; }
        public string Offer { get; set; }//почта для офера с контрагентом
        public string Region { get; set; }
        public string CompDirect { get; set; }//Тип компании
        public string Descr { get; set; }
        public virtual ICollection<Telephone> Telephones { get; set; }
        public virtual ICollection<PersonContact> PersonContacts { get; set; }
        public virtual ICollection<Bank> Banks { get; set; }
    }

    public class Telephone
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public virtual Details DetailsOf { get; set; }
    }

    public class Bank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Korr { get; set; }
        public string Bik { get; set; }
        public bool Main { get; set; }
        public virtual Details DetailsOf { get; set; }
    }

    public class CustomerLogo
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string AltText { get; set; }
        public string FullPath { get; set; }

    }

    public class CustomerStatus
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Discount { get; set; }
        public PriceType PriceType { get; set; }
    }

    public class PersonContact
    {
        public int Id { get; set; }
        public string PersonName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public virtual Details Details { get; set; }
    }
}