using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiAccounting.Reports
{
    public class TrialBalance
    {
        public int Row { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Parent { get; set; }
        public int Level { get; set; }
        public int Kind { get; set; }
        public int T { get; set; }
        public decimal oDebit { get; set; }
        public decimal oCredit { get; set; }
        public decimal oBalance { get; set; }
        public decimal tDebit { get; set; }
        public decimal tCredit { get; set; }
        public decimal tBalance { get; set; }
        public decimal tbDebit { get; set; }
        public decimal tbCredit { get; set; }
        public decimal Balance { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public bool DC { get; set; }
        public bool Has_Items { get; set; }
        public Guid? Key { get; set; }
        public Guid? ParentKey { get; set; }
        public int ExpensesKind { get; set; }


        public List<TrialBalance> GetTreeList(string DB, DateTime First, DateTime Last,bool ExpKind,bool HideZero)
        {
            List<TrialBalance> items = new List<TrialBalance>();
            string selQuery = "select top 100 percent * from dbo.fnaccReports_TrailBalanceTree(@First,@Last,@ExpKind) where [code] is not null  and ((oDebit+oCredit+tDebit+tCredit+Debit+Credit) <> 0 OR @HideZero=1)    order by ExpensesKind,Code";
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
                com.Parameters.Add("@HideZero", SqlDbType.Bit).Value = HideZero;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    TrialBalance item = new TrialBalance();
                  
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Parent = Convert.ToString(reader["parent"]);
                    item.Level = Convert.ToInt32(reader["Level"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.T = Convert.ToInt32(reader["T"]);
                    item.oDebit = Convert.ToDecimal(reader["oDebit"]);
                    item.oCredit = Convert.ToDecimal(reader["oCredit"]);
                    item.oBalance = Convert.ToDecimal(reader["oBalance"]);
                    item.tDebit = Convert.ToDecimal(reader["tDebit"]);
                    item.tCredit = Convert.ToDecimal(reader["tCredit"]);
                    item.tBalance = Convert.ToDecimal(reader["tBalance"]);
                    item.tbDebit = Convert.ToDecimal(reader["tbDebit"]);
                    item.tbCredit = Convert.ToDecimal(reader["tbCredit"]);
                    item.Balance = Convert.ToDecimal(reader["Balance"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.DC = Convert.ToBoolean(reader["DC"]);
                    item.Has_Items = item.Level == 1 ? true : false;
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.ParentKey = iCore.IsDbNullRtNull(reader["ParentKey"]);
                    item.ExpensesKind = Convert.ToInt32(reader["ExpensesKind"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public List<TrialBalance> GetList(string DB, DateTime First, DateTime Last, int Level,bool ExpKind, bool HideZero)
        {
            List<TrialBalance> items = new List<TrialBalance>();
            string selQuery = "select top 100 percent * from dbo.fnaccReports_TrailBalance(@First,@Last,@ExpKind) where [code] is not null and ([Level]=@Level or @Level=0) and ((oDebit+oCredit+tDebit+tCredit+Debit+Credit) <> 0 OR @HideZero=1) order by ExpensesKind,Code";
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
                com.Parameters.Add("@HideZero", SqlDbType.Bit).Value = HideZero;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    TrialBalance item = new TrialBalance();
                    item.Row = Convert.ToInt32(reader["Row"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Parent = Convert.ToString(reader["parent"]);
                    item.Level = Convert.ToInt32(reader["Level"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.T = Convert.ToInt32(reader["T"]);
                    item.oDebit = Convert.ToDecimal(reader["oDebit"]);
                    item.oCredit = Convert.ToDecimal(reader["oCredit"]);
                    item.oBalance = Convert.ToDecimal(reader["oBalance"]);
                    item.tDebit = Convert.ToDecimal(reader["tDebit"]);
                    item.tCredit = Convert.ToDecimal(reader["tCredit"]);
                    item.tBalance = Convert.ToDecimal(reader["tBalance"]);
                    item.tbDebit = Convert.ToDecimal(reader["tbDebit"]);
                    item.tbCredit = Convert.ToDecimal(reader["tbCredit"]);
                    item.Balance = Convert.ToDecimal(reader["Balance"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.DC = Convert.ToBoolean(reader["DC"]);
                    item.ExpensesKind = Convert.ToInt32(reader["ExpensesKind"]);

                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<TrialBalance> TransactionLevel(string DB, DateTime First, DateTime Last)
        {
            List<TrialBalance> items = new List<TrialBalance>();
            string selQuery = "select top 100 percent * from dbo.fnaccReports_TrailBalance_TL(@First,@Last) where [code] is not null order by Kind,Code,Level";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@First", SqlDbType.Date).Value = First;
                com.Parameters.Add("@Last", SqlDbType.Date).Value = Last;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    TrialBalance item = new TrialBalance();
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Parent = Convert.ToString(reader["parent"]);
                    item.Level = Convert.ToInt32(reader["Level"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.T = Convert.ToInt32(reader["T"]);
                    item.oDebit = Convert.ToDecimal(reader["oDebit"]);
                    item.oCredit = Convert.ToDecimal(reader["oCredit"]);
                    item.oBalance = Convert.ToDecimal(reader["oBalance"]);
                    item.tDebit = Convert.ToDecimal(reader["tDebit"]);
                    item.tCredit = Convert.ToDecimal(reader["tCredit"]);
                    item.tBalance = Convert.ToDecimal(reader["tBalance"]);
                    item.tbDebit = Convert.ToDecimal(reader["tbDebit"]);
                    item.tbCredit = Convert.ToDecimal(reader["tbCredit"]);
                    item.Balance = Convert.ToDecimal(reader["Balance"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.DC = Convert.ToBoolean(reader["DC"]);
                    item.Has_Items = item.Level == 1 ? true : false;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<TrialBalance> InteractiveTrial(string DB, DateTime First, DateTime Last)
        {
            List<TrialBalance> items = new List<TrialBalance>();
            string selQuery = "select top 100 percent * from dbo.fnaccReports_TrailBalanceFull(@First,@Last) where [code] is not null order by Kind,Code,Level";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@First", SqlDbType.Date).Value = First;
                com.Parameters.Add("@Last", SqlDbType.Date).Value = Last;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    TrialBalance item = new TrialBalance();
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Parent = Convert.ToString(reader["parent"]);
                    item.Level = Convert.ToInt32(reader["Level"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.T = Convert.ToInt32(reader["T"]);
                    item.oDebit = Convert.ToDecimal(reader["oDebit"]);
                    item.oCredit = Convert.ToDecimal(reader["oCredit"]);
                    item.oBalance = Convert.ToDecimal(reader["oBalance"]);
                    item.tDebit = Convert.ToDecimal(reader["tDebit"]);
                    item.tCredit = Convert.ToDecimal(reader["tCredit"]);
                    item.tBalance = Convert.ToDecimal(reader["tBalance"]);
                    item.tbDebit = Convert.ToDecimal(reader["tbDebit"]);
                    item.tbCredit = Convert.ToDecimal(reader["tbCredit"]);
                    item.Balance = Convert.ToDecimal(reader["Balance"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.DC = Convert.ToBoolean(reader["DC"]);
                    item.Has_Items = item.Level == 1 ? true : false;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
