using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
using CLiCore.Selections;

namespace CLiFinancial.Documents
{
    public class core
    {
        private readonly static object Locker = new object();
        public static OperationResult UpdateTransaction(string DB, Documents.Transaction Header, List<Documents.TransactionDetails> Details, bool IsNew, List<TransactionInvoices> Invoices)
        {
            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;
                int MonthlyNo;
                if (IsNew == false)
                {
                    VoucherNo = Header.VoucherNo;
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
                string QR;

                QR = iCore.QR(DB, Header.VoucherDate.Value, Details.Sum(x => x.Total), Details.Sum(x => x.vatAmount));

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
                    comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Category);
                    comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = Header.VoucherDate;
                    comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = MonthlyNo;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.DueDate);
                    comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = Header.TransactionNo ?? "";
                    comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Amount);
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = Details.Sum(x => x.vatAmount);
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Total);
                    comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                    comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = Details.Where(x => x.AccountKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false;
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value = Header.RecipientName ?? "";
                    comm.ExecuteNonQuery();

                    foreach (Documents.TransactionDetails item in Details)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = item.Status;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = item.ReferenceNo ?? "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ReferenceDate);
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = item.TransactionNo ?? "";
                        comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = Header.AccountKind;
                        comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                        comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = item.AccountKind;
                        comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = item.AccountType;
                        comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.AccountKey);
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = item.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = item.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(item.Project);
                        comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Client);
                        comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Person);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = DC;
                        comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = item.Amount;
                        comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                        comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = item.Total;
                        comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = item.vatRegNo ?? "";
                        comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = item.vatCurrent ?? "";
                        comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Importation);
                        comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = item.ExpensesKind;
                        comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = item.PaymentKind;
                        comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                        comm.ExecuteNonQuery();

                        if (item.vatAmount > 0)
                        {
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = Details.Max(x => x.Index) + 1;
                            comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                            comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                            comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                            comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = Header.TransactionNo ?? "";
                            comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = Header.AccountKind;
                            comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                            comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.CurrentAccount;
                            comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = (int)PLenums.CurrentAccountKind.Adjustment;
                            comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(xConfig.vatKey(DB));
                            comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                            comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = DC;
                            comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = item.vatAmount;
                            comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                            comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = xConfig.vatRate(DB, item.vatKey);
                            comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = item.vatAmount;
                            comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = item.vatAmount;
                            comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = item.vatAmount;
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
                            comm.Parameters.Add("@vat_Date", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.VoucherDate);
                            comm.Parameters.Add("@vat_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                            comm.Parameters.Add("@vat_Kind", SqlDbType.Int).Value = Header.DocumentKind == (int)DocumentKind.finDebitNote ? (int)PLenums.vatKind.Sales : (int)PLenums.vatKind.Purchase;
                            comm.Parameters.Add("@vat_vatRate", SqlDbType.Decimal).Value = xConfig.vatRate(DB, item.vatKey);
                            comm.Parameters.Add("@vat_Subtotal", SqlDbType.Decimal).Value = item.Amount;
                            comm.Parameters.Add("@vat_Debit", SqlDbType.Decimal).Value =Header.DocumentKind==(int)DocumentKind.finDebitNote?0: item.vatAmount;
                            comm.Parameters.Add("@vat_Credit", SqlDbType.Decimal).Value = Header.DocumentKind == (int)DocumentKind.finDebitNote ?   item.vatAmount:0;
                            comm.Parameters.Add("@vat_vatRegNo", SqlDbType.NVarChar, 25).Value = item.vatRegNo ?? "";
                            comm.Parameters.Add("@vat_AccountName", SqlDbType.NVarChar, 200).Value = item.vatCurrent ?? "";
                            comm.Parameters.Add("@vat_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                            comm.ExecuteNonQuery();
                        }

                        if (item.AccountKind == (int)PLenums.TransactionAccount.Fixture)
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
                            comm.Parameters.Add("@fxd_Fixture", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.AccountKey);
                            //comm.Parameters.Add("@fxd_DepPercent", SqlDbType.Decimal).Value = item.DepPercent;
                            comm.Parameters.Add("@fxd_PurchaseDate", SqlDbType.Date).Value = Header.VoucherDate;
                            comm.Parameters.Add("@fxd_Quantity", SqlDbType.Decimal).Value = 1;
                            comm.Parameters.Add("@fxd_UnitPrice", SqlDbType.Decimal).Value = item.Amount;
                            comm.Parameters.Add("@fxd_Amount", SqlDbType.Decimal).Value = item.Amount;
                            comm.Parameters.Add("@fxd_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                            comm.Parameters.Add("@fxd_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                            comm.ExecuteNonQuery();
                        }

                        if (item.SalaryKey != null)
                        {
                            str.Clear();
                            str.Append("INSERT INTO finDocument_TransactionPaymentSalaries");
                            str.Append("([Fin_OperationKey]");
                            str.Append(",[Fin_RowKey]");
                            str.Append(",[Fin_SalaryKey]");
                            str.Append(",[Fin_Amount])");
                            str.Append(" VALUES ");
                            str.Append("(@Fin_OperationKey");
                            str.Append(",@Fin_RowKey");
                            str.Append(",@Fin_SalaryKey");
                            str.Append(",@Fin_Amount)");
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = str.ToString();
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                            comm.Parameters.Add("@Fin_RowKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                            comm.Parameters.Add("@Fin_SalaryKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.SalaryKey);
                            comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = item.Amount;
                            comm.ExecuteNonQuery();
                        }
                    }



                    // var vatGroup = Details.GroupBy(x => new
                    // {
                    //     x.vatKey,
                    //     x.vatRegNo,
                    //     x.vatCurrent
                    // })
                    //.Select(y => new vatGroup()
                    //{
                    //    Key = y.Key.vatKey,
                    //    Amount = y.Sum(x => x.vatAmount),
                    //    Subtotal=y.Sum(x=>x.Amount),
                    //    AccountName = y.Key.vatCurrent,
                    //    RegNo = y.Key.vatRegNo,
                    //});

                    if(Header.DocumentKind == 7 || Header.DocumentKind == 8)
                    {
                        str.Clear();
                        str.Append("INSERT INTO finDocument_TransactionQR ");
                        str.Append("([Fin_OperationKey]");
                        str.Append(",[Fin_QR])");
                        str.Append(" VALUES ");
                        str.Append("(@Fin_OperationKey");
                        str.Append(",@Fin_QR)");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_QR", SqlDbType.NVarChar, -1).Value = QR;
                        comm.ExecuteNonQuery();
                    }
               




                    foreach (TransactionInvoices invoice in Invoices)
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
                        comm.Parameters.Add("@cls_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@cls_RowKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(invoice.Key);
                        comm.Parameters.Add("@cls_InvoiceKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(invoice.InvoiceKey);
                        comm.Parameters.Add("@cls_Amount", SqlDbType.Decimal).Value = invoice.Amount;
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
                            VoucherNo = VoucherOperation.UpdateVoucherNo(DB, Header.DocumentKind, Header.VoucherDate.Value.Year, opk);
                            VoucherOperation.UpdateVouchermonthlyNo(DB, Header.VoucherDate.Value.Year, Header.VoucherDate.Value.Month, Header.DocumentKind, opk);
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
        public static OperationResult UpdateJournalVoucher(string DB, Documents.Transaction Header, List<Documents.TransactionDetails> Details, bool IsNew)
        {
            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;
                int MonthlyNo;
                if (IsNew == false)
                {
                    VoucherNo = Header.VoucherNo;
                    MonthlyNo = Header.MonthlyNo;
                    opk = Header.OperationKey;
                }
                else
                {
                    opk = Guid.NewGuid();
                    VoucherNo = 0;
                    MonthlyNo = 0;
                }

                bool DC = false;// xCore.HeaderDebitOrCredit(Header.DocumentKind);

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
                    comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Category);
                    comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = Header.VoucherDate;
                    comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = MonthlyNo;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.DueDate);
                    comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = Header.TransactionNo ?? "";
                    comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Debit);
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = Details.Where(x => x.isVAT == true).Sum(x => x.Debit - x.Credit);
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Debit);
                    comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                    comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = Details.Where(x => x.AccountKind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false;
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value = Header.RecipientName ?? "";
                    comm.ExecuteNonQuery();

                    foreach (Documents.TransactionDetails item in Details)
                    {
                        decimal amount = 0;
                        if (item.Debit > 0)
                            amount = item.Debit;
                        else
                            amount = item.Credit;

                        if (item.Debit > 0)
                            DC = false;
                        else
                            DC = true;

                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = item.Status;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = item.ReferenceNo ?? "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ReferenceDate);
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = item.TransactionNo ?? "";
                        comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = item.AccountKind;
                        comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = item.AccountType;
                        comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.AccountKey);
                        comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = item.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = item.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(item.Project);
                        comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Client);
                        comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Person);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                        comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = DC;
                        comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = amount;
                        comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                        comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = item.vatRate;
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = amount;
                        comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = amount;
                        comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = amount;
                        comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = item.isVAT;
                        comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_vatRegNo", SqlDbType.NVarChar, 50).Value = item.vatRegNo ?? "";
                        comm.Parameters.Add("@Fin_vatCurrent", SqlDbType.NVarChar, 200).Value = item.vatCurrent ?? "";
                        comm.Parameters.Add("@Fin_Importation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Importation);
                        comm.Parameters.Add("@Fin_ExpensesKind", SqlDbType.Int).Value = item.ExpensesKind;
                        comm.Parameters.Add("@Fin_PaymentKind", SqlDbType.Int).Value = item.PaymentKind;
                        comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = false;
                        comm.ExecuteNonQuery();

                        if (item.isVAT == true)
                        {
                            decimal vatrate = xConfig.vatRate(DB, item.vatKey);
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
                            comm.Parameters.Add("@vat_Date", SqlDbType.Date).Value = iCore.IsNullRtDbNull(item.ReferenceDate);
                            comm.Parameters.Add("@vat_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                            comm.Parameters.Add("@vat_Kind", SqlDbType.Int).Value = item.Debit > 0 ? (int)PLenums.vatKind.Purchase : (int)PLenums.vatKind.Sales;
                            comm.Parameters.Add("@vat_vatRate", SqlDbType.Decimal).Value = vatrate;
                            comm.Parameters.Add("@vat_Subtotal", SqlDbType.Decimal).Value = ((item.Debit + item.Credit) * 100) / vatrate;
                            comm.Parameters.Add("@vat_Debit", SqlDbType.Decimal).Value = item.Debit;
                            comm.Parameters.Add("@vat_Credit", SqlDbType.Decimal).Value = item.Credit;
                            comm.Parameters.Add("@vat_vatRegNo", SqlDbType.NVarChar, 25).Value = item.vatRegNo ?? "";
                            comm.Parameters.Add("@vat_AccountName", SqlDbType.NVarChar, 200).Value = item.vatCurrent ?? "";
                            comm.Parameters.Add("@vat_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                            comm.ExecuteNonQuery();
                        }

                        if (item.AccountKind == (int)PLenums.TransactionAccount.Fixture && item.AccountType == 0)
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
                            comm.Parameters.Add("@fxd_Fixture", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.AccountKey);
                            //comm.Parameters.Add("@fxd_DepPercent", SqlDbType.Decimal).Value = item.DepPercent;
                            comm.Parameters.Add("@fxd_PurchaseDate", SqlDbType.Date).Value = Header.VoucherDate;
                            comm.Parameters.Add("@fxd_Quantity", SqlDbType.Decimal).Value = 1;
                            comm.Parameters.Add("@fxd_UnitPrice", SqlDbType.Decimal).Value = amount;
                            comm.Parameters.Add("@fxd_Amount", SqlDbType.Decimal).Value = amount;
                            comm.Parameters.Add("@fxd_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                            comm.Parameters.Add("@fxd_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
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
                            VoucherNo = VoucherOperation.UpdateVoucherNo(DB, Header.DocumentKind, Header.VoucherDate.Value.Year, opk);
                            VoucherOperation.UpdateVouchermonthlyNo(DB, Header.VoucherDate.Value.Year, Header.VoucherDate.Value.Month, Header.DocumentKind, opk);
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
        public static OperationResult PostSalaries(string DB, Documents.Transaction Header, int Year, int Month)
        {
            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;
                int MonthlyNo;

                opk = Guid.NewGuid();
                VoucherNo = 0;
                MonthlyNo = 0;
                DateTime voucherdate = new DateTime(Year, Month, 1).AddMonths(1).AddDays(-1);
                List<CLiFinancial.Operation.SalariesPost> Details = new Operation.SalariesPost().GetList(DB, Year, Month);
                bool DC = false;// xCore.HeaderDebitOrCredit(Header.DocumentKind);

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
                    comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Category);
                    comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = (int)DocumentKind.finJournalVoucher;
                    comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = VoucherNo;
                    comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = voucherdate;
                    comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = MonthlyNo;
                    comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                    comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.DueDate);
                    comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = Header.AccountKind;
                    comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.AccountKey);
                    comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = Header.TransactionNo ?? "";
                    comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = Header.Currency ?? "";
                    comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Details.Sum(x => x.Debit);
                    comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Details.Sum(x => x.Debit);
                    comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                    comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = Details.Where(x => x.Kind == (int)PLenums.TransactionAccount.Expenses).Count() > 0 ? true : false;
                    comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value = Header.RecipientName ?? "";
                    comm.ExecuteNonQuery();

                    foreach (Operation.SalariesPost item in Details)
                    {
                        decimal amount = 0;
                        if (item.Debit > 0)
                            amount = item.Debit;
                        else
                            amount = item.Credit;

                        if (item.Debit > 0)
                            DC = false;
                        else
                            DC = true;

                        int index = 0;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.ReferenceDate);
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = Header.TransactionNo ?? "";
                        comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = item.Kind;
                        comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = item.Type;
                        comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Account);
                        comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = item.CostCenter == null ? iCore.IsNullRtDbNull(Header.CostCenter) : iCore.IsNullRtDbNull(item.CostCenter);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = item.Project == null ? iCore.IsNullRtDbNull(Header.Project) : iCore.IsNullRtDbNull(item.Project);
                        comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                        comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = DC;
                        comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = amount;
                        comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = amount;
                        comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = amount;
                        comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = amount;
                        comm.Parameters.Add("@Fin_IsVAT", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_Hidden", SqlDbType.Bit).Value = false;
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
                    str.Append("update [hrDocument_Salaries] set [sal_Posted]=1,[sal_Reference]=@Key");
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

                        VoucherNo = VoucherOperation.UpdateVoucherNo(DB, Header.DocumentKind, voucherdate.Year, opk);
                        VoucherOperation.UpdateVouchermonthlyNo(DB, voucherdate.Year, voucherdate.Month, (int)DocumentKind.finJournalVoucher, opk);

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
    public class vatGroup
    {
        public string AccountName { get; set; }
        public string RegNo { get; set; }
        public Guid? Key { get; set; }
        public decimal Amount { get; set; }
        public decimal Subtotal { get; set; }
    }
}
