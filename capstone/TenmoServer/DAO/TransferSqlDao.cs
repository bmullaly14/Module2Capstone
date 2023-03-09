﻿using System;
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

        public Transfer CreateTransfer(int transferTypeId, int transferStatusId, int accountFrom, int accountTo, decimal amount)//adds transfer to sql database.  SHould it return a transfer? Should it take in tranfer information as parameters instead of tranfer object?
        {
            Transfer transfer = new Transfer();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO transfer (transfer_type_id,transfer_status_id,account_from,account_to,amount) 
                                                    Values (@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount)", conn);
                    cmd.Parameters.AddWithValue("@transfer_type_id", transferTypeId);
                    cmd.Parameters.AddWithValue("@transfer_status_id", transferStatusId);
                    cmd.Parameters.AddWithValue("@account_from", accountFrom);
                    cmd.Parameters.AddWithValue("@account_to", accountTo);
                    cmd.Parameters.AddWithValue("@amount", amount);

                    cmd.ExecuteNonQuery();

                }
            }
            catch (SqlException) { throw; }

            transfer.TransferTypeId=transferTypeId;
            transfer.TransferStatusId=transferStatusId;
            transfer.AccountFrom=accountFrom;
            transfer.AccountTo=accountTo;
            transfer.Amount=amount;
            return transfer;
        }

        public Transfer GetTransfer(int transferId)//Gets single transfer given transfer id from sql database
        {
            Transfer transfer = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FFROM transfer WHERE transfer_id = @transfer_id", conn);

                    cmd.Parameters.AddWithValue("@transfer_id", transferId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        transfer = CreateTransferFromReader(reader);
                    }
                }
            }
            catch (SqlException)
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
                                                    WHERE user_id = transfer.account_from OR user_id= transfer.account_to;", conn);//I dont know if this makes sense???

                    cmd.Parameters.AddWithValue("@user_id", userId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transfer transfer = CreateTransferFromReader(reader);
                        transfers.Add(transfer);
                    }
                }

            }
            catch (SqlException) { return null; }

            return transfers;
        }


        public bool ExecuteTransfer(Transfer transfer)//actually add/remove money from two accounts.  
        {
           
            try
            {
                using(SqlConnection conn = new SqlConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"Begin Transaction; 
                                                    UPDATE account SET balance = balance-(@amount)
                                                    WHERE account_id = (SELECT account_from FROM transfer WHERE account_id = @account_from);
                    
                                                    UPDATE account SET balance = balance+(@amount)
                                                    WHERE account_id = (SELECT account_to FROM transfer where account_id = @account_to);

                                                    COMMIT;", conn);

                    cmd.Parameters.AddWithValue("@amount",transfer.Amount);
                    cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);

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
            transfer.TransferTypeId = Convert.ToInt32(reader["trasnfer_type_id"]);
            transfer.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
            transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
            transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
            transfer.Amount = Convert.ToDecimal(reader["amount"]);

            return transfer;


        }
    }
}
