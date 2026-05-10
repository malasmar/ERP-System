using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Reports.AccountBalance
{
    public class CurrentAccount
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal Opening { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public decimal Total { get; set; }
        public Guid? Parent { get; set; }
        public Guid? Group { get; set; }
        public int AccountType { get; set; }
        public List<CurrentAccount> CurrentBalance(string DB, DateTime? FirstDate, DateTime? LastDate,string Parents,string Groups,Guid? UserKey=null)
        {

            List<CurrentAccount> items = new List<CurrentAccount>();
            Guid? UserPrefix = null;
            if (UserKey != null)
            {
                if (xConfig.UserPrefixFilter(UserKey) == true)
                {
                    UserPrefix = xConfig.UserPrefix(UserKey);
                }
            }
            string selQuery = "select top 100 percent * from dbo.ReportFin_BalanceCurrentAccount(@FirstDate,@LastDate,@Parents,@Groups)  where ([Prefix]=@Prefix or @Prefix is null)  order by [Parent],[Code]";
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
                com.Parameters.Add("@Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(UserPrefix);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    CurrentAccount item = new CurrentAccount();
                    item.Key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Opening = Convert.ToDecimal(reader["Opening"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.Balance = item.Debit - item.Credit;
                    item.Total = item.Opening + (item.Debit - item.Credit);
                    item.Parent = iCore.IsDbNullRtNull(reader["Parent"]);
                    item.Group = iCore.IsDbNullRtNull(reader["Group"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<CurrentAccount> CashBalance(string DB, DateTime? FirstDate, DateTime? LastDate)
        {

            List<CurrentAccount> items = new List<CurrentAccount>();

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
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    CurrentAccount item = new CurrentAccount();
                    item.Key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Opening = Convert.ToDecimal(reader["Opening"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.Balance = item.Debit - item.Credit;
                    item.Total = item.Opening + (item.Debit - item.Credit);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<CurrentAccount> BankBalance(string DB, DateTime? FirstDate, DateTime? LastDate)
        {

            List<CurrentAccount> items = new List<CurrentAccount>();

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
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    CurrentAccount item = new CurrentAccount();
                    item.Key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Opening = Convert.ToDecimal(reader["Opening"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.Balance = item.Debit - item.Credit;
                    item.Total = item.Opening + (item.Debit - item.Credit);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<CurrentAccount> ExpenseBalance(string DB, DateTime? FirstDate, DateTime? LastDate)
        {

            List<CurrentAccount> items = new List<CurrentAccount>();

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
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    CurrentAccount item = new CurrentAccount();
                    item.Key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Opening = Convert.ToDecimal(reader["Opening"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.Balance = item.Debit - item.Credit;
                    item.Total = item.Opening + (item.Debit - item.Credit);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<CurrentAccount> RevenueBalance(string DB, DateTime? FirstDate, DateTime? LastDate)
        {

            List<CurrentAccount> items = new List<CurrentAccount>();

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
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    CurrentAccount item = new CurrentAccount();
                    item.Key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Opening = Convert.ToDecimal(reader["Opening"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.Balance = item.Debit - item.Credit;
                    item.Total = item.Opening + (item.Debit - item.Credit);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<CurrentAccount> FixtureBalance(string DB, DateTime? FirstDate, DateTime? LastDate, string Kind)
        {

            List<CurrentAccount> items = new List<CurrentAccount>();

            string selQuery = "select top 100 percent * from dbo.ReportFin_BalanceFixture(@FirstDate,@LastDate,@Kind) order by [Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(FirstDate);
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(LastDate);
                com.Parameters.Add("@Kind", SqlDbType.NVarChar, 25).Value = Kind ?? "";
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    CurrentAccount item = new CurrentAccount();
                    item.Key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Opening = Convert.ToDecimal(reader["Opening"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.Balance = item.Debit - item.Credit;
                    item.Total = item.Opening + (item.Debit - item.Credit);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<CurrentAccount> EmployeeBalance(string DB, DateTime? FirstDate, DateTime? LastDate, int Kind)
        {

            List<CurrentAccount> items = new List<CurrentAccount>();

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
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    CurrentAccount item = new CurrentAccount();
                    item.Key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Opening = Convert.ToDecimal(reader["Opening"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.AccountType = Convert.ToInt32(reader["type"]);
                    item.Balance = item.Debit - item.Credit;
                    item.Total = item.Opening + (item.Debit - item.Credit);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
