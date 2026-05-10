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
    public class vatKind
    {
        public Guid? Key { get; set; }
        public bool Calculate { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }
        public string xDisplay { get; set; }
        public static List<vatKind> vatRates(string DB, string xLan)
        {
            List<vatKind> items = new List<vatKind>();
 
            string selQuery = "select top 100 percent [vat_Key],[vat_Order],[vat_Calculate],[vat_Name1],[vat_Name2] from [com_vatKind] order by [vat_Order] ";
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
                    vatKind item = new vatKind();
                    item.Key = iCore.IsDbNullRtNull(reader["vat_Key"]);
                    item.Calculate = Convert.ToBoolean(reader["vat_Calculate"]);
                    item.Name1 = Convert.ToString(reader["vat_Name1"]);
                    item.Name2 = Convert.ToString(reader["vat_Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2);
                            item.xDisplay = (item.Name2 == "" ? item.Name1 : item.Name2);
                            break;
                        case "ar":
                            item.xDisplay = (item.Name1 == "" ? item.Name2 : item.Name1);
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1);
                            break;
                        default:
                            item.xDisplay = (item.Name1 == "" ? item.Name2 : item.Name1);
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1);
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
