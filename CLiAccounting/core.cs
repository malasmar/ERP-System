using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
using System.Reflection.PortableExecutable;

namespace CLiAccounting
{
    public class core
    {
        public static CLiCore.OperationResult UpdateGeneralLedger(string DB, Documents.GeneralLedger Header, List<Documents.GeneralLedgerDetails> Details, bool IsNew)
        {
            Guid? opk;
            int GLNo;
            if (IsNew == false)
            {
                GLNo = Header.No;
                opk = Header.OperationKey;
            }
            else
            {
                opk = Guid.NewGuid();
                GLNo = core.MaxGeneralLedger(DB, Header.Date.Value);
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

                comm.CommandText = "delete from [accDocument_GeneralLedger] where  [gl_OperationKey]=@Key ";
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = opk;
                comm.ExecuteNonQuery();

                str.Clear();
                //Header Data
                str.Append("INSERT INTO accDocument_GeneralLedger");
                str.Append("([gl_OperationKey]");
                str.Append(",[gl_CreateDate]");
                str.Append(",[gl_DocumentKind]");
                str.Append(",[gl_No]");
                str.Append(",[gl_Date]");
                str.Append(",[GL_Description]");
                str.Append(",[gl_Total])");
                str.Append(" VALUES ");
                str.Append("(@gl_OperationKey");
                str.Append(",@gl_CreateDate");
                str.Append(",@gl_DocumentKind");
                str.Append(",@gl_No");
                str.Append(",@gl_Date");
                str.Append(",@GL_Description");
                str.Append(",@gl_Total)");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@gl_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                comm.Parameters.Add("@gl_CreateDate", SqlDbType.DateTime).Value = DateTime.Now;
                comm.Parameters.Add("@gl_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                comm.Parameters.Add("@gl_No", SqlDbType.Int).Value = GLNo;
                comm.Parameters.Add("@gl_Date", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.Date);
                comm.Parameters.Add("@GL_Description", SqlDbType.NVarChar, 1000).Value = Header.Description ?? "";
                comm.Parameters.Add("@gl_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Debit);
                comm.ExecuteNonQuery();

                int i;
                i = 0;
                foreach (Documents.GeneralLedgerDetails item in Details)
                {
                    str.Clear();
                    str.Append("INSERT INTO accDocument_GeneralLedgerDetails");
                    str.Append("([gl_OperationKey]");
                    str.Append(",[gl_Index]");
                    str.Append(",[gl_Account]");
                    str.Append(",[gl_Description]");
                    str.Append(",[gl_Debit]");
                    str.Append(",[gl_Credit]");
                    str.Append(",[gl_Project])");
                    str.Append(" VALUES ");
                    str.Append("(@gl_OperationKey");
                    str.Append(",@gl_Index");
                    str.Append(",@gl_Account");
                    str.Append(",@gl_Description");
                    str.Append(",@gl_Debit");
                    str.Append(",@gl_Credit");
                    str.Append(",@gl_Project)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@gl_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@gl_Index", SqlDbType.Int).Value = i;
                    comm.Parameters.Add("@gl_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Account);
                    comm.Parameters.Add("@gl_Description", SqlDbType.NVarChar, 1000).Value = item.Description ?? "";
                    comm.Parameters.Add("@gl_Debit", SqlDbType.Decimal).Value = item.Debit;
                    comm.Parameters.Add("@gl_Credit", SqlDbType.Decimal).Value = item.Credit;
                    comm.Parameters.Add("@gl_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                    comm.Parameters.Add("@gl_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                    comm.ExecuteNonQuery();
                    ++i;
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
            res.VoucherNo = GLNo;
            return res;
        }
        public static bool CloseFinancialYear(string DB, int Year, string Description)
        {
            Guid? opk = Guid.NewGuid();
            DateTime date = new DateTime(Year, 12, 31);
            List<CLiAccounting.Audit.CloseFinancialYear> data = new CLiAccounting.Audit.CloseFinancialYear().GetList(DB, Year);
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


                str.Clear();
                //Header Data
                str.Append("INSERT INTO accDocument_GeneralLedger");
                str.Append("([gl_OperationKey]");
                str.Append(",[gl_CreateDate]");
                str.Append(",[gl_DocumentKind]");
                str.Append(",[gl_No]");
                str.Append(",[gl_Date]");
                str.Append(",[GL_Description]");
                str.Append(",[gl_Total])");
                str.Append(" VALUES ");
                str.Append("(@gl_OperationKey");
                str.Append(",@gl_CreateDate");
                str.Append(",@gl_DocumentKind");
                str.Append(",@gl_No");
                str.Append(",@gl_Date");
                str.Append(",@GL_Description");
                str.Append(",@gl_Total)");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@gl_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                comm.Parameters.Add("@gl_CreateDate", SqlDbType.DateTime).Value = DateTime.Now;
                comm.Parameters.Add("@gl_DocumentKind", SqlDbType.Int).Value = 3;
                comm.Parameters.Add("@gl_No", SqlDbType.Int).Value = 0;
                comm.Parameters.Add("@gl_Date", SqlDbType.Date).Value = iCore.IsNullRtDbNull(date);
                comm.Parameters.Add("@GL_Description", SqlDbType.NVarChar, 1000).Value = Description ?? "";
                comm.Parameters.Add("@gl_Total", SqlDbType.Decimal).Value = data.Sum(x => x.Debit);
                comm.ExecuteNonQuery();

                int i;
                i = 0;
                foreach (CLiAccounting.Audit.CloseFinancialYear item in data)
                {
                    str.Clear();
                    str.Append("INSERT INTO accDocument_GeneralLedgerDetails");
                    str.Append("([gl_OperationKey]");
                    str.Append(",[gl_Index]");
                    str.Append(",[gl_Account]");
                    str.Append(",[gl_Description]");
                    str.Append(",[gl_Debit]");
                    str.Append(",[gl_Credit]");
                    str.Append(",[gl_Project])");
                    str.Append(" VALUES ");
                    str.Append("(@gl_OperationKey");
                    str.Append(",@gl_Index");
                    str.Append(",@gl_Account");
                    str.Append(",@gl_Description");
                    str.Append(",@gl_Debit");
                    str.Append(",@gl_Credit");
                    str.Append(",@gl_Project)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@gl_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@gl_Index", SqlDbType.Int).Value = i;
                    comm.Parameters.Add("@gl_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                    comm.Parameters.Add("@gl_Description", SqlDbType.NVarChar, 1000).Value = Description ?? "";
                    comm.Parameters.Add("@gl_Debit", SqlDbType.Decimal).Value = item.Debit;
                    comm.Parameters.Add("@gl_Credit", SqlDbType.Decimal).Value = item.Credit;
                    comm.Parameters.Add("@gl_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@gl_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.ExecuteNonQuery();
                    ++i;
                }

                ++i;
                decimal total = data.Sum(x => x.Debit - x.Credit);
                decimal debit = 0, credit = 0;
                if (total >= 0)
                    credit = total;
                else debit = total;

                str.Clear();
                str.Append("INSERT INTO accDocument_GeneralLedgerDetails");
                str.Append("([gl_OperationKey]");
                str.Append(",[gl_Index]");
                str.Append(",[gl_Account]");
                str.Append(",[gl_Description]");
                str.Append(",[gl_Debit]");
                str.Append(",[gl_Credit]");
                str.Append(",[gl_Project])");
                str.Append(" VALUES ");
                str.Append("(@gl_OperationKey");
                str.Append(",@gl_Index");
                str.Append(",@gl_Account");
                str.Append(",@gl_Description");
                str.Append(",@gl_Debit");
                str.Append(",@gl_Credit");
                str.Append(",@gl_Project)");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@gl_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                comm.Parameters.Add("@gl_Index", SqlDbType.Int).Value = i;
                comm.Parameters.Add("@gl_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(CLiCore.xConfig.CloseFinancialYear(DB));
                comm.Parameters.Add("@gl_Description", SqlDbType.NVarChar, 1000).Value = Description ?? "";
                comm.Parameters.Add("@gl_Debit", SqlDbType.Decimal).Value = debit;
                comm.Parameters.Add("@gl_Credit", SqlDbType.Decimal).Value = credit;
                comm.Parameters.Add("@gl_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                comm.Parameters.Add("@gl_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                comm.ExecuteNonQuery();

                str.Clear();
                str.Append("UPDATE com_FinancialYear SET fyr_Closed=1 WHERE fyr_No=@Year");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                comm.ExecuteNonQuery();

                try
                {
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public static string AccountCode(string DB, string Parent)
        {
            string Res = "";
            if (Parent == "")
                return Res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "select top 100 PERCENT dbo.sfnaccCard_GetCode(@Parent) ";
                SqlCommand command = new SqlCommand();
                command = new SqlCommand();
                command.Connection = con;
                command.CommandType = CommandType.Text;
                command.CommandText = delQuery;
                command.Parameters.Clear();
                command.Parameters.Add("@Parent", SqlDbType.NVarChar).Value = Parent ?? "";
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
        public static string BalanceItemCode(string DB, string Parent)
        {
            string Res = "";
            if (Parent == "")
                return Res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "select top 100 PERCENT dbo.sfnaccCard_GetBalanceItemCode(@Parent) ";
                SqlCommand command = new SqlCommand();
                command = new SqlCommand();
                command.Connection = con;
                command.CommandType = CommandType.Text;
                command.CommandText = delQuery;
                command.Parameters.Clear();
                command.Parameters.Add("@Parent", SqlDbType.NVarChar).Value = Parent ?? "";
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
        public static int MaxGeneralLedger(string DB, DateTime Date)
        {
            DateTime first = new DateTime(Date.Year, 1, 1);
            DateTime last = new DateTime(Date.Year, 12, 31);
            int Res;
            Res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "SELECT TOP 100 PERCENT isnull(max([gl_No])+1,1) FROM [dbo].[accDocument_GeneralLedger] where gl_Date>=@First and gl_Date<=@Last ";
                SqlCommand command = new SqlCommand();
                command = new SqlCommand();
                command.Connection = con;
                command.CommandType = CommandType.Text;
                command.CommandText = delQuery;
                command.Parameters.Clear();
                command.Parameters.Add("@First", SqlDbType.Date).Value = first;
                command.Parameters.Add("@Last", SqlDbType.Date).Value = last;
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Res = Convert.ToInt32(reader[0]);
                }
                reader.Close();
            }
            return Res;
        }
        public static decimal NetIncome(string DB, DateTime FirstDate, DateTime LastDate)
        {

            decimal Res;
            Res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "SELECT TOP 100 PERCENT isnull(SUM(d.gl_Debit-d.gl_Credit),0.00) " +
                    " FROM accDocument_GeneralLedgerDetails d " +
                    " LEFT JOIN accDocument_GeneralLedger h " +
                    " ON d.gl_OperationKey = h.gl_OperationKey " +
                    " LEFT JOIN accCard_Accounts ac " +
                    " ON d.gl_Account = ac.acc_Key " +
                    " WHERE ac.acc_Kind IN (3,4) " +
                    " AND h.gl_DocumentKind <> 3 " +
                    " AND h.gl_Date >=@FirstDate " +
                    " AND h.gl_Date<=@LastDate ";
                SqlCommand command = new SqlCommand();
                command = new SqlCommand();
                command.Connection = con;
                command.CommandType = CommandType.Text;
                command.CommandText = delQuery;
                command.Parameters.Clear();
                command.Parameters.Add("@FirstDate", SqlDbType.Date).Value = FirstDate;
                command.Parameters.Add("@LastDate", SqlDbType.Date).Value = LastDate;
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Res = Convert.ToDecimal(reader[0]);
                }
                reader.Close();
            }
            return Res;
        }

        public static void PostVoucher(string DB, Guid? Key)
        {
            if (Key == null)
                return;

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

                comm.CommandText = "exec dbo.spAccOperation_PostTransaction @Key ";
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                comm.ExecuteNonQuery();
                try
                {
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }
        public static int CountGLTransaction(string DB, Guid? Key)
        {
            int Res = 0;
            if (Key == null)
                return Res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "SELECT TOP 100 PERCENT COUNT(*) FROM accDocument_GeneralLedgerDetails WHERE GL_Account = @Key ";
                SqlCommand command = new SqlCommand();
                command.Connection = con;
                command.CommandType = CommandType.Text;
                command.CommandText = delQuery;
                command.Parameters.Clear();
                command.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Res = (int)reader[0];
                }
                reader.Close();
            }
            return Res;
        }
        public static void TransferGLTransaction(string DB, Guid? Source, Guid? Target)
        {
            using (SqlConnection connection = new SqlConnection(iCore.GetCon(DB)))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                // Start a local transaction.
                transaction = connection.BeginTransaction("RQTransaction");
                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;
                System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                ScStr.Clear();
                ScStr.Append("UPDATE accDocument_GeneralLedgerDetails set GL_Account=@Target where GL_Account=@Source ");
                command.CommandType = CommandType.Text;
                command.CommandText = ScStr.ToString();
                command.Parameters.Clear();
                command.Parameters.Add("@Source", SqlDbType.UniqueIdentifier).Value = Source;
                command.Parameters.Add("@Target", SqlDbType.UniqueIdentifier).Value = Target; ;
                command.ExecuteNonQuery();

                try
                {
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }
    }
}
