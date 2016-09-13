using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Customers;

namespace IndDev.Domain.Context
{
    public class CrmRepository : ICrm
    {
        private readonly DataContext _context = new DataContext();

        public IEnumerable<Details> Company => _context.Detailses.Where(c=>!string.IsNullOrEmpty(c.CompanyName)).ToList();

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

        public async Task<Details> UpdateCompany(Details currCompany)
        {
            var dbComp = new Details();
            if (currCompany.Id == 0)
            {
                dbComp = currCompany;
                _context.Detailses.Add(currCompany);
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
                foreach (var item in currCompany.Telephones)
                {
                    var tel = await _context.Telephones.FindAsync(item.Id);
                    if (tel != null && !string.IsNullOrEmpty(item.PhoneNumber))
                    {
                        tel.PhoneNumber = item.PhoneNumber;
                        tel.Title = item.Title;
                    }
                    else
                    {
                        item.DetailsOf = dbComp;
                        _context.Telephones.Add(item);
                    }
                }
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
                        _context.PersonContacts.Add(item);
                    }
                }

            }
            await _context.SaveChangesAsync();
            return dbComp;
        }
    }
}