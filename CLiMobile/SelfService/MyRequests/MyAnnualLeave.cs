using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.MyRequests
{
    public class MyAnnualLeave
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
        public Boolean ReturnStatus { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string ReturnDescription { get; set; }
        public Boolean ConfirmReturn { get; set; }
        public string Comment { get; set; }
        public string StructureName1 { get; set; }
        public string StructureName2 { get; set; }
        public string EmployeeName1 { get; set; }
        public string EmployeeName2 { get; set; }

        public int ApprovalCont { get; set; }
        public int ApprovalRem { get; set; }
        public List<MyAnnualLeave> GetList(string DB,Guid? Key)
        {
            List<MyAnnualLeave> items = new List<MyAnnualLeave>();
            if (Key == null)
                return items;
            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_MyRequest_AnnualLeave(@Key) order by [Create] desc ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key",SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    MyAnnualLeave item = new MyAnnualLeave();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Create = iCore.IsDbNullRtNullDate(reader["Create"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.LeaveDate = iCore.IsDbNullRtNullDate(reader["LeaveDate"]);
                    item.LeaveDays = Convert.ToInt32(reader["LeaveDays"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.AttachmentStatus = Convert.ToBoolean(reader["AttachmentStatus"]);
                    item.FileName = Convert.ToString(reader["FileName"]);
                    item.Year = Convert.ToInt32(reader["Year"]);
                    item.Month = Convert.ToInt32(reader["Month"]);
                    item.ReturnStatus = Convert.ToBoolean(reader["ReturnStatus"]);
                    item.ReturnDate = iCore.IsDbNullRtNullDate(reader["ReturnDate"]);
                    item.StartDate = iCore.IsDbNullRtNullDate(reader["StartDate"]);
                    item.ReturnDescription = Convert.ToString(reader["ReturnDescription"]);
                    item.ConfirmReturn = Convert.ToBoolean(reader["ConfirmReturn"]);
                    item.Comment = Convert.ToString(reader["Comment"]);
                    item.StructureName1 = Convert.ToString(reader["StructureName1"]);
                    item.StructureName2 = Convert.ToString(reader["StructureName2"]);
                    item.EmployeeName1 = Convert.ToString(reader["EmployeeName1"]);
                    item.EmployeeName2 = Convert.ToString(reader["EmployeeName2"]);
                    item.ApprovalCont = Convert.ToInt32(reader["ApprovalCont"]);
                    item.ApprovalRem = Convert.ToInt32(reader["ApprovalRem"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
