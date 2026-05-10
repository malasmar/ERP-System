using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiTracking.Selections
{
    public class ClientVehicles
    {
        public Guid? Key { get; set; }
        public string Plate { get; set; }
        public string Arabic { get; set; }
        public Guid? Technical { get; set; }
        public static List<ClientVehicles> GetList(string DB, Guid? Key)
        {
            List<ClientVehicles> items = new List<ClientVehicles>();
            if (Key == null)
                return items;

            string selQuery = "SELECT TOP 100 PERCENT " +
                "v.veh_Key as [Key] " +
                ",v.veh_Plate AS [Plate] " +
                ",v.veh_Arabic AS [Arabic] " +
                ",v.veh_Technician AS [Technical] " +
                " FROM [dbo].[TrackingDocument_JobOrderVehicles] v " +
                " LEFT JOIN TrackingDocument_JobOrder j " +
                " ON v.veh_OperationKey = j.job_OperationKey " +
                " WHERE j.job_Client = @Key ";
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
                    ClientVehicles item = new ClientVehicles();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Plate = Convert.ToString(reader["Plate"]);
                    item.Arabic = Convert.ToString(reader["Arabic"]);
                    item.Technical = iCore.IsDbNullRtNull(reader["Technical"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
