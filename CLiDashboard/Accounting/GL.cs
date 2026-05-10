using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiDashboard.Accounting
{
    public class GL
    {
        public int Transaction { get; set; }
        public int Opening { get; set; }
        public int Total { get; set; }
        public GL GetList(string DB)
        {

            GL item = new GL();
            string selQuery = "select top 100 percent * from dbo.fnDashAccounting_GLTotal()  ";
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
                    item.Transaction = Convert.ToInt32(reader["Transaction"]);
                    item.Opening = Convert.ToInt32(reader["Opening"]);
                    item.Total = Convert.ToInt32(reader["Total"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
