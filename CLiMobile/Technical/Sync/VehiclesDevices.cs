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
    public class VehiclesDevices
    {
        public Guid? Key { get; set; }
        public Guid? Vehicle { get; set; }
        public Guid? Order { get; set; }
        public Guid? ItemKey { get; set; }
        public string ItemName1 { get; set; }
        public string ItemName2 { get; set; }
        public string SerialNo { get; set; }
        public int Status { get; set; }
        public bool EnableSerial { get; set; }
        public Guid? SerialKey { get; set; }
        public List<VehiclesDevices> GetList(string DB, Guid? Key)
        {
            List<VehiclesDevices> items = new List<VehiclesDevices>();
            string selQuery = "select top 100 percent * from dbo.fnTracking_SyncVehiclesDevices(@Key)";
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
                    VehiclesDevices item = new VehiclesDevices();
                    item.Order = iCore.IsDbNullRtNull(reader["Order"]);
                    item.Vehicle = iCore.IsDbNullRtNull(reader["Vehicle"]);
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.ItemKey = iCore.IsDbNullRtNull(reader["ItemKey"]);
                    item.ItemName1 = Convert.ToString(reader["ItemName1"]);
                    item.ItemName2 = Convert.ToString(reader["ItemName2"]);
                    item.SerialNo = Convert.ToString(reader["SerialNo"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.EnableSerial = Convert.ToBoolean(reader["EnableSerial"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
