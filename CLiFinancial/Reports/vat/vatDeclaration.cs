using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Reports.vat
{
    public class vatDeclaration
    {
        public Guid? Key { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal Rate { get; set; }
        public decimal AmountSales { get; set; }
        public decimal AmountSalesReturn { get; set; }
        public decimal AmountPurchase { get; set; }
        public decimal AmountPurchaseReturn { get; set; }
        public decimal Sales { get; set; }
        public decimal SalesReturn { get; set; }
        public decimal Purchase { get; set; }
        public decimal PurchaseReturn { get; set; }
        public int Order { get; set; }
        public string Display { get; set; }
        public decimal Balance { get; set; }
        public List<vatDeclaration> GetList(string DB, string xLan, DateTime FirstDate, DateTime LastDate)
        {
            List<vatDeclaration> items = new List<vatDeclaration>();
            string selQuery = "select top 100 percent * from dbo.ReportFin_vatDeclaration(@FirstDate,@LastDate) order by [Order]";
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
                    vatDeclaration item = new vatDeclaration();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Rate = Convert.ToDecimal(reader["Rate"]);
                    item.AmountSales = Convert.ToDecimal(reader["AmountSales"]);
                    item.AmountSalesReturn = Convert.ToDecimal(reader["AmountSalesReturn"]);
                    item.AmountPurchase = Convert.ToDecimal(reader["AmountPurchase"]);
                    item.AmountPurchaseReturn = Convert.ToDecimal(reader["AmountPurchaseReturn"]);
                    item.Sales = Convert.ToDecimal(reader["Sales"]);
                    item.SalesReturn = Convert.ToDecimal(reader["SalesReturn"]);
                    item.Purchase = Convert.ToDecimal(reader["Purchase"]);
                    item.PurchaseReturn = Convert.ToDecimal(reader["PurchaseReturn"]);
                    item.Balance = Convert.ToDecimal(reader["Balance"]);

                    switch (xLan)
                    {
                        case "en":
                            item.Display = item.Name2 == "" ? item.Name1 : item.Name2;
                            break;
                        case "ar":
                            item.Display = item.Name1 == "" ? item.Name2 : item.Name1;
                            break;
                        default:
                            item.Display = item.Name1 == "" ? item.Name2 : item.Name1;
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

    }
}
