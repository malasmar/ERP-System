using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiSales.CRM
{
    public class Activity
    {
        public Guid? Key { get; set; }
        public DateTime? Date { get; set; }
        public int Kind { get; set; }
        public string CompanyName { get; set; }
        public int CompanySize { get; set; }
        public Guid? ClientActivity { get; set; }
        public Guid? City { get; set; }
        public string Person { get; set; }
        public string Posetion { get; set; }
        public string Phone { get; set; }
        public Boolean Competitor { get; set; }
        public string CompetitorName { get; set; }
        public int competitorStatus { get; set; }
        public Boolean Quotation { get; set; }
        public int Expect { get; set; }
        public Boolean Contracted { get; set; }
        public string Comment { get; set; }
        public Guid? User { get; set; }

        public List<Activity> GetListByUser(string DB,Guid? UserKey)
        {
            List<Activity> items = new List<Activity>();
            if (UserKey == null)
                return items;

            string selQuery = "select top 100 percent * from AppCRM_Activity where [act_User]=@Key";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = UserKey;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Activity item = new Activity();
                    item.Key = iCore.IsDbNullRtNull(reader["act_Key"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["act_Date"]);
                    item.Kind = Convert.ToInt32(reader["act_Kind"]);
                    item.CompanyName = Convert.ToString(reader["act_CompanyName"]);
                    item.CompanySize = Convert.ToInt32(reader["act_CompanySize"]);
                    item.ClientActivity = iCore.IsDbNullRtNull(reader["act_Activity"]);
                    item.City = iCore.IsDbNullRtNull(reader["act_City"]);
                    item.Person = Convert.ToString(reader["act_Person"]);
                    item.Posetion = Convert.ToString(reader["act_Posetion"]);
                    item.Phone = Convert.ToString(reader["act_Phone"]);
                    item.Competitor = Convert.ToBoolean(reader["act_Competitor"]);
                    item.CompetitorName = Convert.ToString(reader["act_CompetitorName"]);
                    item.competitorStatus = Convert.ToInt32(reader["act_competitorStatus"]);
                    item.Quotation = Convert.ToBoolean(reader["act_Quotation"]);
                    item.Expect = Convert.ToInt32(reader["act_Expect"]);
                    item.Contracted = Convert.ToBoolean(reader["act_Contracted"]);
                    item.Comment = Convert.ToString(reader["act_Comment"]);
                    item.User = iCore.IsDbNullRtNull(reader["act_User"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public Activity GetItem(string DB,Guid? Key)
        {
            Activity item = new Activity();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from AppCRM_Activity where [act_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["act_Key"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["act_Date"]);
                    item.Kind = Convert.ToInt32(reader["act_Kind"]);
                    item.CompanyName = Convert.ToString(reader["act_CompanyName"]);
                    item.CompanySize = Convert.ToInt32(reader["act_CompanySize"]);
                    item.ClientActivity = iCore.IsDbNullRtNull(reader["act_Activity"]);
                    item.City = iCore.IsDbNullRtNull(reader["act_City"]);
                    item.Person = Convert.ToString(reader["act_Person"]);
                    item.Posetion = Convert.ToString(reader["act_Posetion"]);
                    item.Phone = Convert.ToString(reader["act_Phone"]);
                    item.Competitor = Convert.ToBoolean(reader["act_Competitor"]);
                    item.CompetitorName = Convert.ToString(reader["act_CompetitorName"]);
                    item.competitorStatus = Convert.ToInt32(reader["act_competitorStatus"]);
                    item.Quotation = Convert.ToBoolean(reader["act_Quotation"]);
                    item.Expect = Convert.ToInt32(reader["act_Expect"]);
                    item.Contracted = Convert.ToBoolean(reader["act_Contracted"]);
                    item.Comment = Convert.ToString(reader["act_Comment"]);
                    item.User = iCore.IsDbNullRtNull(reader["act_User"]);
                }
                reader.Close();
            }
            return item;
        }
        public static void Insert(string DB, Activity item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO AppCRM_Activity");
                str.Append("([act_Date]");
                str.Append(",[act_Kind]");
                str.Append(",[act_CompanyName]");
                str.Append(",[act_CompanySize]");
                str.Append(",[act_Activity]");
                str.Append(",[act_City]");
                str.Append(",[act_Person]");
                str.Append(",[act_Posetion]");
                str.Append(",[act_Phone]");
                str.Append(",[act_Competitor]");
                str.Append(",[act_CompetitorName]");
                str.Append(",[act_competitorStatus]");
                str.Append(",[act_Quotation]");
                str.Append(",[act_Expect]");
                str.Append(",[act_Contracted]");
                str.Append(",[act_Comment]");
                str.Append(",[act_User])");
                str.Append(" VALUES ");
                str.Append("(@act_Date");
                str.Append(",@act_Kind");
                str.Append(",@act_CompanyName");
                str.Append(",@act_CompanySize");
                str.Append(",@act_Activity");
                str.Append(",@act_City");
                str.Append(",@act_Person");
                str.Append(",@act_Posetion");
                str.Append(",@act_Phone");
                str.Append(",@act_Competitor");
                str.Append(",@act_CompetitorName");
                str.Append(",@act_competitorStatus");
                str.Append(",@act_Quotation");
                str.Append(",@act_Expect");
                str.Append(",@act_Contracted");
                str.Append(",@act_Comment");
                str.Append(",@act_User)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@act_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.Date);
                comm.Parameters.Add("@act_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@act_CompanyName", SqlDbType.NVarChar, 250).Value = item.CompanyName ?? "";
                comm.Parameters.Add("@act_CompanySize", SqlDbType.Int).Value = item.CompanySize;
                comm.Parameters.Add("@act_Activity", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ClientActivity);
                comm.Parameters.Add("@act_City", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.City);
                comm.Parameters.Add("@act_Person", SqlDbType.NVarChar, 100).Value = item.Person ?? "";
                comm.Parameters.Add("@act_Posetion", SqlDbType.NVarChar,100).Value = item.Posetion??"";
                comm.Parameters.Add("@act_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@act_Competitor", SqlDbType.Bit).Value = item.Competitor;
                comm.Parameters.Add("@act_CompetitorName", SqlDbType.NVarChar, 250).Value = item.CompetitorName ?? "";
                comm.Parameters.Add("@act_competitorStatus", SqlDbType.Int).Value = item.competitorStatus;
                comm.Parameters.Add("@act_Quotation", SqlDbType.Bit).Value = item.Quotation;
                comm.Parameters.Add("@act_Expect", SqlDbType.Int).Value = item.Expect;
                comm.Parameters.Add("@act_Contracted", SqlDbType.Bit).Value = item.Contracted;
                comm.Parameters.Add("@act_Comment", SqlDbType.NVarChar, 500).Value = item.Comment ?? "";
                comm.Parameters.Add("@act_User", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.User);
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public static void Update(string DB, Activity item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update AppCRM_Activity SET ");
                str.Append("[act_Date]=@act_Date");
                str.Append(",[act_Kind]=@act_Kind");
                str.Append(",[act_CompanyName]=@act_CompanyName");
                str.Append(",[act_CompanySize]=@act_CompanySize");
                str.Append(",[act_Activity]=@act_Activity");
                str.Append(",[act_City]=@act_City");
                str.Append(",[act_Person]=@act_Person");
                str.Append(",[act_Posetion]=@act_Posetion");
                str.Append(",[act_Phone]=@act_Phone");
                str.Append(",[act_Competitor]=@act_Competitor");
                str.Append(",[act_CompetitorName]=@act_CompetitorName");
                str.Append(",[act_competitorStatus]=@act_competitorStatus");
                str.Append(",[act_Quotation]=@act_Quotation");
                str.Append(",[act_Expect]=@act_Expect");
                str.Append(",[act_Contracted]=@act_Contracted");
                str.Append(",[act_Comment]=@act_Comment");
                str.Append(",[act_User]=@act_User");
                str.Append(" WHERE act_Key=@act_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@act_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@act_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.Date);
                comm.Parameters.Add("@act_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@act_CompanyName", SqlDbType.NVarChar, 250).Value = item.CompanyName ?? "";
                comm.Parameters.Add("@act_CompanySize", SqlDbType.Int).Value = item.CompanySize;
                comm.Parameters.Add("@act_Activity", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ClientActivity);
                comm.Parameters.Add("@act_City", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.City);
                comm.Parameters.Add("@act_Person", SqlDbType.NVarChar, 100).Value = item.Person ?? "";
                comm.Parameters.Add("@act_Posetion", SqlDbType.NVarChar,100).Value = item.Posetion??"";
                comm.Parameters.Add("@act_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@act_Competitor", SqlDbType.Bit).Value = item.Competitor;
                comm.Parameters.Add("@act_CompetitorName", SqlDbType.NVarChar, 250).Value = item.CompetitorName ?? "";
                comm.Parameters.Add("@act_competitorStatus", SqlDbType.Int).Value = item.competitorStatus;
                comm.Parameters.Add("@act_Quotation", SqlDbType.Bit).Value = item.Quotation;
                comm.Parameters.Add("@act_Expect", SqlDbType.Int).Value = item.Expect;
                comm.Parameters.Add("@act_Contracted", SqlDbType.Bit).Value = item.Contracted;
                comm.Parameters.Add("@act_Comment", SqlDbType.NVarChar, 500).Value = item.Comment ?? "";
                comm.Parameters.Add("@act_User", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.User);
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public static int Delete(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [AppCRM_Activity] where [act_Key]=@Key";
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
