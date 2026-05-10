using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiFinancial.FixedAssets.Operation
{
    public class core
    {
        private readonly static object Locker = new object();
        public static CLiCore.OperationResult UpdateBookValues(string DB, BookValues Header, List<BookValuesDetails> Details, bool IsNew)
        {
            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;

                if (IsNew == false)
                {
                    opk = Header.OperationKey;
                    VoucherNo = Header.VoucherNo;

                }
                else
                {
                    opk = Guid.NewGuid();
                    VoucherNo = 1;// xcore.MaxTransaction(DB, Header.DocumentKind, Header.InvoiceDate.Value.Year);
                }


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
                    comm.CommandText = " delete from finFixedAssets_BookValues where [fxd_OperationKey]=@key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();

                    comm.CommandType = CommandType.Text;
                    comm.CommandText = " delete from finFixedAssets_Transactions where [fxd_OperationKey]=@key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();

                    //Header Data
                    str.Clear();
                    str.Append("INSERT INTO finFixedAssets_BookValues");
                    str.Append("([fxd_OperationKey]");
                    str.Append(",[fxd_CreateUser]");
                    str.Append(",[fxd_CreateDate]");
                    str.Append(",[fxd_LastupUser]");
                    str.Append(",[fxd_LastupDate]");
                    str.Append(",[fxd_Status]");
                    str.Append(",[fxd_Branch]");
                    str.Append(",[fxd_Prefix]");
                    str.Append(",[fxd_VoucherNo]");
                    str.Append(",[fxd_VoucherDate]");
                    str.Append(",[fxd_ReferenceNo]");
                    str.Append(",[fxd_Description]");
                    str.Append(",[fxd_Quantity]");
                    str.Append(",[fxd_Total]");
                    str.Append(",[fxd_CostCenter]");
                    str.Append(",[fxd_Project])");
                    str.Append(" VALUES ");
                    str.Append("(@fxd_OperationKey");
                    str.Append(",@fxd_CreateUser");
                    str.Append(",@fxd_CreateDate");
                    str.Append(",@fxd_LastupUser");
                    str.Append(",@fxd_LastupDate");
                    str.Append(",@fxd_Status");
                    str.Append(",@fxd_Branch");
                    str.Append(",@fxd_Prefix");
                    str.Append(",@fxd_VoucherNo");
                    str.Append(",@fxd_VoucherDate");
                    str.Append(",@fxd_ReferenceNo");
                    str.Append(",@fxd_Description");
                    str.Append(",@fxd_Quantity");
                    str.Append(",@fxd_Total");
                    str.Append(",@fxd_CostCenter");
                    str.Append(",@fxd_Project)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@fxd_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@fxd_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@fxd_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@fxd_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@fxd_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@fxd_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@fxd_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@fxd_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@fxd_VoucherNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@fxd_VoucherDate", SqlDbType.Date).Value = Header.VoucherDate;
                    comm.Parameters.Add("@fxd_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@fxd_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@fxd_Quantity", SqlDbType.Decimal).Value = Details.Sum(x=>x.Quantity);
                    comm.Parameters.Add("@fxd_Total", SqlDbType.Decimal).Value = Details.Sum(x=>x.Total);
                    comm.Parameters.Add("@fxd_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@fxd_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();

                    foreach (BookValuesDetails item in Details)
                    {
                        str.Clear();
                        str.Append("INSERT INTO finFixedAssets_BookValuesDetails");
                        str.Append("([fxd_OperationKey]");
                        str.Append(",[fxd_Key]");
                        str.Append(",[fxd_Index]");
                        str.Append(",[fxd_Fixture]");
                        str.Append(",[fxd_PurDate]");
                        str.Append(",[fxd_UnitPrice]");
                        str.Append(",[fxd_Quantity]");
                        str.Append(",[fxd_Total]");
                        str.Append(",[fxd_Description]");
                        str.Append(",[fxd_CostCenter]");
                        str.Append(",[fxd_Project])");
                        str.Append(" VALUES ");
                        str.Append("(@fxd_OperationKey");
                        str.Append(",@fxd_Key");
                        str.Append(",@fxd_Index");
                        str.Append(",@fxd_Fixture");
                        str.Append(",@fxd_PurDate");
                        str.Append(",@fxd_UnitPrice");
                        str.Append(",@fxd_Quantity");
                        str.Append(",@fxd_Total");
                        str.Append(",@fxd_Description");
                        str.Append(",@fxd_CostCenter");
                        str.Append(",@fxd_Project)");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@fxd_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@fxd_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@fxd_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@fxd_Fixture", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Fixture);
                        comm.Parameters.Add("@fxd_PurDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.PurDate);
                        comm.Parameters.Add("@fxd_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@fxd_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@fxd_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@fxd_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.Parameters.Add("@fxd_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@fxd_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                        comm.ExecuteNonQuery();


                        str.Clear();
                        str.Append("INSERT INTO finFixedAssets_Transactions");
                        str.Append("([fxd_Key]");
                        str.Append(",[fxd_OperationKey]");
                        str.Append(",[fxd_Fixture]");
                        str.Append(",[fxd_DepPercent]");
                        str.Append(",[fxd_PurchaseDate]");
                        str.Append(",[fxd_Quantity]");
                        str.Append(",[fxd_UnitPrice]");
                        str.Append(",[fxd_Amount]");
                        str.Append(",[fxd_CostCenter]");
                        str.Append(",[fxd_Project])");
                        str.Append(" VALUES ");
                        str.Append("(@fxd_Key");
                        str.Append(",@fxd_OperationKey");
                        str.Append(",@fxd_Fixture");
                        str.Append(",(select top 100 percent x.[fxd_Percent] from [finFixedAssets_Fixture] x where [fxd_Key]=@fxd_Fixture)");
                        str.Append(",@fxd_PurchaseDate");
                        str.Append(",@fxd_Quantity");
                        str.Append(",@fxd_UnitPrice");
                        str.Append(",@fxd_Amount");
                        str.Append(",@fxd_CostCenter");
                        str.Append(",@fxd_Project)");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@fxd_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@fxd_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@fxd_Fixture", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Fixture);
                        //comm.Parameters.Add("@fxd_DepPercent", SqlDbType.Decimal).Value = item.DepPercent;
                        comm.Parameters.Add("@fxd_PurchaseDate", SqlDbType.Date).Value = item.PurDate;
                        comm.Parameters.Add("@fxd_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@fxd_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@fxd_Amount", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@fxd_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@fxd_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                        comm.ExecuteNonQuery();

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
                res.OperationKey = opk;
                res.VoucherNo = VoucherNo;
                return res;
            }
        }
    }
}
