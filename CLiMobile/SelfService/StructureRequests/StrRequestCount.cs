using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.StructureRequests
{
    public class StrRequestCount
    {
        public int Advance { get; set; }
        public int Loan { get; set; }
        public int Annual { get; set; }
        public int Leave { get; set; }
        public int Hourly { get; set; }
        public int Penalty { get; set; }
        public int Reward { get; set; }
        public StrRequestCount GetItem(string DB,Guid? Key)
        {
            StrRequestCount item = new StrRequestCount();
            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_StructureRequestCount(@Key) ";
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
                    item.Advance = Convert.ToInt32(reader["Advance"]);
                    item.Loan = Convert.ToInt32(reader["Loan"]);
                    item.Annual = Convert.ToInt32(reader["Annual"]);
                    item.Leave = Convert.ToInt32(reader["Leave"]);
                    item.Hourly = Convert.ToInt32(reader["Hourly"]);
                    item.Penalty = Convert.ToInt32(reader["Penalty"]);
                    item.Reward = Convert.ToInt32(reader["Reward"]);
                }
                reader.Close();
            }
            return item;
        }

    }
}
