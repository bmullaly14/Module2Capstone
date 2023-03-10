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

        public Transfer CreateTransfer(Transfer transfer)//adds transfer to sql database.  SHould it return a transfer? Should it take in tranfer information as parameters instead of tranfer object?
        {
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO transfer (transfer_type_id,transfer_status_id,account_from,account_to,amount) 
                                                    OUTPUT INSERTED.transfer_id
                                                    Values (@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount)", conn);
                    cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferTypeId);
                    cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusId);
                    cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                    int newID = Convert.ToInt32(cmd.ExecuteScalar());
                    Transfer returnTransfer = GetTransfer(newID);

                    return returnTransfer;
                }
            }
            catch (SqlException) { throw; }

            
            //return transfer;
        }

        public Transfer GetTransfer(int transferId)//Gets single transfer given transfer id from sql database
        {
            Transfer returnTransfer = new Transfer();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM transfer WHERE transfer_id = @transfer_id", conn);

                    cmd.Parameters.AddWithValue("@transfer_id", transferId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        returnTransfer = CreateTransferFromReader(reader);
                    }
                }
                return returnTransfer;
            }
            catch (SqlException)
            {
                return returnTransfer;//would return empty transfer object
            }

            
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
                                                    WHERE @user_id = transfer.account_from OR @user_id= transfer.account_to;", conn);//I dont know if this makes sense???

                    cmd.Parameters.AddWithValue("@user_id", userId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transfer transfer = CreateTransferFromReader(reader);
                        transfers.Add(transfer);
                    }
                }
                return transfers;
            }
            catch (SqlException) { return null; }

            
        }


        public bool ExecuteTransfer(Transfer transfer)//actually add/remove money from two accounts.  
        {
           
            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"Begin Transaction; 
                                                    UPDATE account SET balance = balance-(@amount)
                                                    WHERE account_id = (SELECT account_from FROM transfer WHERE transfer_id = @id);
                    
                                                    UPDATE account SET balance = balance+(@amount)
                                                    WHERE account_id = (SELECT account_to FROM transfer where transfer_id = @id);

                                                    COMMIT;", conn);

                    cmd.Parameters.AddWithValue("@amount",transfer.Amount);
                    cmd.Parameters.AddWithValue("@id", transfer.TransferId);
                    //cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    //cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);

                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (SqlException ) { throw; }


       
        }

       




        public Transfer CreateTransferFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer();
            transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
            transfer.TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
            transfer.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
            transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
            transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
            transfer.Amount = Convert.ToDecimal(reader["amount"]);

            return transfer;


        }
    }
}
