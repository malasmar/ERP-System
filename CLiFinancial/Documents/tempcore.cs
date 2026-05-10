using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
using CLiCore.Selections;

namespace CLiFinancial.Documents
{
    public class tempcore
    {
        private readonly static object Locker = new object();
        public static OperationResult UpdateTransaction(string DB, Documents.Transaction Header, List<Documents.TransactionDetails> Details, bool IsNew, List<TransactionInvoices> Invoices)
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
                    foreach (Documents.TransactionDetails item in Details)
                    {
                        if (item.vatAmount > 0)
                        {

                            if (Header.DocumentKind == (int)DocumentKind.finDebitNote)
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
                                comm.Parameters.Add("@vat_Date", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.VoucherDate);
                                comm.Parameters.Add("@vat_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                                comm.Parameters.Add("@vat_Kind", SqlDbType.Int).Value = (int)PLenums.vatKind.Sales;
                                comm.Parameters.Add("@vat_vatRate", SqlDbType.Decimal).Value = xConfig.vatRate(DB, item.vatKey);
                                comm.Parameters.Add("@vat_Subtotal", SqlDbType.Decimal).Value = item.Amount;
                                comm.Parameters.Add("@vat_Debit", SqlDbType.Decimal).Value =0 ;
                                comm.Parameters.Add("@vat_Credit", SqlDbType.Decimal).Value = item.vatAmount;
                                comm.Parameters.Add("@vat_vatRegNo", SqlDbType.NVarChar, 25).Value = item.vatRegNo ?? "";
                                comm.Parameters.Add("@vat_AccountName", SqlDbType.NVarChar, 200).Value = item.vatCurrent ?? "";
                                comm.Parameters.Add("@vat_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                                comm.ExecuteNonQuery();
                            }
                            else
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
                                comm.Parameters.Add("@vat_Date", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.VoucherDate);
                                comm.Parameters.Add("@vat_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                                comm.Parameters.Add("@vat_Kind", SqlDbType.Int).Value = (int)PLenums.vatKind.Purchase;
                                comm.Parameters.Add("@vat_vatRate", SqlDbType.Decimal).Value = xConfig.vatRate(DB, item.vatKey);
                                comm.Parameters.Add("@vat_Subtotal", SqlDbType.Decimal).Value = item.Amount;
                                comm.Parameters.Add("@vat_Debit", SqlDbType.Decimal).Value = item.vatAmount;
                                comm.Parameters.Add("@vat_Credit", SqlDbType.Decimal).Value = 0;
                                comm.Parameters.Add("@vat_vatRegNo", SqlDbType.NVarChar, 25).Value = item.vatRegNo ?? "";
                                comm.Parameters.Add("@vat_AccountName", SqlDbType.NVarChar, 200).Value = item.vatCurrent ?? "";
                                comm.Parameters.Add("@vat_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                                comm.ExecuteNonQuery();
                            }
                     
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
        public static OperationResult UpdateJournalVoucher(string DB, Documents.Transaction Header, List<Documents.TransactionDetails> Details, bool IsNew)
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
                    foreach (Documents.TransactionDetails item in Details)
                    {
                        if (item.isVAT == true)
                        {
                            decimal vatrate = xConfig.vatRate(DB, item.vatKey);
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
                            comm.Parameters.Add("@vat_Date", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ReferenceDate);
                            comm.Parameters.Add("@vat_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                            comm.Parameters.Add("@vat_Kind", SqlDbType.Int).Value = item.Debit > 0 ? (int)PLenums.vatKind.Purchase : (int)PLenums.vatKind.Sales;
                            comm.Parameters.Add("@vat_vatRate", SqlDbType.Decimal).Value = vatrate;
                            comm.Parameters.Add("@vat_Subtotal", SqlDbType.Decimal).Value = ((item.Debit + item.Credit) * 100) / vatrate;
                            comm.Parameters.Add("@vat_Debit", SqlDbType.Decimal).Value = item.Debit;
                            comm.Parameters.Add("@vat_Credit", SqlDbType.Decimal).Value = item.Credit;
                            comm.Parameters.Add("@vat_vatRegNo", SqlDbType.NVarChar, 25).Value = item.vatRegNo ?? "";
                            comm.Parameters.Add("@vat_AccountName", SqlDbType.NVarChar, 200).Value = item.vatCurrent ?? "";
                            comm.Parameters.Add("@vat_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
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
