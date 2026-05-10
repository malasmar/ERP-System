using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Reports.Audit
{
    public class WarehousesValues
    {
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal Amount { get; set; }
        public Guid? IntegrationAccount { get; set; }
        public List<WarehousesValues> GetList(string DB, DateTime? Date)
        {
            List<WarehousesValues> items = new List<WarehousesValues>();
            string selQuery = "select top 100 percent * from dbo.ReportInv_WarehousesValue(@Date) order by [No]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Date", SqlDbType.Date).Value = Date;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WarehousesValues item = new WarehousesValues();
                    item.No = Convert.ToInt32(reader["No"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.IntegrationAccount = iCore.IsDbNullRtNull(reader["IntegrationAccount"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

    }
}
