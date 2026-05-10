using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
using CLiTracking.Operation;

namespace CLiTracking.Selections
{
    public class FreeDevices
    {
        public Guid? Key { get; set; }
        public Guid? ItemKey { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Serial { get; set; }
        public string Sim { get; set; }
        public string SimSerial { get; set; }
        public List<FreeDevices> GetList(string DB)
        {
            List<FreeDevices> items = new List<FreeDevices>();
            string selQuery =
                " SELECT TOP 100 PERCENT " +
                " [dev_Key] AS [Key]" +
                ",[dev_Item] AS [ItemKey]" +
                ",ISNULL(s.item_Code, '') AS [Code]" +
                ",ISNULL(s.item_Name1, '') AS [Name]" +
                ",[dev_SerialNo] AS [Serial]" +
                ",[dev_Sim] AS [Sim]" +
                ",[dev_SimSerial] AS [SimSerial] " +
                " FROM [dbo].[TrackingOperation_Devices] d " +
                " LEFT JOIN invCard_StockItem AS s " +
                " ON d.[dev_Item] = s.item_Key " +
                 " where [dev_Status]=0 order by s.item_Code,[dev_SerialNo] ";

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
                    FreeDevices item = new FreeDevices();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.ItemKey = iCore.IsDbNullRtNull(reader["ItemKey"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name = Convert.ToString(reader["Name"]);
                    item.Serial = Convert.ToString(reader["Serial"]);
                    item.Sim = Convert.ToString(reader["Sim"]);
                    item.SimSerial = Convert.ToString(reader["SimSerial"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
