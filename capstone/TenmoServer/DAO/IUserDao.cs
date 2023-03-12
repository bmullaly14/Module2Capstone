﻿using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IUserDao
    {
        User GetUserById(int userId);
        User AddUser(string username, string password);
        List<User> GetUsers();
        User GetUserByName(string username);

        User GetUserByAccountId(int accountId);
    }
}
