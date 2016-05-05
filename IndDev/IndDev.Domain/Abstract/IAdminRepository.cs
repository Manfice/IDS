using System.Collections.Generic;
using IndDev.Auth.Logic;
using IndDev.Domain.Entity;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Menu;
using IndDev.Domain.Entity.Products;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Abstract
{
    public interface IAdminRepository
    {
        User User(int id);
        IEnumerable<User> Users();
        Customer Customer(int id);
        IEnumerable<UsrRoles> Roleses { get; }
        IEnumerable<CustomerStatus> GetCustomerStatuses { get; }
        IEnumerable<Customer> GetCustomers { get; }
        ValidationInfo UpdateUser(User user);
        IEnumerable<ProductMenu> ProductMenus();
        ValidEvent AddCategory(ProductMenu menu);
        void DeleteCatImage(ProdMenuImage image);
        ValidEvent RemoveCategory(int id);
        ProductMenu GetProductMenu(int id);
        void AddSubCategory(ProductMenuItem item);
        ValidEvent UpdateCategory(ProductMenu menu, ProdMenuImage image);
        ValidEvent UpdateSubCategory(ProductMenuItem menu, ProdMenuImage image);
        ProductMenuItem GetSubCat(int id);
        ValidEvent RemoveSubCat(int id);
        IEnumerable<Menu> GetrootMenuItems();
        IEnumerable<Menu> GetSubMenuItems(int id);
        Menu GetMenu(int id);
        void AddSubMenuItem(Menu menu);
        void RemoveSubMenu(int id);
        IEnumerable<ProductViewModel> GetProductsByMenu(int menuId);
        IEnumerable<Brand> GetAllBrands();
        IEnumerable<MesureUnit> GetAllMesureUnits();
        IEnumerable<Vendor> GetAllVendors();
        ValidEvent SaveProduct(AddProductViewModel model);
        IEnumerable<Product> GetCatProduct(int catId);
        Product GetProduct(int id);
        ValidEvent RemoveProduct(int id);
        ValidEvent AddPhotoToProduct(int prodId, ProductPhoto photo);
        IEnumerable<PriceViewModel> GetProdPrices(int prodId);
        PriceSetter GetPriceSetter(int id);
        ValidEvent SetPrice(PriceSetter model);
        Product UpdateProduct(Product model);
        Customer SaveCustomer(EditCustomer model);
    }
}