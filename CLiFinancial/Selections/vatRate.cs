using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Selections
{
    public class vatRate
    {
        public Guid? Key { get; set; }
        public decimal Rate { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }
        public string xDisplay { get; set; }
        public static List<vatRate> vatRates(string DB, string xLan)
        {
            List<vatRate> items = new List<vatRate>();
            items.Add(new vatRate() { Key = iCore.vatDefault, Rate = 0, Name1 = "No/VAT", Name2 = "No/VAT", xDisplay = "No/VAT", Display = "No/VAT" });
            string selQuery = "select top 100 percent [vat_Key],[vat_Order],[vat_Rate],[vat_Name1],[vat_Name2] from [com_vatRates] order by [vat_Order] ";
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
                    vatRate item = new vatRate();
                    item.Key = iCore.IsDbNullRtNull(reader["vat_Key"]);
                    item.Rate = Convert.ToDecimal(reader["vat_Rate"]);
                    item.Name1 = Convert.ToString(reader["vat_Name1"]);
                    item.Name2 = Convert.ToString(reader["vat_Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.Rate.ToString() + ")";
                            item.xDisplay = (item.Name2 == "" ? item.Name1 : item.Name2);
                            break;
                        case "ar":
                            item.xDisplay = (item.Name1 == "" ? item.Name2 : item.Name1);
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.Rate.ToString() + ")";
                            break;
                        default:
                            item.xDisplay = (item.Name1 == "" ? item.Name2 : item.Name1);
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.Rate.ToString() + ")";
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
