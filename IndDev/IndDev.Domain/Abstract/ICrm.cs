using System.Collections.Generic;
using System.Threading.Tasks;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Abstract
{
    public interface ICrm
    {
        IEnumerable<DetailsTitle> Company { get; }
        IEnumerable<DetailsTitle> GetCompanysByUser(int id); 
        Details GetCompanyDetails(int id);
        Task<Details> DeleteCompanyAsync(int id);
        Task<Details> DeletePhoneAsync(int id);
        Task<Details> DeleteContactAsync(int id);
        Task<Details> UpdateCompany(Details currCompany);
        Task<Telephone> UpdatePhone(Phone phone);
        Task<int> UpdatePerson(Person person);
        Task<Details> SendKpMarkAsync(PersonContact contact);
        Task<User> GetUserById(int id);
    }
}