using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Claims;
using TenmoClient.Models;
using TenmoClient.Services;

namespace TenmoClient
{
    public class TenmoApp
    {
        private readonly TenmoConsoleService console = new TenmoConsoleService();
        private readonly TenmoApiService tenmoApiService;
        private ApiUser User;

        public TenmoApp(string apiUrl)
        {
            tenmoApiService = new TenmoApiService(apiUrl);
        }

        public void Run()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                // The menu changes depending on whether the user is logged in or not
                if (tenmoApiService.IsLoggedIn)
                {
                    keepGoing = RunAuthenticated();
                }
                else // User is not yet logged in
                {
                    keepGoing = RunUnauthenticated();
                }
            }
        }

        private bool RunUnauthenticated()
        {
            console.PrintLoginMenu();
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 2, 1);
            while (true)
            {
                if (menuSelection == 0)
                {
                    return false;   // Exit the main menu loop
                }

                if (menuSelection == 1)
                {
                    // Log in
                    Login();
                    return true;    // Keep the main menu loop going
                }

                if (menuSelection == 2)
                {
                    // Register a new user
                    Register();
                    return true;    // Keep the main menu loop going
                }
                console.PrintError("Invalid selection. Please choose an option.");
                console.Pause();
            }
        }

        private bool RunAuthenticated()
        {
            console.PrintMainMenu(tenmoApiService.Username);
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 6);
            if (menuSelection == 0)
            {
                // Exit the loop
                return false;
            }

            if (menuSelection == 1)
            {


               
                // View your current balance

                ShowBalance(); 


            }

            if (menuSelection == 2)
            {
                ShowTransfers();
                // View your past transfers
            }

            if (menuSelection == 3)
            {
                // View your pending requests
                //ShowPendingRequests(); optional 
            }

            if (menuSelection == 4)
            {
                
                SendTEBucks();
                // Send TE bucks
            }

            if (menuSelection == 5)
            {
                //RequestTEBucks(); optional 
                // Request TE bucks
            }

            if (menuSelection == 6)
            {
                // Log out
                tenmoApiService.Logout();
                console.PrintSuccess("You are now logged out");
            }

            return true;    // Keep the main menu loop going
        }
        //public void RetrieveBalance()
        //{
            
            
        //    decimal balance = tenmoApiService.GetBalance(tenmoApiService.UserId);
        //    Console.WriteLine(balance);
        //}
        private void Login()
        {
            LoginUser loginUser = console.PromptForLogin();
            if (loginUser == null)
            {
                return;
            }

            try
            {
                ApiUser user = tenmoApiService.Login(loginUser);
                if (user == null)
                {
                    console.PrintError("Login failed.");
                }
                else
                {
                    this.User = user;
                    console.PrintSuccess("You are now logged in");
                }
            }
            catch (Exception)
            {
                console.PrintError("Login failed.");
            }
            console.Pause();
        }

        private void Register()
        {
            LoginUser registerUser = console.PromptForLogin();
            if (registerUser == null)
            {
                return;
            }
            try
            {
                bool isRegistered = tenmoApiService.Register(registerUser);
                if (isRegistered)
                {
                    console.PrintSuccess("Registration was successful. Please log in.");
                }
                else
                {
                    console.PrintError("Registration was unsuccessful.");
                }
            }
            catch (Exception)
            {
                console.PrintError("Registration was unsuccessful.");
            }
            console.Pause();
        }



        private void ShowBalance()
        {
            try
            {
                Account account= tenmoApiService.GetAccountByUserId(User);
                
                if (account != null)
                {
                    console.PrintBalance(account);
                }
            }
            catch (Exception ex)
            {
                console.PrintError(ex.Message);
            }
            console.Pause();
        }


        private void ShowTransfers()
        {
            try
            {
                int userId = User.UserId;
               
                List<Transfer> transfers = tenmoApiService.GetTransferHistory(User.UserId);
                if (transfers != null)
                {
                    foreach (Transfer transfer in transfers)
                    {
                        string accountTo = tenmoApiService.GetUserByAccountId(transfer.AccountTo).Username;
                        string accountFrom = tenmoApiService.GetUserByAccountId(transfer.AccountFrom).Username;

                        console.PrintTransfers(transfer, accountTo, accountFrom);
                    }
                }
                else { Console.WriteLine("You have no previous transfers!"); }

            }
            catch (Exception ex)
            {
                console.PrintError(ex.Message);
            }
            console.Pause();
        }

        public int ShowUsers()
        {
            try
            {
                List<ApiUser> users = tenmoApiService.GetUsers();
                Console.WriteLine("    Users     ");
                
                foreach (ApiUser user in users)
                {
                    
                    if (user.Username != User.Username)
                    {
                        
                        console.PrintUsers(user);
                    }

                    else { continue; }
                }
                
            }
            catch(Exception ex)//copied try catch format from other methods
            {
                console.PrintError(ex.Message);
            }
            return console.PromptForInteger("Choose a user by id number ");
            
        }

        private void SendTEBucks()
        {
            int selecteduser = ShowUsers();
            int amount = console.PromptForInteger(" Amount ");
            try
            {
               if(User.UserId == selecteduser)
                {
                    Console.WriteLine("you can not send yourself money!");
                }
                else
                {
                    ApiUser selectedUser = tenmoApiService.GetUserByUserId(selecteduser);
                    Transfer transfer = new Transfer();
                    transfer.AccountFrom = tenmoApiService.GetAccountByUserId(User).AccountId;
                    transfer.AccountTo = tenmoApiService.GetAccountByUserId(selectedUser).AccountId;
                    transfer.Amount = amount;
                    transfer.TransferStatusId = 2;
                    transfer.TransferTypeId = 2;
                    tenmoApiService.ExecuteTransfer(transfer);
                }
               
              
            }
            catch (Exception ex)
            {
                console.PrintError(ex.Message);
            }
            console.Pause();

        }
    }
}
