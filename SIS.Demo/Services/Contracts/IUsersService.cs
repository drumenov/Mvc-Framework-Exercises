﻿using SIS.Demo.ViewModels;

namespace SIS.Demo.Services.Contracts
{
    public interface IUsersService
    {
        bool ExistsByUsernameAndPassword(string username, string password);

        bool TryRegisterUser(RegisterViewModel model);
    }
}