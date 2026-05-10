using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Cards
{
    public class Expenses
    {
        public Guid? Key { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public Guid? Parent { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Guid? Group { get; set; }
        public Guid? Account { get; set; }
        public decimal MonthlyLimit { get; set; }
        public Boolean Disable { get; set; }
        public Boolean Controlled { get; set; }
        public Boolean Distribution { get; set; }
        public List<Expenses> GetList(string DB)
        {
            List<Expenses> items = new List<Expenses>();
            string selQuery = "select top 100 percent * from finCard_Expenses order by [exp_Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Expenses item = new Expenses();
                    item.Key = iCore.IsDbNullRtNull(reader["exp_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["exp_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["exp_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["exp_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["exp_LastupDate"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["exp_Parent"]);
                    item.Code = Convert.ToString(reader["exp_Code"]);
                    item.Name1 = Convert.ToString(reader["exp_Name1"]);
                    item.Name2 = Convert.ToString(reader["exp_Name2"]);
                    item.Group = iCore.IsDbNullRtNull(reader["exp_Group"]);
                    item.Account = iCore.IsDbNullRtNull(reader["exp_Account"]);
                    item.MonthlyLimit = Convert.ToDecimal(reader["exp_MonthlyLimit"]);
                    item.Disable = Convert.ToBoolean(reader["exp_Disable"]);
                    item.Controlled = Convert.ToBoolean(reader["exp_Controlled"]);
                    item.Distribution = Convert.ToBoolean(reader["exp_Distribution"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public Expenses GetItem(string DB, Guid? Key)
        {
            Expenses item = new Expenses();
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from finCard_Expenses where [exp_Key]=@Key";
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
                    item.Key = iCore.IsDbNullRtNull(reader["exp_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["exp_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["exp_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["exp_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["exp_LastupDate"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["exp_Parent"]);
                    item.Code = Convert.ToString(reader["exp_Code"]);
                    item.Name1 = Convert.ToString(reader["exp_Name1"]);
                    item.Name2 = Convert.ToString(reader["exp_Name2"]);
                    item.Group = iCore.IsDbNullRtNull(reader["exp_Group"]);
                    item.Account = iCore.IsDbNullRtNull(reader["exp_Account"]);
                    item.MonthlyLimit = Convert.ToDecimal(reader["exp_MonthlyLimit"]);
                    item.Disable = Convert.ToBoolean(reader["exp_Disable"]);
                    item.Controlled = Convert.ToBoolean(reader["exp_Controlled"]);
                    item.Distribution = Convert.ToBoolean(reader["exp_Distribution"]);
                }
                reader.Close();
            }
            return item;
        }

        public static Guid? Insert(string DB, Expenses item)
        {
            CLiCore.Platx.Account parentdetails = new CLiCore.Platx.Account().GetItem(DB, item.Parent);
            Guid? account;
            if (parentdetails.CloseSubAccounts == true)
            {
                account = parentdetails.Key;
            }
            else
            {
                string code = xConfig.AccountCode(DB, item.Parent);
                account = CLiCore.Platx.Account.Insert(DB, item.Parent, code, item.Name1, item.Name2);
            }

     
           
            Guid? key = Guid.NewGuid();
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO finCard_Expenses ");
                str.Append("([exp_Key]");
                str.Append(",[exp_CreateUser]");
                str.Append(",[exp_CreateDate]");
                str.Append(",[exp_LastupUser]");
                str.Append(",[exp_LastupDate]");
                str.Append(",[exp_Parent]");
                str.Append(",[exp_Code]");
                str.Append(",[exp_Name1]");
                str.Append(",[exp_Name2]");
                str.Append(",[exp_Group]");
                str.Append(",[exp_Account]");
                str.Append(",[exp_MonthlyLimit]");
                str.Append(",[exp_Disable]");
                str.Append(",[exp_Controlled]");
                str.Append(",[exp_Distribution])");
                str.Append(" VALUES ");
                str.Append("(@exp_Key");
                str.Append(",@exp_CreateUser");
                str.Append(",@exp_CreateDate");
                str.Append(",@exp_LastupUser");
                str.Append(",@exp_LastupDate");
                str.Append(",@exp_Parent");
                str.Append(",@exp_Code");
                str.Append(",@exp_Name1");
                str.Append(",@exp_Name2");
                str.Append(",@exp_Group");
                str.Append(",@exp_Account");
                str.Append(",@exp_MonthlyLimit");
                str.Append(",@exp_Disable");
                str.Append(",@exp_Controlled");
                str.Append(",@exp_Distribution)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@exp_Key", SqlDbType.UniqueIdentifier).Value = key;
                comm.Parameters.Add("@exp_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@exp_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@exp_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@exp_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@exp_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Parent);
                comm.Parameters.Add("@exp_Code", SqlDbType.NVarChar, 50).Value = item.Code ?? "";
                comm.Parameters.Add("@exp_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@exp_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@exp_Group", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Group);
                comm.Parameters.Add("@exp_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(account);
                comm.Parameters.Add("@exp_MonthlyLimit", SqlDbType.Decimal).Value = item.MonthlyLimit;
                comm.Parameters.Add("@exp_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@exp_Controlled", SqlDbType.Bit).Value = item.Controlled;
                comm.Parameters.Add("@exp_Distribution", SqlDbType.Bit).Value = item.Distribution;
                con.Open();
                comm.ExecuteNonQuery();
            }
            return key;
        }

        public static Guid? Update(string DB, Expenses item)
        {
            CLiCore.Platx.Account parentdetails = new CLiCore.Platx.Account().GetItem(DB, item.Parent);
            if (parentdetails.CloseSubAccounts == false)
            {
                CLiCore.Platx.Account.Update(DB, item.Account, item.Name1, item.Name2, item.Disable);
            }
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update finCard_Expenses SET ");
                str.Append("[exp_CreateUser]=@exp_CreateUser");
                str.Append(",[exp_CreateDate]=@exp_CreateDate");
                str.Append(",[exp_LastupUser]=@exp_LastupUser");
                str.Append(",[exp_LastupDate]=@exp_LastupDate");
                str.Append(",[exp_Parent]=@exp_Parent");
                str.Append(",[exp_Code]=@exp_Code");
                str.Append(",[exp_Name1]=@exp_Name1");
                str.Append(",[exp_Name2]=@exp_Name2");
                str.Append(",[exp_Group]=@exp_Group");
                str.Append(",[exp_Account]=@exp_Account");
                str.Append(",[exp_MonthlyLimit]=@exp_MonthlyLimit");
                str.Append(",[exp_Disable]=@exp_Disable");
                str.Append(",[exp_Controlled]=@exp_Controlled");
                str.Append(",[exp_Distribution]=@exp_Distribution");
                str.Append(" WHERE exp_Key=@exp_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@exp_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@exp_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@exp_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@exp_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@exp_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@exp_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Parent);
                comm.Parameters.Add("@exp_Code", SqlDbType.NVarChar, 50).Value = item.Code ?? "";
                comm.Parameters.Add("@exp_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@exp_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@exp_Group", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Group);
                comm.Parameters.Add("@exp_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Account);
                comm.Parameters.Add("@exp_MonthlyLimit", SqlDbType.Decimal).Value = item.MonthlyLimit;
                comm.Parameters.Add("@exp_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@exp_Controlled", SqlDbType.Bit).Value = item.Controlled;
                comm.Parameters.Add("@exp_Distribution", SqlDbType.Bit).Value = item.Distribution;
                con.Open();
                comm.ExecuteNonQuery();
            }
            return item.Key;
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "EXEC dbo.spfinDelete_Expenses @Key";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                res = comm.ExecuteNonQuery();
            }
            return res;
        }

    }
}
