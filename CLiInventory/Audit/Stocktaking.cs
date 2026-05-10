using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Audit
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
        public decimal Quantity { get; set; }
        public decimal Balance { get; set; }
        public decimal IncreaseQty { get; set; }
        public decimal DeficiencyQty { get; set; }
        public decimal IncreaseValue { get; set; }
        public decimal DeficiencyValue { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public Guid? Prefix { get; set; }
        public int Branch { get; set; }
        public List<Stocktaking> GetList(string DB)
        {
            List<Stocktaking> items = new List<Stocktaking>();
            string selQuery = "select top 100 percent * from dbo.fninvOperation_Stocktaking() order by [Date] desc ";
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
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.CreateUser = Convert.ToInt32(reader["CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["CreateDate"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Warehouse = Convert.ToInt32(reader["Warehouse"]);
                    item.No = Convert.ToInt32(reader["No"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.Comment = Convert.ToString(reader["Comment"]);
                    item.Person = Convert.ToString(reader["Person"]);
                    item.FileName = Convert.ToString(reader["FileName"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.Balance = Convert.ToDecimal(reader["Balance"]);
                    item.IncreaseQty = Convert.ToDecimal(reader["IncreaseQty"]);
                    item.DeficiencyQty = Convert.ToDecimal(reader["DeficiencyQty"]);
                    item.IncreaseValue = Convert.ToDecimal(reader["IncreaseValue"]);
                    item.DeficiencyValue = Convert.ToDecimal(reader["DeficiencyValue"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public Stocktaking GetItem(string DB,Guid? Key)
        {
            Stocktaking item = new Stocktaking();
            string selQuery = "select top 100 percent * from dbo.fninvOperation_StocktakingItem(@Key) order by [Date] desc ";
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
                   
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.CreateUser = Convert.ToInt32(reader["CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["CreateDate"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Warehouse = Convert.ToInt32(reader["Warehouse"]);
                    item.No = Convert.ToInt32(reader["No"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.Comment = Convert.ToString(reader["Comment"]);
                    item.Person = Convert.ToString(reader["Person"]);
                    item.FileName = Convert.ToString(reader["FileName"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.Balance = Convert.ToDecimal(reader["Balance"]);
                    item.IncreaseQty = Convert.ToDecimal(reader["IncreaseQty"]);
                    item.DeficiencyQty = Convert.ToDecimal(reader["DeficiencyQty"]);
                    item.IncreaseValue = Convert.ToDecimal(reader["IncreaseValue"]);
                    item.DeficiencyValue = Convert.ToDecimal(reader["DeficiencyValue"]);
                   
                }
                reader.Close();
            }
            return item;
        }
    }
}
