using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
 

namespace CLiInventory.Documents
{
    public class clstempcore
    {
        private readonly static object Locker = new object();
        public static CLiCore.OperationResult UpdatePurchaseInvoice(string DB, Transaction Header, List<TransactionDetails> Details, bool IsNew)
        {
            lock (Locker)
            {
              
                using (SqlConnection conn = new SqlConnection(iCore.GetCon(DB)))
                {
                    System.Text.StringBuilder str = new System.Text.StringBuilder();

                    conn.Open();
                    SqlCommand comm = conn.CreateCommand();
                    SqlTransaction transaction;
                    transaction = conn.BeginTransaction("Transaction");
                    comm.Connection = conn;
                    comm.Transaction = transaction;
                    comm.CommandType = CommandType.Text;

                    var vatGroup = Details.GroupBy(x => x.vatKey);
                    foreach (var vat in vatGroup)
                    {
                        if (Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount) > 0)
                        {
                            str.Clear();
                            str.Append("INSERT INTO vatDocument_Transaction");
                            str.Append("([vat_OperationKey]");
                            str.Append(",[vat_Date]");
                            str.Append(",[vat_vatKey]");
                            str.Append(",[vat_Kind]");
                            str.Append(",[vat_vatRate]");
                            str.Append(",[vat_Subtotal]");
                            str.Append(",[vat_Debit]");
                            str.Append(",[vat_Credit]");
                            str.Append(",[vat_vatRegNo]");
                            str.Append(",[vat_AccountName]");
                            str.Append(",[vat_Description])");
                            str.Append(" VALUES ");
                            str.Append("(@vat_OperationKey");
                            str.Append(",@vat_Date");
                            str.Append(",@vat_vatKey");
                            str.Append(",@vat_Kind");
                            str.Append(",@vat_vatRate");
                            str.Append(",@vat_Subtotal");
                            str.Append(",@vat_Debit");
                            str.Append(",@vat_Credit");
                            str.Append(",@vat_vatRegNo");
                            str.Append(",@vat_AccountName");
                            str.Append(",@vat_Description)");
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = str.ToString();
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@vat_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OperationKey);
                            comm.Parameters.Add("@vat_Date", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                            comm.Parameters.Add("@vat_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(vat.Key);
                            comm.Parameters.Add("@vat_Kind", SqlDbType.Int).Value = (int)PLenums.vatKind.Purchase;
                            comm.Parameters.Add("@vat_vatRate", SqlDbType.Decimal).Value = xConfig.vatRate(DB, vat.Key);
                            comm.Parameters.Add("@vat_Subtotal", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.Amount - x.Discount);
                            comm.Parameters.Add("@vat_Debit", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@vat_Credit", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@vat_vatRegNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@vat_AccountName", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@vat_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                            comm.ExecuteNonQuery();
                        }
                    }
                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
                OperationResult res = new OperationResult();
                return res;
            }
        }
        public static CLiCore.OperationResult UpdateReturnPurchaseInvoice(string DB, Transaction Header, List<TransactionDetails> Details, bool IsNew)
        {
            lock (Locker)
            {
                using (SqlConnection conn = new SqlConnection(iCore.GetCon(DB)))
                {
                    System.Text.StringBuilder str = new System.Text.StringBuilder();

                    conn.Open();
                    SqlCommand comm = conn.CreateCommand();
                    SqlTransaction transaction;
                    transaction = conn.BeginTransaction("Transaction");
                    comm.Connection = conn;
                    comm.Transaction = transaction;
                    comm.CommandType = CommandType.Text;
                    //VAT
                    var vatGroup = Details.GroupBy(x => x.vatKey);
                    foreach (var vat in vatGroup)
                    {
                        if (Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount) > 0)
                        {
                            str.Clear();
                            str.Append("INSERT INTO vatDocument_Transaction");
                            str.Append("([vat_OperationKey]");
                            str.Append(",[vat_Date]");
                            str.Append(",[vat_vatKey]");
                            str.Append(",[vat_Kind]");
                            str.Append(",[vat_vatRate]");
                            str.Append(",[vat_Subtotal]");
                            str.Append(",[vat_Debit]");
                            str.Append(",[vat_Credit]");
                            str.Append(",[vat_vatRegNo]");
                            str.Append(",[vat_AccountName]");
                            str.Append(",[vat_Description])");
                            str.Append(" VALUES ");
                            str.Append("(@vat_OperationKey");
                            str.Append(",@vat_Date");
                            str.Append(",@vat_vatKey");
                            str.Append(",@vat_Kind");
                            str.Append(",@vat_vatRate");
                            str.Append(",@vat_Subtotal");
                            str.Append(",@vat_Debit");
                            str.Append(",@vat_Credit");
                            str.Append(",@vat_vatRegNo");
                            str.Append(",@vat_AccountName");
                            str.Append(",@vat_Description)");
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = str.ToString();
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@vat_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OperationKey);
                            comm.Parameters.Add("@vat_Date", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                            comm.Parameters.Add("@vat_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(vat.Key);
                            comm.Parameters.Add("@vat_Kind", SqlDbType.Int).Value = (int)PLenums.vatKind.Purchase;
                            comm.Parameters.Add("@vat_vatRate", SqlDbType.Decimal).Value = xConfig.vatRate(DB, vat.Key);
                            comm.Parameters.Add("@vat_Subtotal", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.Amount - x.Discount);
                            comm.Parameters.Add("@vat_Debit", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@vat_Credit", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@vat_vatRegNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@vat_AccountName", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@vat_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                            comm.ExecuteNonQuery();
                        }
                    }
                    try
                    {
                        transaction.Commit();
                  
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }

                OperationResult res = new OperationResult();
                return res;
            }
        }
        public static CLiCore.OperationResult UpdateSalesInvoice(string DB, Transaction Header, List<TransactionDetails> Details)
        {
            lock (Locker)
            {
              
                using (SqlConnection conn = new SqlConnection(iCore.GetCon(DB)))
                {
                    System.Text.StringBuilder str = new System.Text.StringBuilder();

                    conn.Open();
                    SqlCommand comm = conn.CreateCommand();
                    SqlTransaction transaction;
                    transaction = conn.BeginTransaction("Transaction");
                    comm.Connection = conn;
                    comm.Transaction = transaction;
                    comm.CommandType = CommandType.Text;
                    //VAT
                    var vatGroup = Details.Where(x=>x.vatAmount>0).GroupBy(x => x.vatKey);
                    foreach (var vat in vatGroup)
                    {
                        if (Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount) > 0)
                        {
                            str.Clear();
                            str.Append("INSERT INTO vatDocument_Transaction");
                            str.Append("([vat_OperationKey]");
                            str.Append(",[vat_Date]");
                            str.Append(",[vat_vatKey]");
                            str.Append(",[vat_Kind]");
                            str.Append(",[vat_vatRate]");
                            str.Append(",[vat_Subtotal]");
                            str.Append(",[vat_Debit]");
                            str.Append(",[vat_Credit]");
                            str.Append(",[vat_vatRegNo]");
                            str.Append(",[vat_AccountName]");
                            str.Append(",[vat_Description])");
                            str.Append(" VALUES ");
                            str.Append("(@vat_OperationKey");
                            str.Append(",@vat_Date");
                            str.Append(",@vat_vatKey");
                            str.Append(",@vat_Kind");
                            str.Append(",@vat_vatRate");
                            str.Append(",@vat_Subtotal");
                            str.Append(",@vat_Debit");
                            str.Append(",@vat_Credit");
                            str.Append(",@vat_vatRegNo");
                            str.Append(",@vat_AccountName");
                            str.Append(",@vat_Description)");
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = str.ToString();
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@vat_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OperationKey);
                            comm.Parameters.Add("@vat_Date", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                            comm.Parameters.Add("@vat_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(vat.Key);
                            comm.Parameters.Add("@vat_Kind", SqlDbType.Int).Value = (int)PLenums.vatKind.Sales;
                            comm.Parameters.Add("@vat_vatRate", SqlDbType.Decimal).Value = xConfig.vatRate(DB, vat.Key);
                            comm.Parameters.Add("@vat_Subtotal", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.Amount - x.Discount);
                            comm.Parameters.Add("@vat_Debit", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@vat_Credit", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@vat_vatRegNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@vat_AccountName", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@vat_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                            comm.ExecuteNonQuery();
                        }
                    }
                    try
                    {
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }

                OperationResult res = new OperationResult();

                return res;
            }
        }
        public static CLiCore.OperationResult UpdateReturnSalesInvoice(string DB, Transaction Header, List<TransactionDetails> Details, bool IsNew)
        {
            lock (Locker)
            {
              
                using (SqlConnection conn = new SqlConnection(iCore.GetCon(DB)))
                {
                    System.Text.StringBuilder str = new System.Text.StringBuilder();

                    conn.Open();
                    SqlCommand comm = conn.CreateCommand();
                    SqlTransaction transaction;
                    transaction = conn.BeginTransaction("Transaction");
                    comm.Connection = conn;
                    comm.Transaction = transaction;
                    comm.CommandType = CommandType.Text;
                    var vatGroup = Details.GroupBy(x => x.vatKey);
                    foreach (var vat in vatGroup)
                    {
                        if (Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount) > 0)
                        {
                            str.Clear();
                            str.Append("INSERT INTO vatDocument_Transaction");
                            str.Append("([vat_OperationKey]");
                            str.Append(",[vat_Date]");
                            str.Append(",[vat_vatKey]");
                            str.Append(",[vat_Kind]");
                            str.Append(",[vat_vatRate]");
                            str.Append(",[vat_Subtotal]");
                            str.Append(",[vat_Debit]");
                            str.Append(",[vat_Credit]");
                            str.Append(",[vat_vatRegNo]");
                            str.Append(",[vat_AccountName]");
                            str.Append(",[vat_Description])");
                            str.Append(" VALUES ");
                            str.Append("(@vat_OperationKey");
                            str.Append(",@vat_Date");
                            str.Append(",@vat_vatKey");
                            str.Append(",@vat_Kind");
                            str.Append(",@vat_vatRate");
                            str.Append(",@vat_Subtotal");
                            str.Append(",@vat_Debit");
                            str.Append(",@vat_Credit");
                            str.Append(",@vat_vatRegNo");
                            str.Append(",@vat_AccountName");
                            str.Append(",@vat_Description)");
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = str.ToString();
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@vat_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OperationKey);
                            comm.Parameters.Add("@vat_Date", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                            comm.Parameters.Add("@vat_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(vat.Key);
                            comm.Parameters.Add("@vat_Kind", SqlDbType.Int).Value = (int)PLenums.vatKind.Sales;
                            comm.Parameters.Add("@vat_vatRate", SqlDbType.Decimal).Value = xConfig.vatRate(DB, vat.Key);
                            comm.Parameters.Add("@vat_Subtotal", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.Amount - x.Discount);
                            comm.Parameters.Add("@vat_Debit", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@vat_Credit", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@vat_vatRegNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@vat_AccountName", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@vat_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                            comm.ExecuteNonQuery();
                        }
                    }

                    try
                    {
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }

                OperationResult res = new OperationResult();

                return res;
            }
        }


    
    }
}
