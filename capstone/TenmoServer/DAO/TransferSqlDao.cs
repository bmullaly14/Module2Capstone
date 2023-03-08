using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;


namespace TenmoServer.DAO

{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public void CreateTransfer(Transfer transfer)//adds transfer to sql database.  SHould it return a transfer? Should it take in tranfer information as parameters instead of tranfer object?
        {
            try
            {
                using(SqlConnection conn=new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO transfer (transfer_type_id,transfer_status_id,account_from,account_to,amount) 
                                                    Values (@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount)", conn);
                    cmd.Parameters.AddWithValue("@transfer_type_id",transfer.TransferTypeId);
                    cmd.Parameters.AddWithValue("@transfer_status_id",transfer.TransferStatusId);
                    cmd.Parameters.AddWithValue("@account_from",transfer.AccountFrom);
                    cmd.Parameters.AddWithValue("@account_to",transfer.AccountTo);
                    cmd.Parameters.AddWithValue("@amount",transfer.Amount);

                    cmd.ExecuteNonQuery();

                }
            }
            catch(SqlException) {throw;}
        }

        public Transfer GetTransfer(int transferId)//Gets single transfer given transfer id from sql database
        {
            Transfer transfer= null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FFROM transfer WHERE transfer_id = @transfer_id", conn);

                    cmd.Parameters.AddWithValue("@transfer_id", transferId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if(reader.Read())
                    {
                        transfer = CreateTransferFromReader(reader);
                    }
                }
            }
            catch(SqlException) 
            { 
                return transfer;//would return empty transfer object
            } 

            return transfer;
        }

        public IList<Transfer> GetTransfersByUserId(int userId)//(should lol) return list of all transfers assosiacted with user id
        {
            IList<Transfer> transfers = new List<Transfer>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"SELECT transfer_id FROM transfer 
                                                    JOIN account ON account.account_id = transfer.account_from 
                                                    JOIN tenmo_user ON account.user_id=tenmo_user.user_id 
                                                    WHERE 
                                                    (SELECT account_id FROM account WHERE tenmo_user.@user_id = account.user_id) = transfer.account_from 
                                                    OR 
                                                    (SELECT account_id FROM account WHERE tenmo_user.@user_id = account.user_id) = transfer.account_to;", conn);//I dont know if this makes sense???

                    cmd.Parameters.AddWithValue("@user_id", userId);

                    SqlDataReader reader= cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        Transfer transfer = CreateTransferFromReader(reader);
                        transfers.Add(transfer);
                    }
                }

            }
            catch(SqlException) { return null;}

            return transfers;
        }

        public Transfer ExecuteTransfer() {
            //TODO fill this out 
            return null;
        }




        public Transfer CreateTransferFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer();
            transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
            transfer.TransferTypeId = Convert.ToInt32(reader["trasnfer_type_id"]);
            transfer.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
            transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
            transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
            transfer.Amount = Convert.ToDecimal(reader["amount"]);

            return transfer;


        }
    }
}
