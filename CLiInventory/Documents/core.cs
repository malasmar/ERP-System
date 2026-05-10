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
    public class core
    {
        private readonly static object Locker = new object();
        public static CLiCore.OperationResult UpdatePurchaseInvoice(string DB, Transaction Header, List<TransactionDetails> Details, bool IsNew)
        {
            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;
                int MonthlyNo;
                if (IsNew == false)
                {
                    VoucherNo = Header.InvoiceNo;
                    MonthlyNo = Header.MonthlyNo;
                    opk = Header.OperationKey;
                }
                else
                {
                    opk = Guid.NewGuid();
                    VoucherNo = 0;
                    MonthlyNo = 0;
                }

                bool DC = xCore.HeaderDebitOrCredit(Header.DocumentKind);
                int index = 0;



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

                    comm.CommandText = " exec dbo.spfinDocument_DeleteTransactionUpdate @Key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();

                    //Header Data

                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Session);
                    comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                    comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                    comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = Header.InvoiceKind;
                    comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                    comm.Parameters.Add("@inv_InvoiceDatetime", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.InvoiceDate.Value.Date + DateTime.Now.TimeOfDay);
                    comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = MonthlyNo;
                    comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate); ;
                    comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.DueDate); ;
                    comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CurrentKey);
                    comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                    comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = Header.SalesHand;
                    comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                    comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus * (x.UnitPrice - x.Discount));
                    comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount - x.Discount + x.vatAmount);
                    comm.Parameters.Add("@inv_PaymentDiscount", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                    comm.Parameters.Add("@inv_RetentionLess", SqlDbType.Decimal).Value = Header.RetentionLess;
                    comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = Details.Sum(x => x.UnitCost);
                    comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus);
                    comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = Header.DeliveryKind;
                    comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Fixture).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.ImportationKey);
                    comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = Header.Returned;
                    comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                    comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = Header.AccountKind == 0 ? true : false;
                    comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();

                    //Financial Header

                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@Fin_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@Fin_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@Fin_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@Fin_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = Header.InvoiceDate;
                    comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = MonthlyNo;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.DueDate);
                    comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                    comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                    comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false;
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value = "";
                    comm.ExecuteNonQuery();

                    foreach (Documents.TransactionDetails item in Details)
                    {

                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = 1;
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                        comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                        comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ProDate);
                        comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ExpDate);
                        comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = item.Color;
                        comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = item.Size;
                        comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@inv_SalesPrice", SqlDbType.Decimal).Value = item.SalesPrice;
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                        comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@inv_DiscountText", SqlDbType.NVarChar,10).Value = item.DiscountText??"";
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                        comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Batch);
                        comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = item.ConsumptionKind;
                        comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = item.Status;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = item.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = item.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(item.Project); ;
                        comm.ExecuteNonQuery();

                        if (item.ItemKind == (int)PLenums.TransactionAccount.Expenses || item.ItemKind == (int)PLenums.TransactionAccount.Fixture)
                        {

                            comm.CommandType = CommandType.Text;
                            comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                            comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = item.Status;
                            comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                            comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = item.ItemKind;
                            comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                            comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = item.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(item.CostCenter);
                            comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = item.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(item.Project);
                            comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                            comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
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
                            comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                            comm.ExecuteNonQuery();
                            ++index;
                        }
                        if (item.ItemKind == (int)PLenums.TransactionAccount.Fixture)
                        {
                            str.Clear();
                            str.Append("INSERT INTO finFixedAssets_Transactions");
                            str.Append("([fxd_Key]");
                            str.Append(",[fxd_OperationKey]");
                            str.Append(",[fxd_Fixture]");
                            str.Append(",[fxd_DepPercent]");
                            str.Append(",[fxd_PurchaseDate]");
                            str.Append(",[fxd_Quantity]");
                            str.Append(",[fxd_UnitPrice]");
                            str.Append(",[fxd_Amount]");
                            str.Append(",[fxd_CostCenter]");
                            str.Append(",[fxd_Project])");
                            str.Append(" VALUES ");
                            str.Append("(@fxd_Key");
                            str.Append(",@fxd_OperationKey");
                            str.Append(",@fxd_Fixture");
                            str.Append(",(select top 100 percent x.[fxd_Percent] from [finFixedAssets_Fixture] x where [fxd_Key]=@fxd_Fixture)");
                            str.Append(",@fxd_PurchaseDate");
                            str.Append(",@fxd_Quantity");
                            str.Append(",@fxd_UnitPrice");
                            str.Append(",@fxd_Amount");
                            str.Append(",@fxd_CostCenter");
                            str.Append(",@fxd_Project)");
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = str.ToString();
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@fxd_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                            comm.Parameters.Add("@fxd_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@fxd_Fixture", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                            //comm.Parameters.Add("@fxd_DepPercent", SqlDbType.Decimal).Value = item.DepPercent;
                            comm.Parameters.Add("@fxd_PurchaseDate", SqlDbType.Date).Value = Header.InvoiceDate;
                            comm.Parameters.Add("@fxd_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                            comm.Parameters.Add("@fxd_UnitPrice", SqlDbType.Decimal).Value = ((item.Quantity * item.UnitPrice) - item.Discount) / item.Quantity;
                            comm.Parameters.Add("@fxd_Amount", SqlDbType.Decimal).Value = (item.Quantity * item.UnitPrice) - item.Discount;
                            comm.Parameters.Add("@fxd_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                            comm.Parameters.Add("@fxd_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                            comm.ExecuteNonQuery();
                        }
                        if (item.ItemKind == (int)PLenums.TransactionAccount.Stock)
                        {
                            str.Clear();
                            str.Append("Update [invCard_StockItem] set ");
                            str.Append(" [item_PurchasePrice]=@item_PurchasePrice");
                            str.Append(",[item_SalesPrice]=@item_SalesPrice");
                            str.Append(" WHERE item_Key=@item_Key");
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = str.ToString();
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@item_Key", SqlDbType.UniqueIdentifier).Value = item.ItemKey;
                            comm.Parameters.Add("@item_PurchasePrice", SqlDbType.Decimal).Value = item.UnitPrice;
                            comm.Parameters.Add("@item_SalesPrice", SqlDbType.Decimal).Value = item.SalesPrice;
                            comm.ExecuteNonQuery();
                        }
                  

                    }

                    //Close Account
                    ++index;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = true;
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
                    comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                    comm.ExecuteNonQuery();

                    //VAT

                    var vatGroup = Details.GroupBy(x => x.vatRate);
                    foreach (var vat in vatGroup)
                    {
                        if (Details.Where(x => x.vatRate == vat.Key).Sum(x => x.vatAmount) > 0)
                        {
                            ++index;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                            comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                            comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
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
                            comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                            comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
                            comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = Details.Where(x => x.vatRate == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = vat.Key;
                            comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Where(x => x.vatRate == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = Details.Where(x => x.vatRate == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = Details.Where(x => x.vatRate == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                            comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
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
                            comm.Parameters.Add("@vat_Date", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                            comm.Parameters.Add("@vat_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@vat_Kind", SqlDbType.Int).Value = (int)PLenums.vatKind.Purchase;
                            comm.Parameters.Add("@vat_vatRate", SqlDbType.Decimal).Value = vat.Key;
                            comm.Parameters.Add("@vat_Subtotal", SqlDbType.Decimal).Value = Details.Where(x => x.vatRate == vat.Key).Sum(x => x.Amount - x.Discount);
                            comm.Parameters.Add("@vat_Debit", SqlDbType.Decimal).Value = Details.Where(x => x.vatRate == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@vat_Credit", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@vat_vatRegNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@vat_AccountName", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@vat_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                            comm.ExecuteNonQuery();
                        }
                    }
                    if (Details.Count(x => x.ItemKind == 8) > 0)
                    {
                        //Purchase
                        ++index;
                        CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
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
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                        comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).Sum(x => x.Amount - x.Discount);
                        comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).Sum(x => x.Amount - x.Discount);
                        comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).Sum(x => x.Amount - x.Discount);
                        comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).Sum(x => x.Amount - x.Discount);
                        comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                        comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                        comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                        comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = true;
                        comm.ExecuteNonQuery();
                    }
     
                    //str.Clear();
                    //str.Append("exec dbo.spAccOperation_PostTransaction @Key ");
                    //comm.CommandType = CommandType.Text;
                    //comm.CommandText = str.ToString();
                    //comm.Parameters.Clear();
                    //comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    //comm.ExecuteNonQuery();

                    //str.Clear();
                    //str.Append("exec dbo.spInvOperation_UpdateStockBalance @Key ");
                    //comm.CommandType = CommandType.Text;
                    //comm.CommandText = str.ToString();
                    //comm.Parameters.Clear();
                    //comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    //comm.ExecuteNonQuery();

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
                        if (IsNew == true)
                        {
                            VoucherNo = VoucherOperation.UpdateInvoiceNo(DB, Header.DocumentKind, opk);
                            VoucherOperation.UpdateInvoiceMonthlyNo(DB, Header.InvoiceDate.Value.Year, Header.InvoiceDate.Value.Month, Header.DocumentKind, opk);
                        }
                        //UpdateSourceItemsCost(DB, opk, Header.DocumentKind);
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
        public static CLiCore.OperationResult UpdateReturnPurchaseInvoice(string DB, Transaction Header, List<TransactionDetails> Details, bool IsNew)
        {
            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;
                int MonthlyNo;
                if (IsNew == false)
                {
                    VoucherNo = Header.InvoiceNo;
                    MonthlyNo = Header.MonthlyNo;
                    opk = Header.OperationKey;
                }
                else
                {
                    opk = Guid.NewGuid();
                    VoucherNo = 0;// CLiCore.VouchersNo.MaxTransaction(DB, Header.DocumentKind, Header.InvoiceDate.Value.Year);
                    MonthlyNo = 0;
                }

                bool DC = xCore.HeaderDebitOrCredit(Header.DocumentKind);
                int index = 0;
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

                    comm.CommandText = " exec dbo.spfinDocument_DeleteTransactionUpdate @Key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();

                    //Header Data

                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Session);
                    comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                    comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                    comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = Header.InvoiceKind;
                    comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                    comm.Parameters.Add("@inv_InvoiceDatetime", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.InvoiceDate.Value.Date + DateTime.Now.TimeOfDay);
                    comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = MonthlyNo;
                    comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate); ;
                    comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.DueDate); ;
                    comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CurrentKey);
                    comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                    comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = Header.SalesHand;
                    comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                    comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus * (x.UnitPrice - x.Discount));
                    comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount - x.Discount + x.vatAmount);
                    comm.Parameters.Add("@inv_PaymentDiscount", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                    comm.Parameters.Add("@inv_RetentionLess", SqlDbType.Decimal).Value = Header.RetentionLess;
                    comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = Details.Sum(x => x.UnitCost);
                    comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus);
                    comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = Header.DeliveryKind;
                    comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Fixture).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.ImportationKey);
                    comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = Header.Returned;
                    comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                    comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = Header.AccountKind == 0 ? true : false;
                    comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();

                    //Financial Header

                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@Fin_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@Fin_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@Fin_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@Fin_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = Header.InvoiceDate;
                    comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = MonthlyNo;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.DueDate);
                    comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                    comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                    comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false;
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value = "";
                    comm.ExecuteNonQuery();

                    foreach (Documents.TransactionDetails item in Details)
                    {

                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = -1;
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                        comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                        comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ProDate);
                        comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ExpDate);
                        comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = item.Color;
                        comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = item.Size;
                        comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@inv_SalesPrice", SqlDbType.Decimal).Value = item.SalesPrice;
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                        comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@inv_DiscountText", SqlDbType.NVarChar, 10).Value = item.DiscountText ?? "";
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                        comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Batch);
                        comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = item.ConsumptionKind;
                        comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = item.Status;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                        comm.ExecuteNonQuery();

                        if (item.ItemKind == (int)PLenums.TransactionAccount.Expenses || item.ItemKind == (int)PLenums.TransactionAccount.Fixture)
                        {

                            comm.CommandType = CommandType.Text;
                            comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                            comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = item.Status;
                            comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                            comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = item.ItemKind;
                            comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                            comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = item.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(item.CostCenter);
                            comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = item.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(item.Project);
                            comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
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
                            comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                            comm.ExecuteNonQuery();
                            ++index;
                        }
                    }

                    //Close Account
                    ++index;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
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
                    comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                    comm.ExecuteNonQuery();

                    //VAT
                    var vatGroup = Details.GroupBy(x => x.vatKey);
                    foreach (var vat in vatGroup)
                    {
                        if (Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount) > 0)
                        {
                            ++index;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                            comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                            comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
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
                            comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                            comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(vat.Key);
                            comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = xConfig.vatRate(DB, vat.Key);
                            comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                            comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
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
                            comm.Parameters.Add("@vat_Date", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                            comm.Parameters.Add("@vat_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(vat.Key);
                            comm.Parameters.Add("@vat_Kind", SqlDbType.Int).Value = (int)PLenums.vatKind.Purchase;
                            comm.Parameters.Add("@vat_vatRate", SqlDbType.Decimal).Value = xConfig.vatRate(DB, vat.Key);
                            comm.Parameters.Add("@vat_Subtotal", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.Amount - x.Discount);
                            comm.Parameters.Add("@vat_Debit", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@vat_Credit", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@vat_vatRegNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@vat_AccountName", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@vat_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                            comm.ExecuteNonQuery();
                        }

                      
                    }

                    if (Details.Count(x => x.ItemKind == 8) > 0)
                    {
                        //Purchase
                        ++index;
                        CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
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
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
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
                        comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = true;
                        comm.ExecuteNonQuery();
                    }
                 



                    str.Clear();
                    str.Append("exec dbo.spAccOperation_PostTransaction @Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.ExecuteNonQuery();

                    str.Clear();
                    str.Append("exec dbo.spInvOperation_UpdateStockBalance @Key ");
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
                    comm.Parameters.Add("@sys_User", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@sys_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                    comm.ExecuteNonQuery();

                    try
                    {
                        transaction.Commit();
                        if (IsNew == true)
                        {
                            VoucherNo = VoucherOperation.UpdateInvoiceNo(DB, Header.DocumentKind, opk);
                            VoucherOperation.UpdateInvoiceMonthlyNo(DB, Header.InvoiceDate.Value.Year, Header.InvoiceDate.Value.Month, Header.DocumentKind, opk);
                        }
                        UpdateSourceItemsCost(DB, opk, Header.DocumentKind);
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
        public static CLiCore.OperationResult UpdateSalesInvoice(string DB, Transaction Header, List<TransactionDetails> Details, bool IsNew, List<Selections.AdvancePayments> advancePayment)
        {
            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;
                int MonthlyNo;
                if (IsNew == false)
                {
                    VoucherNo = Header.InvoiceNo;
                    MonthlyNo = Header.MonthlyNo;
                    opk = Header.OperationKey;
                }
                else
                {
                    opk = Guid.NewGuid();
                    MonthlyNo = 0;
                    VoucherNo = 0;// CLiCore.VouchersNo.MaxTransaction(DB, Header.DocumentKind, Header.InvoiceDate.Value.Year);
                }

                bool DC = xCore.HeaderDebitOrCredit(Header.DocumentKind);
                int index = 0;
                string QR;

                QR = iCore.QR(DB, Header.InvoiceDate.Value, Details.Sum(x => x.Total), Details.Sum(x => x.vatAmount));

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

                    comm.CommandText = " exec dbo.spfinDocument_DeleteTransactionUpdate @Key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();

                    //Header Data
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Session);
                    comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                    comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                    comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = Header.InvoiceKind;
                    comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                    comm.Parameters.Add("@inv_InvoiceDatetime", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.InvoiceDate.Value.Date + DateTime.Now.TimeOfDay);
                    comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = MonthlyNo;
                    comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate); ;
                    comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.DueDate); ;
                    comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CurrentKey);
                    comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                    comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = Header.SalesHand;
                    comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                    comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus * (x.UnitPrice - x.Discount));
                    comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount - x.Discount + x.vatAmount);
                    comm.Parameters.Add("@inv_PaymentDiscount", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                    comm.Parameters.Add("@inv_RetentionLess", SqlDbType.Decimal).Value = Header.RetentionLess;
                    comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = Details.Where(x => x.ItemKind == 8).Sum(x => x.UnitCost * x.Quantity);
                    comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus);
                    comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = Header.DeliveryKind;
                    comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Fixture).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.ImportationKey);
                    comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = Header.Returned;
                    comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                    comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = Header.AccountKind == 0 ? true : false;
                    comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();

                    //Financial Header
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@Fin_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@Fin_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@Fin_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@Fin_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = Header.InvoiceDate;
                    comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = MonthlyNo;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.DueDate);
                    comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                    comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                    comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false;
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value = "";
                    comm.ExecuteNonQuery();

                    foreach (Documents.TransactionDetails item in Details)
                    {

                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = -1;
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                        comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                        comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ProDate);
                        comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ExpDate);
                        comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = item.Color;
                        comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = item.Size;
                        comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@inv_SalesPrice", SqlDbType.Decimal).Value = item.SalesPrice;
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                        comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@inv_DiscountText", SqlDbType.NVarChar, 10).Value = item.DiscountText ?? "";
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                        comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Batch);
                        comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = item.ConsumptionKind;
                        comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = item.Status;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = item.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = item.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(item.Project);
                        comm.ExecuteNonQuery();

                        if (item.ItemKind == (int)PLenums.TransactionAccount.Revenue || item.ItemKind == (int)PLenums.TransactionAccount.Expenses || item.ItemKind == (int)PLenums.TransactionAccount.Fixture)
                        {

                            comm.CommandType = CommandType.Text;
                            comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                            comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = item.Status;
                            comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                            comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = item.ItemKind;
                            comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                            comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = item.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(item.CostCenter);
                            comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = item.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(item.Project);
                            comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
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
                            comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                            comm.ExecuteNonQuery();
                            ++index;
                        }

                        //Quotation Lines
                        if (item.CloseRowKey != null)
                        {
                            str.Clear();
                            str.Append("INSERT INTO InvDocument_TransactionDetailsClosed");
                            str.Append("([inv_OperationKey]");
                            str.Append(",[inv_InvoiceKey]");
                            str.Append(",[inv_RowKey]");
                            str.Append(",[inv_Quantity]");
                            str.Append(",[inv_Amount])");
                            str.Append(" VALUES ");
                            str.Append("(@inv_OperationKey");
                            str.Append(",@inv_InvoiceKey");
                            str.Append(",@inv_RowKey");
                            str.Append(",@inv_Quantity");
                            str.Append(",@inv_Amount)");
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = str.ToString();
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@inv_InvoiceKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                            comm.Parameters.Add("@inv_RowKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CloseRowKey);
                            comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                            comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Total;
                            comm.ExecuteNonQuery();
                        }

                    }

                    if (Header.Quotation > 0)
                    {
                        str.Clear();
                        str.Append("UPDATE [SalesDocument_Quotation] SET [sal_Invoiced]=1,[sal_OriginalInvoice]=@Key where [sal_OperationKey]=@opk");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@opk", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                        comm.ExecuteNonQuery();
                    }
                    if (Header.Proforma > 0)
                    {
                        str.Clear();
                        str.Append("UPDATE [SalesDocument_Proforma] SET [sal_Invoiced]=1,[sal_OriginalInvoice]=@Key where [sal_OperationKey]=@opk");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@opk", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                        comm.ExecuteNonQuery();
                    }

                    #region "Close Accounts Financial"

                    //Close Account
                    ++index;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
                    comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = Details.Sum(x => x.Total) - Header.RetentionLess - Header.PaymentDiscount;
                    comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total) - Header.RetentionLess - Header.PaymentDiscount;
                    comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Total) - Header.RetentionLess - Header.PaymentDiscount;
                    comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Total) - Header.RetentionLess - Header.PaymentDiscount;
                    comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                    comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                    comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                    comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                    comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                    comm.ExecuteNonQuery();

                    //Retention
                    if (Header.RetentionLess > 0)
                    {
                        ++index;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(xConfig.RetentionAccount(DB));
                        comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                        comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = Header.RetentionLess;
                        comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Header.RetentionLess;
                        comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = Header.RetentionLess;
                        comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = Header.RetentionLess;
                        comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                        comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                        comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                        comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                        comm.ExecuteNonQuery();
                    }

                    //Payment Discount
                    if (Header.PaymentDiscount > 0)
                    {
                        ++index;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = PLenums.TransactionAccount.Expenses;
                        comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(xConfig.PaymentDiscount(DB));
                        comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                        comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                        comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                        comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                        comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                        comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                        comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                        comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                        comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                        comm.ExecuteNonQuery();
                    }

                    if (Details.Count(x => x.ItemKind == 8) > 0)
                    {
                        //Revenue
                        CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);

                        var ConsumptionGroup = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).GroupBy(x => new
                        {
                            x.CostCenter,
                            x.Project
                        }).Select(G => new CostCenterDistribution()
                        {
                            Amount = G.Sum(x => x.Amount),
                            CostCenter = G.Key.CostCenter,
                            Project = G.Key.Project
                        });

                        foreach (CostCenterDistribution costitem in ConsumptionGroup)
                        {
                            ++index;

                            comm.CommandType = CommandType.Text;
                            comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                            comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                            comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                            comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Revenue;
                            comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(accounts.SalesAccount);
                            comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = costitem.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(costitem.CostCenter);
                            comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = costitem.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(costitem.Project);
                            comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                            comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = costitem.Amount;
                            comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = costitem.Amount;
                            comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = costitem.Amount;
                            comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = costitem.Amount;
                            comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                            comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                            comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                            comm.ExecuteNonQuery();
                        }



                    }
                 



                    if (Details.Sum(x => x.Discount) > 0)
                    {
                        ++index;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Expenses;
                        comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(xConfig.SalesDiscountAccount(DB));
                        comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                        comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
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
                        comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                        comm.ExecuteNonQuery();
                    }

                    //VAT
                    var vatGroup = Details.GroupBy(x => x.vatKey);
                    foreach (var vat in vatGroup)
                    {
                        if (Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount) > 0)
                        {
                            ++index;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                            comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                            comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                            comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = PLenums.CurrentAccountKind.Adjustment;
                            comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(xConfig.vatKey(DB));
                            comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                            comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                            comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                            comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(vat.Key);
                            comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = xConfig.vatRate(DB, vat.Key);
                            comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                            comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
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
                            comm.Parameters.Add("@vat_Date", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                            comm.Parameters.Add("@vat_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(vat.Key);
                            comm.Parameters.Add("@vat_Kind", SqlDbType.Int).Value = (int)PLenums.vatKind.Sales;
                            comm.Parameters.Add("@vat_vatRate", SqlDbType.Decimal).Value = xConfig.vatRate(DB, vat.Key);
                            comm.Parameters.Add("@vat_Subtotal", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.Amount - x.Discount);
                            comm.Parameters.Add("@vat_Debit", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@vat_Credit", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@vat_vatRegNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@vat_AccountName", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@vat_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                            comm.ExecuteNonQuery();
                        }

                   
                    }

                    if (Details.Count(x => x.ItemKind == 8) > 0)
                    {
                        CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);
                        //Purchase Cost
                        ++index;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.ChartofAccount;
                        comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(accounts.PurchaseAccount);
                        comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                        comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
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
                        comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = true;
                        comm.ExecuteNonQuery();

                        //Sales Cost

                        var DisSalesCost = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).GroupBy(x => new
                        {
                            x.CostCenter,
                            x.Project
                        }).Select(G => new CostCenterDistribution()
                        {
                            Amount = G.Sum(x => x.UnitCost * x.Quantity),
                            CostCenter = G.Key.CostCenter,
                            Project = G.Key.Project
                        });
                        foreach (CostCenterDistribution disitem in DisSalesCost)
                        {
                            ++index;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                            comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                            comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                            comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Expenses;
                            comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(accounts.CostAccount);
                            comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = disitem.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(disitem.CostCenter);
                            comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = disitem.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(disitem.Project);
                            comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                            comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
                            comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = disitem.Amount;
                            comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = disitem.Amount;
                            comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = disitem.Amount;
                            comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = disitem.Amount;
                            comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                            comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                            comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = true;
                            comm.ExecuteNonQuery();
                        }
                    }
                       

                    #endregion

                    if (advancePayment != null)
                    {
                        foreach (Selections.AdvancePayments payment in advancePayment)
                        {
                            str.Clear();
                            str.Append("INSERT INTO finDocument_TransactionInvoices");
                            str.Append("([cls_OperationKey]");
                            str.Append(",[cls_RowKey]");
                            str.Append(",[cls_InvoiceKey]");
                            str.Append(",[cls_Amount])");
                            str.Append(" VALUES ");
                            str.Append("(@cls_OperationKey");
                            str.Append(",@cls_RowKey");
                            str.Append(",@cls_InvoiceKey");
                            str.Append(",@cls_Amount)");
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = str.ToString();
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@cls_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(payment.OperationKey);
                            comm.Parameters.Add("@cls_RowKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(payment.RowKey);
                            comm.Parameters.Add("@cls_InvoiceKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@cls_Amount", SqlDbType.Decimal).Value = payment.Amount;
                            comm.ExecuteNonQuery();
                        }
                    }


                    str.Clear();
                    str.Append("exec dbo.spAccOperation_PostTransaction @Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.ExecuteNonQuery();

                    str.Clear();
                    str.Append("exec dbo.spInvOperation_UpdateStockBalance @Key ");
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
                    comm.Parameters.Add("@sys_User", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@sys_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                    comm.ExecuteNonQuery();

                    str.Clear();
                    str.Append("INSERT INTO InvDocument_TransactionQR");
                    str.Append("([inv_OperationKey]");
                    str.Append(",[inv_QR])");
                    str.Append(" VALUES ");
                    str.Append("(@inv_OperationKey");
                    str.Append(",@inv_QR)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_QR", SqlDbType.NVarChar, -1).Value = QR;
                    comm.ExecuteNonQuery();

                    try
                    {
                        transaction.Commit();
                        if (IsNew == true)
                        {
                            VoucherNo = VoucherOperation.UpdateInvoiceNo(DB, Header.DocumentKind, opk);
                            VoucherOperation.UpdateInvoiceMonthlyNo(DB, Header.InvoiceDate.Value.Year, Header.InvoiceDate.Value.Month, Header.DocumentKind, opk);
                        }
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
        public static CLiCore.OperationResult UpdateReturnSalesInvoice(string DB, Transaction Header, List<TransactionDetails> Details, bool IsNew)
        {
            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;
                int MonthlyNo;
                if (IsNew == false)
                {
                    VoucherNo = Header.InvoiceNo;
                    MonthlyNo = Header.MonthlyNo;
                    opk = Header.OperationKey;
                }
                else
                {
                    opk = Guid.NewGuid();
                    VoucherNo = 0;// CLiCore.VouchersNo.MaxTransaction(DB, Header.DocumentKind, Header.InvoiceDate.Value.Year);
                    MonthlyNo = 0;
                }
               string QR = iCore.QR(DB, Header.InvoiceDate.Value, Details.Sum(x => x.Total), Details.Sum(x => x.vatAmount));
                bool DC = xCore.HeaderDebitOrCredit(Header.DocumentKind);
                int index = 0;
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

                    comm.CommandText = " exec dbo.spfinDocument_DeleteTransactionUpdate @Key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();

                    //Header Data
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Session);
                    comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                    comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                    comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = Header.InvoiceKind;
                    comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                    comm.Parameters.Add("@inv_InvoiceDatetime", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.InvoiceDate.Value.Date + DateTime.Now.TimeOfDay);
                    comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = MonthlyNo;
                    comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate); ;
                    comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.DueDate); ;
                    comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CurrentKey);
                    comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                    comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = Header.SalesHand;
                    comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                    comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus * (x.UnitPrice - x.Discount));
                    comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount - x.Discount + x.vatAmount);
                    comm.Parameters.Add("@inv_PaymentDiscount", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                    comm.Parameters.Add("@inv_RetentionLess", SqlDbType.Decimal).Value = Header.RetentionLess;
                    comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = Details.Sum(x => x.UnitCost);
                    comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus);
                    comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = Header.DeliveryKind;
                    comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Fixture).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.ImportationKey);
                    comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = Header.Returned;
                    comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                    comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = Header.AccountKind == 0 ? true : false;
                    comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();

                    //Financial Header
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@Fin_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@Fin_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@Fin_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@Fin_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = Header.InvoiceDate;
                    comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = MonthlyNo;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.DueDate);
                    comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                    comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                    comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false;
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value = "";
                    comm.ExecuteNonQuery();

                    foreach (Documents.TransactionDetails item in Details)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = 1;
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                        comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                        comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ProDate);
                        comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ExpDate);
                        comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = item.Color;
                        comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = item.Size;
                        comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@inv_SalesPrice", SqlDbType.Decimal).Value = item.SalesPrice;
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                        comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@inv_DiscountText", SqlDbType.NVarChar, 10).Value = item.DiscountText ?? "";
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                        comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Batch);
                        comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = item.ConsumptionKind;
                        comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = item.Status;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = item.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = item.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(item.Project);
                        comm.ExecuteNonQuery();

                        if (item.ItemKind == (int)PLenums.TransactionAccount.Revenue || item.ItemKind == (int)PLenums.TransactionAccount.Expenses || item.ItemKind == (int)PLenums.TransactionAccount.Fixture)
                        {

                            comm.CommandType = CommandType.Text;
                            comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                            comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = item.Status;
                            comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                            comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = item.ItemKind;
                            comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                            comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = item.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(item.CostCenter);
                            comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = item.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(item.Project);
                            comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                            comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
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
                            comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                            comm.ExecuteNonQuery();
                            ++index;
                        }
                        if (Header.OperationKey != null && item.CloseRowKey!=null)
                        {
                            //Return Lines
                            str.Clear();
                            str.Append("INSERT INTO InvDocument_TransactionDetailsClosed");
                            str.Append("([inv_OperationKey]");
                            str.Append(",[inv_InvoiceKey]");
                            str.Append(",[inv_RowKey]");
                            str.Append(",[inv_Quantity]");
                            str.Append(",[inv_Amount])");
                            str.Append(" VALUES ");
                            str.Append("(@inv_OperationKey");
                            str.Append(",@inv_InvoiceKey");
                            str.Append(",@inv_RowKey");
                            str.Append(",@inv_Quantity");
                            str.Append(",@inv_Amount)");
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = str.ToString();
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@inv_InvoiceKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                            comm.Parameters.Add("@inv_RowKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CloseRowKey);
                            comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                            comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Total;
                            comm.ExecuteNonQuery();
                        }
                       

                    }

                    //Close Account
                    ++index;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = true;
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
                    comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                    comm.ExecuteNonQuery();


                    if (Details.Count(x => x.ItemKind == 8)>0)
                    {
                        //Revenue

                        CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);

                        var DisRevenue = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).GroupBy(x => new
                        {
                            x.CostCenter,
                            x.Project
                        }).Select(G => new CostCenterDistribution()
                        {
                            Amount = G.Sum(x => x.Amount),
                            CostCenter = G.Key.CostCenter,
                            Project = G.Key.Project
                        });
                        foreach (CostCenterDistribution disitem in DisRevenue)
                        {
                            ++index;

                            comm.CommandType = CommandType.Text;
                            comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                            comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                            comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                            comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Revenue;
                            comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(accounts.SalesAccount);
                            comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = disitem.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(disitem.CostCenter);
                            comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = disitem.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(disitem.Project);
                            comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                            comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
                            comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = disitem.Amount;
                            comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = disitem.Amount;
                            comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = disitem.Amount;
                            comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = disitem.Amount;
                            comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                            comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                            comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                            comm.ExecuteNonQuery();
                        }
                    }
                   


                    //VAT

                    var vatGroup = Details.GroupBy(x => x.vatKey);
                    foreach (var vat in vatGroup)
                    {
                        if (Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount) > 0)
                        {
                            ++index;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                            comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                            comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
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
                            comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                            comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
                            comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(vat.Key);
                            comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = xConfig.vatRate(DB, vat.Key);
                            comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                            comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
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
                            comm.Parameters.Add("@vat_Date", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                            comm.Parameters.Add("@vat_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(vat.Key);
                            comm.Parameters.Add("@vat_Kind", SqlDbType.Int).Value = (int)PLenums.vatKind.Sales;
                            comm.Parameters.Add("@vat_vatRate", SqlDbType.Decimal).Value = xConfig.vatRate(DB, vat.Key);
                            comm.Parameters.Add("@vat_Subtotal", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.Amount - x.Discount);
                            comm.Parameters.Add("@vat_Debit", SqlDbType.Decimal).Value = Details.Where(x => x.vatKey == vat.Key).Sum(x => x.vatAmount);
                            comm.Parameters.Add("@vat_Credit", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@vat_vatRegNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@vat_AccountName", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@vat_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                            comm.ExecuteNonQuery();
                        }

                       
                    }

                    if (Details.Count(x => x.ItemKind == 8) > 0)
                    {
                        CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);
                        //Sales Cost
                        ++index;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
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
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
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
                        comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = true;
                        comm.ExecuteNonQuery();

                        //Sales Cost


                        var DisSalesCost = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).GroupBy(x => new
                        {
                            x.CostCenter,
                            x.Project
                        }).Select(G => new CostCenterDistribution()
                        {
                            Amount = G.Sum(x => x.UnitCost * x.Quantity),
                            CostCenter = G.Key.CostCenter,
                            Project = G.Key.Project
                        });
                        foreach (CostCenterDistribution disitem in DisSalesCost)
                        {
                            ++index;

                            comm.CommandType = CommandType.Text;
                            comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                            comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                            comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                            comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Expenses;
                            comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(accounts.CostAccount);
                            comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = disitem.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(disitem.CostCenter);
                            comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = disitem.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(disitem.Project);
                            comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                            comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = disitem.Amount;
                            comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = disitem.Amount;
                            comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = disitem.Amount;
                            comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = disitem.Amount;
                            comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                            comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                            comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                            comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = true;
                            comm.ExecuteNonQuery();
                        }
                    }
                       


                    if (Details.Sum(x => x.Discount) > 0)
                    {
                        ++index;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
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
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                        comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = true;
                        comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = Details.Sum(x => (x.Bonus * x.UnitCost) + x.Discount);
                        comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => (x.Bonus * x.UnitCost) + x.Discount);
                        comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = Details.Sum(x => (x.Bonus * x.UnitCost) + x.Discount);
                        comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = Details.Sum(x => (x.Bonus * x.UnitCost) + x.Discount);
                        comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                        comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                        comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                        comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                        comm.ExecuteNonQuery();
                    }


                    str.Clear();
                    str.Append("exec dbo.spAccOperation_PostTransaction @Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.ExecuteNonQuery();

                    str.Clear();
                    str.Append("exec dbo.spInvOperation_UpdateStockBalance @Key ");
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
                    comm.Parameters.Add("@sys_User", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@sys_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                    comm.ExecuteNonQuery();

                    str.Clear();
                    str.Append("INSERT INTO InvDocument_TransactionQR");
                    str.Append("([inv_OperationKey]");
                    str.Append(",[inv_QR])");
                    str.Append(" VALUES ");
                    str.Append("(@inv_OperationKey");
                    str.Append(",@inv_QR)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_QR", SqlDbType.NVarChar, -1).Value = QR;
                    comm.ExecuteNonQuery();

                    try
                    {
                        transaction.Commit();
                        if (IsNew == true)
                        {
                            VoucherNo = VoucherOperation.UpdateInvoiceNo(DB, Header.DocumentKind, opk);
                            VoucherOperation.UpdateInvoiceMonthlyNo(DB, Header.InvoiceDate.Value.Year, Header.InvoiceDate.Value.Month, Header.DocumentKind, opk);
                        }
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
        public static CLiCore.OperationResult UpdateStockTransaction(string DB, Transaction Header, List<TransactionDetails> Details, bool IsNew)
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
                    VoucherNo = 0;
                }
                int IO = xCore.GetIO(Header.DocumentKind);
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

                    comm.CommandText = " exec dbo.spfinDocument_DeleteTransactionUpdate @Key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();

                    //Header Data
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Session);
                    comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                    comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                    comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = Header.InvoiceKind;
                    comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                    comm.Parameters.Add("@inv_InvoiceDatetime", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.InvoiceDate.Value.Date + DateTime.Now.TimeOfDay);
                    comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate); ;
                    comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.DueDate); ;
                    comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CurrentKey);
                    comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                    comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = Header.SalesHand;
                    comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                    comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus * (x.UnitPrice - x.Discount));
                    comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount - x.Discount + x.vatAmount);
                    comm.Parameters.Add("@inv_PaymentDiscount", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                    comm.Parameters.Add("@inv_RetentionLess", SqlDbType.Decimal).Value = Header.RetentionLess;
                    comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = Details.Sum(x => x.UnitCost);
                    comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus);
                    comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = Header.DeliveryKind;
                    comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Fixture).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.ImportationKey);
                    comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = Header.Returned;
                    comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                    comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = Header.AccountKind == 0 ? true : false;
                    comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();

                    foreach (Documents.TransactionDetails item in Details)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = IO;
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                        comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                        comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ProDate);
                        comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ExpDate);
                        comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = item.Color;
                        comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = item.Size;
                        comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@inv_SalesPrice", SqlDbType.Decimal).Value = item.SalesPrice;
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                        comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@inv_DiscountText", SqlDbType.NVarChar, 10).Value = item.DiscountText ?? "";
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                        comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Batch);
                        comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = item.ConsumptionKind;
                        comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = item.Status;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
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

                    str.Clear();
                    str.Append("exec dbo.spInvOperation_UpdateStockBalance @Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.ExecuteNonQuery();

                    try
                    {
                        transaction.Commit();
                        if (IsNew == true)
                        {
                            VoucherNo = VoucherOperation.UpdateInvoiceNo(DB, Header.DocumentKind, opk);

                        }
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
        public static CLiCore.OperationResult UpdateConsumption(string DB, Transaction Header, List<TransactionDetails> Details, bool IsNew)
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
                    VoucherNo = CLiCore.VoucherOperation.MaxTransaction(DB, Header.DocumentKind, Header.InvoiceDate.Value.Year);
                }

                bool DC = xCore.HeaderDebitOrCredit(Header.DocumentKind);
                int index = 0;
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

                    comm.CommandText = " exec dbo.spfinDocument_DeleteTransactionUpdate @Key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();

                    //Header Data
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Session);
                    comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                    comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                    comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = Header.InvoiceKind;
                    comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                    comm.Parameters.Add("@inv_InvoiceDatetime", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.InvoiceDate.Value.Date + DateTime.Now.TimeOfDay);
                    comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = Header.MonthlyNo;
                    comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate); ;
                    comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.DueDate); ;
                    comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CurrentKey);
                    comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                    comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = Header.SalesHand;
                    comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                    comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus * (x.UnitPrice - x.Discount));
                    comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount - x.Discount + x.vatAmount);
                    comm.Parameters.Add("@inv_PaymentDiscount", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                    comm.Parameters.Add("@inv_RetentionLess", SqlDbType.Decimal).Value = Header.RetentionLess;
                    comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = Details.Sum(x => x.UnitCost);
                    comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus);
                    comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = Header.DeliveryKind;
                    comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Fixture).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.ImportationKey);
                    comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = Header.Returned;
                    comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                    comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = Header.AccountKind == 0 ? true : false;
                    comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();

                    //Financial Header
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@Fin_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@Fin_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@Fin_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@Fin_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = Header.InvoiceDate;
                    comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = Header.MonthlyNo;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.DueDate);
                    comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                    comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                    comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false;
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value = "";
                    comm.ExecuteNonQuery();

                    foreach (Documents.TransactionDetails item in Details)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = -1;
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                        comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                        comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ProDate);
                        comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ExpDate);
                        comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = item.Color;
                        comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = item.Size;
                        comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@inv_SalesPrice", SqlDbType.Decimal).Value = item.SalesPrice;
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                        comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@inv_DiscountText", SqlDbType.NVarChar, 10).Value = item.DiscountText ?? "";
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                        comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Batch);
                        comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = item.ConsumptionKind;
                        comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = item.Status;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = item.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = item.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(item.Project);
                        comm.ExecuteNonQuery();

                    }

                    var ConsumptionGroup = Details.GroupBy(x => new
                    {
                        x.CostCenter,
                        x.Project
                    })
                   .Select(G => new CostCenterDistribution()
                   {
                       Amount = G.Sum(x => x.Amount),
                       CostCenter = G.Key.CostCenter,
                       Project = G.Key.Project
                   });
                    index = 0;
                    CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);
                    foreach (CostCenterDistribution conitem in ConsumptionGroup)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Expenses;
                        comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                        comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.ChartofAccount;
                        comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(accounts.PurchaseAccount);
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = conitem.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(conitem.CostCenter);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = conitem.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(conitem.Project);
                        comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                        comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = conitem.Amount;
                        comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = conitem.Amount;
                        comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = conitem.Amount;
                        comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = conitem.Amount;
                        comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                        comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                        comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                        comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                        comm.ExecuteNonQuery();
                        ++index;
                    }


                    str.Clear();
                    str.Append("exec dbo.spAccOperation_PostTransaction @Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.ExecuteNonQuery();

                    str.Clear();
                    str.Append("exec dbo.spInvOperation_UpdateStockBalance @Key ");
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
                    comm.Parameters.Add("@sys_User", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@sys_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                    comm.ExecuteNonQuery();

                    try
                    {
                        transaction.Commit();
                        if (IsNew == true)
                        {
                            VoucherNo = VoucherOperation.UpdateInvoiceNo(DB, Header.DocumentKind, opk);
                            VoucherOperation.UpdateInvoiceMonthlyNo(DB, Header.InvoiceDate.Value.Year, Header.InvoiceDate.Value.Month, Header.DocumentKind, opk);
                        }
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
        public static CLiCore.OperationResult UpdateConsumptionReturn(string DB, Transaction Header, List<TransactionDetails> Details, bool IsNew)
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
                    VoucherNo = CLiCore.VoucherOperation.MaxTransaction(DB, Header.DocumentKind, Header.InvoiceDate.Value.Year);
                }

                bool DC = xCore.HeaderDebitOrCredit(Header.DocumentKind);
                int index = 0;
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

                    comm.CommandText = " exec dbo.spfinDocument_DeleteTransactionUpdate @Key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();

                    //Header Data
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Session);
                    comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                    comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                    comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = Header.InvoiceKind;
                    comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = Header.InvoiceNo;
                    comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                    comm.Parameters.Add("@inv_InvoiceDatetime", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.InvoiceDate.Value.Date + DateTime.Now.TimeOfDay);
                    comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = Header.MonthlyNo;
                    comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate); ;
                    comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.DueDate); ;
                    comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CurrentKey);
                    comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                    comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = Header.SalesHand;
                    comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                    comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus * (x.UnitPrice - x.Discount));
                    comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount - x.Discount + x.vatAmount);
                    comm.Parameters.Add("@inv_PaymentDiscount", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                    comm.Parameters.Add("@inv_RetentionLess", SqlDbType.Decimal).Value = Header.RetentionLess;
                    comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = Details.Sum(x => x.UnitCost);
                    comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus);
                    comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = Header.DeliveryKind;
                    comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Fixture).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.ImportationKey);
                    comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = Header.Returned;
                    comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                    comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = Header.AccountKind == 0 ? true : false;
                    comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();

                    //Financial Header
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@Fin_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@Fin_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@Fin_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@Fin_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = Header.InvoiceDate;
                    comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = Header.MonthlyNo;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.DueDate);
                    comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                    comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                    comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false;
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value = "";
                    comm.ExecuteNonQuery();

                    foreach (Documents.TransactionDetails item in Details)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = 1;
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                        comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                        comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ProDate);
                        comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ExpDate);
                        comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = item.Color;
                        comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = item.Size;
                        comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@inv_SalesPrice", SqlDbType.Decimal).Value = item.SalesPrice;
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                        comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@inv_DiscountText", SqlDbType.NVarChar, 10).Value = item.DiscountText ?? "";
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                        comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Batch);
                        comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = item.ConsumptionKind;
                        comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = item.Status;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = item.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = item.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(item.Project);
                        comm.ExecuteNonQuery();

                        //Return Lines
                        //str.Clear();
                        //str.Append("INSERT INTO InvDocument_TransactionDetailsClosed");
                        //str.Append("([inv_OperationKey]");
                        //str.Append(",[inv_InvoiceKey]");
                        //str.Append(",[inv_RowKey]");
                        //str.Append(",[inv_Quantity]");
                        //str.Append(",[inv_Amount])");
                        //str.Append(" VALUES ");
                        //str.Append("(@inv_OperationKey");
                        //str.Append(",@inv_InvoiceKey");
                        //str.Append(",@inv_RowKey");
                        //str.Append(",@inv_Quantity");
                        //str.Append(",@inv_Amount)");
                        //comm.CommandType = CommandType.Text;
                        //comm.CommandText = str.ToString();
                        //comm.Parameters.Clear();
                        //comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        //comm.Parameters.Add("@inv_InvoiceKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                        //comm.Parameters.Add("@inv_RowKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CloseRowKey);
                        //comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        //comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Total;
                        //comm.ExecuteNonQuery();

                    }

                    var ConsumptionGroup = Details.GroupBy(x => new
                    {
                        x.CostCenter,
                        x.Project
                    })
                   .Select(G => new CostCenterDistribution()
                   {
                       Amount = G.Sum(x => x.Amount),
                       CostCenter = G.Key.CostCenter,
                       Project = G.Key.Project
                   });
                    index = 0;
                    CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);
                    foreach (CostCenterDistribution conitem in ConsumptionGroup)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Expenses;
                        comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                        comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.ChartofAccount;
                        comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(accounts.PurchaseAccount);
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = conitem.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(conitem.CostCenter);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = conitem.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(conitem.Project);
                        comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                        comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = true;
                        comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = conitem.Amount;
                        comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = conitem.Amount;
                        comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = conitem.Amount;
                        comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = conitem.Amount;
                        comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                        comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                        comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                        comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                        comm.ExecuteNonQuery();
                        ++index;
                    }


                    str.Clear();
                    str.Append("exec dbo.spAccOperation_PostTransaction @Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.ExecuteNonQuery();

                    str.Clear();
                    str.Append("exec dbo.spInvOperation_UpdateStockBalance @Key ");
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
                    comm.Parameters.Add("@sys_User", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@sys_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                    comm.ExecuteNonQuery();

                    try
                    {
                        transaction.Commit();
                        if (IsNew == true)
                        {
                            VoucherNo = VoucherOperation.UpdateInvoiceNo(DB, Header.DocumentKind, opk);
                            VoucherOperation.UpdateInvoiceMonthlyNo(DB, Header.InvoiceDate.Value.Year, Header.InvoiceDate.Value.Month, Header.DocumentKind, opk);
                        }
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

        public static CLiCore.OperationResult UpdateSendToWarehouse(string DB, Transaction Header, List<TransactionDetails> Details, bool IsNew)
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
                    VoucherNo = 0;
                }
                int IO = xCore.GetIO(Header.DocumentKind);
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

                    comm.CommandText = " exec dbo.spfinDocument_DeleteTransactionUpdate @Key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();

                    //Header Data
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Session);
                    comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                    comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                    comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = Header.InvoiceKind;
                    comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                    comm.Parameters.Add("@inv_InvoiceDatetime", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.InvoiceDate.Value.Date + DateTime.Now.TimeOfDay);
                    comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate); ;
                    comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.DueDate); ;
                    comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CurrentKey);
                    comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                    comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = Header.SalesHand;
                    comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                    comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus * (x.UnitPrice - x.Discount));
                    comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount - x.Discount + x.vatAmount);
                    comm.Parameters.Add("@inv_PaymentDiscount", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                    comm.Parameters.Add("@inv_RetentionLess", SqlDbType.Decimal).Value = Header.RetentionLess;
                    comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = Details.Sum(x => x.UnitCost);
                    comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus);
                    comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = Header.DeliveryKind;
                    comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Fixture).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.ImportationKey);
                    comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = Header.Returned;
                    comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                    comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = Header.AccountKind == 0 ? true : false;
                    comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();

                    foreach (Documents.TransactionDetails item in Details)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = IO;
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                        comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                        comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ProDate);
                        comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ExpDate);
                        comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = item.Color;
                        comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = item.Size;
                        comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@inv_SalesPrice", SqlDbType.Decimal).Value = item.SalesPrice;
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                        comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@inv_DiscountText", SqlDbType.NVarChar, 10).Value = item.DiscountText ?? "";
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                        comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Batch);
                        comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = item.ConsumptionKind;
                        comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = item.Status;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
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

                    Guid? RecKey = Guid.NewGuid();
                    if (xConfig.AutoReceiptShipping(DB) == true)
                    {
                        //Header Data


                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(RecKey);
                        comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Session);
                        comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                        comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                        comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                        comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                        comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                        comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = (int)DocumentKind.ReceiptInWarehouse;
                        comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = Header.InvoiceKind;
                        comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = VoucherNo;
                        comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                        comm.Parameters.Add("@inv_InvoiceDatetime", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.InvoiceDate.Value.Date + DateTime.Now.TimeOfDay);
                        comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                        comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate); ;
                        comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.DueDate); ;
                        comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                        comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                        comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CurrentKey);
                        comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                        comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = Header.SalesHand;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                        comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = Header.Currency ?? "";
                        comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                        comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus * (x.UnitPrice - x.Discount));
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount - x.Discount + x.vatAmount);
                        comm.Parameters.Add("@inv_PaymentDiscount", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                        comm.Parameters.Add("@inv_RetentionLess", SqlDbType.Decimal).Value = Header.RetentionLess;
                        comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = Details.Sum(x => x.UnitCost);
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                        comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus);
                        comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = Header.DeliveryKind;
                        comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Fixture).Count() > 0 ? true : false; ;
                        comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false; ;
                        comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.ImportationKey);
                        comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = Header.Returned;
                        comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                        comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = Header.AccountKind == 0 ? true : false;
                        comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                        comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                        comm.ExecuteNonQuery();

                        foreach (Documents.TransactionDetails item in Details)
                        {
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(RecKey);
                            comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                            comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                            comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = 1;
                            comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                            comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                            comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                            comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                            comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ProDate);
                            comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ExpDate);
                            comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = item.Color;
                            comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = item.Size;
                            comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                            comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                            comm.Parameters.Add("@inv_SalesPrice", SqlDbType.Decimal).Value = item.SalesPrice;
                            comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                            comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                            comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Amount;
                            comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = item.Discount;
                            comm.Parameters.Add("@inv_DiscountText", SqlDbType.NVarChar, 10).Value = item.DiscountText ?? "";
                            comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                            comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                            comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                            comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                            comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = item.Total;
                            comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Batch);
                            comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = item.ConsumptionKind;
                            comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                            comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                            comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = item.Status;
                            comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                            comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                            comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                            comm.ExecuteNonQuery();

                            str.Clear();
                            str.Append("INSERT INTO InvDocument_TransactionDetailsClosed");
                            str.Append("([inv_OperationKey]");
                            str.Append(",[inv_InvoiceKey]");
                            str.Append(",[inv_RowKey]");
                            str.Append(",[inv_Quantity]");
                            str.Append(",[inv_Amount])");
                            str.Append(" VALUES ");
                            str.Append("(@inv_OperationKey");
                            str.Append(",@inv_InvoiceKey");
                            str.Append(",@inv_RowKey");
                            str.Append(",@inv_Quantity");
                            str.Append(",@inv_Amount)");
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = str.ToString();
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(RecKey);
                            comm.Parameters.Add("@inv_InvoiceKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@inv_RowKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                            comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                            comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Total;
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
                        comm.Parameters.Add("@sys_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(RecKey);
                        comm.Parameters.Add("@sys_User", SqlDbType.Int).Value = Header.LastupUser;
                        comm.Parameters.Add("@sys_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                        comm.ExecuteNonQuery();
                    }


                    //Financial and General Ledger
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@Fin_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@Fin_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@Fin_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@Fin_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = Header.InvoiceDate;
                    comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.DueDate);
                    comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                    comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                    comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false;
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value = "";
                    comm.ExecuteNonQuery();

                    int index = 0;
                    ++index;
                    CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);
                    Guid? OnRoadAccount = xConfig.wOnRoad(DB);
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.ChartofAccount;
                    comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(accounts.PurchaseAccount);
                    comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.ChartofAccount;
                    comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(OnRoadAccount);
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
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
                    comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                    comm.ExecuteNonQuery();


                    str.Clear();
                    str.Append("exec dbo.spAccOperation_PostTransaction @Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.ExecuteNonQuery();

                    str.Clear();
                    str.Append("exec dbo.spInvOperation_UpdateStockBalance @Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.ExecuteNonQuery();

                    if (xConfig.AutoReceiptShipping(DB) == true)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_Transaction;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(RecKey);
                        comm.Parameters.Add("@Fin_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                        comm.Parameters.Add("@Fin_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                        comm.Parameters.Add("@Fin_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                        comm.Parameters.Add("@Fin_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@Fin_Branch", SqlDbType.Int).Value = Header.Branch;
                        comm.Parameters.Add("@Fin_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                        comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                        comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = Header.InvoiceDate;
                        comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                        comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.DueDate);
                        comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                        comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = Header.Currency ?? "";
                        comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                        comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                        comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false;
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                        comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value = "";
                        comm.ExecuteNonQuery();

                        int xindex = 0;
                        ++xindex;
                        CLiInventory.Data.WarehouseAccounts xaccounts = new Data.WarehouseAccounts().GetItem(DB, Header.TargetWarehouse);

                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(RecKey);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = xindex;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.ChartofAccount;
                        comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(xaccounts.PurchaseAccount);
                        comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.ChartofAccount;
                        comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(OnRoadAccount);
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                        comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
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
                        comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                        comm.ExecuteNonQuery();

                        str.Clear();
                        str.Append("exec dbo.spAccOperation_PostTransaction @Key ");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(RecKey);
                        comm.ExecuteNonQuery();

                        str.Clear();
                        str.Append("exec dbo.spInvOperation_UpdateStockBalance @Key ");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(RecKey);
                        comm.ExecuteNonQuery();

                    }





                    try
                    {
                        transaction.Commit();
                        if (IsNew == true)
                        {
                            VoucherNo = VoucherOperation.UpdateInvoiceNo(DB, Header.DocumentKind, opk);
                            VoucherOperation.UpdateInvoiceNo(DB, (int)DocumentKind.ReceiptInWarehouse, RecKey);
                        }
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
        public static CLiCore.OperationResult UpdateReceiptInWarehouse(string DB, Transaction Header, List<TransactionDetails> Details, bool IsNew)
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
                    VoucherNo = 0;
                }
                int IO = xCore.GetIO(Header.DocumentKind);
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

                    comm.CommandText = " exec dbo.spfinDocument_DeleteTransactionUpdate @Key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();

                    //Header Data
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Session);
                    comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                    comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                    comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = Header.InvoiceKind;
                    comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                    comm.Parameters.Add("@inv_InvoiceDatetime", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.InvoiceDate.Value.Date + DateTime.Now.TimeOfDay);
                    comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate); ;
                    comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.DueDate); ;
                    comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CurrentKey);
                    comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                    comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = Header.SalesHand;
                    comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                    comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus * (x.UnitPrice - x.Discount));
                    comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount - x.Discount + x.vatAmount);
                    comm.Parameters.Add("@inv_PaymentDiscount", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                    comm.Parameters.Add("@inv_RetentionLess", SqlDbType.Decimal).Value = Header.RetentionLess;
                    comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = Details.Sum(x => x.UnitCost);
                    comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus);
                    comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = Header.DeliveryKind;
                    comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Fixture).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.ImportationKey);
                    comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = Header.Returned;
                    comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                    comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = Header.AccountKind == 0 ? true : false;
                    comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();

                    foreach (Documents.TransactionDetails item in Details)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = IO;
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                        comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                        comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ProDate);
                        comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ExpDate);
                        comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = item.Color;
                        comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = item.Size;
                        comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@inv_SalesPrice", SqlDbType.Decimal).Value = item.SalesPrice;
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                        comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@inv_DiscountText", SqlDbType.NVarChar, 10).Value = item.DiscountText ?? "";
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                        comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Batch);
                        comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = item.ConsumptionKind;
                        comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = item.Status;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                        comm.ExecuteNonQuery();

                        //str.Clear();
                        //str.Append("INSERT INTO InvDocument_TransactionDetailsClosed");
                        //str.Append("([inv_OperationKey]");
                        //str.Append(",[inv_InvoiceKey]");
                        //str.Append(",[inv_RowKey]");
                        //str.Append(",[inv_Quantity]");
                        //str.Append(",[inv_Amount])");
                        //str.Append(" VALUES ");
                        //str.Append("(@inv_OperationKey");
                        //str.Append(",@inv_InvoiceKey");
                        //str.Append(",@inv_RowKey");
                        //str.Append(",@inv_Quantity");
                        //str.Append(",@inv_Amount)");
                        //comm.CommandType = CommandType.Text;
                        //comm.CommandText = str.ToString();
                        //comm.Parameters.Clear();
                        //comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        //comm.Parameters.Add("@inv_InvoiceKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                        //comm.Parameters.Add("@inv_RowKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CloseRowKey);
                        //comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        //comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Total;
                        //comm.ExecuteNonQuery();
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


                    //Financial and General Ledger
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@Fin_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@Fin_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@Fin_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@Fin_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = Header.InvoiceDate;
                    comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.DueDate);
                    comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                    comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                    comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false;
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value = "";
                    comm.ExecuteNonQuery();

                    int index = 0;
                    ++index;
                    CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);
                    Guid? OnRoadAccount = xConfig.wOnRoad(DB);
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.ChartofAccount;
                    comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(accounts.PurchaseAccount);
                    comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.ChartofAccount;
                    comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(OnRoadAccount);
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
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
                    comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                    comm.ExecuteNonQuery();


                    str.Clear();
                    str.Append("exec dbo.spAccOperation_PostTransaction @Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.ExecuteNonQuery();

                    str.Clear();
                    str.Append("exec dbo.spInvOperation_UpdateStockBalance @Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.ExecuteNonQuery();

                    try
                    {
                        transaction.Commit();
                        if (IsNew == true)
                        {
                            VoucherNo = VoucherOperation.UpdateInvoiceNo(DB, Header.DocumentKind, opk);

                        }
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
        private static void UpdateSourceItemsCost(string DB, Guid? Key, int DocKind)
        {

            if (Key == null)
                return;

            ////Thread th = new Thread(delegate ()
            ////{
            //using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            //{
            //    SqlCommand command = new SqlCommand();
            //    command = new SqlCommand();
            //    command.Connection = con;
            //    command.CommandType = CommandType.Text;
            //    command.CommandText = "exec dbo.spInvOperation_UpdateStockCost @opk,@DocKind ";
            //    command.Parameters.Clear();
            //    command.Parameters.Add("@opk", SqlDbType.UniqueIdentifier).Value = Key;
            //    command.Parameters.Add("@DocKind", SqlDbType.Int).Value = DocKind;
            //    con.Open();
            //    command.ExecuteNonQuery();
            //}

            Documents.Transaction Header = new Documents.Transaction().GetItem(DB, Key);
            List<Documents.TransactionDetails> Details = new Documents.TransactionDetails().GetList(DB, Key);
            StringBuilder str = new StringBuilder();

            var Invoiceitems = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).GroupBy(x => new
            {
                x.ItemKey,
                x.Unit
            }).Select(G => new InvoiceItems()
            {
                Quantity = G.Sum(x => x.Quantity),
                ItemKey = G.Key.ItemKey,
                Unit = G.Key.Unit,
                Amount = G.Sum(x => x.Amount - x.Discount)
            });

            foreach (InvoiceItems item in Invoiceitems)
            {
                TimeSpan CrestTime = Header.CreateDate.Value.TimeOfDay;
                DateTime invoicedate = Header.InvoiceDate.Value.Date + CrestTime;

                decimal ItemBalance = 0;
                ItemBalance = CLiInventory.core.ItemUnitBalanceFullWarehouse(DB, item.ItemKey, item.Unit, invoicedate);
                decimal UnitCost = CLiInventory.core.GetLastUnitCost(DB, item.ItemKey, item.Unit, invoicedate);
                if (ItemBalance - item.Quantity >= 0)
                {
                    using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
                    {
                        SqlCommand com = new SqlCommand();
                        com.Connection = con;

                        str.Clear();
                        str.Append("INSERT INTO InvDocument_TransactionUnitCost");
                        str.Append("([cost_OperationKey]");
                        str.Append(",[cost_Date]");
                        str.Append(",[cost_ItemKey]");
                        str.Append(",[cost_Balance]");
                        str.Append(",[cost_Quantity]");
                        str.Append(",[cost_Amount]");
                        str.Append(",[cost_LastCost]");
                        str.Append(",[cost_UnitCost]");
                        str.Append(",[cost_Additional])");
                        str.Append(" VALUES ");
                        str.Append("(@cost_OperationKey");
                        str.Append(",@cost_Date");
                        str.Append(",@cost_ItemKey");
                        str.Append(",@cost_Balance");
                        str.Append(",@cost_Quantity");
                        str.Append(",@cost_Amount");
                        str.Append(",@cost_LastCost");
                        str.Append(",@cost_UnitCost");
                        str.Append(",@cost_Additional)");
                        com.CommandType = CommandType.Text;
                        com.CommandText = str.ToString();
                        com.Parameters.Clear();
                        com.Parameters.Add("@cost_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                        com.Parameters.Add("@cost_Date", SqlDbType.DateTime).Value = invoicedate;
                        com.Parameters.Add("@cost_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        com.Parameters.Add("@cost_Balance", SqlDbType.Decimal).Value = ItemBalance - item.Quantity;
                        com.Parameters.Add("@cost_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        com.Parameters.Add("@cost_Amount", SqlDbType.Decimal).Value = (ItemBalance - item.Quantity) * UnitCost;
                        com.Parameters.Add("@cost_LastCost", SqlDbType.Decimal).Value = UnitCost;
                        com.Parameters.Add("@cost_UnitCost", SqlDbType.Decimal).Value = (((ItemBalance - item.Quantity) * UnitCost) + item.Amount) / ItemBalance;
                        com.Parameters.Add("@cost_Additional", SqlDbType.Decimal).Value = 0;
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                }
                else
                {
                    List<Audit.ItemSales> sales = new Audit.ItemSales().GetList(DB, item.ItemKey, invoicedate, new DateTime(invoicedate.Year, 1, 1));
                    decimal xtot = sales.Sum(x => x.Quantity);
                    decimal dif = ItemBalance - item.Quantity;

                    if (sales == null || sales.Count == 0 || ((dif + xtot) < 0))
                    {
                        using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
                        {
                            SqlCommand com = new SqlCommand();
                            com.Connection = con;

                            str.Clear();
                            str.Append("INSERT INTO InvDocument_TransactionUnitCost");
                            str.Append("([cost_OperationKey]");
                            str.Append(",[cost_Date]");
                            str.Append(",[cost_ItemKey]");
                            str.Append(",[cost_Balance]");
                            str.Append(",[cost_Quantity]");
                            str.Append(",[cost_Amount]");
                            str.Append(",[cost_LastCost]");
                            str.Append(",[cost_UnitCost]");
                            str.Append(",[cost_Additional])");
                            str.Append(" VALUES ");
                            str.Append("(@cost_OperationKey");
                            str.Append(",@cost_Date");
                            str.Append(",@cost_ItemKey");
                            str.Append(",@cost_Balance");
                            str.Append(",@cost_Quantity");
                            str.Append(",@cost_Amount");
                            str.Append(",@cost_LastCost");
                            str.Append(",@cost_UnitCost");
                            str.Append(",@cost_Additional)");
                            com.CommandType = CommandType.Text;
                            com.CommandText = str.ToString();
                            com.Parameters.Clear();
                            com.Parameters.Add("@cost_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                            com.Parameters.Add("@cost_Date", SqlDbType.DateTime).Value = invoicedate;
                            com.Parameters.Add("@cost_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                            com.Parameters.Add("@cost_Balance", SqlDbType.Decimal).Value = item.Quantity;
                            com.Parameters.Add("@cost_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                            com.Parameters.Add("@cost_Amount", SqlDbType.Decimal).Value = item.Amount;
                            com.Parameters.Add("@cost_LastCost", SqlDbType.Decimal).Value = UnitCost;
                            com.Parameters.Add("@cost_UnitCost", SqlDbType.Decimal).Value = item.Amount / item.Quantity;
                            com.Parameters.Add("@cost_Additional", SqlDbType.Decimal).Value = 0;
                            con.Open();
                            com.ExecuteNonQuery();
                        }
                    }
                    else
                    {

                        foreach (Audit.ItemSales salitem in sales)
                        {
                            dif += salitem.Quantity;
                            if (dif >= 0)
                            {
                                using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
                                {
                                    SqlCommand com = new SqlCommand();
                                    com.Connection = con;

                                    str.Clear();
                                    str.Append("INSERT INTO InvDocument_TransactionUnitCost");
                                    str.Append("([cost_OperationKey]");
                                    str.Append(",[cost_Date]");
                                    str.Append(",[cost_ItemKey]");
                                    str.Append(",[cost_Balance]");
                                    str.Append(",[cost_Quantity]");
                                    str.Append(",[cost_Amount]");
                                    str.Append(",[cost_LastCost]");
                                    str.Append(",[cost_UnitCost]");
                                    str.Append(",[cost_Additional])");
                                    str.Append(" VALUES ");
                                    str.Append("(@cost_OperationKey");
                                    str.Append(",@cost_Date");
                                    str.Append(",@cost_ItemKey");
                                    str.Append(",@cost_Balance");
                                    str.Append(",@cost_Quantity");
                                    str.Append(",@cost_Amount");
                                    str.Append(",@cost_LastCost");
                                    str.Append(",@cost_UnitCost");
                                    str.Append(",@cost_Additional)");
                                    com.CommandType = CommandType.Text;
                                    com.CommandText = str.ToString();
                                    com.Parameters.Clear();
                                    com.Parameters.Add("@cost_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                                    com.Parameters.Add("@cost_Date", SqlDbType.DateTime).Value = salitem.InvoiceDate;
                                    com.Parameters.Add("@cost_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                                    com.Parameters.Add("@cost_Balance", SqlDbType.Decimal).Value = dif;
                                    com.Parameters.Add("@cost_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                                    com.Parameters.Add("@cost_Amount", SqlDbType.Decimal).Value = dif * UnitCost;
                                    com.Parameters.Add("@cost_LastCost", SqlDbType.Decimal).Value = UnitCost;
                                    com.Parameters.Add("@cost_UnitCost", SqlDbType.Decimal).Value = ((dif * UnitCost) + item.Amount) / (dif + item.Quantity);
                                    com.Parameters.Add("@cost_Additional", SqlDbType.Decimal).Value = 0;
                                    con.Open();
                                    com.ExecuteNonQuery();
                                }
                                break;
                            }
                        }
                    }

                }
            }


            ////});
            ////th.Start();

        }


        public static CLiCore.OperationResult InsertReceiptInWarehouse(string DB, Transaction Header, List<TransactionDetails> Details)
        {
            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;

                opk = Guid.NewGuid();
                VoucherNo = 0;


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

                    comm.CommandText = " exec dbo.spfinDocument_DeleteTransactionUpdate @Key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();

                    //Header Data
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Session);
                    comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                    comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                    comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = (int)DocumentKind.ReceiptInWarehouse;
                    comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = Header.InvoiceKind;
                    comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                    comm.Parameters.Add("@inv_InvoiceDatetime", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.InvoiceDate.Value.Date + DateTime.Now.TimeOfDay);
                    comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate); ;
                    comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.DueDate); ;
                    comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CurrentKey);
                    comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                    comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = Header.SalesHand;
                    comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                    comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus * (x.UnitPrice - x.Discount));
                    comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount - x.Discount + x.vatAmount);
                    comm.Parameters.Add("@inv_PaymentDiscount", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                    comm.Parameters.Add("@inv_RetentionLess", SqlDbType.Decimal).Value = Header.RetentionLess;
                    comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = Details.Sum(x => x.UnitCost);
                    comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus);
                    comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = Header.DeliveryKind;
                    comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Fixture).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.ImportationKey);
                    comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = Header.Returned;
                    comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                    comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = Header.AccountKind == 0 ? true : false;
                    comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();

                    foreach (Documents.TransactionDetails item in Details)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = 1;
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                        comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                        comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ProDate);
                        comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ExpDate);
                        comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = item.Color;
                        comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = item.Size;
                        comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@inv_SalesPrice", SqlDbType.Decimal).Value = item.SalesPrice;
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                        comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@inv_DiscountText", SqlDbType.NVarChar, 10).Value = item.DiscountText ?? "";
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                        comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Batch);
                        comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = item.ConsumptionKind;
                        comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = item.Status;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                        comm.ExecuteNonQuery();

                        //str.Clear();
                        //str.Append("INSERT INTO InvDocument_TransactionDetailsClosed");
                        //str.Append("([inv_OperationKey]");
                        //str.Append(",[inv_InvoiceKey]");
                        //str.Append(",[inv_RowKey]");
                        //str.Append(",[inv_Quantity]");
                        //str.Append(",[inv_Amount])");
                        //str.Append(" VALUES ");
                        //str.Append("(@inv_OperationKey");
                        //str.Append(",@inv_InvoiceKey");
                        //str.Append(",@inv_RowKey");
                        //str.Append(",@inv_Quantity");
                        //str.Append(",@inv_Amount)");
                        //comm.CommandType = CommandType.Text;
                        //comm.CommandText = str.ToString();
                        //comm.Parameters.Clear();
                        //comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        //comm.Parameters.Add("@inv_InvoiceKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                        //comm.Parameters.Add("@inv_RowKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CloseRowKey);
                        //comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        //comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Total;
                        //comm.ExecuteNonQuery();
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
                        VoucherNo = VoucherOperation.UpdateInvoiceNo(DB, (int)DocumentKind.ReceiptInWarehouse, opk);
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
        public static CLiCore.OperationResult xUpdateConsumptionReturn(string DB, Transaction Header, List<TransactionDetails> Details)
        {
            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;

                VoucherNo = 0;// Header.InvoiceNo;
                opk = Header.OperationKey;
                Header.AccountKind = (int)PLenums.TransactionAccount.Expenses;
                Header.AccountKey = Guid.Parse("ECB12F3F-7712-419E-AB64-C81B3D64AB30");
                Header.DocumentKind = (int)DocumentKind.RetConsumptionStock;

                bool DC = xCore.HeaderDebitOrCredit(Header.DocumentKind);
                int index = 0;
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

                    comm.CommandText = " exec dbo.spfinDocument_DeleteTransactionUpdate @Key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();

                    //Header Data
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Session);
                    comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                    comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                    comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = Header.InvoiceKind;
                    comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = Header.InvoiceNo;
                    comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                    comm.Parameters.Add("@inv_InvoiceDatetime", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.InvoiceDate.Value.Date + DateTime.Now.TimeOfDay);
                    comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = Header.MonthlyNo;
                    comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate); ;
                    comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.DueDate); ;
                    comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CurrentKey);
                    comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                    comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = Header.SalesHand;
                    comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                    comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus * (x.UnitPrice - x.Discount));
                    comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount - x.Discount + x.vatAmount);
                    comm.Parameters.Add("@inv_PaymentDiscount", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                    comm.Parameters.Add("@inv_RetentionLess", SqlDbType.Decimal).Value = Header.RetentionLess;
                    comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = Details.Sum(x => x.UnitCost);
                    comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus);
                    comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = Header.DeliveryKind;
                    comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Fixture).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.ImportationKey);
                    comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = Header.Returned;
                    comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                    comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = Header.AccountKind == 0 ? true : false;
                    comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();

                    //Financial Header
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@Fin_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@Fin_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@Fin_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@Fin_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = Header.InvoiceDate;
                    comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = Header.MonthlyNo;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.DueDate);
                    comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                    comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                    comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false;
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value = "";
                    comm.ExecuteNonQuery();

                    foreach (Documents.TransactionDetails item in Details)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = 1;
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                        comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                        comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ProDate);
                        comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ExpDate);
                        comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = item.Color;
                        comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = item.Size;
                        comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@inv_SalesPrice", SqlDbType.Decimal).Value = item.SalesPrice;
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                        comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@inv_DiscountText", SqlDbType.NVarChar, 10).Value = item.DiscountText ?? "";
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                        comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Batch);
                        comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = item.ConsumptionKind;
                        comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = item.Status;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = item.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = item.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(item.Project);
                        comm.ExecuteNonQuery();

                        ////Return Lines
                        //str.Clear();
                        //str.Append("INSERT INTO InvDocument_TransactionDetailsClosed");
                        //str.Append("([inv_OperationKey]");
                        //str.Append(",[inv_InvoiceKey]");
                        //str.Append(",[inv_RowKey]");
                        //str.Append(",[inv_Quantity]");
                        //str.Append(",[inv_Amount])");
                        //str.Append(" VALUES ");
                        //str.Append("(@inv_OperationKey");
                        //str.Append(",@inv_InvoiceKey");
                        //str.Append(",@inv_RowKey");
                        //str.Append(",@inv_Quantity");
                        //str.Append(",@inv_Amount)");
                        //comm.CommandType = CommandType.Text;
                        //comm.CommandText = str.ToString();
                        //comm.Parameters.Clear();
                        //comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        //comm.Parameters.Add("@inv_InvoiceKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                        //comm.Parameters.Add("@inv_RowKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CloseRowKey);
                        //comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        //comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Total;
                        //comm.ExecuteNonQuery();

                    }

                    var ConsumptionGroup = Details.GroupBy(x => new
                    {
                        x.CostCenter,
                        x.Project
                    })
                   .Select(G => new CostCenterDistribution()
                   {
                       Amount = G.Sum(x => x.Amount),
                       CostCenter = G.Key.CostCenter,
                       Project = G.Key.Project
                   });
                    index = 0;
                    CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);
                    foreach (CostCenterDistribution conitem in ConsumptionGroup)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Expenses;
                        comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                        comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.ChartofAccount;
                        comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(accounts.PurchaseAccount);
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = conitem.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(conitem.CostCenter);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = conitem.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(conitem.Project);
                        comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                        comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = true;
                        comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = conitem.Amount;
                        comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = conitem.Amount;
                        comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = conitem.Amount;
                        comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = conitem.Amount;
                        comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                        comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                        comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                        comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                        comm.ExecuteNonQuery();
                        ++index;
                    }


                    str.Clear();
                    str.Append("exec dbo.spAccOperation_PostTransaction @Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.ExecuteNonQuery();

                    str.Clear();
                    str.Append("exec dbo.spInvOperation_UpdateStockBalance @Key ");
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
                    comm.Parameters.Add("@sys_User", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@sys_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                    comm.ExecuteNonQuery();

                    try
                    {
                        transaction.Commit();
                       
                            VoucherNo = VoucherOperation.UpdateInvoiceNo(DB, Header.DocumentKind, opk);
                         //   VoucherOperation.UpdateInvoiceMonthlyNo(DB, Header.InvoiceDate.Value.Year, Header.InvoiceDate.Value.Month, Header.DocumentKind, opk);
                        
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
        public static CLiCore.OperationResult xUpdateConsumption(string DB, Transaction Header, List<TransactionDetails> Details)
        {
            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;

                VoucherNo = 0;// Header.InvoiceNo;
                opk = Header.OperationKey;
                Header.AccountKind = (int)PLenums.TransactionAccount.Expenses;
              //  Header.AccountKey = Guid.Parse("ECB12F3F-7712-419E-AB64-C81B3D64AB30");
                Header.DocumentKind = (int)DocumentKind.RetConsumptionStock;

                bool DC = xCore.HeaderDebitOrCredit(Header.DocumentKind);
                int index = 0;
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

                    comm.CommandText = " exec dbo.spfinDocument_DeleteTransactionUpdate @Key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();

                    //Header Data
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Session);
                    comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                    comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                    comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = Header.InvoiceKind;
                    comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.InvoiceDate);
                    comm.Parameters.Add("@inv_InvoiceDatetime", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.InvoiceDate.Value.Date + DateTime.Now.TimeOfDay);
                    comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = Header.MonthlyNo;
                    comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate); ;
                    comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.DueDate); ;
                    comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CurrentKey);
                    comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.SalesPerson);
                    comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = Header.SalesHand;
                    comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = Details.Sum(x => x.Discount);
                    comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus * (x.UnitPrice - x.Discount));
                    comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount - x.Discount + x.vatAmount);
                    comm.Parameters.Add("@inv_PaymentDiscount", SqlDbType.Decimal).Value = Header.PaymentDiscount;
                    comm.Parameters.Add("@inv_RetentionLess", SqlDbType.Decimal).Value = Header.RetentionLess;
                    comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = Details.Sum(x => x.UnitCost);
                    comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Quantity);
                    comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = Details.Sum(x => x.Bonus);
                    comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = Header.DeliveryKind;
                    comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Fixture).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false; ;
                    comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.ImportationKey);
                    comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = Header.Returned;
                    comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OriginalInvoice);
                    comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = Header.AccountKind == 0 ? true : false;
                    comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();

                    //Financial Header
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@Fin_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@Fin_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@Fin_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@Fin_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@Fin_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = Header.InvoiceDate;
                    comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = Header.MonthlyNo;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.DueDate);
                    comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                    comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                    comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false;
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value = "";
                    comm.ExecuteNonQuery();

                    foreach (Documents.TransactionDetails item in Details)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = -1;
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = Header.TargetWarehouse;
                        comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = item.ItemKind;
                        comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ProDate);
                        comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ExpDate);
                        comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = item.Color;
                        comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = item.Size;
                        comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitPrice;
                        comm.Parameters.Add("@inv_SalesPrice", SqlDbType.Decimal).Value = item.SalesPrice;
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                        comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = item.Bonus;
                        comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = item.Discount;
                        comm.Parameters.Add("@inv_DiscountText", SqlDbType.NVarChar, 10).Value = item.DiscountText ?? "";
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                        comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Batch);
                        comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = item.ConsumptionKind;
                        comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = item.Status;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = item.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = item.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(item.Project);
                        comm.ExecuteNonQuery();

                    }

                    var ConsumptionGroup = Details.GroupBy(x => new
                    {
                        x.CostCenter,
                        x.Project
                    })
                   .Select(G => new CostCenterDistribution()
                   {
                       Amount = G.Sum(x => x.Amount),
                       CostCenter = G.Key.CostCenter,
                       Project = G.Key.Project
                   });
                    index = 0;
                    CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);
                    foreach (CostCenterDistribution conitem in ConsumptionGroup)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Expenses;
                        comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                        comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.ChartofAccount;
                        comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(accounts.PurchaseAccount);
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = conitem.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(conitem.CostCenter);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = conitem.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(conitem.Project);
                        comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                        comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = conitem.Amount;
                        comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = conitem.Amount;
                        comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = conitem.Amount;
                        comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = conitem.Amount;
                        comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = true;
                        comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = "";
                        comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = "";
                        comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                        comm.ExecuteNonQuery();
                        ++index;
                    }


                    //str.Clear();
                    //str.Append("exec dbo.spAccOperation_PostTransaction @Key ");
                    //comm.CommandType = CommandType.Text;
                    //comm.CommandText = str.ToString();
                    //comm.Parameters.Clear();
                    //comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    //comm.ExecuteNonQuery();

                    //str.Clear();
                    //str.Append("exec dbo.spInvOperation_UpdateStockBalance @Key ");
                    //comm.CommandType = CommandType.Text;
                    //comm.CommandText = str.ToString();
                    //comm.Parameters.Clear();
                    //comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    //comm.ExecuteNonQuery();

                    //str.Clear();
                    //str.Append("INSERT INTO system_DocumentEdited");
                    //str.Append("([sys_Key]");
                    //str.Append(",[sys_User]");
                    //str.Append(",[sys_Date])");
                    //str.Append(" VALUES ");
                    //str.Append("(@sys_Key");
                    //str.Append(",@sys_User");
                    //str.Append(",@sys_Date)");
                    //comm.CommandType = CommandType.Text;
                    //comm.CommandText = str.ToString();
                    //comm.Parameters.Clear();
                    //comm.Parameters.Add("@sys_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    //comm.Parameters.Add("@sys_User", SqlDbType.Int).Value = Header.LastupUser;
                    //comm.Parameters.Add("@sys_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                    //comm.ExecuteNonQuery();

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
    public class CostCenterDistribution
    {
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public decimal Amount { get; set; }

    }
    public class InvoiceItems
    {
        public Guid? ItemKey { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}
