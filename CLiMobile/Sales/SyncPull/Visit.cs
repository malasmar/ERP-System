using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiMobile.Sales.SyncPull
{
    public class Visit
    {
        public Guid Key { get; set; }
        public DateTime VisitDate { get; set; }
        public string Client { get; set; }
        public string Person { get; set; }
        public string Phone { get; set; }
        public Guid? Activity { get; set; }
        public int Posetion { get; set; }
        public int Expect { get; set; }
        public string Comment { get; set; }

        public static int Insert(string DB, Visit item,Guid User)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO AppSales_Visit ");
                str.Append("([vis_Key]");
                str.Append(",[vis_Date]");
                str.Append(",[vis_Client]");
                str.Append(",[vis_Person]");
                str.Append(",[vis_Phone]");
                str.Append(",[vis_Activity]");
                str.Append(",[vis_Posetion]");
                str.Append(",[vis_Expect]");
                str.Append(",[vis_Comment]");
                str.Append(",[vis_User])");
                str.Append(" VALUES ");
                str.Append("(@vis_Key");
                str.Append(",@vis_Date");
                str.Append(",@vis_Client");
                str.Append(",@vis_Person");
                str.Append(",@vis_Phone");
                str.Append(",@vis_Activity");
                str.Append(",@vis_Posetion");
                str.Append(",@vis_Expect");
                str.Append(",@vis_Comment");
                str.Append(",@vis_User)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@vis_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@vis_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.VisitDate);
                comm.Parameters.Add("@vis_Client", SqlDbType.NVarChar, 250).Value = item.Client ?? "";
                comm.Parameters.Add("@vis_Person", SqlDbType.NVarChar, 100).Value = item.Person ?? "";
                comm.Parameters.Add("@vis_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@vis_Activity", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Activity);
                comm.Parameters.Add("@vis_Posetion", SqlDbType.Int).Value = item.Posetion;
                comm.Parameters.Add("@vis_Expect", SqlDbType.Int).Value = item.Expect;
                comm.Parameters.Add("@vis_Comment", SqlDbType.NVarChar, 500).Value = item.Comment ?? "";
                comm.Parameters.Add("@vis_User", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(User);
                con.Open();
                res=comm.ExecuteNonQuery();
            }
            return res;
        }
    }
}
