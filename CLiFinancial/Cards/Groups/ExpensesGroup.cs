using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Cards.Groups
{
    public class ExpensesGroup
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Boolean Disable { get; set; }
        public int Relations { get; set; }
        public List<ExpensesGroup> GetList(string DB)
        {
            List<ExpensesGroup> items = new List<ExpensesGroup>();
            string selQuery = "select top 100 percent *,(select top 100 percent count(x.[exp_Group]) from dbo.finCard_Expenses x where x.exp_Group = grp_Key) as [Relations] from finCard_Group_Expenses order by [grp_No] ";
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
                    ExpensesGroup item = new ExpensesGroup();
                    item.Key = iCore.IsDbNullRtNull(reader["grp_Key"]);
                    item.No = Convert.ToInt32(reader["grp_No"]);
                    item.Name1 = Convert.ToString(reader["grp_Name1"]);
                    item.Name2 = Convert.ToString(reader["grp_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["grp_Disable"]);
                    item.Relations = Convert.ToInt32(reader["Relations"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public ExpensesGroup GetItem(string DB,Guid? Key)
        {
            ExpensesGroup item = new ExpensesGroup();
            item.No = MaxNo(DB);
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from finCard_Group_Expenses where [grp_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["grp_Key"]);
                    item.No = Convert.ToInt32(reader["grp_No"]);
                    item.Name1 = Convert.ToString(reader["grp_Name1"]);
                    item.Name2 = Convert.ToString(reader["grp_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["grp_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, ExpensesGroup item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO finCard_Group_Expenses");
                str.Append("([grp_No]");
                str.Append(",[grp_Name1]");
                str.Append(",[grp_Name2]");
                str.Append(",[grp_Disable])");
                str.Append(" VALUES ");
                str.Append("(@grp_No");
                str.Append(",@grp_Name1");
                str.Append(",@grp_Name2");
                str.Append(",@grp_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@grp_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@grp_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@grp_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@grp_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, ExpensesGroup item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update finCard_Group_Expenses SET ");
                str.Append("[grp_No]=@grp_No");
                str.Append(",[grp_Name1]=@grp_Name1");
                str.Append(",[grp_Name2]=@grp_Name2");
                str.Append(",[grp_Disable]=@grp_Disable");
                str.Append(" WHERE grp_Key=@grp_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@grp_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@grp_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@grp_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@grp_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@grp_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res ;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [finCard_Group_Expenses] where [grp_Key]=@Key";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                res= comm.ExecuteNonQuery();
            }
            return res;
        }
        public int MaxNo(string DB)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top 100 percent isnull(max([grp_No])+1,1) from [finCard_Group_Expenses]";
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    res = Convert.ToInt32(reader[0]);
                }
                reader.Close();
            }
            return res;
        }
    }
}
