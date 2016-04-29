using System;
using System.Collections.Generic;
using IndDev.Auth.Logic;
using IndDev.Auth.Model;
using IndDev.Domain.Entity.Auth;

namespace IndDev.Domain.Abstract
{
    public interface ISecureRepository
    {
        IEnumerable<User> Users { get; }
        User GetUser(string email);
        UsrRoles GetRoleForUser(string role);
        ValidationInfo Register(RegisterViewModel reg);
        ValidationInfo Login(LoginViewModel log);
        User GetUserById(int id);
        User ValidResetPassRequest(string email, Guid secret);
        User ResetGuid(string email);
        ValidEvent RestorePassword(ResetPasswordVm model);
        ValidEvent ConfirmEmail(string email, Guid secret);
    }
}