using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiTracking.Operation
{
    public class AssignTasks
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public int Status { get; set; }
        public int Branch { get; set; }
        public Guid? Prefix { get; set; }
        public int OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }
        public string ReferenceNo { get; set; }
        public Guid? Client { get; set; }
        public int Vehicles { get; set; }
        public string Description { get; set; }
        public Guid? Invoice { get; set; }
        public int Handeled { get; set; }
        public int Remaining { get; set; }
        public List<AssignTasks> GetList(string DB)
        {
            List<AssignTasks> items = new List<AssignTasks>();
            string selQuery = "select top 100 percent * from dbo.fnTracking_AssignOrders() order by [OrderDate]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AssignTasks item = new AssignTasks();
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Branch = Convert.ToInt32(reader["Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["Prefix"]);
                    item.OrderNo = Convert.ToInt32(reader["OrderNo"]);
                    item.OrderDate = iCore.IsDbNullRtNullDate(reader["OrderDate"]);
                    item.ReferenceNo = Convert.ToString(reader["ReferenceNo"]);
                    item.Client = iCore.IsDbNullRtNull(reader["Client"]);
                    item.Vehicles = Convert.ToInt32(reader["Vehicles"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Invoice = iCore.IsDbNullRtNull(reader["Invoice"]);
                    item.Handeled = Convert.ToInt32(reader["Handeled"]);
                    item.Remaining = item.Vehicles - item.Handeled;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
