using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace CLiInventory.Reports.Statments
{
    public class StockStatment
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public int DocumentKind { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int InvoiceNo { get; set; }
        public int SourceWarehouse { get; set; }
        public int TargetWarehouse { get; set; }
        public DateTime? ProDate { get; set; }
        public DateTime? ExpDate { get; set; }
        public int Color { get; set; }
        public int Size { get; set; }
        public string Unit { get; set; }
        public decimal Incoming { get; set; }
        public decimal Outgoing { get; set; }
        public string Description { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public decimal Balance { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Cost { get; set; }
        public List<StockStatment> GetList(string DB, Guid? Key,int Warehouse, DateTime? FirstDate, DateTime? LastDate)
        {
            decimal balance;
            balance = 0;
            List<StockStatment> items = new List<StockStatment>();
            string selQuery = "select top 100 percent * from dbo.ReportInv_StockStatment(@Key,@Warehouse,@FirstDate,@LastDate) order by [InvoiceDate],[DocumentKind],[InvoiceNo] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                com.Parameters.Add("@Warehouse", SqlDbType.Int).Value = Warehouse;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(FirstDate);
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(LastDate);
   
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    StockStatment item = new StockStatment();
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    item.SourceWarehouse = Convert.ToInt32(reader["SourceWarehouse"]);
                    item.TargetWarehouse = Convert.ToInt32(reader["TargetWarehouse"]);
                    item.ProDate = iCore.IsDbNullRtNullDate(reader["ProDate"]);
                    item.ExpDate = iCore.IsDbNullRtNullDate(reader["ExpDate"]);
                    item.Color = Convert.ToInt32(reader["Color"]);
                    item.Size = Convert.ToInt32(reader["Size"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
                    item.Incoming = Convert.ToDecimal(reader["Incoming"]);
                    item.Outgoing = Convert.ToDecimal(reader["Outgoing"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Project"]);
                    item.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                    item.UnitCost = Convert.ToDecimal(reader["UnitCost"]);
                    item.Cost = Convert.ToDecimal(reader["Cost"]);
                    balance += item.Incoming - item.Outgoing;
                    item.Balance = balance;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<StockStatment> ReportWithoutWarehouse(string DB, Guid? Key, DateTime? FirstDate, DateTime? LastDate)
        {
            decimal balance;
            balance = 0;
            List<StockStatment> items = new List<StockStatment>();
            string selQuery = "select top 100 percent * from dbo.ReportInv_StockStatmentFull(@Key,@FirstDate,@LastDate) order by [InvoiceDate]  ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(FirstDate);
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(LastDate);

                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    StockStatment item = new StockStatment();
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    item.SourceWarehouse = Convert.ToInt32(reader["SourceWarehouse"]);
                    item.TargetWarehouse = Convert.ToInt32(reader["TargetWarehouse"]);
                    item.ProDate = iCore.IsDbNullRtNullDate(reader["ProDate"]);
                    item.ExpDate = iCore.IsDbNullRtNullDate(reader["ExpDate"]);
                    item.Color = Convert.ToInt32(reader["Color"]);
                    item.Size = Convert.ToInt32(reader["Size"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
                    item.Incoming = Convert.ToDecimal(reader["Incoming"]);
                    item.Outgoing = Convert.ToDecimal(reader["Outgoing"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Project"]);
                    item.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                    item.UnitCost = Convert.ToDecimal(reader["UnitCost"]);
                    item.Cost = Convert.ToDecimal(reader["Cost"]);
                    balance += item.Incoming - item.Outgoing;
                    item.Balance = balance;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
