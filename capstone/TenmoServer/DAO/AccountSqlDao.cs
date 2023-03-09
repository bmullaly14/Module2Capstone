using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            Account account = new Account();
            try
            {
                using(SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM account WHERE account_id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", accountId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        account = CreateAccountFromReader(reader);
                    }
                }
                return account;
            }
            catch(SqlException ex)
            {
                Console.WriteLine(ex.Message); //CHANGE THIS LATER!!! HERE FOR NOW
            }
            return account;
        }
        public Account GetAccountByUserId(int userId)
        {
            Account account = new Account();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM account WHERE user_id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", userId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        account = CreateAccountFromReader(reader);
                    }
                }
                return account;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message); //CHANGE THIS LATER!!! HERE FOR NOW
            }
            return account;
        }



        public Account AddAccountToUser(int userId)//will need to add an account anytime a user is created.  Maybe add an if statment if user id already has an existing account?
        {
            
            try
            {
                using(SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO account (user_id,balance) 
                                                    OUTPUT INSERTED.account_id
                                                    VALUES(@userId,1000);", conn); //I manually made balance 1000 idk if thats neccessary or not but I did it.
                    cmd.Parameters.AddWithValue("@userId",userId);
                   
                    int newID = Convert.ToInt32(cmd.ExecuteScalar());

                    Account createdAccount = GetAccountByUserId(newID);
                    return createdAccount;
                }
                
            }
            catch 
            {
                throw;
            }
           
        }

        public Account CreateAccountFromReader(SqlDataReader reader)
        {
            Account newAccount = new Account();

            newAccount.UserId = Convert.ToInt32(reader["user_id"]);
            newAccount.Balance = Convert.ToInt32(reader["balance"]);
            newAccount.AccountId = Convert.ToInt32(reader["account_id"]);

            return newAccount;
        }

    }
}
