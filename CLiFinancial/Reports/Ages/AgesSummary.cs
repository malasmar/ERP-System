using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLiCore;
using System.Data;
using System.Data.SqlClient;

namespace CLiFinancial.Reports.Ages
{
    public class AgesSummary
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal Limit { get; set; }
        public decimal Credit { get; set; }
        public decimal Delayed { get; set; }
        public decimal Due { get; set; }
        public decimal Due30 { get; set; }
        public decimal Due60 { get; set; }
        public decimal Due90 { get; set; }
        public List<AgesSummary> GetList(string DB,int DocKind)
        {
            List<AgesSummary> items = new List<AgesSummary>();
            string selQuery = "select top 100 percent * from dbo.ReportFin_AgesSummary(@DocKind)  order by [Code] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@DocKind", SqlDbType.Int).Value = DocKind;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AgesSummary item = new AgesSummary();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Limit = Convert.ToDecimal(reader["Limit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.Delayed = Convert.ToDecimal(reader["Delayed"]);
                    item.Due = Convert.ToDecimal(reader["Due"]);
                    item.Due30 = Convert.ToDecimal(reader["Due30"]);
                    item.Due60 = Convert.ToDecimal(reader["Due60"]);
                    item.Due90 = Convert.ToDecimal(reader["Due90"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
