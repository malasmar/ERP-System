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
    public class TechnicalDelivery
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int SourceWarehouse { get; set; }
        public Guid? Technical { get; set; }
        public int No { get; set; }
        public DateTime? Date { get; set; }
        public string Comment { get; set; } 
        public string ReferenceNo { get; set; }
        public string Description { get; set; }
        public Boolean Status { get; set; }
        public List<TechnicalDelivery> GetList(string DB)
        {
            List<TechnicalDelivery> items = new List<TechnicalDelivery>();
            string selQuery = "select top 100 percent * from TrackingOperation_TechnicalDelivery order by [tdd_Date] desc";
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
                    TechnicalDelivery item = new TechnicalDelivery();
                    item.RecNo = Convert.ToInt32(reader["tdd_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["tdd_OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["tdd_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["tdd_CreateDate"]);
                    item.SourceWarehouse = Convert.ToInt32(reader["tdd_SourceWarehouse"]);
                    item.Technical = iCore.IsDbNullRtNull(reader["tdd_Technical"]);
                    item.No = Convert.ToInt32(reader["tdd_No"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["tdd_Date"]);
                    item.Comment = Convert.ToString(reader["tdd_Comment"]);
                    item.ReferenceNo = Convert.ToString(reader["tdd_ReferenceNo"]);
                    item.Description = Convert.ToString(reader["tdd_Description"]);
                    item.Status = Convert.ToBoolean(reader["tdd_Status"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public TechnicalDelivery GetItem(string DB,Guid? Key)
        {
            TechnicalDelivery item = new TechnicalDelivery();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from TrackingOperation_TechnicalDelivery where [tdd_OperationKey]=@Key";
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
                    item.RecNo = Convert.ToInt32(reader["tdd_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["tdd_OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["tdd_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["tdd_CreateDate"]);
                    item.SourceWarehouse = Convert.ToInt32(reader["tdd_SourceWarehouse"]);
                    item.Technical = iCore.IsDbNullRtNull(reader["tdd_Technical"]);
                    item.No = Convert.ToInt32(reader["tdd_No"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["tdd_Date"]);
                    item.Comment = Convert.ToString(reader["tdd_Comment"]);
                    item.ReferenceNo = Convert.ToString(reader["tdd_ReferenceNo"]);
                    item.Description = Convert.ToString(reader["tdd_Description"]);
                    item.Status = Convert.ToBoolean(reader["tdd_Status"]);
                }
                reader.Close();
            }
            return item;
        }

       

 

    }
}
