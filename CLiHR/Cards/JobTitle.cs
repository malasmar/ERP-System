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
    public class JobTitle
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public int Qualification { get; set; }
        public Boolean Disable { get; set; }
        public List<JobTitle> GetList(string DB)
        {
            List<JobTitle> items = new List<JobTitle>();
            string selQuery = "select top 100 percent * from HRCard_JobTitle order by [job_No] ";
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
                    JobTitle item = new JobTitle();
                    item.Key = iCore.IsDbNullRtNull(reader["job_Key"]);
                    item.No = Convert.ToInt32(reader["job_No"]);
                    item.Name1 = Convert.ToString(reader["job_Name1"]);
                    item.Name2 = Convert.ToString(reader["job_Name2"]);
                    item.Qualification = Convert.ToInt32(reader["job_Qualification"]);
                    item.Disable = Convert.ToBoolean(reader["job_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public JobTitle GetItem(string DB,Guid? Key)
        {
            JobTitle item = new JobTitle();
            item.No = MaxNo(DB);
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from HRCard_JobTitle where [job_Key]=@Key";
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
                    item.Key = iCore.IsDbNullRtNull(reader["job_Key"]);
                    item.No = Convert.ToInt32(reader["job_No"]);
                    item.Name1 = Convert.ToString(reader["job_Name1"]);
                    item.Name2 = Convert.ToString(reader["job_Name2"]);
                    item.Qualification = Convert.ToInt32(reader["job_Qualification"]);
                    item.Disable = Convert.ToBoolean(reader["job_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, JobTitle item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO HRCard_JobTitle");
                str.Append("([job_No]");
                str.Append(",[job_Name1]");
                str.Append(",[job_Name2]");
                str.Append(",[job_Qualification]");
                str.Append(",[job_Disable])");
                str.Append(" VALUES ");
                str.Append("(@job_No");
                str.Append(",@job_Name1");
                str.Append(",@job_Name2");
                str.Append(",@job_Qualification");
                str.Append(",@job_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@job_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@job_Name1", SqlDbType.NVarChar, 150).Value = item.Name1 ?? "";
                comm.Parameters.Add("@job_Name2", SqlDbType.NVarChar, 150).Value = item.Name2 ?? "";
                comm.Parameters.Add("@job_Qualification", SqlDbType.Int).Value = item.Qualification;
                comm.Parameters.Add("@job_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, JobTitle item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update HRCard_JobTitle SET ");
                str.Append("[job_No]=@job_No");
                str.Append(",[job_Name1]=@job_Name1");
                str.Append(",[job_Name2]=@job_Name2");
                str.Append(",[job_Qualification]=@job_Qualification");
                str.Append(",[job_Disable]=@job_Disable");
                str.Append(" WHERE job_Key=@job_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@job_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@job_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@job_Name1", SqlDbType.NVarChar, 150).Value = item.Name1 ?? "";
                comm.Parameters.Add("@job_Name2", SqlDbType.NVarChar, 150).Value = item.Name2 ?? "";
                comm.Parameters.Add("@job_Qualification", SqlDbType.Int).Value = item.Qualification;
                comm.Parameters.Add("@job_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [HRCard_JobTitle] where [job_Key]=@Key";
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
        public int MaxNo(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([job_No])+1,1) from [HRCard_JobTitle]";
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
    }
}
