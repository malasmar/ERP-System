using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiHR.Request
{
    public class Advance
    {
        public Guid? Key { get; set; }
        public DateTime? Create { get; set; }
        public Guid? Employee { get; set; }
        public Guid? Structure { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public Boolean Payment { get; set; }
        public List<Advance> GetList(string DB,int Status,Guid? Key)
        {
            List<Advance> items = new List<Advance>();
            string selQuery = "select top 100 percent req.*,e.emp_Structure from hrRequest_Advance as req " +
                " left join hrCard_Employee e on req.req_Employee=e.emp_Key " +
                " where (req.req_Status=@Status or @Status=100)  and (e.emp_Structure=@Key or @key is null)";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Status", SqlDbType.Int).Value = Status;
                com.Parameters.Add("@key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Advance item = new Advance();
                    item.Key = iCore.IsDbNullRtNull(reader["req_Key"]);
                    item.Create = iCore.IsDbNullRtNullDate(reader["req_Create"]);
                    item.Employee = iCore.IsDbNullRtNull(reader["req_Employee"]);
                    item.Structure = iCore.IsDbNullRtNull(reader["emp_Structure"]);
                    item.Year = Convert.ToInt32(reader["req_Year"]);
                    item.Month = Convert.ToInt32(reader["req_Month"]);
                    item.Amount = Convert.ToDecimal(reader["req_Amount"]);
                    item.Description = Convert.ToString(reader["req_Description"]);
                    item.Status = Convert.ToInt32(reader["req_Status"]);
                    item.Payment = Convert.ToBoolean(reader["req_Payment"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }


    }
}
