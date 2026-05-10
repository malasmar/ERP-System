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
    public class Transaction
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public Guid? Session { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public int Status { get; set; }
        public int Branch { get; set; }
        public Guid? Prefix { get; set; }
        public int SourceWarehouse { get; set; }
        public int TargetWarehouse { get; set; }
        public int DocumentKind { get; set; }
        public int InvoiceKind { get; set; }
        public int InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? InvoiceDateTime { get; set; }
        public int MonthlyNo { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int AccountKind { get; set; }
        public Guid? AccountKey { get; set; }
        public Guid? CurrentKey { get; set; }
        public Guid? SalesPerson { get; set; }
        public int SalesHand { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal vatAmount { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal InvoiceCost { get; set; }
        public decimal Quantity { get; set; }
        public decimal BonusQuantity { get; set; }
        public decimal Total { get; set; }
        public int DeliveryKind { get; set; }
        public Boolean IncludeFxd { get; set; }
        public Boolean IncludeExp { get; set; }
        public Guid? ImportationKey { get; set; }
        public bool Returned { get; set; }
        public Guid? OriginalInvoice { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public int Quotation { get; set; }
        public int Proforma {get;set;}
        public int Contract { get; set; }
        public decimal RetentionLess { get; set; }
        public decimal PaymentDiscount { get; set; }
        public Guid? vatKey { get; set; }



        public CLiCore.CardsInfo.User CreateUserInfo { get; set; }
        public CLiCore.CardsInfo.User UpdateUserInfo { get; set; }
        public string QR { get; set; }
        public bool IsBounsDiscount { get; set; }
        public decimal ItemsDiscount { get; set; }
        public bool ItemsDiscountKind { get; set; }
        public string ItemsDiscountText { get; set; }
        public Transaction GetItem(string DB,string xLan, Guid UserKey, Guid? Key, int DocKind)
        {
            Transaction item = new Transaction();
            item.InvoiceNo =  VoucherOperation.GetMaxInvoices(DB, DocKind);
            item.MonthlyNo = VoucherOperation.GetMaxMonthlyInvoices(DB, DateTime.Now.Year, DateTime.Now.Month, DocKind);

            item.DocumentKind = DocKind;
            item.InvoiceDate = DateTime.Now;
            item.Currency = xConfig.DefaultCurrency(DB);
            item.DueDate = DateTime.Now;
            item.CreateDate = DateTime.Now;
            item.CreateUserInfo = new CLiCore.CardsInfo.User().GetItem(xLan, UserKey);
            item.UpdateUserInfo = new CLiCore.CardsInfo.User().GetItem(xLan, UserKey);
            item.CreateUser = item.CreateUserInfo.No;
            CLiCore.Shared.UserDefaultData defaultData = CLiCore.Shared.UserDefaultData.GetItem(UserKey);
            item.SourceWarehouse = defaultData.WarehouseNo;
            item.Prefix = defaultData.Prefix;
            item.Branch= defaultData.Branch;
            item.Project= defaultData.Project;
            item.CostCenter= defaultData.CostCenter;
            item.ItemsDiscountText = "0";

            if (DocKind == (int)CLiCore.DocumentKind.ConsumptionStock || DocKind == (int)CLiCore.DocumentKind.RetConsumptionStock)
                item.AccountKind = (int)PLenums.TransactionAccount.Expenses;

            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from dbo.fninvDocuments_Transaction(@Key) ";
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
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.OriginalInvoice = iCore.IsDbNullRtNull(reader["OriginalInvoice"]);
                    item.CreateUser = Convert.ToInt32(reader["CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["LastupDate"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Branch = Convert.ToInt32(reader["Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["Prefix"]);
                    item.SourceWarehouse = Convert.ToInt32(reader["SourceWarehouse"]);
                    item.TargetWarehouse = Convert.ToInt32(reader["TargetWarehouse"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.InvoiceKind = Convert.ToInt32(reader["InvoiceKind"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.InvoiceDateTime = iCore.IsDbNullRtNullDate(reader["InvoiceDatetime"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.ReferenceNo = Convert.ToString(reader["ReferenceNo"]);
                    item.ReferenceDate = iCore.IsDbNullRtNullDate(reader["ReferenceDate"]);
                    item.DueDate = iCore.IsDbNullRtNullDate(reader["DueDate"]);
                    item.AccountKind = Convert.ToInt32(reader["AccountKind"]);
                    item.AccountKey = iCore.IsDbNullRtNull(reader["AccountKey"]);
                    item.CurrentKey = iCore.IsDbNullRtNull(reader["CurrentKey"]);
                    item.SalesPerson = iCore.IsDbNullRtNull(reader["SalesPerson"]);
                    item.SalesHand = Convert.ToInt32(reader["SalesHand"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Currency = Convert.ToString(reader["Currency"]);
                    item.SubTotal = Convert.ToDecimal(reader["SubTotal"]);
                    item.Discount = Convert.ToDecimal(reader["Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.BonusAmount = Convert.ToDecimal(reader["BonusAmount"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.InvoiceCost = Convert.ToDecimal(reader["InvoiceCost"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.BonusQuantity = Convert.ToDecimal(reader["BonusQuantity"]);
                    item.DeliveryKind = Convert.ToInt32(reader["DeliveryKind"]);
                    item.IncludeFxd = Convert.ToBoolean(reader["IncludeFxd"]);
                    item.IncludeExp = Convert.ToBoolean(reader["IncludeExp"]);
                    item.ImportationKey = iCore.IsDbNullRtNull(reader["ImportationKey"]);
                    item.Session = iCore.IsDbNullRtNull(reader["Session"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Project"]);
                    item.Quotation = Convert.ToInt32(reader["Quotation"]);
                    item.Proforma = Convert.ToInt32(reader["Proforma"]);
                    item.Contract = Convert.ToInt32(reader["Contract"]);
                    item.RetentionLess = Convert.ToDecimal(reader["RetentionLess"]);
                    item.PaymentDiscount = Convert.ToDecimal(reader["PaymentDiscount"]);
                    item.CreateUserInfo = new CLiCore.CardsInfo.User().GetItem(xLan, item.CreateUser);
                    item.UpdateUserInfo = new CLiCore.CardsInfo.User().GetItem(xLan, item.LastupUser);
                    item.QR = iCore.QR(DB, item.InvoiceDate.Value, item.Total, item.vatAmount);
                }
                reader.Close();
            }
            return item;
        }

        public Transaction GetItem(string DB, Guid? Key)
        {
            Transaction item = new Transaction();
          
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from dbo.fninvDocuments_Transaction(@Key) ";
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
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.OriginalInvoice = iCore.IsDbNullRtNull(reader["OriginalInvoice"]);
                    item.CreateUser = Convert.ToInt32(reader["CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["LastupDate"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Branch = Convert.ToInt32(reader["Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["Prefix"]);
                    item.SourceWarehouse = Convert.ToInt32(reader["SourceWarehouse"]);
                    item.TargetWarehouse = Convert.ToInt32(reader["TargetWarehouse"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.InvoiceKind = Convert.ToInt32(reader["InvoiceKind"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.InvoiceDateTime = iCore.IsDbNullRtNullDate(reader["InvoiceDatetime"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.ReferenceNo = Convert.ToString(reader["ReferenceNo"]);
                    item.ReferenceDate = iCore.IsDbNullRtNullDate(reader["ReferenceDate"]);
                    item.DueDate = iCore.IsDbNullRtNullDate(reader["DueDate"]);
                    item.AccountKind = Convert.ToInt32(reader["AccountKind"]);
                    item.AccountKey = iCore.IsDbNullRtNull(reader["AccountKey"]);
                    item.CurrentKey = iCore.IsDbNullRtNull(reader["CurrentKey"]);
                    item.SalesPerson = iCore.IsDbNullRtNull(reader["SalesPerson"]);
                    item.SalesHand = Convert.ToInt32(reader["SalesHand"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Currency = Convert.ToString(reader["Currency"]);
                    item.SubTotal = Convert.ToDecimal(reader["SubTotal"]);
                    item.Discount = Convert.ToDecimal(reader["Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.BonusAmount = Convert.ToDecimal(reader["BonusAmount"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.InvoiceCost = Convert.ToDecimal(reader["InvoiceCost"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.BonusQuantity = Convert.ToDecimal(reader["BonusQuantity"]);
                    item.DeliveryKind = Convert.ToInt32(reader["DeliveryKind"]);
                    item.IncludeFxd = Convert.ToBoolean(reader["IncludeFxd"]);
                    item.IncludeExp = Convert.ToBoolean(reader["IncludeExp"]);
                    item.ImportationKey = iCore.IsDbNullRtNull(reader["ImportationKey"]);
                    item.Session = iCore.IsDbNullRtNull(reader["Session"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Project"]);
                    item.Quotation = Convert.ToInt32(reader["Quotation"]);
                    item.Proforma = Convert.ToInt32(reader["Proforma"]);
                    item.Contract = Convert.ToInt32(reader["Contract"]);
                    item.RetentionLess = Convert.ToInt32(reader["RetentionLess"]);
                    item.PaymentDiscount = Convert.ToInt32(reader["PaymentDiscount"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
