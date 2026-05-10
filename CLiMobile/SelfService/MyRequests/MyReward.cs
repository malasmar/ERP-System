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
    public class MyReward
    {
        public Guid? Key { get; set; }
        public DateTime? Create { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Kind { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Comment { get; set; }
        public string StructureName1 { get; set; }
        public string StructureName2 { get; set; }
        public string EmployeeName1 { get; set; }
        public string EmployeeName2 { get; set; }
        public string SupervisorName1 { get; set; }
        public string SupervisorName2 { get; set; }
        public string PenaltyName1 { get; set; }
        public string PenaltyName2 { get; set; }
        public int ApprovalCont { get; set; }
        public int ApprovalRem { get; set; }

        public List<MyReward> GetList(string DB,Guid? Key)
        {
            List<MyReward> items = new List<MyReward>();
            if (Key == null)
                return items;
            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_MyRequest_Reward(@Key) order by [Create] desc ";
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
                    MyReward item = new MyReward();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Create = iCore.IsDbNullRtNullDate(reader["Create"]);
                    item.Year = Convert.ToInt32(reader["Year"]);
                    item.Month = Convert.ToInt32(reader["Month"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Value = Convert.ToDecimal(reader["Value"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Comment = Convert.ToString(reader["Comment"]);
                    item.StructureName1 = Convert.ToString(reader["StructureName1"]);
                    item.StructureName2 = Convert.ToString(reader["StructureName2"]);
                    item.EmployeeName1 = Convert.ToString(reader["EmployeeName1"]);
                    item.EmployeeName2 = Convert.ToString(reader["EmployeeName2"]);
                    item.SupervisorName1 = Convert.ToString(reader["SupervisorName1"]);
                    item.SupervisorName2 = Convert.ToString(reader["SupervisorName2"]);
                    item.PenaltyName1 = Convert.ToString(reader["PenaltyName1"]);
                    item.PenaltyName2 = Convert.ToString(reader["PenaltyName2"]);
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
