using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
using System.Xml.Schema;

namespace CLiSales.Documents
{
    public class QuotationInfo
    {
        public Guid? OperationKey { get; set; }
        public CLiCore.CardsInfo.User CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public CLiCore.CardsInfo.User LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public int Status { get; set; }
        public int Branch { get; set; }
        public Guid? Prefix { get; set; }
        public int InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int MonthlyNo { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? DueDate { get; set; }
        public CLiCore.CardsInfo.Client Client { get; set; }
        public CLiCore.CardsInfo.AccountDetails SalesPerson { get; set; }
        public int SalesHand { get; set; }
        public string Description { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal vatAmount { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal Quantity { get; set; }
        public decimal BonusQuantity { get; set; }
        public decimal Total { get; set; }
        public int DeliveryKind { get; set; }
        public Boolean Invoiced { get; set; }
        public Guid? OriginalInvoice { get; set; }
        public int Source { get; set; }
   
        public QuotationInfo GetItem(string DB,string xLan, Guid? Key)
        {
            QuotationInfo item = new QuotationInfo();
    
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from [SalesDocument_Quotation] where [sal_OperationKey]=@Key ";
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
                   
                    item.OperationKey = iCore.IsDbNullRtNull(reader["sal_OperationKey"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["sal_CreateDate"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["sal_LastupDate"]);
                    item.Status = Convert.ToInt32(reader["sal_Status"]);
                    item.Branch = Convert.ToInt32(reader["sal_Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["sal_Prefix"]);
                    item.InvoiceNo = Convert.ToInt32(reader["sal_InvoiceNo"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["sal_InvoiceDate"]);
                    item.MonthlyNo = Convert.ToInt32(reader["sal_MonthlyNo"]);
                    item.ReferenceNo = Convert.ToString(reader["sal_ReferenceNo"]);
                    item.DueDate = iCore.IsDbNullRtNullDate(reader["sal_DueDate"]);
                    item.SalesHand = Convert.ToInt32(reader["sal_SalesHand"]);
                    item.Description = Convert.ToString(reader["sal_Description"]);
                    item.SubTotal = Convert.ToDecimal(reader["sal_SubTotal"]);
                    item.Discount = Convert.ToDecimal(reader["sal_Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["sal_vatAmount"]);
                    item.BonusAmount = Convert.ToDecimal(reader["sal_BonusAmount"]);
                    item.Quantity = Convert.ToDecimal(reader["sal_Quantity"]);
                    item.BonusQuantity = Convert.ToDecimal(reader["sal_BonusQuantity"]);
                    item.DeliveryKind = Convert.ToInt32(reader["sal_DeliveryKind"]);
                    item.Total = Convert.ToDecimal(reader["sal_Total"]);
                    item.Invoiced = Convert.ToBoolean(reader["sal_Invoiced"]);
                    item.OriginalInvoice = iCore.IsDbNullRtNull(reader["sal_OriginalInvoice"]);
                    item.Source = Convert.ToInt32(reader["sal_Source"]);
              
                    item.CreateUser = new CLiCore.CardsInfo.User().GetItem(xLan, Convert.ToInt32(reader["sal_CreateUser"]));
                    item.LastupUser = new CLiCore.CardsInfo.User().GetItem(xLan, Convert.ToInt32(reader["sal_LastupUser"]));
                    item.Client = new CLiCore.CardsInfo.Client().GetItem(DB, xLan, iCore.IsDbNullRtNull(reader["sal_Client"]));
                    item.SalesPerson = new CLiCore.CardsInfo.AccountDetails().GetItem(DB, xLan, iCore.IsDbNullRtNull(reader["sal_SalesPerson"]));
                }
                reader.Close();
            }
            return item;
        }
    }
}
