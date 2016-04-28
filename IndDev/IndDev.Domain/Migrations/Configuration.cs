using System.Data.Entity.Migrations;
using IndDev.Domain.Context;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Orders;

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
                new DeliveryType {Id = 1, Title = "���������",Cost = 0,FreeFrom = 0,Description = "����� ������ ������������ ������ ��������������, �� ������������, �� ������ ��������."},
                new DeliveryType {Id = 2, Title = "�������� �� ��",Cost = 300,FreeFrom = 15000, Description = "�������� �� ������������ �������� ������������ ��������."},
                new DeliveryType {Id = 3, Title = "���������",Cost = 500,FreeFrom = 10000, Description = "�������� ����� �������������� �� ��������� ���������� ��������� � ����������� � �. ����������. ��������! ���� ������ �������� ����� ����������. ����������� ��� ������ � ������� �������."},
                new DeliveryType { Id = 4, Title = "��������", Cost = 0, FreeFrom = 0,Description = "�������� �������� ��������� ��� ��������� ������ �� 5�� ��."},
                new DeliveryType { Id = 5, Title = "�������� �� ������ ����������",Cost = 500, FreeFrom = 15000, Description = ""}
                );
            context.PaymentMethods.AddOrUpdate(method => method.Id,
                new PaymentMethod {Id = 1,Title = "������������� �� ��. ����.",Description = "������ �� ������������ �������, ����� ����."},
                new PaymentMethod {Id = 2,Title = "������ �� �����.",Description = "������ �� �����. ������ ��� ���������� ���."},
                new PaymentMethod {Id = 3,Title = "��������� � �����",Description = "������ ���������. ��� ���. ���."}
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
