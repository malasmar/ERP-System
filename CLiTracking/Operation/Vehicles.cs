using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
namespace CLiTracking.Operation
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

        public List<Vehicles> GetList(string DB,Guid? Key)
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
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<Vehicles> NotAssign(string DB, Guid? Key)
        {
            List<Vehicles> items = new List<Vehicles>();
            if (Key == null)
                return items;
            string selQuery = "select top 100 percent * from TrackingDocument_JobOrderVehicles where [veh_OperationKey]=@Key and [veh_Status] in (0,1) order by [veh_RecNo] ";
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
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
