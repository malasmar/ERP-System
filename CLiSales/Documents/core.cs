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
    public class core
    {
        private readonly static object Locker = new object();
        public static CLiCore.OperationResult UpdateQuotation(string DB, Documents.Quotation Header, List<Documents.QuotationDetails> Details, bool IsNew)
        {
            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;
                if (IsNew == false)
                {
                    VoucherNo = Header.InvoiceNo;
                    opk = Header.OperationKey;
                }
                else
                {
                    opk = Guid.NewGuid();
                    VoucherNo = VoucherOperation.GetMaxQuotation(DB);
                    Header.MonthlyNo = VoucherOperation.GetMaxMonthlyQuotation(DB,Header.InvoiceDate.Value.Year,Header.InvoiceDate.Value.Month);
                }
                using (SqlConnection conn = new SqlConnection(iCore.GetCon(DB)))
                {
                    System.Text.StringBuilder str = new System.Text.StringBuilder();
                    conn.Open();
                    SqlCommand comm = conn.CreateCommand();
                    SqlTransaction transaction;
                    transaction = conn.BeginTransaction("Transaction");
                    comm.Connection = conn;
                    comm.Transaction = transaction;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = " exec dbo.spSalesDocument_DeleteQuotationUpdate @Key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();
                    //Header Data
                    str.Clear();
                    str.Append("INSERT INTO SalesDocument_Quotation");
                    str.Append("([sal_OperationKey]");
                    str.Append(",[sal_CreateUser]");
                    str.Append(",[sal_CreateDate]");
                    str.Append(",[sal_LastupUser]");
                    str.Append(",[sal_LastupDate]");
                    str.Append(",[sal_Status]");
                    str.Append(",[sal_Branch]");
                    str.Append(",[sal_Prefix]");
                    str.Append(",[sal_InvoiceNo]");
                    str.Append(",[sal_InvoiceDate]");
                    str.Append(",[sal_MonthlyNo]");
                    str.Append(",[sal_ReferenceNo]");
                    str.Append(",[sal_DueDate]");
                    str.Append(",[sal_Client]");
                    str.Append(",[sal_SalesPerson]");
                    str.Append(",[sal_SalesHand]");
                    str.Append(",[sal_Description]");
                    str.Append(",[sal_SubTotal]");
                    str.Append(",[sal_Discount]");
                    str.Append(",[sal_vatAmount]");
                    str.Append(",[sal_BonusAmount]");
                    str.Append(",[sal_Quantity]");
                    str.Append(",[sal_BonusQuantity]");
                    str.Append(",[sal_Total]");
                    str.Append(",[sal_DeliveryKind]");
                    str.Append(",[sal_Invoiced]");
                    str.Append(",[sal_OriginalInvoice]");
                    str.Append(",[sal_Source]");
                    str.Append(",[sal_CostCenter]");
                    str.Append(",[sal_Project])");
                    str.Append(" VALUES ");
                    str.Append("(@sal_OperationKey");
                    str.Append(",@sal_CreateUser");
                    str.Append(",@sal_CreateDate");
                    str.Append(",@sal_LastupUser");
                    str.Append(",@sal_LastupDate");
                    str.Append(",@sal_Status");
                    str.Append(",@sal_Branch");
                    str.Append(",@sal_Prefix");
                    str.Append(",@sal_InvoiceNo");
                    str.Append(",@sal_InvoiceDate");
                    str.Append(",@sal_MonthlyNo");
                    str.Append(",@sal_ReferenceNo");
                    str.Append(",@sal_DueDate");
                    str.Append(",@sal_Client");
                    str.Append(",@sal_SalesPerson");
                    str.Append(",@sal_SalesHand");
                    str.Append(",@sal_Description");
                    str.Append(",@sal_SubTotal");
                    str.Append(",@sal_Discount");
                    str.Append(",@sal_vatAmount");
                    str.Append(",@sal_BonusAmount");
                    str.Append(",@sal_Quantity");
                    str.Append(",@sal_BonusQuantity");
                    str.Append(",@sal_Total");
                    str.Append(",@sal_DeliveryKind");
                    str.Append(",@sal_Invoiced");
                    str.Append(",@sal_OriginalInvoice");
                    str.Append(",@sal_Source");
                    str.Append(",@sal_CostCenter");
                    str.Append(",@sal_Project)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@sal_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@sal_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@sal_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@sal_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@sal_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@sal_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@sal_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@sal_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@sal_InvoiceNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@sal_InvoiceDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                    comm.Parameters.Add("@sal_MonthlyNo", SqlDbType.Int).Value = Header.MonthlyNo;
                    comm.Parameters.Add("@sal_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@sal_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.DueDate)  ;
                    comm.Parameters.Add("@sal_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Client);
                    comm.Parameters.Add("@sal_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                    comm.Parameters.Add("@sal_SalesHand", SqlDbType.Int).Value = Header.SalesHand;
                    comm.Parameters.Add("@sal_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@sal_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x=>x.Amount);
                    comm.Parameters.Add("@sal_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                    comm.Parameters.Add("@sal_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@sal_BonusAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus * x.UnitPrice);
                    comm.Parameters.Add("@sal_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@sal_BonusQuantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus);
                    comm.Parameters.Add("@sal_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                    comm.Parameters.Add("@sal_DeliveryKind", SqlDbType.Int).Value = Header.DeliveryKind;
                    comm.Parameters.Add("@sal_Invoiced", SqlDbType.Bit).Value = Header.Invoiced;
                    comm.Parameters.Add("@sal_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                    comm.Parameters.Add("@sal_Source", SqlDbType.Int).Value = Header.Source;
                    comm.Parameters.Add("@sal_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@sal_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();

                    foreach (QuotationDetails item in Details)
                    {
                        str.Clear();
                        str.Append("INSERT INTO SalesDocument_QuotationDetails");
                        str.Append("([sal_OperationKey]");
                        str.Append(",[sal_Key]");
                        str.Append(",[sal_Index]");
                        str.Append(",[sal_ItemKind]");
                        str.Append(",[sal_ItemKey]");
                        str.Append(",[sal_Color]");
                        str.Append(",[sal_Size]");
                        str.Append(",[sal_Unit]");
                        str.Append(",[sal_UnitPrice]");
                        str.Append(",[sal_Quantity]");
                        str.Append(",[sal_Bonus]");
                        str.Append(",[sal_Amount]");
                        str.Append(",[sal_Discount]");
                        str.Append(",[sal_vatAmount]");
                        str.Append(",[sal_vatKey]");
                        str.Append(",[sal_vatRate]");
                        str.Append(",[sal_Total]");
                        str.Append(",[sal_Description])");
                        str.Append(" VALUES ");
                        str.Append("(@sal_OperationKey");
                        str.Append(",@sal_Key");
                        str.Append(",@sal_Index");
                        str.Append(",@sal_ItemKind");
                        str.Append(",@sal_ItemKey");
                        str.Append(",@sal_Color");
                        str.Append(",@sal_Size");
                        str.Append(",@sal_Unit");
                        str.Append(",@sal_UnitPrice");
                        str.Append(",@sal_Quantity");
                        str.Append(",@sal_Bonus");
                        str.Append(",@sal_Amount");
                        str.Append(",@sal_Discount");
                        str.Append(",@sal_vatAmount");
                        str.Append(",@sal_vatKey");
                        str.Append(",@sal_vatRate");
                        str.Append(",@sal_Total");
                        str.Append(",@sal_Description)");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@sal_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@sal_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@sal_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@sal_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                        comm.Parameters.Add("@sal_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        comm.Parameters.Add("@sal_Color", SqlDbType.Int).Value = item.Color;
                        comm.Parameters.Add("@sal_Size", SqlDbType.Int).Value = item.Size;
                        comm.Parameters.Add("@sal_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@sal_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@sal_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@sal_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                        comm.Parameters.Add("@sal_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@sal_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@sal_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@sal_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@sal_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@sal_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@sal_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.ExecuteNonQuery();
                    }

                    str.Clear();
                    str.Append("INSERT INTO SalesDocument_QuotationTerms");
                    str.Append("([sal_OperationKey]");
                    str.Append(",[sal_PaymentTerms]");
                    str.Append(",[sal_Terms])");
                    str.Append(" VALUES ");
                    str.Append("(@sal_OperationKey");
                    str.Append(",@sal_PaymentTerms");
                    str.Append(",@sal_Terms)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@sal_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@sal_PaymentTerms", SqlDbType.NVarChar,-1).Value = Header.PaymentTerms??"";
                    comm.Parameters.Add("@sal_Terms", SqlDbType.NVarChar, -1).Value = Header.Terms??"";
                    comm.ExecuteNonQuery();

                    str.Clear();
                    str.Append("INSERT INTO system_DocumentEdited");
                    str.Append("([sys_Key]");
                    str.Append(",[sys_User]");
                    str.Append(",[sys_Date])");
                    str.Append(" VALUES ");
                    str.Append("(@sys_Key");
                    str.Append(",@sys_User");
                    str.Append(",@sys_Date)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@sys_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@sys_User", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@sys_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                    comm.ExecuteNonQuery();

                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
                OperationResult res = new OperationResult();
                res.OperationKey = opk;
                res.VoucherNo = VoucherNo;
                return res;
            }
        }
        public static CLiCore.OperationResult UpdateProformaInvoice(string DB, Documents.Proforma Header, List<Documents.ProformaDetails> Details, bool IsNew)
        {
            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;
                if (IsNew == false)
                {
                    VoucherNo = Header.InvoiceNo;
                    opk = Header.OperationKey;
                }
                else
                {
                    opk = Guid.NewGuid();
                    VoucherNo = VoucherOperation.GetMaxProforma(DB);
                    Header.MonthlyNo = VoucherOperation.GetMaxMonthlyProforma(DB, Header.InvoiceDate.Value.Year, Header.InvoiceDate.Value.Month);
                }
                using (SqlConnection conn = new SqlConnection(iCore.GetCon(DB)))
                {
                    System.Text.StringBuilder str = new System.Text.StringBuilder();
                    conn.Open();
                    SqlCommand comm = conn.CreateCommand();
                    SqlTransaction transaction;
                    transaction = conn.BeginTransaction("Transaction");
                    comm.Connection = conn;
                    comm.Transaction = transaction;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = " exec dbo.spSalesDocument_DeleteProformaUpdate @Key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();
                    //Header Data
                    str.Clear();
                    str.Append("INSERT INTO SalesDocument_Proforma");
                    str.Append("([sal_OperationKey]");
                    str.Append(",[sal_CreateUser]");
                    str.Append(",[sal_CreateDate]");
                    str.Append(",[sal_LastupUser]");
                    str.Append(",[sal_LastupDate]");
                    str.Append(",[sal_Status]");
                    str.Append(",[sal_Branch]");
                    str.Append(",[sal_Prefix]");
                    str.Append(",[sal_InvoiceNo]");
                    str.Append(",[sal_InvoiceDate]");
                    str.Append(",[sal_MonthlyNo]");
                    str.Append(",[sal_ReferenceNo]");
                    str.Append(",[sal_DueDate]");
                    str.Append(",[sal_Client]");
                    str.Append(",[sal_SalesPerson]");
                    str.Append(",[sal_SalesHand]");
                    str.Append(",[sal_Description]");
                    str.Append(",[sal_SubTotal]");
                    str.Append(",[sal_Discount]");
                    str.Append(",[sal_vatAmount]");
                    str.Append(",[sal_BonusAmount]");
                    str.Append(",[sal_Quantity]");
                    str.Append(",[sal_BonusQuantity]");
                    str.Append(",[sal_Total]");
                    str.Append(",[sal_DeliveryKind]");
                    str.Append(",[sal_Invoiced]");
                    str.Append(",[sal_OriginalInvoice]");
                    str.Append(",[sal_CostCenter]");
                    str.Append(",[sal_Project])");
                    str.Append(" VALUES ");
                    str.Append("(@sal_OperationKey");
                    str.Append(",@sal_CreateUser");
                    str.Append(",@sal_CreateDate");
                    str.Append(",@sal_LastupUser");
                    str.Append(",@sal_LastupDate");
                    str.Append(",@sal_Status");
                    str.Append(",@sal_Branch");
                    str.Append(",@sal_Prefix");
                    str.Append(",@sal_InvoiceNo");
                    str.Append(",@sal_InvoiceDate");
                    str.Append(",@sal_MonthlyNo");
                    str.Append(",@sal_ReferenceNo");
                    str.Append(",@sal_DueDate");
                    str.Append(",@sal_Client");
                    str.Append(",@sal_SalesPerson");
                    str.Append(",@sal_SalesHand");
                    str.Append(",@sal_Description");
                    str.Append(",@sal_SubTotal");
                    str.Append(",@sal_Discount");
                    str.Append(",@sal_vatAmount");
                    str.Append(",@sal_BonusAmount");
                    str.Append(",@sal_Quantity");
                    str.Append(",@sal_BonusQuantity");
                    str.Append(",@sal_Total");
                    str.Append(",@sal_DeliveryKind");
                    str.Append(",@sal_Invoiced");
                    str.Append(",@sal_OriginalInvoice");
                    str.Append(",@sal_CostCenter");
                    str.Append(",@sal_Project)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@sal_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@sal_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@sal_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@sal_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@sal_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@sal_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@sal_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@sal_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@sal_InvoiceNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@sal_InvoiceDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                    comm.Parameters.Add("@sal_MonthlyNo", SqlDbType.Int).Value = Header.MonthlyNo;
                    comm.Parameters.Add("@sal_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@sal_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.DueDate);
                    comm.Parameters.Add("@sal_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Client);
                    comm.Parameters.Add("@sal_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                    comm.Parameters.Add("@sal_SalesHand", SqlDbType.Int).Value = Header.SalesHand;
                    comm.Parameters.Add("@sal_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@sal_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@sal_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                    comm.Parameters.Add("@sal_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@sal_BonusAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus * x.UnitPrice);
                    comm.Parameters.Add("@sal_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@sal_BonusQuantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus);
                    comm.Parameters.Add("@sal_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                    comm.Parameters.Add("@sal_DeliveryKind", SqlDbType.Int).Value = Header.DeliveryKind;
                    comm.Parameters.Add("@sal_Invoiced", SqlDbType.Bit).Value = Header.Invoiced;
                    comm.Parameters.Add("@sal_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                    comm.Parameters.Add("@sal_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@sal_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();
                    foreach (ProformaDetails item in Details)
                    {
                        str.Clear();
                        str.Append("INSERT INTO SalesDocument_ProformaDetails");
                        str.Append("([sal_OperationKey]");
                        str.Append(",[sal_Key]");
                        str.Append(",[sal_Index]");
                        str.Append(",[sal_ItemKind]");
                        str.Append(",[sal_ItemKey]");
                        str.Append(",[sal_Color]");
                        str.Append(",[sal_Size]");
                        str.Append(",[sal_Unit]");
                        str.Append(",[sal_UnitPrice]");
                        str.Append(",[sal_Quantity]");
                        str.Append(",[sal_Bonus]");
                        str.Append(",[sal_Amount]");
                        str.Append(",[sal_Discount]");
                        str.Append(",[sal_vatAmount]");
                        str.Append(",[sal_vatKey]");
                        str.Append(",[sal_vatRate]");
                        str.Append(",[sal_Total]");
                        str.Append(",[sal_Description])");
                        str.Append(" VALUES ");
                        str.Append("(@sal_OperationKey");
                        str.Append(",@sal_Key");
                        str.Append(",@sal_Index");
                        str.Append(",@sal_ItemKind");
                        str.Append(",@sal_ItemKey");
                        str.Append(",@sal_Color");
                        str.Append(",@sal_Size");
                        str.Append(",@sal_Unit");
                        str.Append(",@sal_UnitPrice");
                        str.Append(",@sal_Quantity");
                        str.Append(",@sal_Bonus");
                        str.Append(",@sal_Amount");
                        str.Append(",@sal_Discount");
                        str.Append(",@sal_vatAmount");
                        str.Append(",@sal_vatKey");
                        str.Append(",@sal_vatRate");
                        str.Append(",@sal_Total");
                        str.Append(",@sal_Description)");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@sal_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@sal_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@sal_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@sal_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                        comm.Parameters.Add("@sal_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        comm.Parameters.Add("@sal_Color", SqlDbType.Int).Value = item.Color;
                        comm.Parameters.Add("@sal_Size", SqlDbType.Int).Value = item.Size;
                        comm.Parameters.Add("@sal_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@sal_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@sal_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@sal_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                        comm.Parameters.Add("@sal_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@sal_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@sal_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@sal_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@sal_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@sal_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@sal_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.ExecuteNonQuery();
                    }

                    str.Clear();
                    str.Append("INSERT INTO system_DocumentEdited");
                    str.Append("([sys_Key]");
                    str.Append(",[sys_User]");
                    str.Append(",[sys_Date])");
                    str.Append(" VALUES ");
                    str.Append("(@sys_Key");
                    str.Append(",@sys_User");
                    str.Append(",@sys_Date)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@sys_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@sys_User", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@sys_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                    comm.ExecuteNonQuery();

                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
                OperationResult res = new OperationResult();
                res.OperationKey = opk;
                res.VoucherNo = VoucherNo;
                return res;
            }
        }
        public static CLiCore.OperationResult UpdateContract(string DB, Documents.Contract Header, List<Documents.ContractDetails> Details, bool IsNew)
        {
            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;
                if (IsNew == false)
                {
                    VoucherNo = Header.InvoiceNo;
                    opk = Header.OperationKey;
                }
                else
                {
                    opk = Guid.NewGuid();
                    VoucherNo =  VoucherOperation.GetMaxContract(DB);
                 
                }
                using (SqlConnection conn = new SqlConnection(iCore.GetCon(DB)))
                {
                    System.Text.StringBuilder str = new System.Text.StringBuilder();
                    conn.Open();
                    SqlCommand comm = conn.CreateCommand();
                    SqlTransaction transaction;
                    transaction = conn.BeginTransaction("Transaction");
                    comm.Connection = conn;
                    comm.Transaction = transaction;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = " exec dbo.spSalesDocument_DeleteContractUpdate @Key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();
                    //Header Data
                    str.Clear();
                    str.Append("INSERT INTO SalesDocument_Contract");
                    str.Append("([sal_OperationKey]");
                    str.Append(",[sal_CreateUser]");
                    str.Append(",[sal_CreateDate]");
                    str.Append(",[sal_LastupUser]");
                    str.Append(",[sal_LastupDate]");
                    str.Append(",[sal_Status]");
                    str.Append(",[sal_Branch]");
                    str.Append(",[sal_Prefix]");
                    str.Append(",[sal_InvoiceNo]");
                    str.Append(",[sal_InvoiceDate]");
                    str.Append(",[sal_ReferenceNo]");
                    str.Append(",[sal_Client]");
                    str.Append(",[sal_SalesPerson]");
                    str.Append(",[sal_SalesHand]");
                    str.Append(",[sal_Description]");
                    str.Append(",[sal_SubTotal]");
                    str.Append(",[sal_Discount]");
                    str.Append(",[sal_vatAmount]");
                    str.Append(",[sal_Total]");
                    str.Append(",[sal_Quantity]");
                    str.Append(",[sal_Invoiced]");
                    str.Append(",[sal_CostCenter]");
                    str.Append(",[sal_Project])");
                    str.Append(" VALUES ");
                    str.Append("(@sal_OperationKey");
                    str.Append(",@sal_CreateUser");
                    str.Append(",@sal_CreateDate");
                    str.Append(",@sal_LastupUser");
                    str.Append(",@sal_LastupDate");
                    str.Append(",@sal_Status");
                    str.Append(",@sal_Branch");
                    str.Append(",@sal_Prefix");
                    str.Append(",@sal_InvoiceNo");
                    str.Append(",@sal_InvoiceDate");
                    str.Append(",@sal_ReferenceNo");
                    str.Append(",@sal_Client");
                    str.Append(",@sal_SalesPerson");
                    str.Append(",@sal_SalesHand");
                    str.Append(",@sal_Description");
                    str.Append(",@sal_SubTotal");
                    str.Append(",@sal_Discount");
                    str.Append(",@sal_vatAmount");
                    str.Append(",@sal_Total");
                    str.Append(",@sal_Quantity");
                    str.Append(",@sal_Invoiced");
                    str.Append(",@sal_CostCenter");
                    str.Append(",@sal_Project)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@sal_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@sal_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@sal_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@sal_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@sal_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@sal_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@sal_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@sal_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@sal_InvoiceNo", SqlDbType.Int).Value = Header.InvoiceNo;
                    comm.Parameters.Add("@sal_InvoiceDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                    comm.Parameters.Add("@sal_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@sal_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Client);
                    comm.Parameters.Add("@sal_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                    comm.Parameters.Add("@sal_SalesHand", SqlDbType.Int).Value = Header.SalesHand;
                    comm.Parameters.Add("@sal_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@sal_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@sal_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                    comm.Parameters.Add("@sal_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@sal_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                    comm.Parameters.Add("@sal_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@sal_Invoiced", SqlDbType.Bit).Value = Header.Invoiced;
                    comm.Parameters.Add("@sal_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@sal_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();
                    foreach (ContractDetails item in Details)
                    {
                        str.Clear();
                        str.Append("INSERT INTO SalesDocument_ContractDetails");
                        str.Append("([sal_OperationKey]");
                        str.Append(",[sal_Key]");
                        str.Append(",[sal_Index]");
                        str.Append(",[sal_ItemKind]");
                        str.Append(",[sal_ItemKey]");
                        str.Append(",[sal_Unit]");
                        str.Append(",[sal_UnitPrice]");
                        str.Append(",[sal_Quantity]");
                        str.Append(",[sal_Amount]");
                        str.Append(",[sal_Discount]");
                        str.Append(",[sal_vatAmount]");
                        str.Append(",[sal_vatKey]");
                        str.Append(",[sal_vatRate]");
                        str.Append(",[sal_Total]");
                        str.Append(",[sal_Description])");
                        str.Append(" VALUES ");
                        str.Append("(@sal_OperationKey");
                        str.Append(",@sal_Key");
                        str.Append(",@sal_Index");
                        str.Append(",@sal_ItemKind");
                        str.Append(",@sal_ItemKey");
                        str.Append(",@sal_Unit");
                        str.Append(",@sal_UnitPrice");
                        str.Append(",@sal_Quantity");
                        str.Append(",@sal_Amount");
                        str.Append(",@sal_Discount");
                        str.Append(",@sal_vatAmount");
                        str.Append(",@sal_vatKey");
                        str.Append(",@sal_vatRate");
                        str.Append(",@sal_Total");
                        str.Append(",@sal_Description)");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@sal_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@sal_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@sal_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@sal_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                        comm.Parameters.Add("@sal_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        comm.Parameters.Add("@sal_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@sal_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@sal_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@sal_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@sal_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@sal_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@sal_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@sal_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@sal_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@sal_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.ExecuteNonQuery();
                    }

                    str.Clear();
                    str.Append("INSERT INTO system_DocumentEdited");
                    str.Append("([sys_Key]");
                    str.Append(",[sys_User]");
                    str.Append(",[sys_Date])");
                    str.Append(" VALUES ");
                    str.Append("(@sys_Key");
                    str.Append(",@sys_User");
                    str.Append(",@sys_Date)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@sys_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@sys_User", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@sys_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                    comm.ExecuteNonQuery();

                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
                OperationResult res = new OperationResult();
                res.OperationKey = opk;
                res.VoucherNo = VoucherNo;
                return res;
            }
        }
    }
}
