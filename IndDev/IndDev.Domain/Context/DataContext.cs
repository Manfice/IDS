using System;
using System.Data.Entity;
using IndDev.Domain.Entity;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Menu;
using IndDev.Domain.Entity.Orders;
using IndDev.Domain.Entity.Products;

namespace IndDev.Domain.Context
{
    public class DataContext:DbContext
    {
        public DataContext() : base("IndustrialDevelopment")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DataContext>());
        }

        public DbSet<News> News { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UsrRoles> Roles { get; set; }
        public DbSet<UserPhoto> Photos { get; set; }
        public DbSet<GoodsImage> GoodsImages { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Details> Detailses { get; set; }
        public DbSet<Telephone> Telephones { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<CustomerStatus> CustomerStatuses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<MesureUnit> MesureUnits { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Curency> Curses { get; set; }
        //Menu Items in context
        public DbSet<Menu> Menus { get; set; }
        public DbSet<ProductMenuItem> ProductMenuItems { get; set; }
        public DbSet<ProdMenuImage> ProdMenuImages { get; set; }
        public DbSet<ProductMenu> ProductMenus { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        //Orders
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliveryType> DeliveryTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<News>().ToTable("News").HasKey(n => n.Id);
            //modelBuilder.Entity<Details>().HasRequired(d => d.CustomerOf).WithRequiredDependent(c=>c.Details);          
            //modelBuilder.Entity<UserPhoto>().HasRequired(p => p.CustomerOf).WithOptional(c => c.Photo);
            //modelBuilder.Entity<User>().HasRequired(u => u.Customer).WithRequiredPrincipal(c => c.User);
            
        }
    }
}