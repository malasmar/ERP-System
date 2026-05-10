using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.StructureRequests
{
    public class StrHourLeave
    {
        public Guid? Key { get; set; }
        public Guid? ConfirmationKey { get; set; }
        public bool FinalApproval { get; set; }
        public DateTime? Create { get; set; }
        public string Image { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public int Kind { get; set; }
        public DateTime? LeaveDate { get; set; }
        public TimeSpan? LeaveHour { get; set; }
        public TimeSpan? ReturnHour { get; set; }
        public Boolean EndDay { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Structure { get; set; }
        public List<StrHourLeave> GetList(string DB,Guid? Key)
        {
            List<StrHourLeave> items = new List<StrHourLeave>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_SturctureHourlyLeave(@Key) order by [Create] ";
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
                    StrHourLeave item = new StrHourLeave();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.ConfirmationKey = iCore.IsDbNullRtNull(reader["ConfirmationKey"]);
                    item.FinalApproval = Convert.ToBoolean(reader["FinalApproval"]);
                    item.Create = iCore.IsDbNullRtNullDate(reader["Create"]);
                    item.Image = Convert.ToString(reader["Image"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.LeaveDate = iCore.IsDbNullRtNullDate(reader["LeaveDate"]);
                    item.LeaveHour = iCore.IsDbNullRtNullTime(reader["LeaveHour"]);
                    item.ReturnHour = iCore.IsDbNullRtNullTime(reader["ReturnHour"]);
                    item.EndDay = Convert.ToBoolean(reader["EndDay"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Structure = Convert.ToString(reader["Structure"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
