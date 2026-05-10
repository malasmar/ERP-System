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
    public class PREmployees
    {
        public int Order { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Image { get; set; }
        public string StructureName1 { get; set; }
        public string StructureName2 { get; set; }
        public string JobTitleName1 { get; set; }
        public string JobTitleName2 { get; set; }
        public List<PREmployees> GetPenaltyDetails(string DB,Guid? Key)
        {
            List<PREmployees> items = new List<PREmployees>();
            if (Key == null)
                return items;
            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_PenaltyEmployees(@Key) order by [Order] ";
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
                    PREmployees item = new PREmployees();
                    item.Order = Convert.ToInt32(reader["Order"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Image = Convert.ToString(reader["Image"]);
                    item.StructureName1 = Convert.ToString(reader["StructureName1"]);
                    item.StructureName2 = Convert.ToString(reader["StructureName2"]);
                    item.JobTitleName1 = Convert.ToString(reader["JobTitleName1"]);
                    item.JobTitleName2 = Convert.ToString(reader["JobTitleName2"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<PREmployees> GetRewardDetails(string DB, Guid? Key)
        {
            List<PREmployees> items = new List<PREmployees>();
            if (Key == null)
                return items;
            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_RewardEmployees(@Key) order by [Order] ";
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
                    PREmployees item = new PREmployees();
                    item.Order = Convert.ToInt32(reader["Order"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Image = Convert.ToString(reader["Image"]);
                    item.StructureName1 = Convert.ToString(reader["StructureName1"]);
                    item.StructureName2 = Convert.ToString(reader["StructureName2"]);
                    item.JobTitleName1 = Convert.ToString(reader["JobTitleName1"]);
                    item.JobTitleName2 = Convert.ToString(reader["JobTitleName2"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
