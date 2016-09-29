using System.Collections.Generic;
using System.Threading.Tasks;
using IndDev.Domain.Entity.Menu;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Abstract
{
    public interface IShopRepository
    {
        IEnumerable<ProductMenu> GetProductMenus { get; }
        IEnumerable<Product> GetProducts { get; }
        //IEnumerable<Menu> GetTopMenus();
        IEnumerable<Menu> GetSubMenu(int id);//Curent menu ID
        ProductMenu GetProductMenu(int id);
        void SaveSearch(SearchRequests model);
        ProductView GetProductDetails(int id);
        ShopProductView GetProduct(int id);
        Task<ShopProductView> SearchProductsAsynk(string request);
        IEnumerable<ProductView> GetTopRetails { get; }
        Task<IEnumerable<ProductMenu>> GetTopMenus();
        Task<IEnumerable<ProductAjax>> GetRetails();
        Task<IEnumerable<Brand>> GetBrandsPicAsync();
    }
}