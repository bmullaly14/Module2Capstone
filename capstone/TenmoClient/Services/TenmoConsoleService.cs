using System;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoConsoleService : ConsoleService
    {
        /************************************************************
            Print methods
        ************************************************************/
        public void PrintLoginMenu()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }

        public void PrintMainMenu(string username)
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine($"Hello, {username}!");
            Console.WriteLine("1: View your current balance");
            Console.WriteLine("2: View your past transfers");
            Console.WriteLine("3: View your pending requests");
            Console.WriteLine("4: Send TE bucks");
            Console.WriteLine("5: Request TE bucks");
            Console.WriteLine("6: Log out");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }
        public LoginUser PromptForLogin()
        {
            string username = PromptForString("User name");
            if (String.IsNullOrWhiteSpace(username))
            {
                return null;
            }
            string password = PromptForHiddenString("Password");

            LoginUser loginUser = new LoginUser
            {
                Username = username,
                Password = password
            };
            username = "";
            password = "";
            return loginUser;
        }

        // Add application-specific UI methods here...

        public void PrintBalance(Account account)
        {
            Console.WriteLine("");
            Console.WriteLine($"Your current balance is {account.Balance}");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("1: Main Menu");
            Console.WriteLine("2: Logout"); 
            Console.WriteLine("3: Exit");
            Console.WriteLine("---------");
        }

        public void PrintTransfers(Transfer transfer, string accountTo, string accountFrom) 
        {
       
                Console.WriteLine($"  Transfer Id: {transfer.TransferId}  |  Account From: {accountFrom}  |  Account To: {accountTo}  |  Amount: {transfer.Amount}");
        }

        public void PrintUsers( ApiUser user) 
        {
            
            Console.WriteLine($"{user.UserId} : { user.Username}");
        }


    }
}
