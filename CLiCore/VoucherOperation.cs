using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLiCore.Account.Selections;
using System.Reflection.PortableExecutable;

namespace CLiCore
{
    public class VoucherOperation
    {

        public static int GetMaxVouchers(string DB, int DocKind, int Year)
        {
            int result = 1;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top 100 percent isnull(max([Fin_VoucherNo])+1,1) from [finDocument_Transaction] where datepart(year,[Fin_VoucherDate])=@Year and [Fin_DocumentKind]=@DocKind";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                com.Parameters.Add("@DocKind", SqlDbType.Int).Value = DocKind;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (int)reader[0];
                };
                reader.Close();
            }
            return result;
        }
        public static int GetMaxMonthlyVouchers(string DB, int Year, int Month, int DocKind)
        {
            int result = 1;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top 100 percent isnull(max([Fin_MonthlyNo])+1,1) from [finDocument_Transaction] where datepart(Year,[Fin_VoucherDate])=@Year and datepart(Month,[Fin_VoucherDate])=@Month and [Fin_DocumentKind]=@DocKind";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                com.Parameters.Add("@Month", SqlDbType.Int).Value = Month;
                com.Parameters.Add("@DocKind", SqlDbType.Int).Value = DocKind;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (int)reader[0];
                };
                reader.Close();
            }
            return result;
        }

        public static int GetMaxInvoices(string DB, int DocKind)
        {
            int result = 1;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top 100 percent isnull(max([inv_InvoiceNo])+1,1) from [InvDocument_Transaction] where  [inv_DocumentKind]=@DocKind";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@DocKind", SqlDbType.Int).Value = DocKind;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (int)reader[0];
                };
                reader.Close();
            }
            return result;
        }
        public static int GetMaxMonthlyInvoices(string DB, int Year, int Month, int DocKind)
        {
            int result = 1;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top 100 percent isnull(max([inv_MonthlyNo])+1,1) from [InvDocument_Transaction] where datepart(Year,[inv_InvoiceDate])=@Year and datepart(Month,[inv_InvoiceDate])=@Month and [inv_DocumentKind]=@DocKind";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                com.Parameters.Add("@Month", SqlDbType.Int).Value = Month;
                com.Parameters.Add("@DocKind", SqlDbType.Int).Value = DocKind;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (int)reader[0];
                };
                reader.Close();
            }
            return result;
        }
        public static int GetMaxMonthlyQuotation(string DB, int Year, int Month)
        {
            int result = 1;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top 100 percent isnull(max([sal_MonthlyNo])+1,1) from [SalesDocument_Quotation] where datepart(Year,[sal_InvoiceDate])=@Year and datepart(Month,[sal_InvoiceDate])=@Month ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                com.Parameters.Add("@Month", SqlDbType.Int).Value = Month;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (int)reader[0];
                };
                reader.Close();
            }
            return result;
        }
        public static int GetMaxMonthlyProforma(string DB, int Year, int Month)
        {
            int result = 1;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top 100 percent isnull(max([sal_MonthlyNo])+1,1) from [SalesDocument_Proforma] where datepart(Year,[sal_InvoiceDate])=@Year and datepart(Month,[sal_InvoiceDate])=@Month ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                com.Parameters.Add("@Month", SqlDbType.Int).Value = Month;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (int)reader[0];
                };
                reader.Close();
            }
            return result;
        }
        public static int GetMaxProforma(string DB)
        {
            int result = 1;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top 100 percent isnull(max([sal_InvoiceNo])+1,1) from [SalesDocument_Proforma] ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (int)reader[0];
                };
                reader.Close();
            }
            return result;
        }
        public static int GetMaxQuotation(string DB)
        {
            int result = 1;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top 100 percent isnull(max([sal_InvoiceNo])+1,1) from [SalesDocument_Quotation] ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (int)reader[0];
                };
                reader.Close();
            }
            return result;
        }
        public static int GetMaxContract(string DB)
        {
            int result = 1;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top 100 percent isnull(max([sal_InvoiceNo])+1,1) from [SalesDocument_Contract] ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (int)reader[0];
                };
                reader.Close();
            }
            return result;
        }
        public static int UpdateInvoiceNo(string DB, int DocumentKind, Guid? OperationKey)
        {
            int result = 1;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "EXEC dbo.spfinOperation_UpdateInvoiceNo @DocKind,@OperationKey ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@DocKind", SqlDbType.Int).Value = DocumentKind;
                com.Parameters.Add("@OperationKey", SqlDbType.UniqueIdentifier).Value = OperationKey;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (int)reader[0];
                };
                reader.Close();
            }
            return result;
        }
        public static int UpdateInvoiceMonthlyNo(string DB, int Year, int Month, int DocumentKind, Guid? OperationKey)
        {
            int result = 1;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "EXEC dbo.spfinOperation_UpdateInvoiceMonthlyNo @DocKind,@Year,@Month,@OperationKey ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@DocKind", SqlDbType.Int).Value = DocumentKind;
                com.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                com.Parameters.Add("@Month", SqlDbType.Int).Value = Month;
                com.Parameters.Add("@OperationKey", SqlDbType.UniqueIdentifier).Value = OperationKey;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (int)reader[0];
                };
                reader.Close();
            }
            return result;
        }

        public static int UpdateVoucherNo(string DB, int DocumentKind, int Year, Guid? OperationKey)
        {
            int result = 1;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "EXEC dbo.spfinOperation_UpdateVoucherNo @DocKind,@Year,@OperationKey ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@DocKind", SqlDbType.Int).Value = DocumentKind;
                com.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                com.Parameters.Add("@OperationKey", SqlDbType.UniqueIdentifier).Value = OperationKey;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (int)reader[0];
                };
                reader.Close();
            }
            return result;
        }
        public static int UpdateVouchermonthlyNo(string DB, int Year, int Month, int DocumentKind, Guid? OperationKey)
        {
            int result = 1;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "EXEC dbo.spfinOperation_UpdateVoucherMonthlyNo @DocKind,@Year,@Month,@OperationKey ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@DocKind", SqlDbType.Int).Value = DocumentKind;
                com.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                com.Parameters.Add("@Month", SqlDbType.Int).Value = Month;
                com.Parameters.Add("@OperationKey", SqlDbType.UniqueIdentifier).Value = OperationKey;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (int)reader[0];
                };
                reader.Close();
            }
            return result;
        }

        //
        public static string DefaultCurrency(string DB)
        {
            string Res = "";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "select top(1) [com_Currency] as [Code] from [com_DefaultSettings]";
                SqlCommand command = new SqlCommand();
                command = new SqlCommand();
                command.Connection = con;
                command.CommandType = CommandType.Text;
                command.CommandText = delQuery;
                command.Parameters.Clear();
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Res = (string)reader[0];
                }
                reader.Close();
            }
            return Res;
        }
        public static int MaxTransaction(string DB, int DocumentKind, int Year)
        {
            DateTime first = new DateTime(Year, 1, 1);
            DateTime last = new DateTime(Year, 12, 31);
            int result = 1;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top 100 percent isnull(max([Fin_VoucherNo])+1,1) from [finDocument_Transaction] where ([Fin_VoucherDate]>=@First and [Fin_VoucherDate]<=@Last) and [Fin_DocumentKind]=@DocumentKind";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@DocumentKind", SqlDbType.Int).Value = DocumentKind;
                com.Parameters.Add("@First", SqlDbType.Date).Value = first;
                com.Parameters.Add("@Last", SqlDbType.Date).Value = last;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (int)reader[0];
                };
                reader.Close();
            }
            return result;
        }

        public static string AssetsAccountCode(string DB, Guid? Key, string Prefix)
        {
            string Res = "";
            if (Key == null)
                return Res;
            int max = 1001;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "select top 100 PERCENT dbo.sfnFixedAssets_FixtureCode(@Key) ";
                SqlCommand command = new SqlCommand();
                command = new SqlCommand();
                command.Connection = con;
                command.CommandType = CommandType.Text;
                command.CommandText = delQuery;
                command.Parameters.Clear();
                command.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    max = (int)reader[0];
                }
                reader.Close();
            }
            string spr = "-";
            if (max == 0)
                max = 1;
            if (max < 2)
            {
                Res = Prefix + spr + "1000" + max.ToString();
            }
            else
            {
                Res = Prefix + spr + max.ToString();
            }
            return Res;
        }
        public static string InventoryAccountCode(string DB, Guid? Key, string Prefix)
        {
            string Res = "";
            if (Key == null)
                return Res;
            int max = 1001;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "select top 100 PERCENT dbo.sfninvCard_ItemCode(@Key) ";
                SqlCommand command = new SqlCommand();
                command = new SqlCommand();
                command.Connection = con;
                command.CommandType = CommandType.Text;
                command.CommandText = delQuery;
                command.Parameters.Clear();
                command.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    max = (int)reader[0];
                }
                reader.Close();
            }
            string spr = "-";
            if (max == 0)
                max = 1;
            if (max < 2)
            {
                Res = Prefix + spr + "1000" + max.ToString();
            }
            else
            {
                Res = Prefix + spr + max.ToString();
            }
            return Res;
        }
        public static void UpdateTransactionStatus(string DB, Guid Key, int Status)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                ScStr.Clear();
                ScStr.Append(" update [finDocument_Transaction] set Fin_Status=@Status where [Fin_OperationKey]=@Key ");
                ScStr.Append(" update [invDocument_Transaction] set inv_Status=@Status where [inv_OperationKey]=@Key ");
                SqlCommand ScCom = new SqlCommand();
                ScCom = new SqlCommand();
                ScCom.Connection = con;
                ScCom.CommandType = CommandType.Text;
                ScCom.CommandText = ScStr.ToString();
                ScCom.Parameters.Clear();
                ScCom.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                ScCom.Parameters.Add("@Status", SqlDbType.Int).Value = Status;
                con.Open();
                ScCom.ExecuteNonQuery();
            }
        }
        public static void UpdateQuotationStatus(string DB, Guid Key, int Status)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                ScStr.Clear();
                ScStr.Append("update [SalesDocument_Quotation] set sal_Status=@Status where [sal_OperationKey]=@Key ");

                SqlCommand ScCom = new SqlCommand();
                ScCom = new SqlCommand();
                ScCom.Connection = con;
                ScCom.CommandType = CommandType.Text;
                ScCom.CommandText = ScStr.ToString();
                ScCom.Parameters.Clear();
                ScCom.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                ScCom.Parameters.Add("@Status", SqlDbType.Int).Value = Status;
                con.Open();
                ScCom.ExecuteNonQuery();
            }
        }

        public static bool DeleteFinancialTransaction(string DB,Guid Key)
        {
            using (SqlConnection connection = new SqlConnection(iCore.GetCon(DB)))
            {
                connection.Open();
                SqlCommand comm = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("RQTransaction");
                comm.Connection = connection;
                comm.Transaction = transaction;
                System.Text.StringBuilder str = new System.Text.StringBuilder();

                str.Clear();
                str.Append("exec dbo.spfinDocument_DeleteTransaction @opk");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@opk", SqlDbType.UniqueIdentifier).Value = Key;
                comm.ExecuteNonQuery();
                try
                {
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
            return true;
        }

        public static bool DeleteInventoryTransaction(string DB, Guid Key)
        {
            using (SqlConnection connection = new SqlConnection(iCore.GetCon(DB)))
            {
                connection.Open();
                SqlCommand comm = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("RQTransaction");
                comm.Connection = connection;
                comm.Transaction = transaction;
                System.Text.StringBuilder str = new System.Text.StringBuilder();

                str.Clear();
                str.Append("exec dbo.spinvDocument_DeleteTransaction @opk");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@opk", SqlDbType.UniqueIdentifier).Value = Key;
                comm.ExecuteNonQuery();
                try
                {
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
            return true;
        }
    }
}
