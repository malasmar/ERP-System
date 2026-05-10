using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiMobile.Technical.Sync
{
    public class Vehicles
    {
        public Guid? Key { get; set; }
        public Guid? JobOrder { get; set; }
        public DateTime? OrderDate { get; set; }
        public int OrderNo { get; set; }
        public Guid? Client { get; set; }
        public string ClientName { get; set; }
        public string ClientPerson { get; set; }
        public string ClientPhone { get; set; }
        public string Plate { get; set; }
        public string Arabic { get; set; }
        public string Person { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public DateTime? InstallDate { get; set; }

        public List<Vehicles> GetList(string DB,Guid? Key)
        {
            List<Vehicles> items = new List<Vehicles>();
            string selQuery = "select top 100 percent * from dbo.fnTracking_AssignedVehicles(@Key)";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Vehicles item = new Vehicles();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.JobOrder = iCore.IsDbNullRtNull(reader["JobOrder"]);
                    item.OrderDate = iCore.IsDbNullRtNullDate(reader["OrderDate"]);
                    item.OrderNo = Convert.ToInt32(reader["OrderNo"]);
                    item.Client = iCore.IsDbNullRtNull(reader["Client"]);
                    item.ClientName = Convert.ToString(reader["ClientName"]);
                    item.ClientPerson = Convert.ToString(reader["ClientPerson"]);
                    item.ClientPhone = Convert.ToString(reader["ClientPhone"]);
                    item.Plate = Convert.ToString(reader["Plate"]);
                    item.Arabic = Convert.ToString(reader["Arabic"]);
                    item.Person = Convert.ToString(reader["Person"]);
                    item.Phone = Convert.ToString(reader["Phone"]);
                    item.City = Convert.ToString(reader["City"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
