using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiSales.Documents
{
    public class Quotation
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public int Status { get; set; }
        public int Branch { get; set; }
        public Guid? Prefix { get; set; }
        public int InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int MonthlyNo { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? Client { get; set; }
        public Guid? SalesPerson { get; set; }
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
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public CLiCore.CardsInfo.User CreateUserInfo { get; set; }
        public CLiCore.CardsInfo.User UpdateUserInfo { get; set; }
        public string PaymentTerms { get; set; }
        public string Terms { get; set; }
        public Quotation GetItem(string DB,string xLan, Guid UserKey, Guid? Key, int DocKind, int Year)
        {
            Quotation item = new Quotation();
            item.InvoiceNo = VoucherOperation.GetMaxQuotation(DB);
            item.InvoiceDate = DateTime.Now;
            item.DueDate = DateTime.Now;
            item.CreateDate = DateTime.Now;
            item.CreateUserInfo = new CLiCore.CardsInfo.User().GetItem(xLan, UserKey);
            item.UpdateUserInfo = new CLiCore.CardsInfo.User().GetItem(xLan, UserKey);
            item.CreateUser = item.CreateUserInfo.No;
            CLiCore.Shared.UserDefaultData defaultData = CLiCore.Shared.UserDefaultData.GetItem(UserKey);
            item.Prefix = defaultData.Prefix;
            item.Branch = defaultData.Branch;
            item.Project = defaultData.Project;
            item.CostCenter = defaultData.CostCenter;
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent q.*,isnull(terms.sal_PaymentTerms,'') as [sal_PaymentTerms],isnull(terms.sal_Terms,'') as [sal_Terms] from [SalesDocument_Quotation] q left join [SalesDocument_QuotationTerms] terms on q.sal_OperationKey=terms.sal_OperationKey where q.[sal_OperationKey]=@Key ";
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
                    item.RecNo = Convert.ToInt32(reader["sal_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["sal_OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["sal_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["sal_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["sal_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["sal_LastupDate"]);
                    item.Status = Convert.ToInt32(reader["sal_Status"]);
                    item.Branch = Convert.ToInt32(reader["sal_Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["sal_Prefix"]);
                    item.InvoiceNo = Convert.ToInt32(reader["sal_InvoiceNo"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["sal_InvoiceDate"]);
                    item.MonthlyNo = Convert.ToInt32(reader["sal_MonthlyNo"]);
                    item.ReferenceNo = Convert.ToString(reader["sal_ReferenceNo"]);
                    item.DueDate = iCore.IsDbNullRtNullDate(reader["sal_DueDate"]);
                    item.Client = iCore.IsDbNullRtNull(reader["sal_Client"]);
                    item.SalesPerson = iCore.IsDbNullRtNull(reader["sal_SalesPerson"]);
                    item.SalesHand = Convert.ToInt32(reader["sal_SalesHand"]);
                    item.Description = Convert.ToString(reader["sal_Description"]);
                    item.SubTotal = Convert.ToDecimal(reader["sal_SubTotal"]);
                    item.Discount = Convert.ToDecimal(reader["sal_Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["sal_vatAmount"]);
                    item.BonusAmount = Convert.ToDecimal(reader["sal_BonusAmount"]);
                    item.Total = Convert.ToDecimal(reader["sal_Total"]);
                    item.Quantity = Convert.ToDecimal(reader["sal_Quantity"]);
                    item.BonusQuantity = Convert.ToDecimal(reader["sal_BonusQuantity"]);
                    item.DeliveryKind = Convert.ToInt32(reader["sal_DeliveryKind"]);
                    item.Invoiced = Convert.ToBoolean(reader["sal_Invoiced"]);
                    item.OriginalInvoice = iCore.IsDbNullRtNull(reader["sal_OriginalInvoice"]);
                    item.Source = Convert.ToInt32(reader["sal_Source"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["sal_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["sal_Project"]);
                    item.PaymentTerms = Convert.ToString(reader["sal_PaymentTerms"]);
                    item.Terms = Convert.ToString(reader["sal_Terms"]);
                    item.CreateUserInfo = new CLiCore.CardsInfo.User().GetItem(xLan, item.CreateUser);
                    item.UpdateUserInfo = new CLiCore.CardsInfo.User().GetItem(xLan, item.LastupUser);
                }
                reader.Close();
            }
            return item;
        }
        public Quotation GetItem(string DB,   Guid? Key )
        {
            Quotation item = new Quotation();
 
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent q.*,isnull(terms.sal_PaymentTerms,'') as [sal_PaymentTerms],isnull(terms.sal_Terms,'') as [sal_Terms] from [SalesDocument_Quotation] q left join [SalesDocument_QuotationTerms] terms on q.sal_OperationKey=terms.sal_OperationKey where q.[sal_OperationKey]=@Key ";
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
                    item.RecNo = Convert.ToInt32(reader["sal_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["sal_OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["sal_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["sal_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["sal_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["sal_LastupDate"]);
                    item.Status = Convert.ToInt32(reader["sal_Status"]);
                    item.Branch = Convert.ToInt32(reader["sal_Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["sal_Prefix"]);
                    item.InvoiceNo = Convert.ToInt32(reader["sal_InvoiceNo"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["sal_InvoiceDate"]);
                    item.MonthlyNo = Convert.ToInt32(reader["sal_MonthlyNo"]);
                    item.ReferenceNo = Convert.ToString(reader["sal_ReferenceNo"]);
                    item.DueDate = iCore.IsDbNullRtNullDate(reader["sal_DueDate"]);
                    item.Client = iCore.IsDbNullRtNull(reader["sal_Client"]);
                    item.SalesPerson = iCore.IsDbNullRtNull(reader["sal_SalesPerson"]);
                    item.SalesHand = Convert.ToInt32(reader["sal_SalesHand"]);
                    item.Description = Convert.ToString(reader["sal_Description"]);
                    item.SubTotal = Convert.ToDecimal(reader["sal_SubTotal"]);
                    item.Discount = Convert.ToDecimal(reader["sal_Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["sal_vatAmount"]);
                    item.BonusAmount = Convert.ToDecimal(reader["sal_BonusAmount"]);
                    item.Total = Convert.ToDecimal(reader["sal_Total"]);
                    item.Quantity = Convert.ToDecimal(reader["sal_Quantity"]);
                    item.BonusQuantity = Convert.ToDecimal(reader["sal_BonusQuantity"]);
                    item.DeliveryKind = Convert.ToInt32(reader["sal_DeliveryKind"]);
                    item.Invoiced = Convert.ToBoolean(reader["sal_Invoiced"]);
                    item.OriginalInvoice = iCore.IsDbNullRtNull(reader["sal_OriginalInvoice"]);
                    item.Source = Convert.ToInt32(reader["sal_Source"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["sal_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["sal_Project"]);
                    item.PaymentTerms = Convert.ToString(reader["sal_PaymentTerms"]);
                    item.Terms = Convert.ToString(reader["sal_Terms"]);
            
                }
                reader.Close();
            }
            return item;
        }
        public List<Quotation> GetList(string DB,int Year)
        {
            DateTime First = new DateTime(Year, 1, 1);
            DateTime Last = new DateTime(Year, 12, 31);
            List<Quotation> items = new List<Quotation>();
            string selQuery = "select top 100 percent * from SalesDocument_Quotation where [sal_InvoiceDate]>=@FirstDate and [sal_InvoiceDate]<=@LastDate order by [sal_InvoiceDate] desc,[sal_InvoiceNo] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = First;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = Last;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Quotation item = new Quotation();
                    item.RecNo = Convert.ToInt32(reader["sal_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["sal_OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["sal_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["sal_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["sal_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["sal_LastupDate"]);
                    item.Status = Convert.ToInt32(reader["sal_Status"]);
                    item.Branch = Convert.ToInt32(reader["sal_Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["sal_Prefix"]);
                    item.InvoiceNo = Convert.ToInt32(reader["sal_InvoiceNo"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["sal_InvoiceDate"]);
                    item.MonthlyNo = Convert.ToInt32(reader["sal_MonthlyNo"]);
                    item.ReferenceNo = Convert.ToString(reader["sal_ReferenceNo"]);
                    item.DueDate = iCore.IsDbNullRtNullDate(reader["sal_DueDate"]);
                    item.Client = iCore.IsDbNullRtNull(reader["sal_Client"]);
                    item.SalesPerson = iCore.IsDbNullRtNull(reader["sal_SalesPerson"]);
                    item.SalesHand = Convert.ToInt32(reader["sal_SalesHand"]);
                    item.Description = Convert.ToString(reader["sal_Description"]);
                    item.SubTotal = Convert.ToDecimal(reader["sal_SubTotal"]);
                    item.Discount = Convert.ToDecimal(reader["sal_Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["sal_vatAmount"]);
                    item.BonusAmount = Convert.ToDecimal(reader["sal_BonusAmount"]);
                    item.Quantity = Convert.ToDecimal(reader["sal_Quantity"]);
                    item.BonusQuantity = Convert.ToDecimal(reader["sal_BonusQuantity"]);
                    item.Total = Convert.ToDecimal(reader["sal_Total"]);
                    item.DeliveryKind = Convert.ToInt32(reader["sal_DeliveryKind"]);
                    item.Invoiced = Convert.ToBoolean(reader["sal_Invoiced"]);
                    item.OriginalInvoice = iCore.IsDbNullRtNull(reader["sal_OriginalInvoice"]);
                    item.Source = Convert.ToInt32(reader["sal_Source"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["sal_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["sal_Project"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
