using System.Collections.Generic;
using TenmoServer.Models;
namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        Account GetAccountByAccountId(int accountId);
        IList<Account> GetAccountsByUserId(int userId);
        Account AddAccountToUser(int userId, Account account);
    }
}
