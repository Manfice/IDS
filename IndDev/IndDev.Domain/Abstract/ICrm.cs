﻿using System.Collections.Generic;
using System.Threading.Tasks;
using IndDev.Domain.Entity.Customers;

namespace IndDev.Domain.Abstract
{
    public interface ICrm
    {
        IEnumerable<Details> Company { get; }
        Details GetCompanyDetails(int id);
        Task<Details> DeleteCompanyAsync(int id);
        Task<Details> DeletePhoneAsync(int id);
        Task<Details> DeleteContactAsync(int id);
        Task<Details> UpdateCompany(Details currCompany);
        Task<Details> SendKpMarkAsync(PersonContact contact);

    }
}