using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace CLiMobile.Technical
{
    public class core
    {
        public static int CheckUserDelivery(string DB, DateTime? Date, Guid? Key)
        {
            int result = 0;
            string selQuery = " SELECT TOP 100 PERCENT COUNT(tdd_RecNo)  FROM [dbo].[TrackingOperation_TechnicalDelivery] " +
                "WHERE (tdd_CreateDate >=@Date or @Date is null) and tdd_Technical=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                com.Parameters.Add("@Date", SqlDbType.DateTime).Value =iCore.IsNullRtDbNull(Date);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (int)reader[0];
                };
                reader.Close();
            }
            return result;
        }


        public static int UpdateDeliveryDetails(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update TrackingOperation_TechnicalDeliveryDetails SET dev_Status=1 where dev_Key=@Key ");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                con.Open();
              res=  comm.ExecuteNonQuery();
            }
            return res;
        }
        public static int UpdateDelivery(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update TrackingOperation_TechnicalDelivery SET tdd_Status=1 where tdd_OperationKey=@Key ");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                con.Open();
                res = comm.ExecuteNonQuery();
            }
            return res;
        }
        public static int UpdateVehicleToSync(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update [TrackingDocument_JobOrderVehicles] SET [veh_Status]=2 where [veh_Key]=@Key ");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                con.Open();
                res = comm.ExecuteNonQuery();
            }
            return res;
        }
        public static int UpdateVehicleDeviceToSync(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update [TrackingDocument_JobOrderVehiclesDevices] SET [dev_Status]=2 where [dev_Key]=@Key ");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                con.Open();
                res = comm.ExecuteNonQuery();
            }
            return res;
        }
        public static int UpdateVehiclesToInstallation(string DB, Guid? Key,string Plate, DateTime? Date)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("UPDATE TrackingDocument_JobOrderVehicles SET [veh_Plate]=@Plate,veh_InstallDate=@Date,veh_Status=3 WHERE veh_Key=@Key ");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                comm.Parameters.Add("@Plate", SqlDbType.NVarChar, 25).Value = Plate ?? "";
                comm.Parameters.Add("@Date", SqlDbType.DateTime).Value = Date;
                con.Open();
                res = comm.ExecuteNonQuery();
            }
            return res;
        }
        public static int UpdateVehicleDeviceToInstallation(string DB, Guid? Key, string SerialNo,Guid? SerialKey)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append(" UPDATE [TrackingDocument_JobOrderVehiclesDevices] SET dev_SerialNo=@SerialNo,[dev_SerialKey]=@SerialKey,dev_Status=3 WHERE dev_Key=@Key ");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                comm.Parameters.Add("@SerialNo", SqlDbType.NVarChar,25).Value = SerialNo;
                comm.Parameters.Add("@SerialKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(SerialKey); ;
                con.Open();
                res = comm.ExecuteNonQuery();
            }
            return res;
        }
    }
}
