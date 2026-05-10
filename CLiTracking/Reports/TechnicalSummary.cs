using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiTracking.Reports
{
    public class TechnicalSummary
    {
        public Guid? Technical { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Phone { get; set; }
        public int Pending { get; set; }
        public int Handled { get; set; }
        public int Receipted { get; set; }
        public int Installed { get; set; }
        public List<TechnicalSummary> GetList(string DB)
        {
            List<TechnicalSummary> items = new List<TechnicalSummary>();
            string selQuery = "select top 100 percent * from dbo.ReportTracking_TechnicalSummary() order by [Technical]";
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
                    TechnicalSummary item = new TechnicalSummary();
                    item.Technical = iCore.IsDbNullRtNull(reader["Technical"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Phone = Convert.ToString(reader["Phone"]);
                    item.Pending = Convert.ToInt32(reader["Pending"]);
                    item.Handled = Convert.ToInt32(reader["Handled"]);
                    item.Receipted = Convert.ToInt32(reader["Receipted"]);
                    item.Installed = Convert.ToInt32(reader["Installed"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
