using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiTracking.Selections
{
    public class OpenQuotations
    {
        public Guid? Key { get; set; }
        public Guid? Client { get; set; }
        public int No { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int Remaining { get; set; }
        public string ClientName1 { get; set; }
        public string ClientName2 { get; set; }
        public static List<OpenQuotations> GetList(string DB)
        {
            List<OpenQuotations> items = new List<OpenQuotations>();
            string selQuery = "SELECT TOP 100 PERCENT * from dbo.fnTracking_OpenQuotations() order by [InvoiceDate],[No] ";
               
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
                    OpenQuotations item = new OpenQuotations();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Client = iCore.IsDbNullRtNull(reader["Client"]);
                    item.No = Convert.ToInt32(reader["No"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.Remaining = Convert.ToInt32(reader["Remaining"]);
                    item.ClientName1 = Convert.ToString(reader["ClientName1"]);
                    item.ClientName2 = Convert.ToString(reader["ClientName2"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
