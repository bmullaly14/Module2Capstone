using RestSharp;
using RestSharp.Authenticators;
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
           


            CheckForError(response);

            return true;

        }
        public Account GetAccountByUserId(ApiUser user)
        {

            RestRequest request = new RestRequest($"account/user/name/{user.Username}");
            IRestResponse<Account> response = client.Get<Account>(request);
            CheckForError(response);
            return response.Data;
        }

       public ApiUser GetUserByUserId(int userId)
        {
            RestRequest request = new RestRequest($"user/id/{userId}");
            IRestResponse<ApiUser> response = client.Get<ApiUser>(request);
            CheckForError(response);
            return response.Data;

        }


        public Account GetBalance(ApiUser user)
        {
            RestRequest request = new RestRequest($"account/{GetAccountByUserId(user).AccountId}/balance");
            IRestResponse<Account> response = client.Get<Account>(request);
            CheckForError(response);
            return response.Data;
        }

        public List<Transfer> GetTransferHistory(int userId)
        {
            RestRequest request = new RestRequest($"/transfer/user/{userId}");
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
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



        public decimal GetBalanceByUserId(int userId)
        {

            RestRequest request = new RestRequest($"user/{userId}/balance");
            IRestResponse<decimal> response = client.Get<decimal>(request);

            CheckForError(response);

            return response.Data;
        }

        public ApiUser GetUserByAccountId(int accountId)
        {
            RestRequest request = new RestRequest($"user/account/{accountId}");
            IRestResponse<ApiUser> response= client.Get<ApiUser>(request);

            CheckForError(response);

            return response.Data;
        }


        public List<ApiUser> GetUsers()
        {
            RestRequest request = new RestRequest("user");
            IRestResponse<List<ApiUser>> response = client.Get<List<ApiUser>>(request);

            CheckForError(response);

            return response.Data;
        }

        public Transfer ExecuteTransfer(Transfer newTransfer)
        {
            RestRequest request = new RestRequest("transfer");
            request.AddJsonBody(newTransfer);
            IRestResponse<Transfer> response= client.Post<Transfer>(request);
            CheckForError(response);
            return response.Data;
        }

    }
}

 



