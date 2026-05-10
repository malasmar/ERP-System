using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiAccounting.Cards
{
    public class Account
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Parent { get; set; }
        public Guid? BalanceItem { get; set; }
        public int Kind { get; set; }
        public int Level { get; set; }
        public Boolean Financial { get; set; }
        public Boolean Source { get; set; }
        public int Category { get; set; }
        public Boolean Disable { get; set; }
        public int Status { get; set; }
        public bool IsParent { get; set; }
        public bool DC { get; set; }
        public bool CloseSubAccounts { get; set; }
        public Account GetItem(string DB, Guid? Key)
        {
            Account item = new Account();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from accCard_Accounts where [acc_Key]=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.Key = iCore.IsDbNullRtNull(reader["acc_Key"]);
                    item.Code = Convert.ToString(reader["acc_Code"]);
                    item.Name1 = Convert.ToString(reader["acc_Name1"]);
                    item.Name2 = Convert.ToString(reader["acc_Name2"]);
                    item.Parent = Convert.ToString(reader["acc_Parent"]);
                    item.BalanceItem = iCore.IsDbNullRtNull(reader["acc_BalanceItem"]);
                    item.Kind = Convert.ToInt32(reader["acc_Kind"]);
                    item.Level = Convert.ToInt32(reader["acc_Level"]);
                    item.Financial = Convert.ToBoolean(reader["acc_Financial"]);
                    item.Source = Convert.ToBoolean(reader["acc_Source"]);
                    item.Category = Convert.ToInt32(reader["acc_Category"]);
                    item.Disable = Convert.ToBoolean(reader["acc_Disable"]);
                    item.Status = Convert.ToInt32(reader["acc_Status"]);
                    item.IsParent = Convert.ToBoolean(reader["acc_IsParent"]);
                    item.DC = Convert.ToBoolean(reader["acc_DC"]);
                    item.CloseSubAccounts = Convert.ToBoolean(reader["acc_CloseSubAccounts"]);
                }
               
                reader.Close();
            }
            return item;
        }
        public Account GetItem(string DB, string Key)
        {
            Account item = new Account();
            if (Key == "")
                return item;

            string selQuery = "select top 100 percent * from accCard_Accounts where [acc_Code]=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.NVarChar, 15).Value = Key ?? "";
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.Key = iCore.IsDbNullRtNull(reader["acc_Key"]);
                    item.Code = Convert.ToString(reader["acc_Code"]);
                    item.Name1 = Convert.ToString(reader["acc_Name1"]);
                    item.Name2 = Convert.ToString(reader["acc_Name2"]);
                    item.Parent = Convert.ToString(reader["acc_Parent"]);
                    item.BalanceItem = iCore.IsDbNullRtNull(reader["acc_BalanceItem"]);
                    item.Kind = Convert.ToInt32(reader["acc_Kind"]);
                    item.Level = Convert.ToInt32(reader["acc_Level"]);
                    item.Financial = Convert.ToBoolean(reader["acc_Financial"]);
                    item.Source = Convert.ToBoolean(reader["acc_Source"]);
                    item.Category = Convert.ToInt32(reader["acc_Category"]);
                    item.Disable = Convert.ToBoolean(reader["acc_Disable"]);
                    item.Status = Convert.ToInt32(reader["acc_Status"]);
                    item.IsParent = Convert.ToBoolean(reader["acc_IsParent"]);
                    item.DC = Convert.ToBoolean(reader["acc_DC"]);
                    item.CloseSubAccounts = Convert.ToBoolean(reader["acc_CloseSubAccounts"]);
                }
                reader.Close();
            }
            return item;
        }
        public static void Insert(string DB, Account item)
        {
            Account account = new Account().GetItem(DB, item.Parent);
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO accCard_Accounts");
                str.Append("([acc_Code]");
                str.Append(",[acc_Name1]");
                str.Append(",[acc_Name2]");
                str.Append(",[acc_Parent]");
                str.Append(",[acc_BalanceItem]");
                str.Append(",[acc_Kind]");
                str.Append(",[acc_Level]");
                str.Append(",[acc_Financial]");
                str.Append(",[acc_Source]");
                str.Append(",[acc_Category]");
                str.Append(",[acc_Disable]");
                str.Append(",[acc_Status]");
                str.Append(",[acc_DC]");
                str.Append(",[acc_CloseSubAccounts]");
                str.Append(",[acc_IsParent])");
                str.Append(" VALUES ");
                str.Append("(@acc_Code");
                str.Append(",@acc_Name1");
                str.Append(",@acc_Name2");
                str.Append(",@acc_Parent");
                str.Append(",@acc_BalanceItem");
                str.Append(",@acc_Kind");
                str.Append(",@acc_Level");
                str.Append(",@acc_Financial");
                str.Append(",@acc_Source");
                str.Append(",@acc_Category");
                str.Append(",@acc_Disable");
                str.Append(",@acc_Status");
                str.Append(",@acc_DC");
                str.Append(",@acc_CloseSubAccounts");
                str.Append(",@acc_IsParent)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@acc_Code", SqlDbType.NVarChar, 15).Value = item.Code ?? "";
                comm.Parameters.Add("@acc_Name1", SqlDbType.NVarChar, 250).Value = item.Name1 ?? "";
                comm.Parameters.Add("@acc_Name2", SqlDbType.NVarChar, 250).Value = item.Name2 ?? "";
                comm.Parameters.Add("@acc_Parent", SqlDbType.NVarChar, 15).Value = item.Parent ?? "";
                comm.Parameters.Add("@acc_BalanceItem", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.BalanceItem);
                comm.Parameters.Add("@acc_Kind", SqlDbType.Int).Value = account.Kind;
                comm.Parameters.Add("@acc_Level", SqlDbType.Int).Value = account.Level + 1;
                comm.Parameters.Add("@acc_Financial", SqlDbType.Bit).Value = item.Financial;
                comm.Parameters.Add("@acc_Source", SqlDbType.Bit).Value = item.Source;
                comm.Parameters.Add("@acc_Category", SqlDbType.Int).Value = item.Category;
                comm.Parameters.Add("@acc_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@acc_Status", SqlDbType.Int).Value = item.Status;
                comm.Parameters.Add("@acc_IsParent", SqlDbType.Bit).Value = item.IsParent;
                comm.Parameters.Add("@acc_DC", SqlDbType.Bit).Value = item.DC;
                comm.Parameters.Add("@acc_CloseSubAccounts", SqlDbType.Bit).Value = item.CloseSubAccounts;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public static void Update(string DB, Account item)
        {
            Account account = new Account().GetItem(DB, item.Parent);
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update accCard_Accounts SET ");
                str.Append("[acc_Code]=@acc_Code");
                str.Append(",[acc_Name1]=@acc_Name1");
                str.Append(",[acc_Name2]=@acc_Name2");
                str.Append(",[acc_Parent]=@acc_Parent");
                str.Append(",[acc_BalanceItem]=@acc_BalanceItem");
                str.Append(",[acc_Kind]=@acc_Kind");
                str.Append(",[acc_Level]=@acc_Level");
                str.Append(",[acc_Financial]=@acc_Financial");
                str.Append(",[acc_Source]=@acc_Source");
                str.Append(",[acc_Category]=@acc_Category");
                str.Append(",[acc_Disable]=@acc_Disable");
                str.Append(",[acc_IsParent]=@acc_IsParent");
                str.Append(",[acc_DC]=@acc_DC");
                str.Append(",[acc_CloseSubAccounts]=@acc_CloseSubAccounts");
                str.Append(" WHERE acc_Key=@acc_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@acc_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@acc_Code", SqlDbType.NVarChar, 15).Value = item.Code ?? "";
                comm.Parameters.Add("@acc_Name1", SqlDbType.NVarChar, 250).Value = item.Name1 ?? "";
                comm.Parameters.Add("@acc_Name2", SqlDbType.NVarChar, 250).Value = item.Name2 ?? "";
                comm.Parameters.Add("@acc_Parent", SqlDbType.NVarChar, 15).Value = item.Parent ?? "";
                comm.Parameters.Add("@acc_BalanceItem", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.BalanceItem);
                comm.Parameters.Add("@acc_Kind", SqlDbType.Int).Value = account.Kind;
                comm.Parameters.Add("@acc_Level", SqlDbType.Int).Value = account.Level + 1;
                comm.Parameters.Add("@acc_Financial", SqlDbType.Bit).Value = item.Financial;
                comm.Parameters.Add("@acc_Source", SqlDbType.Bit).Value = item.Source;
                comm.Parameters.Add("@acc_Category", SqlDbType.Int).Value = item.Category;
                comm.Parameters.Add("@acc_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@acc_IsParent", SqlDbType.Bit).Value = item.IsParent;
                comm.Parameters.Add("@acc_DC", SqlDbType.Bit).Value = item.DC;
                comm.Parameters.Add("@acc_CloseSubAccounts", SqlDbType.Bit).Value = item.CloseSubAccounts;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public static void Update(string DB, ChartofAccounts item)
        {
            Account account = new Account().GetItem(DB, item.Parent);
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update accCard_Accounts SET ");
                str.Append("[acc_Code]=@acc_Code");
                str.Append(",[acc_Name1]=@acc_Name1");
                str.Append(",[acc_Name2]=@acc_Name2");
                str.Append(",[acc_Parent]=@acc_Parent");
                str.Append(",[acc_Kind]=@acc_Kind");
                str.Append(",[acc_Level]=@acc_Level");
                str.Append(",[acc_Financial]=@acc_Financial");
                str.Append(",[acc_Source]=@acc_Source");
                str.Append(",[acc_Category]=@acc_Category");
                str.Append(",[acc_Disable]=@acc_Disable");
                str.Append(",[acc_IsParent]=@acc_IsParent");
                str.Append(",[acc_DC]=@acc_DC");
                str.Append(",[acc_CloseSubAccounts]=@acc_CloseSubAccounts");
                str.Append(" WHERE acc_Key=@acc_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@acc_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@acc_Code", SqlDbType.NVarChar, 15).Value = item.Code ?? "";
                comm.Parameters.Add("@acc_Name1", SqlDbType.NVarChar, 250).Value = item.Name1 ?? "";
                comm.Parameters.Add("@acc_Name2", SqlDbType.NVarChar, 250).Value = item.Name2 ?? "";
                comm.Parameters.Add("@acc_Parent", SqlDbType.NVarChar, 15).Value = item.Parent ?? "";
                comm.Parameters.Add("@acc_Kind", SqlDbType.Int).Value = account.Kind;
                comm.Parameters.Add("@acc_Level", SqlDbType.Int).Value = account.Level + 1;
                comm.Parameters.Add("@acc_Financial", SqlDbType.Bit).Value = item.Financial;
                comm.Parameters.Add("@acc_Source", SqlDbType.Bit).Value = item.Source;
                comm.Parameters.Add("@acc_Category", SqlDbType.Int).Value = item.Category;
                comm.Parameters.Add("@acc_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@acc_IsParent", SqlDbType.Bit).Value = item.isParent;
                comm.Parameters.Add("@acc_CloseSubAccounts", SqlDbType.Bit).Value = item.CloseSubAccounts;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public static void Delete(string DB, Guid? Key)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "EXEC dbo.spfinDelete_Account @Key";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

    }
}
