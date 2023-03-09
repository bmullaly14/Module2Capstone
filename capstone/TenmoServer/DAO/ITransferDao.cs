using System.Collections.Generic;
using TenmoServer.Models;
using System.Security.Cryptography.Xml;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        void CreateTransfer(Transfer transfer);

        IList<Transfer> GetTransfersByUserId(int userId);

        Transfer GetTransfer(int transferId);

        Transfer ExecuteTransfer(Transfer transfer);


    }
}
