using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiFinancial.FixedAssets.Selections
{
    public class Fixture
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }
        public decimal Percent { get; set; }
        public static List<Fixture> GetList(string DB, string xLan)
        {
            List<Fixture> items = new List<Fixture>();
            string selQuery = "select top 100 percent [fxd_Key],[fxd_Code],[fxd_Name1],[fxd_Name2],[fxd_Percent] from [finFixedAssets_Fixture] order by [fxd_Code] ";
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
                    Fixture item = new Fixture();
                    item.Key = iCore.IsDbNullRtNull(reader["fxd_Key"]);
                    item.Code = Convert.ToString(reader["fxd_Code"]);
                    item.Name1 = Convert.ToString(reader["fxd_Name1"]);
                    item.Name2 = Convert.ToString(reader["fxd_Name2"]);
                    item.Percent = Convert.ToDecimal(reader["fxd_Percent"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.Code.ToString() + ")";
                            break;
                        case "ar":
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.Code.ToString() + ")";
                            break;
                        default:
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.Code.ToString() + ")";
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
