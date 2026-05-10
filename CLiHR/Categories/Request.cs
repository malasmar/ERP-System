using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiHR.Categories
{
    public class Request
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Boolean FinancialConfirmation { get; set; }
        public Boolean HRConfirmation { get; set; }
        public Boolean SupervisorConfirmation { get; set; }
        public Boolean ClosedOnBoss { get; set; }
        public Guid? ClosedOnJobTitle { get; set; }
        public Boolean Disable { get; set; }
        public List<Request> GetList(string DB)
        {
            List<Request> items = new List<Request>();
            string selQuery = "select top 100 percent * from hrCard_Categories_Request";
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
                    Request item = new Request();
                    item.Key = iCore.IsDbNullRtNull(reader["creq_Key"]);
                    item.No = Convert.ToInt32(reader["creq_No"]);
                    item.Name1 = Convert.ToString(reader["creq_Name1"]);
                    item.Name2 = Convert.ToString(reader["creq_Name2"]);
                    item.FinancialConfirmation = Convert.ToBoolean(reader["creq_FinancialConfirmation"]);
                    item.HRConfirmation = Convert.ToBoolean(reader["creq_HRConfirmation"]);
                    item.SupervisorConfirmation = Convert.ToBoolean(reader["creq_SupervisorConfirmation"]);
                    item.ClosedOnBoss = Convert.ToBoolean(reader["creq_ClosedOnBoss"]);
                    item.ClosedOnJobTitle = iCore.IsDbNullRtNull(reader["creq_ClosedOnJobTitle"]);
                    item.Disable = Convert.ToBoolean(reader["creq_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public Request GetItem(string DB,Guid? Key)
        {
            Request item = new Request();
            item.No = MaxNo(DB);
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from hrCard_Categories_Request where [creq_Key]=@Key";
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
                    item.Key = iCore.IsDbNullRtNull(reader["creq_Key"]);
                    item.No = Convert.ToInt32(reader["creq_No"]);
                    item.Name1 = Convert.ToString(reader["creq_Name1"]);
                    item.Name2 = Convert.ToString(reader["creq_Name2"]);
                    item.FinancialConfirmation = Convert.ToBoolean(reader["creq_FinancialConfirmation"]);
                    item.HRConfirmation = Convert.ToBoolean(reader["creq_HRConfirmation"]);
                    item.SupervisorConfirmation = Convert.ToBoolean(reader["creq_SupervisorConfirmation"]);
                    item.ClosedOnBoss = Convert.ToBoolean(reader["creq_ClosedOnBoss"]);
                    item.ClosedOnJobTitle = iCore.IsDbNullRtNull(reader["creq_ClosedOnJobTitle"]);
                    item.Disable = Convert.ToBoolean(reader["creq_Disable"]);
                }
                reader.Close();
            }
            return item;
        }
        public int MaxNo(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([creq_No])+1,1) from [hrCard_Categories_Request]";
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
        public static void Insert(string DB, Request item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO hrCard_Categories_Request");
                str.Append("([creq_No]");
                str.Append(",[creq_Name1]");
                str.Append(",[creq_Name2]");
                str.Append(",[creq_FinancialConfirmation]");
                str.Append(",[creq_HRConfirmation]");
                str.Append(",[creq_SupervisorConfirmation]");
                str.Append(",[creq_ClosedOnBoss]");
                str.Append(",[creq_ClosedOnJobTitle]");
                str.Append(",[creq_Disable])");
                str.Append(" VALUES ");
                str.Append("(@creq_No");
                str.Append(",@creq_Name1");
                str.Append(",@creq_Name2");
                str.Append(",@creq_FinancialConfirmation");
                str.Append(",@creq_HRConfirmation");
                str.Append(",@creq_SupervisorConfirmation");
                str.Append(",@creq_ClosedOnBoss");
                str.Append(",@creq_ClosedOnJobTitle");
                str.Append(",@creq_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@creq_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@creq_Name1", SqlDbType.NVarChar, 150).Value = item.Name1 ?? "";
                comm.Parameters.Add("@creq_Name2", SqlDbType.NVarChar, 150).Value = item.Name2 ?? "";
                comm.Parameters.Add("@creq_FinancialConfirmation", SqlDbType.Bit).Value = item.FinancialConfirmation;
                comm.Parameters.Add("@creq_HRConfirmation", SqlDbType.Bit).Value = item.HRConfirmation;
                comm.Parameters.Add("@creq_SupervisorConfirmation", SqlDbType.Bit).Value = item.SupervisorConfirmation;
                comm.Parameters.Add("@creq_ClosedOnBoss", SqlDbType.Bit).Value = item.ClosedOnBoss;
                comm.Parameters.Add("@creq_ClosedOnJobTitle", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ClosedOnJobTitle);
                comm.Parameters.Add("@creq_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public static void Update(string DB, Request item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update hrCard_Categories_Request SET ");
                str.Append("[creq_No]=@creq_No");
                str.Append(",[creq_Name1]=@creq_Name1");
                str.Append(",[creq_Name2]=@creq_Name2");
                str.Append(",[creq_FinancialConfirmation]=@creq_FinancialConfirmation");
                str.Append(",[creq_HRConfirmation]=@creq_HRConfirmation");
                str.Append(",[creq_SupervisorConfirmation]=@creq_SupervisorConfirmation");
                str.Append(",[creq_ClosedOnBoss]=@creq_ClosedOnBoss");
                str.Append(",[creq_ClosedOnJobTitle]=@creq_ClosedOnJobTitle");
                str.Append(",[creq_Disable]=@creq_Disable");
                str.Append(" WHERE creq_Key=@creq_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@creq_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@creq_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@creq_Name1", SqlDbType.NVarChar, 150).Value = item.Name1 ?? "";
                comm.Parameters.Add("@creq_Name2", SqlDbType.NVarChar, 150).Value = item.Name2 ?? "";
                comm.Parameters.Add("@creq_FinancialConfirmation", SqlDbType.Bit).Value = item.FinancialConfirmation;
                comm.Parameters.Add("@creq_HRConfirmation", SqlDbType.Bit).Value = item.HRConfirmation;
                comm.Parameters.Add("@creq_SupervisorConfirmation", SqlDbType.Bit).Value = item.SupervisorConfirmation;
                comm.Parameters.Add("@creq_ClosedOnBoss", SqlDbType.Bit).Value = item.ClosedOnBoss;
                comm.Parameters.Add("@creq_ClosedOnJobTitle", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ClosedOnJobTitle);
                comm.Parameters.Add("@creq_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public static int Delete(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [hrCard_Categories_Request] where [creq_Key]=@Key";
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
    }
}
