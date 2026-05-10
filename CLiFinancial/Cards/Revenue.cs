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
    public class Revenue
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
        public Boolean Disable { get; set; }
        public decimal Price { get; set; }
        public Guid? vatKey { get; set; }
        public decimal Total { get; set; }
        public Boolean EnablePoint { get; set; }
        public List<Revenue> GetList(string DB)
        {
            List<Revenue> items = new List<Revenue>();
            string selQuery = "select top 100 percent * from finCard_Revenue order by [rev_Code] ";
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
                    Revenue item = new Revenue();
                    item.Key = iCore.IsDbNullRtNull(reader["rev_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["rev_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["rev_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["rev_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["rev_LastupDate"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["rev_Parent"]);
                    item.Code = Convert.ToString(reader["rev_Code"]);
                    item.Name1 = Convert.ToString(reader["rev_Name1"]);
                    item.Name2 = Convert.ToString(reader["rev_Name2"]);
                    item.Group = iCore.IsDbNullRtNull(reader["rev_Group"]);
                    item.Account = iCore.IsDbNullRtNull(reader["rev_Account"]);
                    item.Disable = Convert.ToBoolean(reader["rev_Disable"]);
                    item.Price = Convert.ToDecimal(reader["rev_Price"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["rev_vatKey"]);
                    item.Total = Convert.ToDecimal(reader["rev_Total"]);
                    item.EnablePoint = Convert.ToBoolean(reader["rev_EnablePoint"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public Revenue GetItem(string DB, Guid? Key)
        {
            Revenue item = new Revenue();
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from finCard_Revenue where [rev_Key]=@Key";
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
                    item.Key = iCore.IsDbNullRtNull(reader["rev_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["rev_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["rev_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["rev_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["rev_LastupDate"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["rev_Parent"]);
                    item.Code = Convert.ToString(reader["rev_Code"]);
                    item.Name1 = Convert.ToString(reader["rev_Name1"]);
                    item.Name2 = Convert.ToString(reader["rev_Name2"]);
                    item.Group = iCore.IsDbNullRtNull(reader["rev_Group"]);
                    item.Account = iCore.IsDbNullRtNull(reader["rev_Account"]);
                    item.Disable = Convert.ToBoolean(reader["rev_Disable"]);
                    item.Price = Convert.ToDecimal(reader["rev_Price"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["rev_vatKey"]);
                    item.Total = Convert.ToDecimal(reader["rev_Total"]);
                    item.EnablePoint = Convert.ToBoolean(reader["rev_EnablePoint"]);
                }
                reader.Close();
            }
            return item;
        }

        public static Guid? Insert(string DB, Revenue item)
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
                str.Append("INSERT INTO finCard_Revenue");
                str.Append("([rev_Key]");
                str.Append(",[rev_CreateUser]");
                str.Append(",[rev_CreateDate]");
                str.Append(",[rev_LastupUser]");
                str.Append(",[rev_LastupDate]");
                str.Append(",[rev_Parent]");
                str.Append(",[rev_Code]");
                str.Append(",[rev_Name1]");
                str.Append(",[rev_Name2]");
                str.Append(",[rev_Group]");
                str.Append(",[rev_Account]");
                str.Append(",[rev_Disable]");
                str.Append(",[rev_Price]");
                str.Append(",[rev_vatKey]");
                str.Append(",[rev_Total]");
                str.Append(",[rev_EnablePoint])");
                str.Append(" VALUES ");
                str.Append("(@rev_Key");
                str.Append(",@rev_CreateUser");
                str.Append(",@rev_CreateDate");
                str.Append(",@rev_LastupUser");
                str.Append(",@rev_LastupDate");
                str.Append(",@rev_Parent");
                str.Append(",@rev_Code");
                str.Append(",@rev_Name1");
                str.Append(",@rev_Name2");
                str.Append(",@rev_Group");
                str.Append(",@rev_Account");
                str.Append(",@rev_Disable");
                str.Append(",@rev_Price");
                str.Append(",@rev_vatKey");
                str.Append(",@rev_Total");
                str.Append(",@rev_EnablePoint)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@rev_Key", SqlDbType.UniqueIdentifier).Value = key;
                comm.Parameters.Add("@rev_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@rev_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@rev_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@rev_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@rev_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Parent);
                comm.Parameters.Add("@rev_Code", SqlDbType.NVarChar, 50).Value = item.Code ?? "";
                comm.Parameters.Add("@rev_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@rev_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@rev_Group", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Group);
                comm.Parameters.Add("@rev_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(account);
                comm.Parameters.Add("@rev_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@rev_Price", SqlDbType.Decimal).Value = item.Price;
                comm.Parameters.Add("@rev_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                comm.Parameters.Add("@rev_Total", SqlDbType.Decimal).Value = item.Total;
                comm.Parameters.Add("@rev_EnablePoint", SqlDbType.Bit).Value = item.EnablePoint;
                con.Open();
                comm.ExecuteNonQuery();
            }
            return key;
        }

        public static Guid? Update(string DB, Revenue item)
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
                str.Append("Update finCard_Revenue SET ");
                str.Append("[rev_CreateUser]=@rev_CreateUser");
                str.Append(",[rev_CreateDate]=@rev_CreateDate");
                str.Append(",[rev_LastupUser]=@rev_LastupUser");
                str.Append(",[rev_LastupDate]=@rev_LastupDate");
                str.Append(",[rev_Parent]=@rev_Parent");
                str.Append(",[rev_Code]=@rev_Code");
                str.Append(",[rev_Name1]=@rev_Name1");
                str.Append(",[rev_Name2]=@rev_Name2");
                str.Append(",[rev_Group]=@rev_Group");
                str.Append(",[rev_Account]=@rev_Account");
                str.Append(",[rev_Disable]=@rev_Disable");
                str.Append(",[rev_Price]=@rev_Price");
                str.Append(",[rev_vatKey]=@rev_vatKey");
                str.Append(",[rev_Total]=@rev_Total");
                str.Append(",[rev_EnablePoint]=@rev_EnablePoint");
                str.Append(" WHERE rev_Key=@rev_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@rev_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@rev_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@rev_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@rev_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@rev_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@rev_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Parent);
                comm.Parameters.Add("@rev_Code", SqlDbType.NVarChar, 50).Value = item.Code ?? "";
                comm.Parameters.Add("@rev_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@rev_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@rev_Group", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Group);
                comm.Parameters.Add("@rev_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Account);
                comm.Parameters.Add("@rev_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@rev_Price", SqlDbType.Decimal).Value = item.Price;
                comm.Parameters.Add("@rev_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                comm.Parameters.Add("@rev_Total", SqlDbType.Decimal).Value = item.Total;
                comm.Parameters.Add("@rev_EnablePoint", SqlDbType.Bit).Value = item.EnablePoint;
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
                string delQuery = "EXEC dbo.spfinDelete_Revenue @Key";
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
