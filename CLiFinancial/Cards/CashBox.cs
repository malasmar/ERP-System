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
    public class CashBox
    {
        public Guid? Key { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public Guid? Parent { get; set; }
        public int Kind { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Guid? Group { get; set; }
        public Guid? Account { get; set; }
        public Guid? Person { get; set; }
        public Boolean Disable { get; set; }
        public Guid? Prefix { get; set; }
        public List<CashBox> GetList(string DB)
        {
            List<CashBox> items = new List<CashBox>();
            string selQuery = "select top 100 percent * from finCard_CashBox order by [cash_Parent],[cash_Code] ";
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
                    CashBox item = new CashBox();
                    item.Key = iCore.IsDbNullRtNull(reader["cash_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["cash_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["cash_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["cash_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["cash_LastupDate"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["cash_Parent"]);
                    item.Kind = Convert.ToInt32(reader["cash_Kind"]);
                    item.Code = Convert.ToString(reader["cash_Code"]);
                    item.Name1 = Convert.ToString(reader["cash_Name1"]);
                    item.Name2 = Convert.ToString(reader["cash_Name2"]);
                    item.Group = iCore.IsDbNullRtNull(reader["cash_Group"]);
                    item.Account = iCore.IsDbNullRtNull(reader["cash_Account"]);
                    item.Person = iCore.IsDbNullRtNull(reader["cash_Person"]);
                    item.Disable = Convert.ToBoolean(reader["cash_Disable"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["cash_Prefix"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public CashBox GetItem(string DB, Guid? Key)
        {
            CashBox item = new CashBox();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from finCard_CashBox where cash_Key=@Key";
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
                    item.Key = iCore.IsDbNullRtNull(reader["cash_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["cash_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["cash_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["cash_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["cash_LastupDate"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["cash_Parent"]);
                    item.Kind = Convert.ToInt32(reader["cash_Kind"]);
                    item.Code = Convert.ToString(reader["cash_Code"]);
                    item.Name1 = Convert.ToString(reader["cash_Name1"]);
                    item.Name2 = Convert.ToString(reader["cash_Name2"]);
                    item.Group = iCore.IsDbNullRtNull(reader["cash_Group"]);
                    item.Account = iCore.IsDbNullRtNull(reader["cash_Account"]);
                    item.Person = iCore.IsDbNullRtNull(reader["cash_Person"]);
                    item.Disable = Convert.ToBoolean(reader["cash_Disable"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["cash_Prefix"]);
                }
                reader.Close();
            }
            return item;
        }

        public static Guid? Insert(string DB, CashBox item)
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
                str.Append("INSERT INTO finCard_CashBox");
                str.Append("([cash_Key]");
                str.Append(",[cash_CreateUser]");
                str.Append(",[cash_CreateDate]");
                str.Append(",[cash_LastupUser]");
                str.Append(",[cash_LastupDate]");
                str.Append(",[cash_Parent]");
                str.Append(",[cash_Kind]");
                str.Append(",[cash_Code]");
                str.Append(",[cash_Name1]");
                str.Append(",[cash_Name2]");
                str.Append(",[cash_Group]");
                str.Append(",[cash_Account]");
                str.Append(",[cash_Person]");
                str.Append(",[cash_Disable]");
                str.Append(",[cash_Prefix])");
                str.Append(" VALUES ");
                str.Append("(@cash_Key");
                str.Append(",@cash_CreateUser");
                str.Append(",@cash_CreateDate");
                str.Append(",@cash_LastupUser");
                str.Append(",@cash_LastupDate");
                str.Append(",@cash_Parent");
                str.Append(",@cash_Kind");
                str.Append(",@cash_Code");
                str.Append(",@cash_Name1");
                str.Append(",@cash_Name2");
                str.Append(",@cash_Group");
                str.Append(",@cash_Account");
                str.Append(",@cash_Person");
                str.Append(",@cash_Disable");
                str.Append(",@cash_Prefix)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@cash_Key", SqlDbType.UniqueIdentifier).Value = key;
                comm.Parameters.Add("@cash_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@cash_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@cash_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@cash_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@cash_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Parent);
                comm.Parameters.Add("@cash_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@cash_Code", SqlDbType.NVarChar, 100).Value = item.Code ?? "";
                comm.Parameters.Add("@cash_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@cash_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@cash_Group", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Group);
                comm.Parameters.Add("@cash_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(account);
                comm.Parameters.Add("@cash_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Person);
                comm.Parameters.Add("@cash_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@cash_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Prefix);
                con.Open();
                comm.ExecuteNonQuery();
            }
            return key;
        }

        public static Guid? Update(string DB, CashBox item)
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
                str.Append("Update finCard_CashBox SET ");
                str.Append("[cash_CreateUser]=@cash_CreateUser");
                str.Append(",[cash_CreateDate]=@cash_CreateDate");
                str.Append(",[cash_LastupUser]=@cash_LastupUser");
                str.Append(",[cash_LastupDate]=@cash_LastupDate");
                str.Append(",[cash_Parent]=@cash_Parent");
                str.Append(",[cash_Kind]=@cash_Kind");
                str.Append(",[cash_Code]=@cash_Code");
                str.Append(",[cash_Name1]=@cash_Name1");
                str.Append(",[cash_Name2]=@cash_Name2");
                str.Append(",[cash_Group]=@cash_Group");
                str.Append(",[cash_Account]=@cash_Account");
                str.Append(",[cash_Person]=@cash_Person");
                str.Append(",[cash_Disable]=@cash_Disable");
                str.Append(",[cash_Prefix]=@cash_Prefix");
                str.Append(" WHERE cash_Key=@cash_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@cash_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@cash_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@cash_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@cash_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@cash_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@cash_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Parent);
                comm.Parameters.Add("@cash_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@cash_Code", SqlDbType.NVarChar, 100).Value = item.Code ?? "";
                comm.Parameters.Add("@cash_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@cash_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@cash_Group", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Group);
                comm.Parameters.Add("@cash_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Account);
                comm.Parameters.Add("@cash_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Person);
                comm.Parameters.Add("@cash_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@cash_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Prefix);
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
                string delQuery = "EXEC dbo.spfinDelete_CashBox @Key";
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
