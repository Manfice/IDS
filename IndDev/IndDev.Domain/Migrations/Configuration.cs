using System.Data.Entity.Migrations;
using System.Linq;
using IndDev.Domain.Context;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Orders;
using IndDev.Domain.Entity.Products;

namespace IndDev.Domain.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DataContext context)
        {
            context.Roles.AddOrUpdate(r=>r.Name,
                new UsrRoles { Name = "M", Descr = "Moderator",Id = 2},
                new UsrRoles { Name = "O", Descr = "Operator", Id = 3},
                new UsrRoles { Name = "P", Descr = "Partner", Id = 4},
                new UsrRoles { Name = "C", Descr = "Customer", Id = 5}
                );
            context.Users.AddOrUpdate(u=>u.EMail,
                new User
                {
                    Id = 1,
                    Name = "Administrator",
                    EMail = "c592@yandex.ru",
                    Phone = "+79034441684",
                    Block = false,
                    ConfirmEmail = true,
                    Region = "",
                    UsrRoles = new UsrRoles { Name = "A", Descr = "Administrator", Id = 1 },
                    PasswordHash = "C84AE35C470C8C2FCA384F8AB27ABF0F" //FB%iS-_Xtr-nm=x
                }
                );
            context.DeliveryTypes.AddOrUpdate(type => type.Id,
                new DeliveryType {Id = 1, Title = "Самовывоз",Cost = 0,FreeFrom = 0,Description = "Забор заказа осуществляет клиент самостоятельно, по доверенности, со склада компании."},
                new DeliveryType {Id = 2, Title = "Доставка до ТК",Cost = 500,FreeFrom = 20000, Description = "Доставка до транспортной компании осуществляет продавец."},
                new DeliveryType {Id = 3, Title = "Автобусом",Cost = 500,FreeFrom = 20000, Description = "Доставка груза осуществляется по средствам маршрутных автобусов с автовокзала в г. Ставрополе. ВНИМАНИЕ! Этот способ доставки самый ненадежный. Используйте его только в крайних случаях."},
                new DeliveryType { Id = 4, Title = "Доставка по городу Ставрополю",Cost = 500, FreeFrom = 20000, Description = ""}
                );
            context.CustomerStatuses.AddOrUpdate(status => status.Id, 
                new CustomerStatus { Id = 1, Title = "Новичок",Discount = 2,PriceType = PriceType.Retail},
                new CustomerStatus { Id = 1, Title = "Партнер",Discount = 0, PriceType = PriceType.LowOpt },
                new CustomerStatus { Id = 1, Title = "Постоянный клиент",Discount = 0, PriceType = PriceType.LowOpt },
                new CustomerStatus { Id = 1, Title = "Оптовик",Discount = 0, PriceType = PriceType.Opt }
                );
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //context.Roles.AddOrUpdate(r=>r.Name, new UsrRoles { Name = "Customer"}, 
            //    new UsrRoles { Name = "Content_Meneger"},
            //    new UsrRoles { Name = "Administrator"},
            //    new UsrRoles { Name = "SA"}
            //    );
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
