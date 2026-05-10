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
    public class Vehicles
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public Guid? Key { get; set; }
        public string Plate { get; set; }
        public string Arabic { get; set; }
        public Guid? City { get; set; }
        public string Person { get; set; }
        public string Phone { get; set; }
        public Guid? Technician { get; set; }
        public int Status { get; set; }

        public Guid? Device1 { get; set; }
        public Guid? Device2 { get; set; }
        public Guid? Device3 { get; set; }
        public Guid? Device4 { get; set; }
        public Guid? Device5 { get; set; }
        public Guid? Device6 { get; set; }

        public CLiCore.Selections.Cities xCity { get; set; }

        public List<Vehicles> GetList(string DB,string xLan,Guid? Key)
        {
            List<Vehicles> items = new List<Vehicles>();
            if (Key == null)
                return items;
            string selQuery = "select top 100 percent * from TrackingDocument_JobOrderVehicles where [veh_OperationKey]=@Key order by [veh_RecNo] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key",SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Vehicles item = new Vehicles();
                    item.RecNo = Convert.ToInt32(reader["veh_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["veh_OperationKey"]);
                    item.Key = iCore.IsDbNullRtNull(reader["veh_Key"]);
                    item.Plate = Convert.ToString(reader["veh_Plate"]);
                    item.Arabic = Convert.ToString(reader["veh_Arabic"]);
                    item.City = iCore.IsDbNullRtNull(reader["veh_City"]);
                    item.Person = Convert.ToString(reader["veh_Person"]);
                    item.Phone = Convert.ToString(reader["veh_Phone"]);
                    item.Technician = iCore.IsDbNullRtNull(reader["veh_Technician"]);
                    item.Status = Convert.ToInt32(reader["veh_Status"]);
                    item.xCity = CLiCore.Selections.Cities.GetItem(DB, xLan, item.City);
                    int x = 0;
                    List<JobOrderVehiclesDevices> devices = GetDevices(DB, item.Key);
                    foreach(JobOrderVehiclesDevices device in devices)
                    {
                        switch (x)
                        {
                            case 0:
                                item.Device1 = device.Item;
                                break;
                            case 1:
                                item.Device2 = device.Item;
                                break;
                            case 2:
                                item.Device3 = device.Item;
                                break;
                            case 3:
                                item.Device4 = device.Item;
                                break;
                            case 4:
                                item.Device5 = device.Item;
                                break;
                            case 5:
                                item.Device6 = device.Item;
                                break;
                        }
                        ++x;
                    }
 
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<JobOrderVehiclesDevices> GetDevices(string DB, Guid? Key)
        {
            List<JobOrderVehiclesDevices> items = new List<JobOrderVehiclesDevices>();
            string selQuery = "select top 100 percent * from TrackingDocument_JobOrderVehiclesDevices where [dev_VehicleKey]=@Key ";
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
        public Vehicles GetItem(string DB,Guid? Key)
        {
            Vehicles item = new Vehicles();
            if(DB != null)
                return item;

            string selQuery = "select top 100 percent * from TrackingDocument_JobOrderVehicles where [veh_Key]=@Key ";
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
                    item.RecNo = Convert.ToInt32(reader["veh_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["veh_OperationKey"]);
                    item.Key = iCore.IsDbNullRtNull(reader["veh_Key"]);
                    item.Plate = Convert.ToString(reader["veh_Plate"]);
                    item.Arabic = Convert.ToString(reader["veh_Arabic"]);
                    item.City = iCore.IsDbNullRtNull(reader["veh_City"]);
                    item.Person = Convert.ToString(reader["veh_Person"]);
                    item.Phone = Convert.ToString(reader["veh_Phone"]);
                    item.Technician = iCore.IsDbNullRtNull(reader["veh_Technician"]);
                    item.Status = Convert.ToInt32(reader["veh_Status"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
