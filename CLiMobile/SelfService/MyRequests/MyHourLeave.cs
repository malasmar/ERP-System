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
    public class MyHourLeave
    {
        public Guid? Key { get; set; }
        public DateTime? Create { get; set; }
        public int Kind { get; set; }
        public DateTime? LeaveDate { get; set; }
        public TimeSpan? LeaveHour { get; set; }
        public TimeSpan? ReturnHour { get; set; }
        public Boolean EndDay { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Comment { get; set; }
        public string StructureName1 { get; set; }
        public string StructureName2 { get; set; }
        public string EmployeeName1 { get; set; }
        public string EmployeeName2 { get; set; }
        public int ApprovalCont { get; set; }
        public int ApprovalRem { get; set; }

        public List<MyHourLeave> GetList(string DB,Guid? Key)
        {
            List<MyHourLeave> items = new List<MyHourLeave>();
            if (Key == null)
                return items;
            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_MyRequest_HourLeave(@Key) order by [Create] desc ";
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
                    MyHourLeave item = new MyHourLeave();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Create = iCore.IsDbNullRtNullDate(reader["Create"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.LeaveDate = iCore.IsDbNullRtNullDate(reader["LeaveDate"]);
                    item.LeaveHour = iCore.IsDbNullRtNullTime(reader["LeaveHour"]);
                    item.ReturnHour = iCore.IsDbNullRtNullTime(reader["ReturnHour"]);
                    item.EndDay = Convert.ToBoolean(reader["EndDay"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
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
