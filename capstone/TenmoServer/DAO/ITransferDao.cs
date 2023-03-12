using System.Collections.Generic;
using TenmoServer.Models;
using System.Security.Cryptography.Xml;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        Transfer CreateTransfer(Transfer transfer);

        IList<Transfer> GetTransfersByUserId(int userId);
        IList<Transfer> GetPendingTransfersByUserId(int userId);

        bool UpdatePendingTransfer(Transfer transfer);
        Transfer GetTransfer(int transferId);

        bool ExecuteTransfer(Transfer transfer);


    }
}
