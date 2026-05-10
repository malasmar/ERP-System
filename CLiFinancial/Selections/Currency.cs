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
    public class Currency
    {
        public string Code { get; set; }
        public static List<Currency> GetList(string DB)
        {
            List<Currency> items = new List<Currency>();
            string selQuery = "select top 100 percent [com_Currency] as [Code] from [com_DefaultSettings] union all select top 100 percent [cry_Code] as [Code] from [com_Currency]";
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
                    Currency item = new Currency();
                    item.Code = Convert.ToString(reader["Code"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
