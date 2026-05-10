using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Operation
{
    public class Stocktaking
    {
        public int RecNo { get; set; }
        public Guid? Key { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int Status { get; set; }
        public int Warehouse { get; set; }
        public int No { get; set; }
        public DateTime? Date { get; set; }
        public string Comment { get; set; }
        public string Person { get; set; }
        public string FileName { get; set; }


        public List<Stocktaking> GetList(string DB)
        {
            List<Stocktaking> items = new List<Stocktaking>();
            string selQuery = "select top 100 percent * from InvOperation_Stocktaking order by [inv_Date] desc";
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
                    Stocktaking item = new Stocktaking();
                    item.RecNo = Convert.ToInt32(reader["inv_RecNo"]);
                    item.Key = iCore.IsDbNullRtNull(reader["inv_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["inv_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["inv_CreateDate"]);
                    item.Status = Convert.ToInt32(reader["inv_Status"]);
                    item.Warehouse = Convert.ToInt32(reader["inv_Warehouse"]);
                    item.No = Convert.ToInt32(reader["inv_No"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["inv_Date"]);
                    item.Comment = Convert.ToString(reader["inv_Comment"]);
                    item.Person = Convert.ToString(reader["inv_Person"]);
                    item.FileName = Convert.ToString(reader["inv_FileName"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }




        private readonly static object Locker = new object();
        public static bool UpdateReceiptInWarehouse(string DB, Stocktaking Header, List<StocktakingDetails> Details)
        {
            bool res = false;
            lock (Locker)
            {
                Guid? opk=Guid.NewGuid();
         
           
                using (SqlConnection conn = new SqlConnection(iCore.GetCon(DB)))
                {
                    System.Text.StringBuilder str = new System.Text.StringBuilder();

                    conn.Open();
                    SqlCommand comm = conn.CreateCommand();
                    SqlTransaction transaction;
                    transaction = conn.BeginTransaction("Transaction");
                    comm.Connection = conn;
                    comm.Transaction = transaction;
                    comm.CommandType = CommandType.Text;



                    str.Clear();
                    str.Append("INSERT INTO InvOperation_Stocktaking");
                    str.Append("([inv_Key]");
                    str.Append(",[inv_CreateUser]");
                    str.Append(",[inv_CreateDate]");
                    str.Append(",[inv_Status]");
                    str.Append(",[inv_Warehouse]");
                    str.Append(",[inv_No]");
                    str.Append(",[inv_Date]");
                    str.Append(",[inv_Comment]");
                    str.Append(",[inv_Person]");
                    str.Append(",[inv_FileName])");
                    str.Append(" VALUES ");
                    str.Append("(@inv_Key");
                    str.Append(",@inv_CreateUser");
                    str.Append(",@inv_CreateDate");
                    str.Append(",@inv_Status");
                    str.Append(",@inv_Warehouse");
                    str.Append(",@inv_No");
                    str.Append(",@inv_Date");
                    str.Append(",@inv_Comment");
                    str.Append(",@inv_Person");
                    str.Append(",@inv_FileName)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@inv_Warehouse", SqlDbType.Int).Value = Header.Warehouse;
                    comm.Parameters.Add("@inv_No", SqlDbType.Int).Value = Header.No;
                    comm.Parameters.Add("@inv_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.Date);
                    comm.Parameters.Add("@inv_Comment", SqlDbType.NVarChar, 500).Value = Header.Comment ?? "";
                    comm.Parameters.Add("@inv_Person", SqlDbType.NVarChar, 250).Value = Header.Person ?? "";
                    comm.Parameters.Add("@inv_FileName", SqlDbType.NVarChar, 250).Value = Header.FileName ?? "";
                    comm.ExecuteNonQuery();

                    foreach (Operation.StocktakingDetails item in Details)
                    {
                        str.Clear();
                        str.Append("INSERT INTO InvOperation_StocktakingDetails");
                        str.Append("([inv_Key]");
                        str.Append(",[inv_Index]");
                        str.Append(",[inv_Item]");
                        str.Append(",[inv_Unit]");
                        str.Append(",[inv_Quantity]");
                        str.Append(",[inv_Balance])");
                        str.Append(" VALUES ");
                        str.Append("(@inv_Key");
                        str.Append(",@inv_Index");
                        str.Append(",@inv_Item");
                        str.Append(",@inv_Unit");
                        str.Append(",@inv_Quantity");
                        str.Append(",@inv_Balance)");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@inv_Item", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Item);
                        comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@inv_Balance", SqlDbType.Decimal).Value = item.Balance;
                        comm.ExecuteNonQuery();
                    }
 
                    try
                    {
                        transaction.Commit();
                        res = false;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        res = true;
                    }
                }

                return res;
            }
        }

    }
}
