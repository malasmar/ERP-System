using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
namespace CLiDashboard.Purchase
{
    public class Suppliers
    {
        public int Count { get; set; }
        public int Active { get; set; }
        public Suppliers GetList(string DB)
        {
            Suppliers item = new Suppliers();
            string selQuery = "select top 100 percent * from dbo.fnDashPurchase_Suppliers()  ";
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
                    item.Count = Convert.ToInt32(reader["Count"]);
                    item.Active = Convert.ToInt32(reader["Active"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
