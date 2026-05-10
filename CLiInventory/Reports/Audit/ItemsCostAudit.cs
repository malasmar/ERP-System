using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Reports.Audit
{
    public class ItemsCostAudit
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Unit { get; set; }
        public DateTime? Date { get; set; }
        public decimal Opening { get; set; }
        public decimal Add { get; set; }
        public decimal Purchase { get; set; }
        public decimal ReturnPurchase { get; set; }
        public decimal Sales { get; set; }
        public decimal ReturnSales { get; set; }
        public decimal Consumption { get; set; }
        public decimal ReConsumption { get; set; }
        public decimal TransferOut { get; set; }
        public decimal TransferIn { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal Amount { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal OutcomeAmount { get; set;}
        public decimal NetAmount { get; set;}
        public decimal Diff { get; set; }
        public List<ItemsCostAudit> GetList(string DB,DateTime? FirstDate,DateTime? LastDate)
        {

            List<ItemsCostAudit> items = new List<ItemsCostAudit>();
            string selQuery = "select top 100 percent * from dbo.ReportInv_AuditItemsCost(@FirstDate,@LastDate) order by [Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = FirstDate;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = LastDate;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ItemsCostAudit item = new ItemsCostAudit();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.Opening = Convert.ToDecimal(reader["Opening"]);
                    item.Add = Convert.ToDecimal(reader["Add"]);
                    item.Purchase = Convert.ToDecimal(reader["Purchase"]);
                    item.ReturnPurchase = Convert.ToDecimal(reader["ReturnPurchase"]);
                    item.Sales = Convert.ToDecimal(reader["Sales"]);
                    item.ReturnSales = Convert.ToDecimal(reader["ReturnSales"]);
                    item.Consumption = Convert.ToDecimal(reader["Consumption"]);
                    item.ReConsumption = Convert.ToDecimal(reader["ReConsumption"]);
                    item.TransferOut = Convert.ToDecimal(reader["TransferOut"]);
                    item.TransferIn = Convert.ToDecimal(reader["TransferIn"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.Cost = Convert.ToDecimal(reader["Cost"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.IncomeAmount = item.Opening + item.Add + item.Purchase + item.ReturnSales + item.TransferIn + item.ReConsumption;
                    item.OutcomeAmount = item.Sales + item.ReturnPurchase + item.Consumption + item.TransferOut;
                    item.NetAmount = item.IncomeAmount - item.OutcomeAmount;
                    item.Diff = item.Amount - item.NetAmount;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<ItemsCostAudit> GetList(string DB, DateTime? FirstDate, DateTime? LastDate,int Warehouse)
        {
            
            List<ItemsCostAudit> items = new List<ItemsCostAudit>();
            if (Warehouse == 0)
                return items;

            string selQuery = "select top 100 percent * from dbo.ReportInv_AuditItemsCostBaseWarehouse(@FirstDate,@LastDate,@Warehouse) order by [Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = FirstDate;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = LastDate;
                com.Parameters.Add("@Warehouse", SqlDbType.Int).Value = Warehouse;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ItemsCostAudit item = new ItemsCostAudit();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.Opening = Convert.ToDecimal(reader["Opening"]);
                    item.Add = Convert.ToDecimal(reader["Add"]);
                    item.Purchase = Convert.ToDecimal(reader["Purchase"]);
                    item.ReturnPurchase = Convert.ToDecimal(reader["ReturnPurchase"]);
                    item.Sales = Convert.ToDecimal(reader["Sales"]);
                    item.ReturnSales = Convert.ToDecimal(reader["ReturnSales"]);
                    item.Consumption = Convert.ToDecimal(reader["Consumption"]);
                    item.ReConsumption = Convert.ToDecimal(reader["ReConsumption"]);
                    item.TransferOut = Convert.ToDecimal(reader["TransferOut"]);
                    item.TransferIn = Convert.ToDecimal(reader["TransferIn"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.Cost = Convert.ToDecimal(reader["Cost"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.IncomeAmount = item.Opening + item.Add + item.Purchase + item.ReturnSales + item.TransferIn + item.ReConsumption;
                    item.OutcomeAmount = item.Sales + item.ReturnPurchase + item.Consumption + item.TransferOut;
                    item.NetAmount = item.IncomeAmount - item.OutcomeAmount;
                    item.Diff = item.Amount - item.NetAmount;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

    }
}
