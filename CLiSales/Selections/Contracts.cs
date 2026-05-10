using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiSales.Selections
{
    public class Contracts
    {
        public Guid? Key { get; set; }
        public int Status { get; set; }
        public int Branch { get; set; }
        public Guid? Prefix { get; set; }
        public int InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public Guid? Client { get; set; }
        public Guid? SalesPerson { get; set; }
        public int SalesHand { get; set; }
        public string Description { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal vatAmount { get; set; }
        public Boolean Invoiced { get; set; }
        public decimal Total { get; set; }
        public decimal Remaining { get; set; }
        public List<Contracts> GetList(string DB,Guid? Key)
        {
            List<Contracts> items = new List<Contracts>();
            string selQuery = "select top 100 percent * from dbo.fnSalesSelection_Contracts(@Key) order by [InvoiceDate] desc";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Contracts item = new Contracts();
                    item.Key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Branch = Convert.ToInt32(reader["Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["Prefix"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.Client = iCore.IsDbNullRtNull(reader["Client"]);
                    item.SalesPerson = iCore.IsDbNullRtNull(reader["SalesPerson"]);
                    item.SalesHand = Convert.ToInt32(reader["SalesHand"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Subtotal = Convert.ToDecimal(reader["Subtotal"]);
                    item.Discount = Convert.ToDecimal(reader["Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.Invoiced = Convert.ToBoolean(reader["Invoiced"]);                
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.Remaining = Convert.ToDecimal(reader["Remaining"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

    }
}
