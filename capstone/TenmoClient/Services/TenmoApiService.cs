using RestSharp;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoApiService : AuthenticatedApiService
    {
        public readonly string ApiUrl;

        public TenmoApiService(string apiUrl) : base(apiUrl) { }

        // Add methods to call api here...
        public bool Register(LoginUser registerUser)
        {
            RestRequest request = new RestRequest("/login/register");
            request.AddJsonBody(registerUser);
            IRestResponse<ApiUser> response = client.Post<ApiUser>(request);
            return true;
        }

            CheckForError(response);

            return true;

        }
       

        public Account GetBalance(int accountId)
        {
            RestRequest request = new RestRequest($"account/{accountId}/balance");
            IRestResponse<Account> response = client.Get<Account>(request);
            CheckForError(response);
            return response.Data;
        }

        public Account GetTransferHistory(int accountId)
        {
            RestRequest request = new RestRequest($"account/{accountId}");
            IRestResponse<Account> response = client.Get<Account>(request);
            CheckForError(response);
            return response.Data;
        }

        public Account GetAccount(int accountId) 
        
        {
            RestRequest request = new RestRequest($"account/{accountId}");
            IRestResponse<Account> response = client.Get<Account>(request);
            CheckForError(response);
            return response.Data;
        
        }

<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
            return true;
<<<<<<< HEAD
        }

        
        
        public decimal GetBalance(int userId)
        {
            
            RestRequest request = new RestRequest($"user/{userId}/balance");
            IRestResponse<decimal> response = client.Get<decimal>(request);

            CheckForError(response);
            
            return response.Data;
        }
        
=======
        } 
>>>>>>> 35ee5a35424ee5eab6fa514e35fc3f88e8d2e040
>>>>>>> 2439720368ee2d0a2e6119e398a7dcf169cbf753
>>>>>>> 74bbd434fad6793f9f792ccdff40479cacff0702
>>>>>>> 6e58f904d5a2c92ca7d0754ff22e2317bd2ae16c
    }
}



