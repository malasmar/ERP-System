using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiMobile.Sales.SyncPull
{
    public class core
    {
        private readonly static object Locker = new object();
        public static int UpdateQuotation(string DB, Quotation Header, List<QuotationDetails> Details, Guid user)
        {
            lock (Locker)
            {
                int res = 0;
                Guid? opk;
                int VoucherNo;

                opk = Guid.NewGuid();
                VoucherNo = 1;// xcore.MaxTransaction(DB, Header.DocumentKind, Header.InvoiceDate.Value.Year);
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
                    str.Append(",[sal_Total]");
                    str.Append(",[sal_Quantity]");
                    str.Append(",[sal_BonusQuantity]");
                    str.Append(",[sal_DeliveryKind]");
                    str.Append(",[sal_Invoiced]");
                    str.Append(",[sal_OriginalInvoice]");
                    str.Append(",[sal_Source])");
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
                    str.Append(",@sal_Source)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@sal_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@sal_CreateUser", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@sal_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.VoucherDate);
                    comm.Parameters.Add("@sal_LastupUser", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@sal_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.VoucherDate);
                    comm.Parameters.Add("@sal_Status", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@sal_Branch", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@sal_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@sal_InvoiceNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@sal_InvoiceDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.VoucherDate);
                    comm.Parameters.Add("@sal_MonthlyNo", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@sal_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@sal_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.VoucherDate);
                    comm.Parameters.Add("@sal_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Client);
                    comm.Parameters.Add("@sal_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(user);
                    comm.Parameters.Add("@sal_SalesHand", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@sal_Description", SqlDbType.NVarChar, 500).Value = "";
                    comm.Parameters.Add("@sal_SubTotal", SqlDbType.Decimal).Value = Header.Subtotal;
                    comm.Parameters.Add("@sal_Discount", SqlDbType.Decimal).Value = Header.Discount;
                    comm.Parameters.Add("@sal_vatAmount", SqlDbType.Decimal).Value = Header.VatAmount;
                    comm.Parameters.Add("@sal_BonusAmount", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@sal_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@sal_BonusQuantity", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@sal_Total", SqlDbType.Decimal).Value = Header.Total;
                    comm.Parameters.Add("@sal_DeliveryKind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@sal_Invoiced", SqlDbType.Bit).Value = false;
                    comm.Parameters.Add("@sal_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@sal_Source", SqlDbType.Int).Value = 1;
                    comm.ExecuteNonQuery();

                    int i = 0;
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
                        str.Append(",[inv_Description])");
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
                        str.Append(",@inv_Description)");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@sal_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@sal_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@sal_Index", SqlDbType.Int).Value = i;
                        comm.Parameters.Add("@sal_ItemKind", SqlDbType.Int).Value = 8;
                        comm.Parameters.Add("@sal_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Product);
                        comm.Parameters.Add("@sal_Color", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@sal_Size", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@sal_Unit", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@sal_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@sal_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@sal_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                        comm.Parameters.Add("@sal_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@sal_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@sal_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@sal_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@sal_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@sal_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = "";
                        comm.ExecuteNonQuery();
                        ++i;
                    }

                    try
                    {
                        transaction.Commit();
                        res = 1;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        res = 0;
                    }
                }


                return res;
            }
        }
        public static int UpdateSalesInvoice(string DB, Invoice Header, List<InvoiceDetails> Details, List<InvoicePaymentMethod> Methods)
        {
            lock (Locker)
            {
                Guid? opk;
                opk = Guid.NewGuid();
                bool DC = xCore.HeaderDebitOrCredit((int)DocumentKind.SalesInvoice);
                int index = 0;
                Guid? CostCenter=null;
                Guid? Project=null;
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
                     
                    //Header Data
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Session);
                    comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.VoucherDate);
                    comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.VoucherDate);
                    comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                    comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = (int)DocumentKind.SalesInvoice;
                    comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.VoucherDate);
                    comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null); ;
                    comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null); ;
                    comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = Header.IsCredit == true ? 0 : 2;
                    comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Client);
                    comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                    comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = "";
                    comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = "SAR";
                    comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                    comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus * (x.UnitPrice - x.Discount));
                    comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount - x.Discount + x.vatAmount);
                    comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = Details.Sum(x => x.UnitCost);
                    comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus);
                    comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Fixture).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = false;
                    comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 1;
                    comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = Header.IsCredit;
                    comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(CostCenter);
                    comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Project);
                    comm.ExecuteNonQuery();

                    //Financial Header
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@Fin_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.VoucherDate);
                    comm.Parameters.Add("@Fin_LastupUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@Fin_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.VoucherDate);
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@Fin_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = (int)DocumentKind.SalesInvoice;
                    comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = Header.VoucherDate;
                    comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = "";
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = "SAR";
                    comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                    comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                    comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false;
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(CostCenter);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Project);
                    comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value =  "";
                    comm.ExecuteNonQuery();

                    foreach (InvoiceDetails item in Details)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = -1;
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                        comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Product);
                        comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                        comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                        comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = "";
                        comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value =iCore.IsNullRtDbNull(CostCenter);
                        comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Project);
                        comm.ExecuteNonQuery();

                        if (item.ItemKind == (int)PLenums.TransactionAccount.Revenue || item.ItemKind == (int)PLenums.TransactionAccount.Expenses || item.ItemKind == (int)PLenums.TransactionAccount.Fixture)
                        {
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                            comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = item.ItemKind;
                            comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Product);
                            comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value =iCore.IsNullRtDbNull(CostCenter);
                            comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value =  iCore.IsNullRtDbNull(Project);
                            comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = "";
                            comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = item.Amount;
                            comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                            comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                            comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                            comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = item.Total;
                            comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = item.Amount;
                            comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                            comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = item.Total;
                            comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                            comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                            comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                            comm.ExecuteNonQuery();
                            ++index;
                        }
                    }

                    #region "Close Accounts Financial"
                    //Close Account
                    if (Header.IsCredit)
                    {
                        ++index;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Client);
                        comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = "";
                        comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                        comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                        comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                        comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                        comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                        comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                        comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                        comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                        comm.ExecuteNonQuery();
                    }
                    foreach(InvoicePaymentMethod method in Methods)
                    {
                        Data.PaymentMethod payment = new Data.PaymentMethod().GetItem(DB, method.MethodKey);
                        ++index;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = payment.AccountKind;
                        comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(payment.Account);
                        comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = "";
                        comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = method.Amount;
                        comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = method.Amount;
                        comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = method.Amount;
                        comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = method.Amount;
                        comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                        comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                        comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                        comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                        comm.ExecuteNonQuery();

                        str.Clear();
                        str.Append("INSERT INTO InvDocument_TransactionPaymentMethods");
                        str.Append("([inv_OperationKey]");
                        str.Append(",[inv_Session]");
                        str.Append(",[inv_Method]");
                        str.Append(",[inv_Amount])");
                        str.Append(" VALUES ");
                        str.Append("(@inv_OperationKey");
                        str.Append(",@inv_Session");
                        str.Append(",@inv_Method");
                        str.Append(",@inv_Amount)");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(method.Session);
                        comm.Parameters.Add("@inv_Method", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(method.MethodKey);
                        comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = method.Amount;
                        comm.ExecuteNonQuery();
                    }

                    //Revenue
                    ++index;
                    Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Revenue;
                    comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(accounts.SalesAccount);
                    comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = "";
                    comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = true;
                    comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).Sum(x => x.Amount);
                    comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).Sum(x => x.Amount);
                    comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).Sum(x => x.Amount);
                    comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).Sum(x => x.Amount);
                    comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                    comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                    comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                    comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                    comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                    comm.ExecuteNonQuery();

                    if (Details.Sum(x => x.Discount) > 0 || Details.Sum(x => x.Bonus) > 0)
                    {
                        ++index;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Expenses;
                        comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(xConfig.SalesDiscountAccount(DB));
                        comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = "";
                        comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                        comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                        comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                        comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                        comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                        comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                        comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                        comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                        comm.ExecuteNonQuery();
                    }

                    //VAT
                    List<InvoiceDetails> vatDetails;
                    vatDetails = Details.Where(x => x.vatAmount > 0).ToList();

                    if (vatDetails.Sum(x => x.vatAmount) > 0)
                    {
                        var vatGroup = vatDetails.GroupBy(x => x.vatKey);
                        foreach (var vat in vatGroup)
                        {
                            if (vatDetails.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount) > 0)
                            {
                                ++index;
                                comm.CommandType = CommandType.Text;
                                comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                                comm.Parameters.Clear();
                                comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                                comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                                comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                                comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = 0;
                                comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                                comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                                comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                                comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = 0;
                                comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = PLenums.CurrentAccountKind.Adjustment;
                                comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(xConfig.vatKey(DB));
                                comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                                comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                                comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                                comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                                comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                                comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                                comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                                comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = "";
                                comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = true;
                                comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = vatDetails.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                                comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(vat.Key);
                                comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = xConfig.vatRate(DB, vat.Key);
                                comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                                comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = vatDetails.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                                comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = vatDetails.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                                comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                                comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = vatDetails.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                                comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = true;
                                comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                                comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                                comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                                comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                                comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                                comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                                comm.ExecuteNonQuery();

                                str.Clear();
                                str.Append("INSERT INTO vatDocument_Transaction");
                                str.Append("([vat_OperationKey]");
                                str.Append(",[vat_Date]");
                                str.Append(",[vat_vatKey]");
                                str.Append(",[vat_Kind]");
                                str.Append(",[vat_vatRate]");
                                str.Append(",[vat_Subtotal]");
                                str.Append(",[vat_Debit]");
                                str.Append(",[vat_Credit]");
                                str.Append(",[vat_vatRegNo]");
                                str.Append(",[vat_AccountName]");
                                str.Append(",[vat_Description])");
                                str.Append(" VALUES ");
                                str.Append("(@vat_OperationKey");
                                str.Append(",@vat_Date");
                                str.Append(",@vat_vatKey");
                                str.Append(",@vat_Kind");
                                str.Append(",@vat_vatRate");
                                str.Append(",@vat_Subtotal");
                                str.Append(",@vat_Debit");
                                str.Append(",@vat_Credit");
                                str.Append(",@vat_vatRegNo");
                                str.Append(",@vat_AccountName");
                                str.Append(",@vat_Description)");
                                comm.CommandType = CommandType.Text;
                                comm.CommandText = str.ToString();
                                comm.Parameters.Clear();
                                comm.Parameters.Add("@vat_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                                comm.Parameters.Add("@vat_Date", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.VoucherDate);
                                comm.Parameters.Add("@vat_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(vat.Key);
                                comm.Parameters.Add("@vat_Kind", SqlDbType.Int).Value = (int)PLenums.vatKind.Sales;
                                comm.Parameters.Add("@vat_vatRate", SqlDbType.Decimal).Value = xConfig.vatRate(DB, vat.Key);
                                comm.Parameters.Add("@vat_Debit", SqlDbType.Decimal).Value = 0;
                                comm.Parameters.Add("@vat_Credit", SqlDbType.Decimal).Value = vatDetails.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                                comm.Parameters.Add("@vat_vatRegNo", SqlDbType.NVarChar, 25).Value = "";
                                comm.Parameters.Add("@vat_AccountName", SqlDbType.NVarChar, 200).Value = "";
                                comm.Parameters.Add("@vat_Description", SqlDbType.NVarChar, 500).Value =  "";
                                comm.ExecuteNonQuery();
                            }
                        }
                    }

                    //Purchase Cost
                    ++index;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.ChartofAccount;
                    comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(accounts.PurchaseAccount);
                    comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = "";
                    comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = true;
                    comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).Sum(x => x.UnitCost * x.Quantity);
                    comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).Sum(x => x.UnitCost * x.Quantity);
                    comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).Sum(x => x.UnitCost * x.Quantity);
                    comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).Sum(x => x.UnitCost * x.Quantity);
                    comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                    comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                    comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                    comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                    comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                    comm.ExecuteNonQuery();

                    //Sales Cost
                    ++index;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Expenses;
                    comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(accounts.CostAccount);
                    comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = "";
                    comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
                    comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).Sum(x => x.UnitCost * x.Quantity);
                    comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).Sum(x => x.UnitCost * x.Quantity);
                    comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).Sum(x => x.UnitCost * x.Quantity);
                    comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).Sum(x => x.UnitCost * x.Quantity);
                    comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                    comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                    comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                    comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                    comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                    comm.ExecuteNonQuery();
                    #endregion



                    str.Clear();
                    str.Append("exec dbo.spAccOperation_PostTransaction @Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
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
                    comm.Parameters.Add("@sys_User", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@sys_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                    comm.ExecuteNonQuery();

                    try
                    {
                        transaction.Commit();

                        VoucherOperation.UpdateInvoiceNo(DB, (int)DocumentKind.SalesInvoice, opk);
                        VoucherOperation.UpdateInvoiceMonthlyNo(DB, Header.VoucherDate.Year, Header.VoucherDate.Month, (int)DocumentKind.SalesInvoice, opk);

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }


                return 1;
            }
        }
    }
}
