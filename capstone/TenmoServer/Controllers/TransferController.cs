using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using TenmoServer.DAO;
using TenmoServer.Models;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {

        private ITransferDao transferDao;
        private IUserDao userDao;
        private IAccountDao accountDao;

        public TransferController(ITransferDao transferDao, IUserDao userDao, IAccountDao accountDao)
        {
            this.transferDao = transferDao;
            this.userDao = userDao;
            this.accountDao = accountDao;

        }
        //need HttpGet(ID), HttpPost to transfer to other user,
        //HttpPut(TransactionID) to update both account -- I think we can do this with a post...
        //The actual "doing" will happen in the DAO, so we just have to make the transfer.

        [HttpGet("/transfer/user/{userId}")]
        public ActionResult<IList<Transfer>> GetAllTransfersForUser(int userId)
        {

            if (CheckUser(userId))
            {
                List<Transfer> transfers = new List<Transfer>();

                transfers = (List<Transfer>)transferDao.GetTransfersByUserId(userId);

                if (transfers.Count == 0)
                {
                    return NoContent();
                }
                else
                {   
                    return transfers;
                }
            } else { return Unauthorized(); }

        }
        [HttpGet("/transfer/{transferId}")]
        public ActionResult<Transfer> GetTransferByTransferId(int transferId)
        {
            Transfer transfer = null;

            transfer = transferDao.GetTransfer(transferId);



            if (CheckAuthForTransfer(transfer))
            {                
               return transfer;
            } 
            else 
            {
                if (transfer.TransferId == 0)
                {
                    return NotFound();
                } 
                return Unauthorized(); 
            }
        }
        //Create Transfer will be HttpPost, take in Transfer, put out Transfer w/ ID from SQL DB
        [HttpGet("/transfer/user/{userId}/pending")]
        public ActionResult<IList<Transfer>> GetPendingTransfersByUserId(int userId)
        {
            if (CheckUser(userId))
            {
                List<Transfer> transfers = new List<Transfer>();

                transfers = (List<Transfer>)transferDao.GetPendingTransfersByUserId(userId);

                if (transfers.Count == 0)
                {
                    return NoContent();
                }
                else
                {
                    return transfers;
                }
            }
            else { return Unauthorized(); }
        }
        [HttpPost]
        public ActionResult<Transfer> CreateTransfer(Transfer transfer)
        {
            if (CheckAuthForTransfer(transfer) && CheckValidTransferAmount(transfer))
            {
                if (CheckSendOrRequest(transfer))
                {
                    transfer.TransferTypeId = 2;
                    transfer.TransferStatusId = 2;
                    Transfer newTransfer = transferDao.CreateTransfer(transfer);

                    return ExecuteSendTransfer(newTransfer);
                }
                else
                {
                    transfer.TransferTypeId = 1;
                    transfer.TransferStatusId = 1;
                    Transfer newTransfer = transferDao.CreateTransfer(transfer);
                    return newTransfer;
                }
            }
            else
            {
                return Unauthorized();
            }
        
            
                //(Created($"/transfer/{newTransfer.TransferId}", newTransfer));
        }

        [HttpPut]
        public ActionResult<Transfer> ExecuteSendTransfer([FromBody] Transfer transfer) // take in a transfer FROM create transfer in DAO
        {            
            //Transfer newTransfer = CreateTransfer(transfer.Value);

            if(transfer.TransferId == 0)
            {
                return NotFound();
            }

            bool result = transferDao.ExecuteTransfer(transfer);            

            if (!result)
            {
                return StatusCode(500);
            }
            else
            {
                return transfer;
            }
             
        }
        [HttpPut("/transfer/{transferid}")]
        public ActionResult<Transfer> ApproveOrRejectTransfer(int transferId, [FromForm] bool approved) //**Requires {"approved" : true/false} in JSON body
        {
            Transfer pending = transferDao.GetTransfer(transferId);
            if (CheckAuthForApprove(pending) == false)
            {
                if (pending.TransferStatusId != 1)
                {
                    return BadRequest();
                }
                return Unauthorized();
            }
            if (CheckValidTransferAmount(pending) == false)
            {
                return BadRequest();
            }
            if (approved)
            {
                pending = ApproveTransfer(pending);
                transferDao.UpdatePendingTransfer(pending);
                return ExecuteSendTransfer(pending);
            }
            else
            {
                pending = RejectTransfer(pending);
                transferDao.UpdatePendingTransfer(pending);
                return pending;
            }

        }

        public bool CheckUser(int userId) //checks if current user matches a given userID
        {
            string userName = User.Identity.Name;

            User user = userDao.GetUserByName(userName);            
            
            if (user.UserId == userId)
            {
                return true;
            } else { return false; }
        }
        public bool CheckAuthForTransfer(Transfer transfer) //checks that user isn't sending to self, and that current user is involved in transfer
        {
            if (transfer.AccountTo == transfer.AccountFrom)
            {
                return false;
            }
            else if (CheckUser(accountDao.GetAccountByAccountId(transfer.AccountTo).UserId) || CheckUser(accountDao.GetAccountByAccountId(transfer.AccountFrom).UserId))
            {
                return true;
            }            
            return false;
        }
        public bool CheckAuthForApprove(Transfer transfer)//make sure current user is "From", make sure status is pending, make sure not sending to self
        {
            if (CheckUser(transfer.AccountFrom) && transfer.TransferStatusId == 1 && CheckAuthForTransfer(transfer)) 
            {
                return true;
            } else { return false; }
        }
        public bool CheckValidTransferAmount(Transfer transfer) //check that From account has balance >= transfer amount, check transfer amount not negative
        { 
            if (transfer.Amount > accountDao.GetAccountByAccountId(transfer.AccountFrom).Balance)
            {
                return false;
            }
            else if (transfer.Amount <= 0)
            {
                return false;
            }
            return true;
        }
        public bool CheckSendOrRequest(Transfer transfer) //Checks to see if transfer is a "Send" or a "Request"
        { 
            if(CheckUser(accountDao.GetAccountByAccountId(transfer.AccountFrom).UserId)) 
            {
                return true;
            } else if (CheckUser(accountDao.GetAccountByAccountId(transfer.AccountTo).UserId))
            {
                return false;
            }
            return false;
        }
        public Transfer ApproveTransfer(Transfer transfer) //Assigns value of 2 to TransferStatusId of given transfer
        {
            transfer.TransferStatusId = 2;
            return transfer;
        }
        public Transfer RejectTransfer(Transfer transfer) //Assigns value of 3 to TransferStatusId of given transfer
        {
            transfer.TransferStatusId = 3;
            return transfer;
        }
    }
}

        //[Authorize(Roles = "user")]
        //[HttpPost] //create new transfer 


    
