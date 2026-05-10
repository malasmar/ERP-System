using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiHR.Cards
{
    public class Biometric
    {
        public Guid? Key { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string IP { get; set; }
        public string SerialNo { get; set; }
        public int Kind { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public List<Biometric> GetList(string DB)
        {
            List<Biometric> items = new List<Biometric>();
            string selQuery = "select top 100 percent * from hrAttendance_Biometric order by [BD_Key]";
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
                    Biometric item = new Biometric();
                    item.Key = iCore.IsDbNullRtNull(reader["BD_Key"]);
                    item.Name1 = Convert.ToString(reader["BD_Name1"]);
                    item.Name2 = Convert.ToString(reader["BD_Name2"]);
                    item.IP = Convert.ToString(reader["BD_IP"]);
                    item.SerialNo = Convert.ToString(reader["BD_SerialNo"]);
                    item.Kind = Convert.ToInt32(reader["BD_Kind"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["BD_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["BD_Project"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public Biometric GetItem(string DB,Guid? Key)
        {
            Biometric item = new Biometric();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from hrAttendance_Biometric where [BD_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["BD_Key"]);
                    item.Name1 = Convert.ToString(reader["BD_Name1"]);
                    item.Name2 = Convert.ToString(reader["BD_Name2"]);
                    item.IP = Convert.ToString(reader["BD_IP"]);
                    item.SerialNo = Convert.ToString(reader["BD_SerialNo"]);
                    item.Kind = Convert.ToInt32(reader["BD_Kind"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["BD_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["BD_Project"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, Biometric item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO hrAttendance_Biometric");
                str.Append("([BD_Name1]");
                str.Append(",[BD_Name2]");
                str.Append(",[BD_IP]");
                str.Append(",[BD_SerialNo]");
                str.Append(",[BD_Kind]");
                str.Append(",[BD_CostCenter]");
                str.Append(",[BD_Project])");
                str.Append(" VALUES ");
                str.Append("(@BD_Name1");
                str.Append(",@BD_Name2");
                str.Append(",@BD_IP");
                str.Append(",@BD_SerialNo");
                str.Append(",@BD_Kind");
                str.Append(",@BD_CostCenter");
                str.Append(",@BD_Project)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@BD_Name1", SqlDbType.NVarChar, 100).Value = item.Name1 ?? "";
                comm.Parameters.Add("@BD_Name2", SqlDbType.NVarChar, 100).Value = item.Name2 ?? "";
                comm.Parameters.Add("@BD_IP", SqlDbType.NVarChar, 200).Value = item.IP ?? "";
                comm.Parameters.Add("@BD_SerialNo", SqlDbType.NVarChar, 100).Value = item.SerialNo ?? "";
                comm.Parameters.Add("@BD_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@BD_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                comm.Parameters.Add("@BD_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, Biometric item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update hrAttendance_Biometric SET ");
                str.Append("[BD_Name1]=@BD_Name1");
                str.Append(",[BD_Name2]=@BD_Name2");
                str.Append(",[BD_IP]=@BD_IP");
                str.Append(",[BD_SerialNo]=@BD_SerialNo");
                str.Append(",[BD_Kind]=@BD_Kind");
                str.Append(",[BD_CostCenter]=@BD_CostCenter");
                str.Append(",[BD_Project]=@BD_Project");
                str.Append(" WHERE BD_Key=@BD_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@BD_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@BD_Name1", SqlDbType.NVarChar, 100).Value = item.Name1 ?? "";
                comm.Parameters.Add("@BD_Name2", SqlDbType.NVarChar, 100).Value = item.Name2 ?? "";
                comm.Parameters.Add("@BD_IP", SqlDbType.NVarChar, 200).Value = item.IP ?? "";
                comm.Parameters.Add("@BD_SerialNo", SqlDbType.NVarChar, 100).Value = item.SerialNo ?? "";
                comm.Parameters.Add("@BD_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@BD_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                comm.Parameters.Add("@BD_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [hrAttendance_Biometric] where [BD_Key]=@Key";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                res=comm.ExecuteNonQuery();
            }
            return res;
        }

    }
}
