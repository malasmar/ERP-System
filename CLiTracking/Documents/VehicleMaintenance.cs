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
    public class VehicleMaintenance
    {
        public int RecNo { get; set; }
        public Guid? Key { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public DateTime? OrderDate { get; set; }
        public string Reference { get; set; }
        public Guid? Client { get; set; }
        public Guid? Vehicle { get; set; }
        public string Description { get; set; }
        public Guid? Technical { get; set; }
        public int Status { get; set; }
        public List<VehicleMaintenance> GetList(string DB)
        {
            List<VehicleMaintenance> items = new List<VehicleMaintenance>();
            string selQuery = "select top 100 percent * from TrackingDocument_VehicleMaintenance order by [mnt_OrderDate]";
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
                    VehicleMaintenance item = new VehicleMaintenance();
                    item.RecNo = Convert.ToInt32(reader["mnt_RecNo"]);
                    item.Key = iCore.IsDbNullRtNull(reader["mnt_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["mnt_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["mnt_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["mnt_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["mnt_LastupDate"]);
                    item.OrderDate = iCore.IsDbNullRtNullDate(reader["mnt_OrderDate"]);
                    item.Reference = Convert.ToString(reader["mnt_Reference"]);
                    item.Client = iCore.IsDbNullRtNull(reader["mnt_Client"]);
                    item.Vehicle = iCore.IsDbNullRtNull(reader["mnt_Vehicle"]);
                    item.Description = Convert.ToString(reader["mnt_Description"]);
                    item.Technical = iCore.IsDbNullRtNull(reader["mnt_Technical"]);
                    item.Status = Convert.ToInt32(reader["mnt_Status"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public VehicleMaintenance GetItem(string DB,Guid? Key)
        {
            VehicleMaintenance item = new VehicleMaintenance();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from TrackingDocument_VehicleMaintenance where [mnt_Key]=@Key ";
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
                    item.RecNo = Convert.ToInt32(reader["mnt_RecNo"]);
                    item.Key = iCore.IsDbNullRtNull(reader["mnt_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["mnt_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["mnt_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["mnt_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["mnt_LastupDate"]);
                    item.OrderDate = iCore.IsDbNullRtNullDate(reader["mnt_OrderDate"]);
                    item.Reference = Convert.ToString(reader["mnt_Reference"]);
                    item.Client = iCore.IsDbNullRtNull(reader["mnt_Client"]);
                    item.Vehicle = iCore.IsDbNullRtNull(reader["mnt_Vehicle"]);
                    item.Description = Convert.ToString(reader["mnt_Description"]);
                    item.Technical = iCore.IsDbNullRtNull(reader["mnt_Technical"]);
                    item.Status = Convert.ToInt32(reader["mnt_Status"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, VehicleMaintenance item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO TrackingDocument_VehicleMaintenance");
                str.Append("([mnt_Key]");
                str.Append(",[mnt_CreateUser]");
                str.Append(",[mnt_CreateDate]");
                str.Append(",[mnt_LastupUser]");
                str.Append(",[mnt_LastupDate]");
                str.Append(",[mnt_OrderDate]");
                str.Append(",[mnt_Reference]");
                str.Append(",[mnt_Client]");
                str.Append(",[mnt_Vehicle]");
                str.Append(",[mnt_Description]");
                str.Append(",[mnt_Technical]");
                str.Append(",[mnt_Status])");
                str.Append(" VALUES ");
                str.Append("(@mnt_Key");
                str.Append(",@mnt_CreateUser");
                str.Append(",@mnt_CreateDate");
                str.Append(",@mnt_LastupUser");
                str.Append(",@mnt_LastupDate");
                str.Append(",@mnt_OrderDate");
                str.Append(",@mnt_Reference");
                str.Append(",@mnt_Client");
                str.Append(",@mnt_Vehicle");
                str.Append(",@mnt_Description");
                str.Append(",@mnt_Technical");
                str.Append(",@mnt_Status)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@mnt_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                comm.Parameters.Add("@mnt_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@mnt_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@mnt_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@mnt_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@mnt_OrderDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.OrderDate);
                comm.Parameters.Add("@mnt_Reference", SqlDbType.NVarChar, 25).Value = item.Reference ?? "";
                comm.Parameters.Add("@mnt_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Client);
                comm.Parameters.Add("@mnt_Vehicle", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Vehicle);
                comm.Parameters.Add("@mnt_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                comm.Parameters.Add("@mnt_Technical", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Technical);
                comm.Parameters.Add("@mnt_Status", SqlDbType.Int).Value = item.Status;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, VehicleMaintenance item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update TrackingDocument_VehicleMaintenance SET ");
                str.Append(" [mnt_CreateUser]=@mnt_CreateUser");
                str.Append(",[mnt_CreateDate]=@mnt_CreateDate");
                str.Append(",[mnt_LastupUser]=@mnt_LastupUser");
                str.Append(",[mnt_LastupDate]=@mnt_LastupDate");
                str.Append(",[mnt_OrderDate]=@mnt_OrderDate");
                str.Append(",[mnt_Reference]=@mnt_Reference");
                str.Append(",[mnt_Client]=@mnt_Client");
                str.Append(",[mnt_Vehicle]=@mnt_Vehicle");
                str.Append(",[mnt_Description]=@mnt_Description");
                str.Append(",[mnt_Technical]=@mnt_Technical");
                str.Append(",[mnt_Status]=@mnt_Status");
                str.Append(" WHERE mnt_Key=@mnt_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@mnt_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@mnt_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@mnt_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@mnt_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@mnt_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@mnt_OrderDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.OrderDate);
                comm.Parameters.Add("@mnt_Reference", SqlDbType.NVarChar, 25).Value = item.Reference ?? "";
                comm.Parameters.Add("@mnt_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Client);
                comm.Parameters.Add("@mnt_Vehicle", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Vehicle);
                comm.Parameters.Add("@mnt_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                comm.Parameters.Add("@mnt_Technical", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Technical);
                comm.Parameters.Add("@mnt_Status", SqlDbType.Int).Value = item.Status;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static void Delete(string DB, Guid? Key)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [TrackingDocument_VehicleMaintenance] where [mnt_Key]=@Key";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

    }
}
