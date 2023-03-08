using System.Collections.Generic;
using TenmoServer.Models;
namespace TenmoServer.DAO
{
    public class AccountSqlDao : IAccountDao
    {
        private readonly string ConnectionString;

        public AccountSqlDao(string dbConnectionString)
        {
            ConnectionString = dbConnectionString;
        }
       
        public Account GetAccountByAccountId(int accountId)
        {
            return new Account();
        }
        public IList<Account> GetAccountsByUserId(int userId)
        {
            return new List<Account>();
        }
        public Account AddAccountToUser(int userId, Account account)
        {
            return new Account();
        }


    }
}
