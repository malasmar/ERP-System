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
    public class QuotationItems
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }
        public List<QuotationItems> GetList(string DB,string xLan,Guid? Key)
        {
            List<QuotationItems> items = new List<QuotationItems>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fnTracking_QuotationItems(@Key) order by [Code] ";
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
                    QuotationItems item = new QuotationItems();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = item.Name2 == "" ? item.Name1 : item.Name2 ;
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
