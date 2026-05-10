using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiTracking
{
    public class core
    {
        public static void UodateAssignTechnical(string DB, Guid? Key, Guid? Technical)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update [TrackingDocument_JobOrderVehicles] SET ");
                str.Append("[veh_Technician]=@Technician");
                str.Append(",[veh_Status]=@Status");
                str.Append(" WHERE veh_Key=@Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                comm.Parameters.Add("@Technician", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Technical);
                comm.Parameters.Add("@Status", SqlDbType.Bit).Value = 1;

                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public static bool CheckDeviceSerial(string DB,Guid? Key, string SerialNo)
        {
            int count = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "select top 100 percent count(*) from [TrackingOperation_Devices] where [dev_Item]=@Key and [dev_SerialNo]=@SerialNo ";
                SqlCommand command = new SqlCommand();
                command = new SqlCommand();
                command.Connection = con;
                command.CommandType = CommandType.Text;
                command.CommandText = delQuery;
                command.Parameters.Clear();
                command.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                command.Parameters.Add("@SerialNo", SqlDbType.NVarChar).Value = SerialNo??"";
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    count = (int)reader[0];
                }
                reader.Close();
            }
            if (count == 0)
                return false;
            else
                return true;
        }
    }
}
