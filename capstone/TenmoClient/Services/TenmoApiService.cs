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


            return true;

        }

        
        
        public decimal GetBalance(int userId)
        {
            
            RestRequest request = new RestRequest($"user/{userId}/balance");
            IRestResponse<decimal> response = client.Get<decimal>(request);

            CheckForError(response);
            
            return response.Data;
        }
        

        } 

 



