using System;
using System.Collections.Generic;
using System.Linq;
using IndDev.Auth.Logic;
using IndDev.Auth.Model;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Customers;

namespace IndDev.Domain.Context
{
    public class DbSecureRepository : ISecureRepository
    {
        readonly DataContext _context = new DataContext();
        public IEnumerable<User> Users => _context.Users;

        public ValidationInfo Login(LoginViewModel log)
        {
            var passHash = SecureLogic.EncodeMd5(log.Password);
            var dbUser =
                _context.Users.Where(
                    u => (u.EMail == log.Login) && (u.PasswordHash == passHash)).ToList();
            if (dbUser.Count==0)
            {
                return new ValidationInfo() {Code = 1,
                    Message = "Пользователь с такой парой e-mail/пароль не найден в базе данных"};
            }
            return new ValidationInfo()
            {
                Code = 0,
                Message = "Вход выполнен успешно"
            };
        }

        public User GetUser(string email)
        {
            return _context.Users.FirstOrDefault(u => u.EMail == email);
        }

        public ValidationInfo Register(RegisterViewModel reg)
        {
            var dbUser = _context.Users.Where(u => u.EMail == reg.Email).ToList();
            if (dbUser.Count != 0)
            {
                return new ValidationInfo()
                {
                    Code = -1,
                    Message = "Пользователь с таким\"" + reg.Email + "\" электронным адресом уже существует!"
                };

            }
            var usr = new User()
            {
                Name = reg.UserName,
                EMail = reg.Email,
                Phone = reg.Phone,
                Region = reg.Region,
                PasswordHash = SecureLogic.EncodeMd5(reg.Password),
                ConfirmEmail = false,
                UsrRoles = GetRoleForUser("C"),
                Block = false,
                Customer = new Customer {Title = reg.UserName, Details = new Details() }
            };
            _context.Users.Add(usr);
            _context.SaveChanges();
            return new ValidationInfo()
            {
                Code = 0,
                Message = "Пользователь добавлен, вы можете перейти в свой личный кабинет."
            };
        }

        public UsrRoles GetRoleForUser(string role)
        {
            var userRole = _context.Roles.FirstOrDefault(r => r.Name == role);
            return userRole ?? null;
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }
    }
}