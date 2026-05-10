using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
namespace CLiTracking.Documents
{
    public class JobOrderVehiclesDevices
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public Guid? VehicleKey { get; set; }
        public Guid? Key { get; set; }
        public Guid? Item { get; set; }
        public string SerialNo { get; set; }
        public int Status { get; set; }
        public List<JobOrderVehiclesDevices> GetList(string DB,Guid? Key)
        {
            List<JobOrderVehiclesDevices> items = new List<JobOrderVehiclesDevices>();
            string selQuery = "select top 100 percent * from TrackingDocument_JobOrderVehiclesDevices where [veh_VehicleKey]=@Key ";
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
                    JobOrderVehiclesDevices item = new JobOrderVehiclesDevices();
                    item.RecNo = Convert.ToInt32(reader["dev_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["dev_OperationKey"]);
                    item.VehicleKey = iCore.IsDbNullRtNull(reader["dev_VehicleKey"]);
                    item.Key = iCore.IsDbNullRtNull(reader["dev_Key"]);
                    item.Item = iCore.IsDbNullRtNull(reader["dev_Item"]);
                    item.SerialNo = Convert.ToString(reader["dev_SerialNo"]);
                    item.Status = Convert.ToInt32(reader["dev_Status"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public JobOrderVehiclesDevices GetItem(string DB,Guid? Key)
        {
            JobOrderVehiclesDevices item = new JobOrderVehiclesDevices();
            string selQuery = "select top 100 percent * from TrackingDocument_JobOrderVehiclesDevices where [dev_Key]=@key ";
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
                    item.RecNo = Convert.ToInt32(reader["dev_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["dev_OperationKey"]);
                    item.VehicleKey = iCore.IsDbNullRtNull(reader["dev_VehicleKey"]);
                    item.Key = iCore.IsDbNullRtNull(reader["dev_Key"]);
                    item.Item = iCore.IsDbNullRtNull(reader["dev_Item"]);
                    item.SerialNo = Convert.ToString(reader["dev_SerialNo"]);
                    item.Status = Convert.ToInt32(reader["dev_Status"]);
                }
                reader.Close();
            }
            return item;
        }

    }
}
