using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.JsonData
{
    public class vatDetails
    {
        public Guid? Key { get; set; }
        public decimal Rate { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }
  
        public vatDetails GetItem(string DB, string xLan, Guid? Key)
        {
            vatDetails item = new vatDetails();
            item.Display = "";
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent [vat_Key],[vat_Order],[vat_Rate],[vat_Name1],[vat_Name2] from [com_vatRates] where [vat_Key]=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.Key = iCore.IsDbNullRtNull(reader["vat_Key"]);
                    item.Rate = Convert.ToDecimal(reader["vat_Rate"]);
                    item.Name1 = Convert.ToString(reader["vat_Name1"]);
                    item.Name2 = Convert.ToString(reader["vat_Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2);
                            break;
                        case "ar":
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1);
                            break;
                        default:
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1);
                            break;
                    }
                }
                reader.Close();
            }
            return item;
        }
    }
}
