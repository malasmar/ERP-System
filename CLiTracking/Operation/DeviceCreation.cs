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
    public class DeviceCreation
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public int SourceWarehouse { get; set; }
        public int OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }
        public string ReferenceNo { get; set; }
        public string Description { get; set; }
        public Guid? Item { get; set; }
        public List<DeviceCreation> GetList(string DB)
        {
            List<DeviceCreation> items = new List<DeviceCreation>();
            string selQuery = "select top 100 percent * from TrackingOperation_DeviceCreation order by [crt_OrderDate] desc";
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
                    DeviceCreation item = new DeviceCreation();
                    item.RecNo = Convert.ToInt32(reader["crt_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["crt_OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["crt_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["crt_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["crt_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["crt_LastupDate"]);
                    item.SourceWarehouse = Convert.ToInt32(reader["crt_SourceWarehouse"]);
                    item.OrderNo = Convert.ToInt32(reader["crt_OrderNo"]);
                    item.OrderDate = iCore.IsDbNullRtNullDate(reader["crt_OrderDate"]);
                    item.ReferenceNo = Convert.ToString(reader["crt_ReferenceNo"]);
                    item.Description = Convert.ToString(reader["crt_Description"]);
                    item.Item = iCore.IsDbNullRtNull(reader["crt_Item"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public DeviceCreation GetItem(string DB,Guid? Key)
        {
            DeviceCreation item = new DeviceCreation();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from TrackingOperation_DeviceCreation where [crt_OperationKey]=@Key";
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
                    item.RecNo = Convert.ToInt32(reader["crt_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["crt_OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["crt_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["crt_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["crt_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["crt_LastupDate"]);
                    item.SourceWarehouse = Convert.ToInt32(reader["crt_SourceWarehouse"]);
                    item.OrderNo = Convert.ToInt32(reader["crt_OrderNo"]);
                    item.OrderDate = iCore.IsDbNullRtNullDate(reader["crt_OrderDate"]);
                    item.ReferenceNo = Convert.ToString(reader["crt_ReferenceNo"]);
                    item.Description = Convert.ToString(reader["crt_Description"]);
                    item.Item = iCore.IsDbNullRtNull(reader["crt_Item"]);
                }
                reader.Close();
            }
            return item;
        }

       

 

    }
}
