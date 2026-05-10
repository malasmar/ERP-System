using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.RequestDetails
{
    public class DetailsReward
    {
        public Guid? Key { get; set; }
        public DateTime? Create { get; set; }
        public string RewardName1 { get; set; }
        public string RewardName2 { get; set; }
        public string SupervisorCode { get; set; }
        public string SupervisorName1 { get; set; }
        public string SupervisorName2 { get; set; }
        public string SupervisorPhone { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Kind { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Structure { get; set; }
        public int Persons { get; set; }
        public PreviousApproval PreviousApproval { get; set; }

        public DetailsReward GetItem(string DB, Guid? Key)
        {
            DetailsReward item = new DetailsReward();
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_RequestDetailsReward(@Key) ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Create = iCore.IsDbNullRtNullDate(reader["Create"]);
                    item.RewardName1 = Convert.ToString(reader["RewardName1"]);
                    item.RewardName2 = Convert.ToString(reader["RewardName2"]);
                    item.SupervisorCode = Convert.ToString(reader["SupervisorCode"]);
                    item.SupervisorName1 = Convert.ToString(reader["SupervisorName1"]);
                    item.SupervisorName2 = Convert.ToString(reader["SupervisorName2"]);
                    item.SupervisorPhone = Convert.ToString(reader["SupervisorPhone"]);
                    item.Year = Convert.ToInt32(reader["Year"]);
                    item.Month = Convert.ToInt32(reader["Month"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Value = Convert.ToDecimal(reader["Value"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Structure = Convert.ToString(reader["Structure"]);
                    item.Persons = Convert.ToInt32(reader["Persons"]);
                    item.PreviousApproval = new PreviousApproval().GetItem(DB, Key);
                }
                reader.Close();
            }
            return item;
        }

    }
}
