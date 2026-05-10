using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore.Print
{
    public class Reports
    {
        public static DataTable CompanyProfile(string DB)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top(1) * from dbo.Print_CompanyProfile() ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable AccountDetails(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top(1) * from dbo.fnfinJson_AccountDetails(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }

        public static DataTable ChartAccountStatment(string DB, Guid? Key, DateTime? FirstDate, DateTime? LastDate, bool Opening)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.fnaccReport_AccountStatment(@Key,@FirstDate,@LastDate,@Opening) order by [Date],[No],[DocumentKind],[VoucherNo] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(FirstDate);
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(LastDate);
                com.Parameters.Add("@Opening", SqlDbType.Bit).Value = Opening;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                decimal Balance = 0;
                foreach (DataRow item in Res.Rows)
                {
                    Balance += Convert.ToDecimal(item["Debit"]) - Convert.ToDecimal(item["Credit"]);
                    if (Balance > 0)
                    {
                        item["DebitBalance"] = Balance;
                    }
                    else
                    {
                        item["CreditBalance"] = Math.Abs(Balance); ;
                    }
                }
                return Res;
            }
        }
        public static DataTable AccountStatment(string DB, Guid? Key, DateTime? FirstDate, DateTime? LastDate, bool Opening)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.ReportFin_AccountStatment(@Key,@FirstDate,@LastDate,@Opening) order by [VoucherDate],[DocumentKind],[VoucherNo]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(FirstDate);
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(LastDate);
                com.Parameters.Add("@Opening", SqlDbType.Bit).Value = Opening;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                decimal Balance = 0;
                foreach (DataRow item in Res.Rows)
                {
                    Balance += Convert.ToDecimal(item["Debit"]) - Convert.ToDecimal(item["Credit"]);
                    if (Balance > 0)
                    {
                        item["DebitBalance"] = Balance;
                    }
                    else
                    {
                        item["CreditBalance"] = Math.Abs(Balance); ;
                    }
                }
                return Res;
            }
        }
        public static DataTable SummaryAccountStatment(string DB, Guid? Key, DateTime? FirstDate, DateTime? LastDate, bool Opening)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.ReportFin_AccountStatmentSummary(@Key,@FirstDate,@LastDate,@Opening) order by [VoucherDate],[DocumentKind],[VoucherNo]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(FirstDate);
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(LastDate);
                com.Parameters.Add("@Opening", SqlDbType.Bit).Value = Opening;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                decimal Balance = 0;
                foreach (DataRow item in Res.Rows)
                {
                    Balance += Convert.ToDecimal(item["Debit"]) - Convert.ToDecimal(item["Credit"]);
                    if (Balance > 0)
                    {
                        item["DebitBalance"] = Balance;
                    }
                    else
                    {
                        item["CreditBalance"] = Math.Abs(Balance); ;
                    }
                }
                return Res;
            }
        }
        public static DataTable TrialBalance(string DB, DateTime First, DateTime Last, int Level, bool ExpKind)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.fnaccReports_TrailBalance(@First,@Last,@ExpKind) where [code] is not null and ([Level]=@Level or @Level=0) order by  ExpensesKind,Code";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@First", SqlDbType.Date).Value = First;
                com.Parameters.Add("@Last", SqlDbType.Date).Value = Last;
                com.Parameters.Add("@Level", SqlDbType.Int).Value = Level;
                com.Parameters.Add("@ExpKind", SqlDbType.Bit).Value = ExpKind;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);

                return Res;
            }
        }
        public static DataTable IncomeStatment(string DB, DateTime First, DateTime Last, bool ExpKind)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.fnaccReport_IncomeStatmentv2(@First,@Last,@ExpKind) where [code] is not null  order by  ExpensesKind,Code";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@First", SqlDbType.Date).Value = First;
                com.Parameters.Add("@Last", SqlDbType.Date).Value = Last;
                com.Parameters.Add("@ExpKind", SqlDbType.Bit).Value = ExpKind;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);

                return Res;
            }
        }

        //Account Balance
        public static DataTable CurrentAccountBalance(string DB, DateTime? FirstDate, DateTime? LastDate, string Parents, string Groups)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.ReportFin_BalanceCurrentAccount(@FirstDate,@LastDate,@Parents,@Groups) order by [Parent],[Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(FirstDate);
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(LastDate);
                com.Parameters.Add("@Parents", SqlDbType.NVarChar, -1).Value = Parents ?? "";
                com.Parameters.Add("@Groups", SqlDbType.NVarChar, -1).Value = Groups ?? "";
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable EmployeeBalance(string DB, DateTime? FirstDate, DateTime? LastDate, int Kind)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.ReportFin_BalanceEmployee(@FirstDate,@LastDate,@Kind) order by [Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(FirstDate);
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(LastDate);
                com.Parameters.Add("@Kind", SqlDbType.Int).Value = Kind;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable CashBalance(string DB, DateTime? FirstDate, DateTime? LastDate)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.ReportFin_BalanceCash(@FirstDate,@LastDate) order by [Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(FirstDate);
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(LastDate);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable BankBalance(string DB, DateTime? FirstDate, DateTime? LastDate)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.ReportFin_BalanceBanks(@FirstDate,@LastDate) order by [Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(FirstDate);
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(LastDate);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable ExpenseBalance(string DB, DateTime? FirstDate, DateTime? LastDate)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.ReportFin_BalanceExpense(@FirstDate,@LastDate) order by [Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(FirstDate);
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(LastDate);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable RevenueBalance(string DB, DateTime? FirstDate, DateTime? LastDate)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.ReportFin_BalanceRevenue(@FirstDate,@LastDate) order by [Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(FirstDate);
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(LastDate);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        //Cost Center and Project
        public static DataTable CostCenterDetails(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "SELECT TOP 100 PERCENT [cst_Kind] AS [Kind],[cst_Code] AS [Code],[cst_Name1] AS [Name1],[cst_Name2] AS [Name2] FROM [dbo].[finCard_CostCenter] WHERE cst_Key=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable ProjectDetails(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "SELECT TOP 100 PERCENT 0 AS [Kind],pjt_Code AS [Code],pjt_Name1 AS [Name1],pjt_Name2 AS [Name2] FROM [dbo].finCard_Project  WHERE pjt_Key=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable ProjectBalance(string DB)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.ReportFin_ProjectBalance() order by [Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable ProjectIncomeSummary(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.ReportFin_ProjectIncomeSummary(@Key) order by [Kind] desc,[Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable ProjectStatment(string DB, Guid? Key, DateTime? FirstDate, DateTime? LastDate)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.ReportFin_ProjectStatment(@Key,@FirstDate,@LastDate) order by [VoucherDate],[VoucherNo],[DocumentKind] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(FirstDate);
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(LastDate);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                decimal Balance = 0;
                foreach (DataRow item in Res.Rows)
                {
                    Balance += Convert.ToDecimal(item["Debit"]) - Convert.ToDecimal(item["Credit"]);
                    if (Balance > 0)
                    {
                        item["DebitBalance"] = Balance;
                    }
                    else
                    {
                        item["CreditBalance"] = Math.Abs(Balance); ;
                    }
                }
                return Res;
            }
        }
        public static DataTable CostCenterBalance(string DB)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.ReportFin_CostCenterBalance() order by [Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable CostCenterIncomeSummary(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.ReportFin_CostCenterIncomeSummary(@Key) order by [Kind] desc,[Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable CostCenterStatment(string DB, Guid? Key, DateTime? FirstDate, DateTime? LastDate)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.ReportFin_CostCenterStatment(@Key,@FirstDate,@LastDate) order by [VoucherDate],[VoucherNo],[DocumentKind] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(FirstDate);
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(LastDate);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                decimal Balance = 0;
                foreach (DataRow item in Res.Rows)
                {
                    Balance += Convert.ToDecimal(item["Debit"]) - Convert.ToDecimal(item["Credit"]);
                    if (Balance > 0)
                    {
                        item["DebitBalance"] = Balance;
                    }
                    else
                    {
                        item["CreditBalance"] = Math.Abs(Balance); ;
                    }
                }
                return Res;
            }
        }
        public static DataTable WarehouseValue(string DB, int? Warehouse, DateTime? Date)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.ReportInv_WarehouseBalance(@Warehouse,@Date) order by [code] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Warehouse", SqlDbType.Int).Value = Warehouse;
                com.Parameters.Add("@Date", SqlDbType.Date).Value = Date;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable WarehouseDetails(string DB, int? Warehouse)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.Print_Warehouse(@Warehouse)";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Warehouse", SqlDbType.Int).Value = Warehouse;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
    }
}
