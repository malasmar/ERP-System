using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Audit.Cost
{
    public class InvoicesKeys
    {
        public Guid? Key { get; set; }
        public int Kind { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int InvoiceNo { get; set; }
        public List<InvoicesKeys> Purchasing(string DB,DateTime? Date)
        {
            List<InvoicesKeys> items = new List<InvoicesKeys>();
            string selQuery = "select top 100 percent * from dbo.fnInvAudit_PurchaseInvoicesKeys(@Date) order by [InvoiceDate],[InvoiceNo] ";
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
                    InvoicesKeys item = new InvoicesKeys();
                    item.Key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<InvoicesKeys> Sales(string DB, DateTime? Date)
        {
            List<InvoicesKeys> items = new List<InvoicesKeys>();
            string selQuery = "select top 100 percent * from dbo.fnInvAudit_SalesInvoicesKeys(@Date) order by [InvoiceDate],[InvoiceNo] ";
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
                    InvoicesKeys item = new InvoicesKeys();
                    item.Key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }


        public List<InvoicesKeys> Invoices(string DB, int Year)
        {
            List<InvoicesKeys> items = new List<InvoicesKeys>();
            string selQuery = "select top 100 percent * from dbo.fnFinAudit_InvoicesKeys(@Year) order by [InvoiceDate],[InvoiceNo] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    InvoicesKeys item = new InvoicesKeys();
                    item.Key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<InvoicesKeys> Transactions(string DB, int Year)
        {
            List<InvoicesKeys> items = new List<InvoicesKeys>();
            string selQuery = "select top 100 percent * from dbo.fnFinAudit_TransactionsKeys(@Year) order by [InvoiceDate],[InvoiceNo] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    InvoicesKeys item = new InvoicesKeys();
                    item.Key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
