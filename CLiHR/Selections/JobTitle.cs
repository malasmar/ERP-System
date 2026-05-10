using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiHR.Selections
{
    public class JobTitle
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }

        public static List<JobTitle> GetList(string DB, string xLan)
        {
            List<JobTitle> items = new List<JobTitle>();
            string selQuery = "select top 100 percent [job_Key],[job_No],[job_Name1],[job_Name2] from [HRCard_JobTitle] order by [job_No] ";
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
                    JobTitle item = new JobTitle();
                    item.Key = iCore.IsDbNullRtNull(reader["job_Key"]);
                    item.No = Convert.ToInt32(reader["job_No"]);
                    item.Name1 = Convert.ToString(reader["job_Name1"]);
                    item.Name2 = Convert.ToString(reader["job_Name2"]);
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
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
