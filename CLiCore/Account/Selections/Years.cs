using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiCore.Account.Selections
{
    public class Years
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public DateTime? From { get; set; }
        public DateTime? Last { get; set; }
        public Boolean Closed { get; set; }
        public Boolean Disable { get; set; }
        public List<Years> GetList(string DB)
        {
            List<Years> items = new List<Years>();
            string selQuery = "select top 100 percent * from com_FinancialYear order by [fyr_No] ";
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
                    Years item = new Years();
                    item.Key = iCore.IsDbNullRtNull(reader["fyr_Key"]);
                    item.No = Convert.ToInt32(reader["fyr_No"]);
                    item.From = iCore.IsDbNullRtNullDate(reader["fyr_From"]);
                    item.Last = iCore.IsDbNullRtNullDate(reader["fyr_Last"]);
                    item.Closed = Convert.ToBoolean(reader["fyr_Closed"]);
                    item.Disable = Convert.ToBoolean(reader["fyr_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
