using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Reports.Balance
{
    public class WarehouseBalance
    {
        public int Row { get; set; }
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Unit { get; set; }
        public int Warehouse { get; set; }
        public string WarehouseName1 { get; set; }
        public string WarehouseName2 { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public DateTime? Date { get; set; }
        public decimal Amount { get; set; }
        public List<WarehouseBalance> GetList(string DB,int? Warehouse,DateTime? Date)
        {
            List<WarehouseBalance> items = new List<WarehouseBalance>();
            if(Warehouse ==null || Date == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.ReportInv_WarehouseBalance(@Warehouse,@Date) order by [code] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Warehouse",SqlDbType.Int).Value= Warehouse;
                com.Parameters.Add("@Date", SqlDbType.Date).Value = Date;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WarehouseBalance item = new WarehouseBalance();
                    item.Key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.Cost = Convert.ToDecimal(reader["Cost"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<WarehouseBalance> GetList(string DB, DateTime? Date)
        {
            List<WarehouseBalance> items = new List<WarehouseBalance>();
            if (Date == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.ReportInv_WarehousesBalance(@Date) order by [code] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Date", SqlDbType.Date).Value = Date;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WarehouseBalance item = new WarehouseBalance();
                    item.Key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.Cost = Convert.ToDecimal(reader["Cost"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<WarehouseBalance> BalanceBaseWarehouse(string DB, DateTime? Date)
        {
            List<WarehouseBalance> items = new List<WarehouseBalance>();
            if (Date == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.ReportInv_ItemsBalanceBaseWarehouse(@Date) order by [Warehouse],[code] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Date", SqlDbType.Date).Value = Date;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WarehouseBalance item = new WarehouseBalance();
                    item.Key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Row = Convert.ToInt32(reader["Row"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
                    item.Warehouse = Convert.ToInt32(reader["Warehouse"]);
                    item.WarehouseName1 = Convert.ToString(reader["WarehouseName1"]);
                    item.WarehouseName2 = Convert.ToString(reader["WarehouseName2"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.Cost = Convert.ToDecimal(reader["Cost"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
