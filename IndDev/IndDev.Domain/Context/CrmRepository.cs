using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Context
{
    public class CrmRepository : ICrm
    {
        private readonly DataContext _context = new DataContext();

        public IEnumerable<DetailsTitle> Company => _context.Detailses.Where(c=>!string.IsNullOrEmpty(c.CompanyName)).Select(company=>new DetailsTitle
        {
            Id = company.Id,
            CompanyName = company.CompanyName,
            Inn = company.Inn,
            Offer = company.Offer,
            Region = company.Region
        }).ToList();

        public Details GetCompanyDetails(int id)
        {
            return _context.Detailses.Find(id);
        }

        public async Task<Details> DeleteCompanyAsync(int id)
        {
            var cust = _context.Customers.FirstOrDefault(c => c.Details.Id == id);
            if (cust!=null)
            {
                cust.Details = new Details();
            }
            var dbDet = await _context.Detailses.FindAsync(id);
            if (dbDet==null)
            {
                return null;
            }
            _context.Detailses.Remove(dbDet);
            await _context.SaveChangesAsync();
            return dbDet;
        }

        public async Task<Details> DeleteContactAsync(int id)
        {
            var dbPers = await _context.PersonContacts.FindAsync(id);
            if (dbPers == null)
            {
                return null;
            }
            var result = dbPers.Details;
            _context.PersonContacts.Remove(dbPers);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Details> DeletePhoneAsync(int id)
        {
            var dbPhone = await _context.Telephones.FindAsync(id);
            if (dbPhone == null)
            {
                return null;
            }
            var result = dbPhone.DetailsOf;
            _context.Telephones.Remove(dbPhone);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Details> SendKpMarkAsync(PersonContact contact)
        {
            var dbCont = await _context.PersonContacts.FindAsync(contact.Id);
            if (dbCont == null) return null;
            var dbDet = await _context.Detailses.FindAsync(dbCont.Details.Id);
            dbDet.Descr += $"Отправлено КП {DateTime.Now.ToLongDateString()}";
            await _context.SaveChangesAsync();
            return dbDet;
        }

        public async Task<Details> UpdateCompany(Details currCompany)
        {
            Details dbComp;
            if (currCompany.Id == 0)
            {
                if (string.IsNullOrEmpty(currCompany.CompanyName) && string.IsNullOrEmpty(currCompany.Region))
                {
                    return null;
                }
                dbComp = currCompany;
                dbComp.RegisterDate = DateTime.Now;
                _context.Detailses.Add(dbComp);
            }
            else
            {
                dbComp = await _context.Detailses.FindAsync(currCompany.Id);
                if (dbComp == null) return null;
                dbComp.CompanyName = currCompany.CompanyName;
                dbComp.Inn = currCompany.Inn;
                dbComp.Kpp = currCompany.Kpp;
                dbComp.Ogrn = currCompany.Ogrn;
                dbComp.UrAdress = currCompany.UrAdress;
                dbComp.RealAdress = currCompany.RealAdress;
                dbComp.Region = currCompany.Region;
                dbComp.Director = currCompany.Director;
                dbComp.Buh = currCompany.Buh;
                dbComp.Descr = currCompany.Descr;
                dbComp.CompDirect = currCompany.CompDirect;
                dbComp.Offer = currCompany.Offer;

                var tells = new List<Telephone>();
                var persons = new List<PersonContact>();
                foreach (var item in currCompany.Telephones)
                {
                    var tel = await _context.Telephones.FindAsync(item.Id);
                    if (tel != null)
                    {
                        tel.PhoneNumber = item.PhoneNumber;
                        tel.Title = item.Title;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(item.PhoneNumber))
                        {
                            continue;
                        }
                        item.DetailsOf = dbComp;
                        tells.Add(item);
                    }
                }
                _context.Telephones.AddRange(tells);

                foreach (var item in currCompany.PersonContacts)
                {
                    var pers = await _context.PersonContacts.FindAsync(item.Id);
                    if (pers != null)
                    {
                        pers.Email = item.Email;
                        pers.PersonName = item.PersonName;
                        pers.Phone = item.Phone;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(item.Email) || string.IsNullOrEmpty(item.PersonName)) continue;
                        item.Details = dbComp;
                        persons.Add(item);
                    }
                }
                _context.PersonContacts.AddRange(persons);
            }
            await _context.SaveChangesAsync();
            return dbComp;
        }

        public async Task<User> GetUserById(int id)
        {
            var result = await _context.Users.FindAsync(id);
            return result;
        }

        public IEnumerable<DetailsTitle> GetCompanysByUser(int id)
        {
            var companys =
                _context.Detailses.Where(c => c.Meneger.Id == id).Select(company => new DetailsTitle
                {
                    Id = company.Id,
                    CompanyName = company.CompanyName,
                    Inn = company.Inn,
                    Offer = company.Offer,
                    Region = company.Region
                }).ToList();

            return companys;
        }

        public async Task<Telephone> UpdatePhone(Phone phone)
        {
            if (phone == null || phone.Details==0) return null;
            var tel = new Telephone();
            if (phone.Id==0)
            {
                tel.PhoneNumber = phone.PhoneNumber;
                tel.Title = phone.Title;
                tel.DetailsOf = _context.Detailses.Find(phone.Details);
                _context.Telephones.Add(tel);
            }
            else
            {
                tel = _context.Telephones.Find(phone.Id);
                tel.PhoneNumber = phone.PhoneNumber;
                tel.Title = phone.Title;
            }
            await _context.SaveChangesAsync();
            return tel;
        }
    }
}