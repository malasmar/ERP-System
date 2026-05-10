using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.Attendance
{
    public class StructureAttendance
    {

        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public string JobTitleName1 { get; set; }
        public string JobTitleName2 { get; set; }
        public string StructureName1 { get; set; }
        public string StructureName2 { get; set; }
        public int AttendanceStatus { get; set; }

        public List<StructureAttendance> GetList(string DB,Guid? Key)
        {
            List<StructureAttendance> items = new List<StructureAttendance>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_Att_Structure(@Key,@Date) order by [Code] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                com.Parameters.Add("@Date", SqlDbType.Date).Value = DateTime.UtcNow.AddMinutes(180);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    StructureAttendance item = new StructureAttendance();
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Phone = Convert.ToString(reader["Phone"]);
                    item.Image = Convert.ToString(reader["Image"]);
                    item.JobTitleName1 = Convert.ToString(reader["JobTitleName1"]);
                    item.JobTitleName2 = Convert.ToString(reader["JobTitleName2"]);
                    item.StructureName1 = Convert.ToString(reader["StructureName1"]);
                    item.StructureName2 = Convert.ToString(reader["StructureName2"]);
                    item.AttendanceStatus = Convert.ToInt32(reader["AttendanceStatus"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
