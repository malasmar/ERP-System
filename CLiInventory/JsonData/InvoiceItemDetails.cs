using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiInventory.JsonData
{
    public class InvoiceItemDetails
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public Guid? vatKey { get; set; }
        public decimal vatRate { get; set; }
        public decimal Total { get; set; }
        public string Unit { get; set; }
        public Boolean EnablePart { get; set; }
        public decimal PartRate { get; set; }
        public string PartName { get; set; }
        public decimal PartPrice { get; set; }
        public Boolean EnableBatch { get; set; }
        public Boolean EnableSerial { get; set; }
        public Boolean EnableColor { get; set; }
        public Boolean EnableSize { get; set; }
        public Boolean EnablePkg { get; set; }

        public decimal PurchasePrice { get; set; }
        public decimal SalesPrice { get; set; }

        public InvoiceItemDetails GetItem(string DB, Guid? Key)
        {
            InvoiceItemDetails item = new InvoiceItemDetails();
            if (Key == null)
                return item;
            string selQuery = "SELECT TOP 100 PERCENT stock.*,ISNULL(cost.[cost_Cost], stock.item_Cost) AS [LastCost],ISNULL(vat.vat_Rate,0.00) AS [vatRate] " +
                "FROM invCard_StockItem AS stock LEFT JOIN [InvStock_UnitCost] AS cost ON stock.item_Key = cost.cost_Item LEFT JOIN com_vatRates vat ON stock.item_vatKey=vat.vat_Key WHERE [item_Key] = @Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["item_Key"]);
                    item.Code = Convert.ToString(reader["item_Code"]);
                    item.Name1 = Convert.ToString(reader["item_Name1"]);
                    item.Name2 = Convert.ToString(reader["item_Name2"]);
                    item.Cost = Convert.ToDecimal(reader["LastCost"]);
                    item.Price = Convert.ToDecimal(reader["item_Price"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["item_vatKey"]);
                    item.Total = Convert.ToDecimal(reader["item_Total"]);
                    item.Unit = Convert.ToString(reader["item_Unit"]);
                    item.EnablePart = Convert.ToBoolean(reader["item_EnablePart"]);
                    item.PartRate = Convert.ToDecimal(reader["item_PartRate"]);
                    item.PartName = Convert.ToString(reader["item_PartName"]);
                    item.PartPrice = Convert.ToDecimal(reader["item_PartPrice"]);
                    item.EnableBatch = Convert.ToBoolean(reader["item_EnableBatch"]);
                    item.EnableSerial = Convert.ToBoolean(reader["item_EnableSerial"]);
                    item.EnableColor = Convert.ToBoolean(reader["item_EnableColor"]);
                    item.EnableSize = Convert.ToBoolean(reader["item_EnableSize"]);
                    item.EnablePkg = Convert.ToBoolean(reader["item_EnablePkg"]);
                    item.PurchasePrice = Convert.ToDecimal(reader["item_PurchasePrice"]);
                    item.SalesPrice = Convert.ToDecimal(reader["item_SalesPrice"]);
                    item.vatRate = Convert.ToDecimal(reader["vatRate"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
