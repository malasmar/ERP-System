using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using CLiInventory.Documents;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using CLiInventory.Audit;

namespace CLiInventory
{
    public class core
    {
        private readonly static object Locker = new object();
        public static int DeleteItemsCost(string DB, DateTime Date)
        {
            int res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "DELETE d FROM [InvDocument_TransactionUnitCost] d LEFT JOIN InvDocument_Transaction t ON d.cost_OperationKey = T.inv_OperationKey WHERE T.inv_InvoiceDate >= @Date";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Date", SqlDbType.Date).Value = Date;
                con.Open();
                res = comm.ExecuteNonQuery();
            }
            return res;
        }
        public static void UpdateItemsCost(string DB, Guid? Key, int DocKind, DateTime? StartDate)
        {


            Documents.Transaction Header = null;
            List<Documents.TransactionDetails> Details = null;
            //if (DocKind == 80)
            //{
            //   Header = new Documents.Transaction().GetItem(DB, Key);
            //    Details = new Documents.TransactionDetails().GetList(DB, Key);
            //}


            using (SqlConnection connection = new SqlConnection(iCore.GetCon(DB)))
            {
                connection.Open();
                SqlCommand comm = connection.CreateCommand();
                SqlTransaction transaction;
                // Start a local transaction.
                transaction = connection.BeginTransaction("RQTransaction");
                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                comm.Connection = connection;
                comm.Transaction = transaction;
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                if (DocKind == 71)
                {


                    str.Clear();
                    str.Append(" DELETE FROM finDocument_TransactionDetails WHERE Fin_OperationKey=@Key AND Fin_IsSalesCost=1 ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    comm.ExecuteNonQuery();

                    Header = new Documents.Transaction().GetItem(DB, Key);
                    Details = new Documents.TransactionDetails().GetList(DB, Key);

                    CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                    comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = Details.Max(x => x.Index) + 1;
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



                    //str.Clear();
                    //str.Append("exec dbo.spInvOperation_UpdateStockCost @opk,@DocKind");
                    //comm.CommandType = CommandType.Text;
                    //comm.CommandText = str.ToString();
                    //comm.Parameters.Clear();
                    //comm.Parameters.Add("@opk", SqlDbType.UniqueIdentifier).Value = Key;
                    //comm.Parameters.Add("@DocKind", SqlDbType.Int).Value = DocKind;
                    //comm.ExecuteNonQuery();


                    //Calc New Cost

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
                        //if (item.ItemKey.ToString() != "e1b41e89-98b4-4ba5-8cf9-ff1fa410494b")
                        //    continue;



                        decimal ItemBalance = 0;
                        ItemBalance = ItemUnitBalanceFullWarehouse(DB, item.ItemKey, item.Unit, Header.InvoiceDateTime);
                        decimal UnitCost = 0;// GetLastUnitCost(DB, item.ItemKey, item.Unit, Header.InvoiceDateTime);
                        UnitCost = GetLastUnitCost(DB, item.ItemKey, item.Unit, Header.InvoiceDateTime);
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
                                str.Append(",[cost_UnitPrice]");
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
                                str.Append(",@cost_UnitPrice");
                                str.Append(",@cost_LastCost");
                                str.Append(",@cost_UnitCost");
                                str.Append(",@cost_Additional)");
                                com.CommandType = CommandType.Text;
                                com.CommandText = str.ToString();
                                com.Parameters.Clear();
                                com.Parameters.Add("@cost_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                                com.Parameters.Add("@cost_Date", SqlDbType.DateTime).Value = Header.InvoiceDateTime;
                                com.Parameters.Add("@cost_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                                com.Parameters.Add("@cost_Balance", SqlDbType.Decimal).Value = ItemBalance - item.Quantity;
                                com.Parameters.Add("@cost_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                                com.Parameters.Add("@cost_Amount", SqlDbType.Decimal).Value = (ItemBalance - item.Quantity) * UnitCost;
                                com.Parameters.Add("@cost_UnitPrice", SqlDbType.Decimal).Value = item.Amount / item.Quantity;
                                com.Parameters.Add("@cost_LastCost", SqlDbType.Decimal).Value = UnitCost;
                                com.Parameters.Add("@cost_UnitCost", SqlDbType.Decimal).Value = (((ItemBalance - item.Quantity) * UnitCost) + item.Amount) / ItemBalance;
                                com.Parameters.Add("@cost_Additional", SqlDbType.Decimal).Value = 0;
                                con.Open();
                                com.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            List<Audit.ItemSales> sales = new Audit.ItemSales().GetList(DB, item.ItemKey, Header.InvoiceDateTime, StartDate);
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
                                    str.Append(",[cost_UnitPrice]");
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
                                    str.Append(",@cost_UnitPrice");
                                    str.Append(",@cost_LastCost");
                                    str.Append(",@cost_UnitCost");
                                    str.Append(",@cost_Additional)");
                                    com.CommandType = CommandType.Text;
                                    com.CommandText = str.ToString();
                                    com.Parameters.Clear();
                                    com.Parameters.Add("@cost_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                                    com.Parameters.Add("@cost_Date", SqlDbType.DateTime).Value = Header.InvoiceDateTime;
                                    com.Parameters.Add("@cost_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                                    com.Parameters.Add("@cost_Balance", SqlDbType.Decimal).Value = item.Quantity;
                                    com.Parameters.Add("@cost_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                                    com.Parameters.Add("@cost_Amount", SqlDbType.Decimal).Value = item.Amount;
                                    com.Parameters.Add("@cost_UnitPrice", SqlDbType.Decimal).Value = item.Amount / item.Quantity;
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
                                        int invoicestatus = CheckInvoiceDate(DB, item.ItemKey, salitem.InvoiceDate) + 1;
                                        UnitCost = GetLastUnitCost(DB, item.ItemKey, item.Unit, salitem.InvoiceDate.Value.AddSeconds(invoicestatus * -1));
                                        using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
                                        {
                                            SqlCommand com = new SqlCommand();
                                            com.Connection = con;
                                            con.Open();

                                            str.Clear();
                                            str.Append("INSERT INTO InvDocument_TransactionUnitCost");
                                            str.Append("([cost_OperationKey]");
                                            str.Append(",[cost_Date]");
                                            str.Append(",[cost_ItemKey]");
                                            str.Append(",[cost_Balance]");
                                            str.Append(",[cost_Quantity]");
                                            str.Append(",[cost_Amount]");
                                            str.Append(",[cost_UnitPrice]");
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
                                            str.Append(",@cost_UnitPrice");
                                            str.Append(",@cost_LastCost");
                                            str.Append(",@cost_UnitCost");
                                            str.Append(",@cost_Additional)");
                                            com.CommandType = CommandType.Text;
                                            com.CommandText = str.ToString();
                                            com.Parameters.Clear();
                                            com.Parameters.Add("@cost_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                                            com.Parameters.Add("@cost_Date", SqlDbType.DateTime).Value = salitem.InvoiceDate.Value.AddSeconds(invoicestatus  * - 1);
                                            com.Parameters.Add("@cost_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                                            com.Parameters.Add("@cost_Balance", SqlDbType.Decimal).Value = dif;
                                            com.Parameters.Add("@cost_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                                            com.Parameters.Add("@cost_Amount", SqlDbType.Decimal).Value = dif * UnitCost;
                                            com.Parameters.Add("@cost_UnitPrice", SqlDbType.Decimal).Value = item.Amount / item.Quantity;
                                            com.Parameters.Add("@cost_LastCost", SqlDbType.Decimal).Value = UnitCost;
                                            com.Parameters.Add("@cost_UnitCost", SqlDbType.Decimal).Value = ((dif * UnitCost) + item.Amount) / (dif + item.Quantity);
                                            com.Parameters.Add("@cost_Additional", SqlDbType.Decimal).Value = 0;
                                            com.ExecuteNonQuery();

                                            //str.Clear();
                                            //str.Append("INSERT INTO InvDocument_TransactionOffsetDate");
                                            //str.Append("([cost_OperationKey]");
                                            //str.Append(",[cost_Date]");
                                            //str.Append(",[cost_ItemKey])");
                                            //str.Append(" VALUES ");
                                            //str.Append("(@cost_OperationKey");
                                            //str.Append(",@cost_Date");
                                            //str.Append(",@cost_ItemKey)");
 
                                            //com.CommandType = CommandType.Text;
                                            //com.CommandText = str.ToString();
                                            //com.Parameters.Clear();
                                            //com.Parameters.Add("@cost_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                                            //com.Parameters.Add("@cost_Date", SqlDbType.DateTime).Value = salitem.InvoiceDate.Value.AddSeconds(invoicestatus * -1);
                                            //com.Parameters.Add("@cost_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                                            //com.ExecuteNonQuery();
                                        }
                                        break;
                                    }
                                }
                            }

                        }
                    }
                }


                if (DocKind == 75)
                {

                    str.Clear();
                    str.Append(" DELETE FROM finDocument_TransactionDetails WHERE Fin_OperationKey=@Key AND Fin_IsSalesCost=1 ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    comm.ExecuteNonQuery();

                    Header = new Documents.Transaction().GetItem(DB, Key);
                    Details = new Documents.TransactionDetails().GetList(DB, Key);

                    CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                    comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = Details.Max(x => x.Index) + 1;
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
                if (DocKind == 80)
                {
                    //str.Clear();
                    //str.Append("UPDATE InvDocument_TransactionDetails SET inv_UnitCost = dbo.invGet_LastUnitCost(inv_ItemKey, inv_Unit, T.inv_InvoiceDate) FROM InvDocument_TransactionDetails d LEFT JOIN InvDocument_Transaction T ON d.inv_OperationKey = T.inv_OperationKey WHERE d.inv_OperationKey = @Key ");
                    //comm.CommandType = CommandType.Text;
                    //comm.CommandText = str.ToString();
                    //comm.Parameters.Clear();
                    //comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    //comm.ExecuteNonQuery();

                    UpdateSalesInvoiceData(DB, Key);


                    str.Clear();
                    str.Append(" DELETE FROM finDocument_TransactionDetails WHERE Fin_OperationKey=@Key AND Fin_IsSalesCost=1 ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    comm.ExecuteNonQuery();

                    Header = new Documents.Transaction().GetItem(DB, Key);
                    Details = new Documents.TransactionDetails().GetList(DB, Key);
                    int index = Details.Max(x => x.Index) + 1;
                    CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);

                    if (Details.Count(x => x.ItemKind == 8) > 0)
                    {

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
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OperationKey);
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
                            comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = true;
                            comm.ExecuteNonQuery();
                        }

                        //Purchase Cost
                        ++index;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OperationKey);
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
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OperationKey);
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

                    str.Clear();
                    str.Append("   DELETE FROM [accDocument_GeneralLedger] WHERE gl_OperationKey=@Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    comm.ExecuteNonQuery();

                    str.Clear();
                    str.Append("exec dbo.spAccOperation_PostTransaction @Key");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    comm.ExecuteNonQuery();

                }
                if (DocKind == 83)
                {
                    //str.Clear();
                    //str.Append("UPDATE InvDocument_TransactionDetails SET inv_UnitCost = dbo.invGet_LastUnitCost(inv_ItemKey, inv_Unit, T.inv_InvoiceDate) FROM InvDocument_TransactionDetails d LEFT JOIN InvDocument_Transaction T ON d.inv_OperationKey = T.inv_OperationKey WHERE d.inv_OperationKey = @Key ");
                    //comm.CommandType = CommandType.Text;
                    //comm.CommandText = str.ToString();
                    //comm.Parameters.Clear();
                    //comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    //comm.ExecuteNonQuery();

                    ////UpdateSalesInvoiceData(DB, Key);






                    str.Clear();
                    str.Append(" DELETE FROM finDocument_TransactionDetails WHERE Fin_OperationKey=@Key AND Fin_IsSalesCost=1 ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    comm.ExecuteNonQuery();

                    Header = new Documents.Transaction().GetItem(DB, Key);
                    Details = new Documents.TransactionDetails().GetList(DB, Key);
                    CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);
                    int index = Details.Max(x => x.Index);



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
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
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
                        comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
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
                        comm.Parameters.Add("@Fin_IsSalesCost", SqlDbType.Int).Value = true;
                        comm.ExecuteNonQuery();
                    }
                    ++index;

                    //Cost
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
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
                    }).Select(G => new Documents.CostCenterDistribution()
                    {
                        Amount = G.Sum(x => x.UnitCost * x.Quantity),
                        CostCenter = G.Key.CostCenter,
                        Project = G.Key.Project
                    });
                    foreach (Documents.CostCenterDistribution disitem in DisSalesCost)
                    {
                        ++index;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
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



                    str.Clear();
                    str.Append("   DELETE FROM [accDocument_GeneralLedger] WHERE gl_OperationKey=@Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    comm.ExecuteNonQuery();

                    str.Clear();
                    str.Append("exec dbo.spAccOperation_PostTransaction @Key");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    comm.ExecuteNonQuery();

                }
                if (DocKind == 77)
                {
                    Header = new Documents.Transaction().GetItem(DB, Key);
                    Details = new Documents.TransactionDetails().GetList(DB, Key);

                    str.Clear();
                    str.Append(" DELETE FROM finDocument_TransactionDetails WHERE Fin_OperationKey=@Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    comm.ExecuteNonQuery();


                    var ItemsDetails = Details.Where(x => x.ItemKind == (int)PLenums.TransactionAccount.Stock).GroupBy(x => new
                    {
                        x.ItemKey,
                        x.Unit
                    }).Select(G => new ItemsDetails()
                    {
                        Quantity = G.Sum(x => x.Quantity),
                        ItemKey = G.Key.ItemKey,
                        Unit = G.Key.Unit
                    });

                    //foreach (ItemsDetails item in ItemsDetails)
                    //{
                    //    decimal ItemBalance = 0;
                    //    ItemBalance = ItemUnitBalanceFullWarehouse(DB, item.ItemKey, item.Unit, Header.InvoiceDate);
                    //    if (ItemBalance >= 0)
                    //    {
                    //        //Normal Cost

                    //        str.Clear();
                    //        str.Append("INSERT INTO InvDocument_TransactionDetailsCost");
                    //        str.Append("([cost_OperationKey]");
                    //        str.Append(",[cost_ItemKey]");
                    //        str.Append(",[cost_Quantity]");
                    //        str.Append(",[cost_UnitCost])");
                    //        str.Append(" VALUES ");
                    //        str.Append("(@cost_OperationKey");
                    //        str.Append(",@cost_ItemKey");
                    //        str.Append(",@cost_Quantity");
                    //        str.Append(",@cost_UnitCost)");
                    //        comm.CommandType = CommandType.Text;
                    //        comm.CommandText = str.ToString();
                    //        comm.Parameters.Clear();
                    //        comm.Parameters.Add("@cost_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OperationKey);
                    //        comm.Parameters.Add("@cost_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                    //        comm.Parameters.Add("@cost_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                    //        comm.Parameters.Add("@cost_UnitCost", SqlDbType.Decimal).Value = GetLastUnitCost(DB, item.ItemKey, item.Unit, Header.InvoiceDate);
                    //        comm.ExecuteNonQuery();

                    //    }
                    //    else
                    //    {

                    //        using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
                    //        {
                    //            SqlCommand com = new SqlCommand();
                    //            com.Connection = con;

                    //            //Minance Balance
                    //            List<Audit.ItemCostOffset> costOffsets = new Audit.ItemCostOffset().GetList(DB, item.ItemKey, Header.InvoiceDate);
                    //            if (costOffsets != null)
                    //            {
                    //                decimal Qty = item.Quantity + ItemBalance;
                    //                foreach (Audit.ItemCostOffset cost in costOffsets)
                    //                {
                    //                    if (Qty > 0)
                    //                    {
                    //                        if (cost.Qty > Qty)
                    //                        {
                    //                            str.Clear();
                    //                            str.Append("INSERT INTO InvDocument_TransactionDetailsCost");
                    //                            str.Append("([cost_OperationKey]");
                    //                            str.Append(",[cost_ItemKey]");
                    //                            str.Append(",[cost_Quantity]");
                    //                            str.Append(",[cost_UnitCost])");
                    //                            str.Append(" VALUES ");
                    //                            str.Append("(@cost_OperationKey");
                    //                            str.Append(",@cost_ItemKey");
                    //                            str.Append(",@cost_Quantity");
                    //                            str.Append(",@cost_UnitCost)");

                    //                            com.CommandType = CommandType.Text;
                    //                            com.CommandText = str.ToString();
                    //                            com.Parameters.Clear();
                    //                            com.Parameters.Add("@cost_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OperationKey);
                    //                            com.Parameters.Add("@cost_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                    //                            com.Parameters.Add("@cost_Quantity", SqlDbType.Decimal).Value = Qty;
                    //                            com.Parameters.Add("@cost_UnitCost", SqlDbType.Decimal).Value = cost.UnitCost;
                    //                            com.ExecuteNonQuery();

                    //                            str.Clear();
                    //                            str.Append("INSERT INTO InvDocument_TransactionDetailsCostOffset");
                    //                            str.Append("([cost_OperationKey]");
                    //                            str.Append(",[cost_ItemKey]");
                    //                            str.Append(",[cost_Quantity])");
                    //                            str.Append(" VALUES ");
                    //                            str.Append("(@cost_OperationKey");
                    //                            str.Append(",@cost_ItemKey");
                    //                            str.Append(",@cost_Quantity)");


                    //                            com.CommandType = CommandType.Text;
                    //                            com.CommandText = str.ToString();
                    //                            com.Parameters.Clear();
                    //                            com.Parameters.Add("@cost_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(cost.OperationKey);
                    //                            com.Parameters.Add("@cost_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                    //                            com.Parameters.Add("@cost_Quantity", SqlDbType.Decimal).Value = Qty;
                    //                            com.ExecuteNonQuery();

                    //                            Qty = 0;
                    //                        }
                    //                        else
                    //                        {
                    //                            Qty = Qty - cost.Qty;
                    //                            str.Clear();
                    //                            str.Append("INSERT INTO InvDocument_TransactionDetailsCost");
                    //                            str.Append("([cost_OperationKey]");
                    //                            str.Append(",[cost_ItemKey]");
                    //                            str.Append(",[cost_Quantity]");
                    //                            str.Append(",[cost_UnitCost])");
                    //                            str.Append(" VALUES ");
                    //                            str.Append("(@cost_OperationKey");
                    //                            str.Append(",@cost_ItemKey");
                    //                            str.Append(",@cost_Quantity");
                    //                            str.Append(",@cost_UnitCost)");
                    //                            com.CommandType = CommandType.Text;
                    //                            com.CommandText = str.ToString();
                    //                            com.Parameters.Clear();
                    //                            com.Parameters.Add("@cost_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OperationKey);
                    //                            com.Parameters.Add("@cost_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                    //                            com.Parameters.Add("@cost_Quantity", SqlDbType.Decimal).Value = Qty;
                    //                            com.Parameters.Add("@cost_UnitCost", SqlDbType.Decimal).Value = cost.UnitCost;
                    //                            com.ExecuteNonQuery();

                    //                            str.Clear();
                    //                            str.Append("INSERT INTO InvDocument_TransactionDetailsCostOffset");
                    //                            str.Append("([cost_OperationKey]");
                    //                            str.Append(",[cost_ItemKey]");
                    //                            str.Append(",[cost_Quantity])");
                    //                            str.Append(" VALUES ");
                    //                            str.Append("(@cost_OperationKey");
                    //                            str.Append(",@cost_ItemKey");
                    //                            str.Append(",@cost_Quantity)");
                    //                            com.CommandType = CommandType.Text;
                    //                            com.CommandText = str.ToString();
                    //                            com.Parameters.Clear();
                    //                            com.Parameters.Add("@cost_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(cost.OperationKey);
                    //                            com.Parameters.Add("@cost_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                    //                            com.Parameters.Add("@cost_Quantity", SqlDbType.Decimal).Value = Qty;
                    //                            com.ExecuteNonQuery();
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //            else
                    //            {
                    //                str.Clear();
                    //                str.Append("INSERT INTO InvDocument_TransactionDetailsCost");
                    //                str.Append("([cost_OperationKey]");
                    //                str.Append(",[cost_ItemKey]");
                    //                str.Append(",[cost_Quantity]");
                    //                str.Append(",[cost_UnitCost])");
                    //                str.Append(" VALUES ");
                    //                str.Append("(@cost_OperationKey");
                    //                str.Append(",@cost_ItemKey");
                    //                str.Append(",@cost_Quantity");
                    //                str.Append(",@cost_UnitCost)");
                    //                com.CommandType = CommandType.Text;
                    //                com.CommandText = str.ToString();
                    //                com.Parameters.Clear();
                    //                com.Parameters.Add("@cost_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.OperationKey);
                    //                com.Parameters.Add("@cost_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                    //                com.Parameters.Add("@cost_Quantity", SqlDbType.Decimal).Value = item.Quantity;
                    //                com.Parameters.Add("@cost_UnitCost", SqlDbType.Decimal).Value = GetLastUnitCost(DB, item.ItemKey, item.Unit, Header.InvoiceDate);
                    //                com.ExecuteNonQuery();
                    //            }
                    //        }



                    //    }
                    //}

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
                    int index = 0;
                    CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);
                    foreach (CostCenterDistribution conitem in ConsumptionGroup)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
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
                }
                if (DocKind == 78)
                {
                    Header = new Documents.Transaction().GetItem(DB, Key);
                    Details = new Documents.TransactionDetails().GetList(DB, Key);

                    str.Clear();
                    str.Append(" DELETE FROM finDocument_TransactionDetails WHERE Fin_OperationKey=@Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    comm.ExecuteNonQuery();

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
                    int index = 0;
                    CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.SourceWarehouse);
                    foreach (CostCenterDistribution conitem in ConsumptionGroup)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
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
                }

                if (DocKind == 76)
                {
                    Header = new Documents.Transaction().GetItem(DB, Key);
                    Details = new Documents.TransactionDetails().GetList(DB, Key);

                    str.Clear();
                    str.Append(" DELETE FROM finDocument_Transaction WHERE Fin_OperationKey=@Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    comm.ExecuteNonQuery();



                    //Financial and General Ledger
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                    comm.Parameters.Add("@Fin_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@Fin_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@Fin_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@Fin_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@Fin_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = Header.InvoiceNo;
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
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
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



                }
                if (DocKind == 79)
                {
                    Header = new Documents.Transaction().GetItem(DB, Key);
                    Details = new Documents.TransactionDetails().GetList(DB, Key);

                    str.Clear();
                    str.Append(" DELETE FROM finDocument_Transaction WHERE Fin_OperationKey=@Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    comm.ExecuteNonQuery();


                    //Financial and General Ledger
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.FinDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                    comm.Parameters.Add("@Fin_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@Fin_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@Fin_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@Fin_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@Fin_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@Fin_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = Header.DocumentKind;
                    comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = Header.InvoiceNo;
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
                    comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
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


                }

                //if (DocKind == 83)
                //{
                //    ScStr.Clear();
                //    ScStr.Append("exec dbo.spInvOperation_UpdateRetSalesInvoiceCost @opk,@TrDate");
                //    command.CommandType = CommandType.Text;
                //    command.CommandText = ScStr.ToString();
                //    command.Parameters.Clear();
                //    command.Parameters.Add("@opk", SqlDbType.UniqueIdentifier).Value = Key;
                //    command.Parameters.Add("@TrDate", SqlDbType.DateTime).Value = TrDate; ;
                //    command.ExecuteNonQuery();
                //}
                //ScStr.Clear();
                //ScStr.Append("exec dbo.spAccOperation_UpdateInvoices @opk,@DocKind");
                //command.CommandType = CommandType.Text;
                //command.CommandText = ScStr.ToString();
                //command.Parameters.Clear();
                //command.Parameters.Add("@opk", SqlDbType.UniqueIdentifier).Value = Key;
                //command.Parameters.Add("@DocKind", SqlDbType.Int).Value = DocKind; ;
                //command.ExecuteNonQuery();
                try
                {

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }
        public static int UpdateSalesInvoiceData(string DB, Guid? Key)
        {
            int res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {

                string delQuery = "UPDATE InvDocument_TransactionDetails SET inv_UnitCost = dbo.invGet_LastUnitCost(inv_ItemKey, inv_Unit, T.inv_InvoiceDatetime) FROM InvDocument_TransactionDetails d LEFT JOIN InvDocument_Transaction T ON d.inv_OperationKey = T.inv_OperationKey WHERE d.inv_OperationKey = @Key ";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                res = comm.ExecuteNonQuery();
            }
            return res;
        }
        public static decimal ItemBalanceMainUnit(string DB, Guid? Key, int Warehouse, DateTime? Date)
        {
            if (Key == null)
                return 0;

            decimal result = 0;

            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select dbo.invGet_StockBalance_MainUnit(@Key,@Warehouse,@Date) ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                com.Parameters.Add("@Warehouse", SqlDbType.Int).Value = Warehouse;
                com.Parameters.Add("@Date", SqlDbType.Date).Value = Date;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = Convert.ToDecimal(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static decimal ItemBalanceMainUnitFast(string DB, Guid? Key, int Warehouse)
        {
            if (Key == null)
                return 0;

            decimal result = 0;

            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select dbo.invGet_StockBalance_MainUnit_Fast(@Key,@Warehouse) ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                com.Parameters.Add("@Warehouse", SqlDbType.Int).Value = Warehouse;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = Convert.ToDecimal(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static decimal ItemBalanceUnitFast(string DB, Guid? Key, int Warehouse, string Unit)
        {
            if (Key == null)
                return 0;

            decimal result = 0;

            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select dbo.invGet_StockBalance_Unit_Fast(@Key,@Warehouse,@Unit) ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                com.Parameters.Add("@Warehouse", SqlDbType.Int).Value = Warehouse;
                com.Parameters.Add("@Unit", SqlDbType.NVarChar, 25).Value = Unit ?? "";
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = Convert.ToDecimal(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static Guid? GetItemKeyFromCode(string DB, string Code)
        {
            Guid? result = null;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "SELECT TOP 100 PERCENT item_Key FROM invCard_StockItem  WHERE item_Code=@Code ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Code", SqlDbType.NVarChar, 25).Value = Code ?? "";
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = iCore.IsDbNullRtNull(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static bool UpdateStocktakingOffset(string DB, Audit.Stocktaking Header, List<Audit.StocktakingDetails> Details, Guid? Account, string Comment)
        {
            bool res = false;

            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;

                opk = Guid.NewGuid();
                VoucherNo = CLiCore.VoucherOperation.MaxTransaction(DB, (int)DocumentKind.RetConsumptionStock, Header.Date.Value.Year);


                Guid? opk2;
                int VoucherNo2;

                opk2 = Guid.NewGuid();
                VoucherNo2 = CLiCore.VoucherOperation.MaxTransaction(DB, (int)DocumentKind.ConsumptionStock, Header.Date.Value.Year);

                CLiInventory.Data.WarehouseAccounts accounts = new Data.WarehouseAccounts().GetItem(DB, Header.Warehouse);
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

                    if (Header.IncreaseValue != 0)
                    {
                        //Header Data
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                        comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                        comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.CreateUser;
                        comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                        comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.Warehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = (int)DocumentKind.RetConsumptionStock;
                        comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.Date);
                        comm.Parameters.Add("@inv_InvoiceDateTime", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.Date.Value.Date + Header.CreateDate.Value.TimeOfDay);
                        comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.Date); ;
                        comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null); ;
                        comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Expenses;
                        comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Account);
                        comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Comment ?? "";
                        comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Header.DeficiencyValue + Header.IncreaseValue;
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Header.DeficiencyValue + Header.IncreaseValue;
                        comm.Parameters.Add("@inv_PaymentDiscount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_RetentionLess", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Header.DeficiencyQty + Header.IncreaseQty;
                        comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Key);
                        comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = false;
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
                        comm.Parameters.Add("@Fin_LastupUser", SqlDbType.Int).Value = Header.CreateUser;
                        comm.Parameters.Add("@Fin_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@Fin_Branch", SqlDbType.Int).Value = Header.Branch;
                        comm.Parameters.Add("@Fin_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                        comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = (int)DocumentKind.RetConsumptionStock;
                        comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = VoucherNo;
                        comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = Header.Date;
                        comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.Date);
                        comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Expenses;
                        comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Account);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Comment ?? "";
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = "";
                        comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Header.DeficiencyValue + Header.IncreaseValue;
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Header.DeficiencyValue + Header.IncreaseValue;
                        comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                        comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                        comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value = "";
                        comm.ExecuteNonQuery();

                        foreach (Audit.StocktakingDetails item in Details)
                        {
                            if (item.Difference > 0)
                            {
                                comm.CommandType = CommandType.Text;
                                comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                                comm.Parameters.Clear();
                                comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                                comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                                comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                                comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = 1;
                                comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.Warehouse;
                                comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = 0;
                                comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Stock;
                                comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Item);
                                comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                                comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                                comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = 0;
                                comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = 0;
                                comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                                comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitCost;
                                comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Math.Abs(item.Difference);
                                comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = 0;
                                comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = Math.Abs(item.Difference) * item.UnitCost;
                                comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = 0;
                                comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = 0;
                                comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                                comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                                comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = 0;
                                comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Math.Abs(item.Difference) * item.UnitCost;
                                comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                                comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = 0;
                                comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                                comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                                comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = 0;
                                comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Comment ?? "";
                                comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                                comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                                comm.ExecuteNonQuery();
                            }
                        }

                        if (Header.IncreaseValue != 0)
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
                            comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.Date);
                            comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Expenses;
                            comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Account);
                            comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.ChartofAccount;
                            comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(accounts.PurchaseAccount);
                            comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                            comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                            comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Comment ?? "";
                            comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = true;
                            comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = Math.Abs(Header.IncreaseValue);
                            comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Math.Abs(Header.IncreaseValue);
                            comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = Math.Abs(Header.IncreaseValue);
                            comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = Math.Abs(Header.IncreaseValue);
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

                    if (Header.DeficiencyValue != 0)
                    {
                        //Header Data
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk2);
                        comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                        comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                        comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.CreateUser;
                        comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                        comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.Warehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = (int)DocumentKind.ConsumptionStock;
                        comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.Date);
                        comm.Parameters.Add("@inv_InvoiceDateTime", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.Date.Value.Date + Header.CreateDate.Value.TimeOfDay);
                        comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.Date); ;
                        comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null); ;
                        comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Expenses;
                        comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Account);
                        comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Comment ?? "";
                        comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Header.DeficiencyValue + Header.IncreaseValue;
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Header.DeficiencyValue + Header.IncreaseValue;
                        comm.Parameters.Add("@inv_PaymentDiscount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_RetentionLess", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Header.DeficiencyQty + Header.IncreaseQty;
                        comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Key);
                        comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                        comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                        comm.ExecuteNonQuery();

                        //Financial Header
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.FinDocument_Transaction;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk2);
                        comm.Parameters.Add("@Fin_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                        comm.Parameters.Add("@Fin_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                        comm.Parameters.Add("@Fin_LastupUser", SqlDbType.Int).Value = Header.CreateUser;
                        comm.Parameters.Add("@Fin_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                        comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                        comm.Parameters.Add("@Fin_Branch", SqlDbType.Int).Value = Header.Branch;
                        comm.Parameters.Add("@Fin_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                        comm.Parameters.Add("@Fin_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_DocumentKind", SqlDbType.Int).Value = (int)DocumentKind.ConsumptionStock;
                        comm.Parameters.Add("@Fin_VoucherNo", SqlDbType.Int).Value = VoucherNo;
                        comm.Parameters.Add("@Fin_VoucherDate", SqlDbType.Date).Value = Header.Date;
                        comm.Parameters.Add("@Fin_MonthlyNo", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.Date);
                        comm.Parameters.Add("@Fin_DueDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@Fin_AccountKind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Expenses;
                        comm.Parameters.Add("@Fin_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Account);
                        comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Comment ?? "";
                        comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                        comm.Parameters.Add("@Fin_Currency", SqlDbType.NVarChar, 3).Value = "";
                        comm.Parameters.Add("@Fin_Subtotal", SqlDbType.Decimal).Value = Header.DeficiencyValue + Header.IncreaseValue;
                        comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Header.DeficiencyValue + Header.IncreaseValue;
                        comm.Parameters.Add("@Fin_Rows", SqlDbType.Int).Value = Details.Count;
                        comm.Parameters.Add("@Fin_IcloudExp", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                        comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                        comm.Parameters.Add("@Fin_RecipientName", SqlDbType.NVarChar, 250).Value = "";
                        comm.ExecuteNonQuery();

                        foreach (Audit.StocktakingDetails item in Details)
                        {
                            if (item.Difference < 0)
                            {
                                comm.CommandType = CommandType.Text;
                                comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                                comm.Parameters.Clear();
                                comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk2);
                                comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                                comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                                comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = -1;
                                comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.Warehouse;
                                comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = 0;
                                comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Stock;
                                comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Item);
                                comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                                comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                                comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = 0;
                                comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = 0;
                                comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                                comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitCost;
                                comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Math.Abs(item.Difference);
                                comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = 0;
                                comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = Math.Abs(item.Difference) * item.UnitCost;
                                comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = 0;
                                comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = 0;
                                comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                                comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                                comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = 0;
                                comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Math.Abs(item.Difference) * item.UnitCost;
                                comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                                comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = 0;
                                comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                                comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                                comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = 0;
                                comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Comment ?? "";
                                comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                                comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                                comm.ExecuteNonQuery();
                            }
                        }

                        if (Header.DeficiencyValue != 0)
                        {
                            ++index;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = CLiCore.Const.Tables.FinDocument_TransactionDetails;
                            comm.Parameters.Clear();
                            comm.Parameters.Add("@Fin_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                            comm.Parameters.Add("@Fin_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk2);
                            comm.Parameters.Add("@Fin_Index", SqlDbType.Int).Value = index;
                            comm.Parameters.Add("@Fin_Status", SqlDbType.Int).Value = Header.Status;
                            comm.Parameters.Add("@Fin_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.Date);
                            comm.Parameters.Add("@Fin_TransactionNo", SqlDbType.NVarChar, 25).Value = "";
                            comm.Parameters.Add("@Fin_Account1Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Expenses;
                            comm.Parameters.Add("@Fin_Account1Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account1Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Account);
                            comm.Parameters.Add("@Fin_Account2Kind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.ChartofAccount;
                            comm.Parameters.Add("@Fin_Account2Type", SqlDbType.Int).Value = 0;
                            comm.Parameters.Add("@Fin_Account2Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(accounts.PurchaseAccount);
                            comm.Parameters.Add("@Fin_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                            comm.Parameters.Add("@Fin_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                            comm.Parameters.Add("@Fin_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_Description", SqlDbType.NVarChar, 500).Value = Comment ?? "";
                            comm.Parameters.Add("@Fin_DC", SqlDbType.Bit).Value = false;
                            comm.Parameters.Add("@Fin_Amount", SqlDbType.Decimal).Value = Math.Abs(Header.DeficiencyValue);
                            comm.Parameters.Add("@Fin_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                            comm.Parameters.Add("@Fin_vatRate", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_vatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_Total", SqlDbType.Decimal).Value = Math.Abs(Header.DeficiencyValue);
                            comm.Parameters.Add("@Fin_cxAmount", SqlDbType.Decimal).Value = Math.Abs(Header.DeficiencyValue);
                            comm.Parameters.Add("@Fin_cxvatAmount", SqlDbType.Decimal).Value = 0;
                            comm.Parameters.Add("@Fin_cxTotal", SqlDbType.Decimal).Value = Math.Abs(Header.DeficiencyValue);
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
                    str.Append("exec dbo.spAccOperation_PostTransaction @Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk2);
                    comm.ExecuteNonQuery();

                    str.Clear();
                    str.Append("exec dbo.spInvOperation_UpdateStockBalance @Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk2);
                    comm.ExecuteNonQuery();

                    str.Clear();
                    str.Append("update [InvOperation_Stocktaking] set [inv_Status]=1 where [inv_Key]=@Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Key);
                    comm.ExecuteNonQuery();

                    try
                    {
                        transaction.Commit();
                        VoucherNo = VoucherOperation.UpdateInvoiceNo(DB, (int)DocumentKind.RetConsumptionStock, opk);
                        res = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        res = false;
                    }
                }
                return res;

            }
        }
        public static bool UpdateOpeningBalance(string DB, Audit.Stocktaking Header, List<Audit.StocktakingDetails> Details, string Comment)
        {
            bool res = false;

            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;

                opk = Guid.NewGuid();
                VoucherNo = CLiCore.VoucherOperation.MaxTransaction(DB, (int)DocumentKind.invOpeningBalance, Header.Date.Value.Year);


                bool DC = xCore.HeaderDebitOrCredit((int)DocumentKind.invOpeningBalance);
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



                    //Header Data
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = CLiCore.Const.Tables.InvDocument_Transaction;
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@inv_Session", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@inv_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@inv_LastupUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@inv_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@inv_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@inv_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.Warehouse;
                    comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_DocumentKind", SqlDbType.Int).Value = (int)DocumentKind.invOpeningBalance;
                    comm.Parameters.Add("@inv_InvoiceKind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_InvoiceNo", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_InvoiceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.Date);
                    comm.Parameters.Add("@inv_InvoiceDateTime", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.Date.Value.Date + Header.CreateDate.Value.TimeOfDay);
                    comm.Parameters.Add("@inv_MonthlyNo", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_ReferenceNo", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@inv_ReferenceDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(Header.Date); ;
                    comm.Parameters.Add("@inv_DueDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null); ;
                    comm.Parameters.Add("@inv_AccountKind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_AccountKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@inv_CurrentKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@inv_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@inv_SalesHand", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Comment ?? "";
                    comm.Parameters.Add("@inv_Currency", SqlDbType.NVarChar, 25).Value = "";
                    comm.Parameters.Add("@inv_SubTotal", SqlDbType.Decimal).Value = Header.DeficiencyValue + Header.IncreaseValue;
                    comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@inv_BonusAmount", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Header.DeficiencyValue + Header.IncreaseValue;
                    comm.Parameters.Add("@inv_PaymentDiscount", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@inv_RetentionLess", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@inv_InvoiceCost", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Header.DeficiencyQty + Header.IncreaseQty;
                    comm.Parameters.Add("@inv_BonusQuantity", SqlDbType.Decimal).Value = 0;
                    comm.Parameters.Add("@inv_DeliveryKind", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_IncludeFxd", SqlDbType.Bit).Value = false;
                    comm.Parameters.Add("@inv_IncludeExp", SqlDbType.Bit).Value = false;
                    comm.Parameters.Add("@inv_ImportationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    comm.Parameters.Add("@inv_Returned", SqlDbType.Bit).Value = false;
                    comm.Parameters.Add("@inv_OriginalInvoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Key);
                    comm.Parameters.Add("@inv_Source", SqlDbType.Int).Value = 0;
                    comm.Parameters.Add("@inv_IsCredit", SqlDbType.Bit).Value = false;
                    comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                    comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                    comm.ExecuteNonQuery();



                    foreach (Audit.StocktakingDetails item in Details)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = CLiCore.Const.Tables.InvDocument_TransactionDetails;
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@inv_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@inv_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@inv_Index", SqlDbType.Int).Value = item.Index;
                        comm.Parameters.Add("@inv_IO", SqlDbType.Int).Value = item.Difference > 0 ? 1 : -1;
                        comm.Parameters.Add("@inv_SourceWarehouse", SqlDbType.Int).Value = Header.Warehouse;
                        comm.Parameters.Add("@inv_TargetWarehouse", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_ItemKind", SqlDbType.Int).Value = (int)PLenums.TransactionAccount.Stock;
                        comm.Parameters.Add("@inv_ItemKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Item);
                        comm.Parameters.Add("@inv_ProDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@inv_ExpDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@inv_Color", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_Size", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                        comm.Parameters.Add("@inv_UnitPrice", SqlDbType.Decimal).Value = item.UnitCost;
                        comm.Parameters.Add("@inv_Quantity", SqlDbType.Decimal).Value = Math.Abs(item.Difference);
                        comm.Parameters.Add("@inv_Bonus", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_Amount", SqlDbType.Decimal).Value = Math.Abs(item.Difference) * item.UnitCost;
                        comm.Parameters.Add("@inv_Discount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_vatAmount", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_UnitCost", SqlDbType.Decimal).Value = item.UnitCost;
                        comm.Parameters.Add("@inv_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@inv_vatRate", SqlDbType.Decimal).Value = 0;
                        comm.Parameters.Add("@inv_Total", SqlDbType.Decimal).Value = Math.Abs(item.Difference) * item.UnitCost;
                        comm.Parameters.Add("@inv_Batch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@inv_ConsumptionKind", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_Hidden", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Printable", SqlDbType.Bit).Value = false;
                        comm.Parameters.Add("@inv_Status", SqlDbType.Int).Value = 0;
                        comm.Parameters.Add("@inv_Description", SqlDbType.NVarChar, 500).Value = Comment ?? "";
                        comm.Parameters.Add("@inv_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.CostCenter);
                        comm.Parameters.Add("@inv_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Project);
                        comm.ExecuteNonQuery();

                    }




                    str.Clear();
                    str.Append("exec dbo.spInvOperation_UpdateStockBalance @Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.ExecuteNonQuery();

                    str.Clear();
                    str.Append("update [InvOperation_Stocktaking] set [inv_Status]=1 where [inv_Key]=@Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Key);
                    comm.ExecuteNonQuery();

                    try
                    {
                        transaction.Commit();
                        VoucherNo = VoucherOperation.UpdateInvoiceNo(DB, (int)DocumentKind.invOpeningBalance, opk);
                        res = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        res = false;
                    }
                }
                return res;

            }
        }
        public static decimal ItemUnitBalanceFullWarehouse(string DB, Guid? Key, string Unit, DateTime? Date)
        {
            if (Key == null)
                return 0;

            decimal result = 0;

            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select dbo.sfninvGet_ItemUnitBalanceFull(@Key,@Unit,@Date) ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                com.Parameters.Add("@Unit", SqlDbType.NVarChar, 25).Value = Unit;
                com.Parameters.Add("@Date", SqlDbType.DateTime).Value = Date;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = Convert.ToDecimal(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static decimal GetLastUnitCost(string DB, Guid? Key, string Unit, DateTime? Date)
        {
            if (Key == null)
                return 0;

            decimal result = 0;

            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select dbo.invGet_LastUnitCost(@Key,@Unit,@Date) ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                com.Parameters.Add("@Unit", SqlDbType.NVarChar, 25).Value = Unit;
                com.Parameters.Add("@Date", SqlDbType.DateTime).Value = Date;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = Convert.ToDecimal(reader[0]);

                };
                reader.Close();
            }
            return result;
        }
        public static int CheckInvoiceDate(string DB, Guid? Key, DateTime? Date)
        {
            if (Key == null)
                return 0;

            int result = 0;

            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "SELECT TOP 100 PERCENT COUNT(*) FROM InvDocument_TransactionUnitCost c WHERE convert(date,c.cost_Date)=@Date AND c.cost_ItemKey=@Key ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                com.Parameters.Add("@Date", SqlDbType.Date).Value = Date;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    result =30- Convert.ToInt32(reader[0]);


                };
                reader.Close();
            }
            return result;
        }

        public static Guid? GetServiceVat(string DB, Guid? Key)
        {
            Guid? result = null;
            if (Key == null)
                return result;

            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "SELECT TOP 100 PERCENT r.rev_vatKey FROM finCard_Revenue r WHERE r.rev_Key=@Key ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = iCore.IsDbNullRtNull(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
    }
    public class ItemsDetails
    {
        public Guid? ItemKey { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
    }
    public class InvoiceItems
    {
        public Guid? ItemKey { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}
