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
    public class StrNormalLeave
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
        public int Days { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Structure { get; set; }
        public List<StrNormalLeave> GetList(string DB, Guid? Key)
        {
            List<StrNormalLeave> items = new List<StrNormalLeave>();
            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_SturctureNormalLeave(@Key) order by [Create]";
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
                    StrNormalLeave item = new StrNormalLeave();
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
                    item.Days = Convert.ToInt32(reader["Days"]);
                    item.FileName = Convert.ToString(reader["FileName"]);
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
