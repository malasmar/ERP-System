using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiDashboard.Sales
{
    public class SalesPerson
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal Sales { get; set; }
        public decimal Return { get; set; }
        public decimal Discount { get; set; }
        public string Display { get; set; }
        public List<SalesPerson> GetList(string DB,string xLan,int Year)
        {
            DateTime First = new DateTime(Year, 1, 1);
            DateTime Last = new DateTime(Year, 12, 31);

            List<SalesPerson> items = new List<SalesPerson>();
            string selQuery = "select top(5) * from dbo.fnDashSales_SalesPerson(@FirstDate,@LastDate) order by [Sales] desc ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = First;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = Last;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SalesPerson item = new SalesPerson();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Sales = Convert.ToDecimal(reader["Sales"]);
                    item.Return = Convert.ToDecimal(reader["Return"]);
                    item.Discount = Convert.ToDecimal(reader["Discount"]);
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
