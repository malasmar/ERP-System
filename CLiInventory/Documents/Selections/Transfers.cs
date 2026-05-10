using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Documents.Selections
{
    public class Transfers
    {
        public Guid? OperationKey { get; set; }
        public int Branch { get; set; }
        public Guid? Prefix { get; set; }
        public int SourceWarehouse { get; set; }
        public int TargetWarehouse { get; set; }
        public int InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal Quantity { get; set; }
        public decimal ClosedQty { get; set; }
        public decimal RemQty { get; set; }
        public List<Transfers> GetList(string DB,int TargetWarehouse)
        {
            List<Transfers> items = new List<Transfers>();
            string selQuery = "select top 100 percent * from dbo.fninvSelection_Transfers(@TargetWarehouse) where ([Quantity]-[ClosedQty])>0 order by [InvoiceDate] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@TargetWarehouse", SqlDbType.Int).Value = TargetWarehouse;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Transfers item = new Transfers();
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.Branch = Convert.ToInt32(reader["Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["Prefix"]);
                    item.SourceWarehouse = Convert.ToInt32(reader["SourceWarehouse"]);
                    item.TargetWarehouse = Convert.ToInt32(reader["TargetWarehouse"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.Subtotal = Convert.ToDecimal(reader["Subtotal"]);
                    item.BonusAmount = Convert.ToDecimal(reader["BonusAmount"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.ClosedQty = Convert.ToDecimal(reader["ClosedQty"]);
                    item.RemQty = item.Quantity -item.ClosedQty;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
