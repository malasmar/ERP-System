using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiFinancial.FixedAssets.Reports
{
    public class FixtureSheet
    {
        public int Row { get; set; }
        public Guid? Fixture { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Guid? Category { get; set; }
        public decimal Percent { get; set; }
        public decimal Qty { get; set; }
        public DateTime? Purchase { get; set; }
        public decimal Amount { get; set; }
        public decimal Current { get; set; }
        public decimal Total { get; set; }
        public decimal Depreciation { get; set; }
        public decimal CurrentDepreciation { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public decimal BookValue { get; set; }

        public List<FixtureSheet> DetailsSheet(string DB,DateTime? FirstDate,DateTime? LastDate)
        {
            List<FixtureSheet> items = new List<FixtureSheet>();
            string selQuery = "select top 100 percent * from dbo.ReportFin_FixtureSheetDetails(@FirstDate,@LastDate) order by [Category],[Code],[Purchase] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = FirstDate;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = LastDate;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    FixtureSheet item = new FixtureSheet();
                    item.Row = Convert.ToInt32(reader["Row"]);
                    item.Fixture = iCore.IsDbNullRtNull(reader["Fixture"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Category = iCore.IsDbNullRtNull(reader["Category"]);
                    item.Percent = Convert.ToDecimal(reader["Percent"]);
                    item.Qty = Convert.ToDecimal(reader["Qty"]);
                    item.Purchase = iCore.IsDbNullRtNullDate(reader["Purchase"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.Current = Convert.ToDecimal(reader["Current"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.Depreciation = Convert.ToDecimal(reader["Depreciation"]);
                    item.CurrentDepreciation = Convert.ToDecimal(reader["CurrentDepreciation"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Project"]);
                    item.BookValue =item.Total -item.Depreciation - item.CurrentDepreciation;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<FixtureSheet> SummarySheet(string DB, DateTime? FirstDate, DateTime? LastDate)
        {
            List<FixtureSheet> items = new List<FixtureSheet>();
            string selQuery = "select top 100 percent * from dbo.ReportFin_FixtureSheetSummary(@FirstDate,@LastDate) order by [Category],[Code] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = FirstDate;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = LastDate;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    FixtureSheet item = new FixtureSheet();
                    item.Row = Convert.ToInt32(reader["Row"]);
                    item.Fixture = iCore.IsDbNullRtNull(reader["Fixture"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Category = iCore.IsDbNullRtNull(reader["Category"]);
                
                    item.Qty = Convert.ToDecimal(reader["Qty"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.Current = Convert.ToDecimal(reader["Current"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.Depreciation = Convert.ToDecimal(reader["Depreciation"]);
                    item.CurrentDepreciation = Convert.ToDecimal(reader["CurrentDepreciation"]);
                 
                    item.BookValue = item.Total - item.Depreciation - item.CurrentDepreciation;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

    }
}
