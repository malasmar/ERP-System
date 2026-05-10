using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiInventory.Documents
{
    public class TransactionInfo
    {
        public Guid? Key { get; set; }
        public CLiCore.CardsInfo.User CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public CLiCore.CardsInfo.User LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public int Status { get; set; }
        public int Branch { get; set; }
        public Guid? Prefix { get; set; }
        public int Warehouse { get; set; }
        public int DocumentKind { get; set; }
        public int InvoiceKind { get; set; }
        public int InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int MonthlyNo { get; set; }
        public int AccountKind { get; set; }
        public CLiCore.CardsInfo.AccountDetails Account { get; set; }
        public CLiCore.CardsInfo.Client Current { get; set; }
        public Guid? SalesPerson { get; set; }
        public int SalesHand { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal vatAmount { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal Total { get; set; }
        public decimal Quantity { get; set; }
        public decimal BonusQuantity { get; set; }
        public Boolean Returned { get; set; }
        public int Source { get; set; }
        public Boolean IsCredit { get; set; }
        public TransactionInfo GetItem(string DB, string xLan, Guid? Key)
        {
            TransactionInfo item = new TransactionInfo();
       
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from dbo.fnInvDocuments_TransactionInfo(@Key) ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["CreateDate"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["LastupDate"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Branch = Convert.ToInt32(reader["Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["Prefix"]);
                    item.Warehouse = Convert.ToInt32(reader["Warehouse"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.InvoiceKind = Convert.ToInt32(reader["InvoiceKind"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.AccountKind = Convert.ToInt32(reader["AccountKind"]);
                    item.SalesPerson = iCore.IsDbNullRtNull(reader["SalesPerson"]);
                    item.SalesHand = Convert.ToInt32(reader["SalesHand"]);
                    item.Description = Convert.ToString(reader["Description"])??"";
                    item.Currency = Convert.ToString(reader["Currency"])??"";
                    item.Subtotal = Convert.ToDecimal(reader["Subtotal"]);
                    item.Discount = Convert.ToDecimal(reader["Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.BonusAmount = Convert.ToDecimal(reader["BonusAmount"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.BonusQuantity = Convert.ToDecimal(reader["BonusQuantity"]);
                    item.Returned = Convert.ToBoolean(reader["Returned"]);
                    item.Source = Convert.ToInt32(reader["Source"]);
                    item.IsCredit = Convert.ToBoolean(reader["IsCredit"]);
                    item.CreateUser = new CLiCore.CardsInfo.User().GetItem(xLan, Convert.ToInt32(reader["CreateUser"]));
                    item.LastupUser = new CLiCore.CardsInfo.User().GetItem(xLan, Convert.ToInt32(reader["LastupUser"]));
                    item.Account = new CLiCore.CardsInfo.AccountDetails().GetItem(DB, xLan, iCore.IsDbNullRtNull(reader["AccountKey"]));
                    item.Current = new CLiCore.CardsInfo.Client().GetItem(DB, xLan, iCore.IsDbNullRtNull(reader["CurrentKey"]));
                }
                reader.Close();
            }
            return item;
        }
     
    }
}
