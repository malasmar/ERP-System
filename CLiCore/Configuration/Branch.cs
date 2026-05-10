using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiCore.Configuration
{
    public class Branch
    {
        public Guid? Key { get; set; }
        public Guid? Prefix { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string CR { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public Boolean Disable { get; set; }
        public List<Branch> GetList(string DB)
        {
            List<Branch> items = new List<Branch>();
            string selQuery = "select top 100 percent * from com_Branch order by [brh_No]";
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
                    Branch item = new Branch();
                    item.Key = iCore.IsDbNullRtNull(reader["brh_Key"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["brh_Prefix"]);
                    item.No = Convert.ToInt32(reader["brh_No"]);
                    item.Name1 = Convert.ToString(reader["brh_Name1"]);
                    item.Name2 = Convert.ToString(reader["brh_Name2"]);
                    item.CR = Convert.ToString(reader["brh_CR"]);
                    item.Phone = Convert.ToString(reader["brh_Phone"]);
                    item.Mobile = Convert.ToString(reader["brh_Mobile"]);
                    item.Fax = Convert.ToString(reader["brh_Fax"]);
                    item.Email = Convert.ToString(reader["brh_Email"]);
                    item.Address = Convert.ToString(reader["brh_Address"]);
                    item.City = Convert.ToString(reader["brh_City"]);
                    item.Disable = Convert.ToBoolean(reader["brh_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public Branch GetItem(string DB,Guid? Key)
        {
            Branch item = new Branch();
            item.No = MaxOrder(DB);
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from com_Branch where [brh_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["brh_Key"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["brh_Prefix"]);
                    item.No = Convert.ToInt32(reader["brh_No"]);
                    item.Name1 = Convert.ToString(reader["brh_Name1"]);
                    item.Name2 = Convert.ToString(reader["brh_Name2"]);
                    item.CR = Convert.ToString(reader["brh_CR"]);
                    item.Phone = Convert.ToString(reader["brh_Phone"]);
                    item.Mobile = Convert.ToString(reader["brh_Mobile"]);
                    item.Fax = Convert.ToString(reader["brh_Fax"]);
                    item.Email = Convert.ToString(reader["brh_Email"]);
                    item.Address = Convert.ToString(reader["brh_Address"]);
                    item.City = Convert.ToString(reader["brh_City"]);
                    item.Disable = Convert.ToBoolean(reader["brh_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, Branch item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO com_Branch");
                str.Append("([brh_Prefix]");
                str.Append(",[brh_No]");
                str.Append(",[brh_Name1]");
                str.Append(",[brh_Name2]");
                str.Append(",[brh_CR]");
                str.Append(",[brh_Phone]");
                str.Append(",[brh_Mobile]");
                str.Append(",[brh_Fax]");
                str.Append(",[brh_Email]");
                str.Append(",[brh_Address]");
                str.Append(",[brh_Disable])");
                str.Append(" VALUES ");
                str.Append("(@brh_Prefix");
                str.Append(",@brh_No");
                str.Append(",@brh_Name1");
                str.Append(",@brh_Name2");
                str.Append(",@brh_CR");
                str.Append(",@brh_Phone");
                str.Append(",@brh_Mobile");
                str.Append(",@brh_Fax");
                str.Append(",@brh_Email");
                str.Append(",@brh_Address");
                str.Append(",@brh_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@brh_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Prefix);
                comm.Parameters.Add("@brh_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@brh_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@brh_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@brh_CR", SqlDbType.NVarChar, 15).Value = item.CR ?? "";
                comm.Parameters.Add("@brh_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@brh_Mobile", SqlDbType.NVarChar, 15).Value = item.Mobile ?? "";
                comm.Parameters.Add("@brh_Fax", SqlDbType.NVarChar, 15).Value = item.Fax ?? "";
                comm.Parameters.Add("@brh_Email", SqlDbType.NVarChar, 200).Value = item.Email ?? "";
                comm.Parameters.Add("@brh_Address", SqlDbType.NVarChar, 255).Value = item.Address ?? "";
                comm.Parameters.Add("@brh_City", SqlDbType.NVarChar, 15).Value = item.City ?? "";
                comm.Parameters.Add("@brh_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, Branch item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update com_Branch SET ");
                str.Append("[brh_Prefix]=@brh_Prefix");
                str.Append(",[brh_No]=@brh_No");
                str.Append(",[brh_Name1]=@brh_Name1");
                str.Append(",[brh_Name2]=@brh_Name2");
                str.Append(",[brh_CR]=@brh_CR");
                str.Append(",[brh_Phone]=@brh_Phone");
                str.Append(",[brh_Mobile]=@brh_Mobile");
                str.Append(",[brh_Fax]=@brh_Fax");
                str.Append(",[brh_Email]=@brh_Email");
                str.Append(",[brh_Address]=@brh_Address");
                str.Append(",[brh_City]=@brh_City");
                str.Append(",[brh_Disable]=@brh_Disable");
                str.Append(" WHERE brh_Key=@brh_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@brh_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@brh_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Prefix);
                comm.Parameters.Add("@brh_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@brh_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@brh_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@brh_CR", SqlDbType.NVarChar, 15).Value = item.CR ?? "";
                comm.Parameters.Add("@brh_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@brh_Mobile", SqlDbType.NVarChar, 15).Value = item.Mobile ?? "";
                comm.Parameters.Add("@brh_Fax", SqlDbType.NVarChar, 15).Value = item.Fax ?? "";
                comm.Parameters.Add("@brh_Email", SqlDbType.NVarChar, 200).Value = item.Email ?? "";
                comm.Parameters.Add("@brh_Address", SqlDbType.NVarChar, 255).Value = item.Address ?? "";
                comm.Parameters.Add("@brh_City", SqlDbType.NVarChar, 15).Value = item.City ?? "";
                comm.Parameters.Add("@brh_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public int MaxOrder(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([brh_No])+1,1) from [com_Branch]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand comm = new SqlCommand();
                comm.CommandText = selQuery;
                comm.CommandType = CommandType.Text;
                comm.Connection = con;
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    res = (int)reader[0];
                }
                reader.Close();
            }
            return res;
        }
        public static int Delete(string DB, Guid? Key)
        {
            int res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                ScStr.Clear();
                ScStr.Append("delete from [com_Branch] where [brh_Key]=@Key ");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = ScStr.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                con.Open();
                res = comm.ExecuteNonQuery();
            }
            return res;
        }

    }
}
