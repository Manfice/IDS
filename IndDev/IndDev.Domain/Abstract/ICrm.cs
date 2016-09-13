using System.Collections.Generic;
using System.Threading.Tasks;
using IndDev.Domain.Entity.Customers;

namespace IndDev.Domain.Abstract
{
    public interface ICrm
    {
        IEnumerable<Details> Company { get; }
        Task<Details> DeleteCompanyAsync(int id);
        Task<Telephone> DeletePhoneAsync(int id);
        Task<PersonContact> DeleteContactAsync(int id);
        Task<int> UpdateCompany(Details currCompany);

    }
}