using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.RequestDetails
{
    public class DetailsAnnualLeave
    {
        public Guid? Key { get; set; }
        public DateTime? Create { get; set; }
        public int Kind { get; set; }
        public DateTime? LeaveDate { get; set; }
        public int LeaveDays { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public Boolean AttachmentStatus { get; set; }
        public string FileName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public PreviousApproval PreviousApproval { get; set; }
        public Data.PersonDetails PersonDetails { get; set; }
        public Data.VacationData VacationData { get; set; }
        public DetailsAnnualLeave GetItem(string DB,Guid? Key)
        {
            DetailsAnnualLeave item = new DetailsAnnualLeave();
            string selQuery = "select top 100 percent * from hrRequest_AnnualLeave where [Req_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["Req_Key"]);
                    item.Create = iCore.IsDbNullRtNullDate(reader["Req_Create"]);
                    item.Kind = Convert.ToInt32(reader["Req_Kind"]);
                    item.LeaveDate = iCore.IsDbNullRtNullDate(reader["Req_LeaveDate"]);
                    item.LeaveDays = Convert.ToInt32(reader["Req_LeaveDays"]);
                    item.Description = Convert.ToString(reader["Req_Description"]);
                    item.Status = Convert.ToInt32(reader["Req_Status"]);
                    item.AttachmentStatus = Convert.ToBoolean(reader["Req_AttachmentStatus"]);
                    item.FileName = Convert.ToString(reader["Req_FileName"]);
                    item.Year = Convert.ToInt32(reader["Req_Year"]);
                    item.Month = Convert.ToInt32(reader["Req_Month"]);
                    item.PreviousApproval = new PreviousApproval().GetItem(DB, Key);
                    item.PersonDetails = new Data.PersonDetails().GetItem(DB, iCore.IsDbNullRtNull(reader["req_Employee"]));
                    item.VacationData = new Data.VacationData().GetItem(DB, iCore.IsDbNullRtNull(reader["req_Employee"]));
                }
                reader.Close();
            }
            return item;
        }
    }
}
