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
    public class DevicesDeliveryDetails
    {
        public Guid Key { get; set; }
        public Guid? OperationKey { get; set; }
        public Guid Device { get; set; }
        public string SerialNo { get; set; }
        public string Sim { get; set; }
        public string SimSerial { get; set; }
        public bool Status { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public List<DevicesDeliveryDetails> GetList(string DB,Guid Key)
        {
            List<DevicesDeliveryDetails> items = new List<DevicesDeliveryDetails>();
            if (Key == null)
                return items;

            string selQuery = "SELECT TOP 100 PERCENT x.*,ISNULL(stock.item_Code,'') AS [Code],ISNULL(stock.item_Name1,'') AS [Name] from [TrackingOperation_TechnicalDeliveryDetails] x LEFT JOIN invCard_StockItem stock ON x.dev_Item=stock.item_Key where [dev_OperationKey]=@Key and [dev_Status]=0 ";
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
                    DevicesDeliveryDetails item = new DevicesDeliveryDetails();
                    item.OperationKey =Guid.Parse(reader["dev_OperationKey"].ToString());
                    item.Key = Guid.Parse(reader["dev_Key"].ToString());
                    item.Device = Guid.Parse(reader["dev_Item"].ToString());
                    item.SerialNo = Convert.ToString(reader["dev_SerialNo"]);
                    item.Sim = Convert.ToString(reader["dev_Sim"]);
                    item.SimSerial = Convert.ToString(reader["dev_SimSerial"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name = Convert.ToString(reader["Name"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
