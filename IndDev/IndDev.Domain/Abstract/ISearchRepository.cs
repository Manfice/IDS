using System.Collections.Generic;
using System.Threading.Tasks;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Abstract
{
    public interface ISearchRepository
    {
        List<Product> GetProducts { get; }
        Task<SearchModel> SearchResult(string request);
    }
}