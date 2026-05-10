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
    public class MyAdvance
    {
        public Guid? Key { get; set; }
        public DateTime? Create { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public Boolean Payment { get; set; }
        public string? Comment { get; set; }
        public string? StructureName1 { get; set; }
        public string? StructureName2 { get; set; }
        public string? EmployeeName1 { get; set; }
        public string? EmployeeName2 { get; set; }
        public int ApprovalCont { get; set; }
        public int ApprovalRem { get; set; }

        public List<MyAdvance> GetList(string DB,Guid? Key)
        {
            List<MyAdvance> items = new List<MyAdvance>();
            if (Key == null)
                return items;
            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_MyRequest_Advance(@Key) order by [Create] desc ";
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
                    MyAdvance item = new MyAdvance();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Create = iCore.IsDbNullRtNullDate(reader["Create"]);
                    item.Year = Convert.ToInt32(reader["year"]);
                    item.Month = Convert.ToInt32(reader["month"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Payment = Convert.ToBoolean(reader["Payment"]);
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
