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
    public class DevicesDelivery
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public DateTime? Date { get; set; }
        public string ReferenceNo { get; set; }
        public string Comment { get; set; }
        public int Count { get; set; }
        public List<DevicesDelivery> GetList(string DB,Guid Key)
        {
            List<DevicesDelivery> items = new List<DevicesDelivery>();
            if (Key == null)
                return items;

            string selQuery = "SELECT TOP 100 PERCENT " +
                " [tdd_OperationKey] as [Key]" +
                ",[tdd_No] as [No]" +
                ",[tdd_Date] as [Date]" +
                ",[tdd_ReferenceNo] as [ReferenceNo]" +
                ",[tdd_Comment] as [Comment]" +
                ",(SELECT top 100 percent count(xd.dev_Key) from [dbo].[TrackingOperation_TechnicalDeliveryDetails] xd where xd.dev_OperationKey=[tdd_OperationKey]) as [Count]" +
                " FROM [dbo].[TrackingOperation_TechnicalDelivery] as d " +
                "where d.tdd_Technical=@Key and d.tdd_Status=0";
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
                    DevicesDelivery item = new DevicesDelivery();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.No = Convert.ToInt32(reader["No"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.ReferenceNo = Convert.ToString(reader["ReferenceNo"]);
                    item.Comment = Convert.ToString(reader["Comment"]);
                    item.Count = Convert.ToInt32(reader["Count"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
