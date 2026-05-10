using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Selections
{
    public class Dynamic
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }
        public string xName { get; set; }

        public decimal Cost { get; set; }
        public static List<Dynamic> TransactionAccount(string DB, string xLan, PLenums.TransactionAccount TA, PLenums.CurrentAccountKind CurrentKind = PLenums.CurrentAccountKind.All, Guid? Category = null,Guid? UserKey=null)
        {
            List<Dynamic> items = new List<Dynamic>();
            string selectstr = "";
            Guid? UserPrefix = null;
            if (UserKey != null)
            {
                if (xConfig.UserPrefixFilter(UserKey) == true)
                {
                    UserPrefix = xConfig.UserPrefix(UserKey);
                }
            }
        
            switch (TA)
            {
                case PLenums.TransactionAccount.CurrentAccount:

                    selectstr = "select top 100 percent * from dbo.fnfinSelections_CurrentAccount(@Kind,@Category) where ([Prefix]=@Prefix or @Prefix is null) order by [Code] ";
                    break;
                case PLenums.TransactionAccount.Employee:
                    selectstr = "select top 100 percent [emp_Key] as [Key],[emp_Code] as [Code],[emp_Name1] as [Name1],[emp_Name2] as [Name2] from [hrCard_Employee] order by [emp_Code]";
                    break;
                case PLenums.TransactionAccount.CashBox:
                    selectstr = "select top 100 percent [cash_Key] as [Key],[cash_Code] as [Code],[cash_Name1] as [Name1],[cash_Name2] as [Name2] from [finCard_CashBox] where [cash_Disable]=0 order by [cash_Code] ";
                    break;
                case PLenums.TransactionAccount.Bank:
                    selectstr = "select top 100 percent [bank_Key] as [Key],[bank_Code] as [Code],[bank_Name1] as [Name1],[bank_Name2] as [Name2] from [finCard_Bank] where [bank_Disable]=0 order by [bank_Code] ";
                    break;
                case PLenums.TransactionAccount.Revenue:
                    selectstr = "select top 100 percent [rev_Key] as [Key],[rev_Code] as [Code],[rev_Name1] as [Name1],[rev_Name2] as [Name2] from [finCard_Revenue] where [rev_Disable]=0 order by [rev_Code] ";
                    break;
                case PLenums.TransactionAccount.Expenses:
                    selectstr = "select top 100 percent [exp_Key] as [Key],[exp_Code] as [Code],[exp_Name1] as [Name1],[exp_Name2] as [Name2] from [finCard_Expenses] where [exp_Disable]=0 order by [exp_Code] ";
                    break;
                case PLenums.TransactionAccount.Fixture:
                    selectstr = "select top 100 percent [fxd_Key] as [Key],[fxd_Code] as [Code],[fxd_Name1] as [Name1],[fxd_Name2] as [Name2] from [finFixedAssets_Fixture] where [fxd_Disable]=0 order by [fxd_Code] ";
                    break;
                case PLenums.TransactionAccount.ChartofAccount:
                    selectstr = "select top 100 percent * from dbo.fnaccSelections_AccountsFinancial() order by [Code] ";
                    break;
                case PLenums.TransactionAccount.Stock:
                    selectstr = "select top 100 percent [item_Key] as [Key],[item_Code] as [Code],[item_Name1] as [Name1],[item_Name2] as [Name2],isnull(cost.[cost_Cost],stock.item_Cost) as [Cost] from [invCard_StockItem] stock " +
                        " left join [InvStock_UnitCost] as cost on stock.item_Key=cost.cost_Item where [item_Disable]=0 order by [item_Code] ";
                    break;
            }

            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selectstr;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Kind", SqlDbType.Int).Value = CurrentKind;
                com.Parameters.Add("@Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Category);
                com.Parameters.Add("@Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(UserPrefix);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Dynamic item = new Dynamic();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    switch (TA)
                    {
                        case PLenums.TransactionAccount.Stock:
                            item.Cost = Convert.ToDecimal(reader["Cost"]);
                            break;
                    }
                    switch (xLan)
                    {
                        case "en":
                            item.xName = (item.Name2 == "" ? item.Name1 : item.Name2);
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.Code + ")";
                            break;
                        case "ar":
                            item.xName = (item.Name1 == "" ? item.Name2 : item.Name1);
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.Code + ")";
                            break;
                        default:
                            item.xName = (item.Name1 == "" ? item.Name2 : item.Name1);
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.Code + ")";
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }

            return items;
        }
        public static List<Dynamic> TransactionAccount(string DB, string xLan, int TA, int CurrentKind = 0, Guid? Category = null, Guid? UserKey = null)
        {
            List<Dynamic> items = new List<Dynamic>();
            string selectstr = "";
            Guid? UserPrefix = null;
            if (UserKey != null)
            {
                if (xConfig.UserPrefixFilter(UserKey) == true)
                {
                    UserPrefix = xConfig.UserPrefix(UserKey);
                }
            }
            switch (TA)
            {
                case (int)PLenums.TransactionAccount.CurrentAccount:
                    selectstr = "select top 100 percent * from dbo.fnfinSelections_CurrentAccount(@Kind,@Category)  where ([Prefix]=@Prefix or @Prefix is null) order by [Code] ";
                    break;
                case (int)PLenums.TransactionAccount.Employee:
                    selectstr = "select top 100 percent [emp_Key] as [Key],[emp_Code] as [Code],[emp_Name1] as [Name1],[emp_Name2] as [Name2] from [hrCard_Employee] order by [emp_Code]";
                    break;
                case (int)PLenums.TransactionAccount.CashBox:
                    selectstr = "select top 100 percent [cash_Key] as [Key],[cash_Code] as [Code],[cash_Name1] as [Name1],[cash_Name2] as [Name2] from [finCard_CashBox] where [cash_Disable]=0 order by [cash_Code] ";
                    break;
                case (int)PLenums.TransactionAccount.Bank:
                    selectstr = "select top 100 percent [bank_Key] as [Key],[bank_Code] as [Code],[bank_Name1] as [Name1],[bank_Name2] as [Name2] from [finCard_Bank] where [bank_Disable]=0 order by [bank_Code] ";
                    break;
                case (int)PLenums.TransactionAccount.Revenue:
                    selectstr = "select top 100 percent [rev_Key] as [Key],[rev_Code] as [Code],[rev_Name1] as [Name1],[rev_Name2] as [Name2] from [finCard_Revenue] where [rev_Disable]=0 order by [rev_Code] ";
                    break;
                case (int)PLenums.TransactionAccount.Expenses:
                    selectstr = "select top 100 percent [exp_Key] as [Key],[exp_Code] as [Code],[exp_Name1] as [Name1],[exp_Name2] as [Name2] from [finCard_Expenses] where [exp_Disable]=0 order by [exp_Code] ";
                    break;
                case (int)PLenums.TransactionAccount.Fixture:
                    selectstr = "select top 100 percent [fxd_Key] as [Key],[fxd_Code] as [Code],[fxd_Name1] as [Name1],[fxd_Name2] as [Name2] from [finFixedAssets_Fixture] where [fxd_Disable]=0 order by [fxd_Code] ";
                    break;
                case (int)PLenums.TransactionAccount.ChartofAccount:
                    selectstr = "select top 100 percent * from dbo.fnaccSelections_AccountsFinancial() order by [Code] ";
                    break;
                case (int)PLenums.TransactionAccount.Stock:
                    selectstr = "select top 100 percent [item_Key] as [Key],[item_Code] as [Code],[item_Name1] as [Name1],[item_Name2] as [Name2],isnull(cost.[cost_Cost],stock.item_Cost) as [Cost] from [invCard_StockItem] stock " +
                        " left join [InvStock_UnitCost] as cost on stock.item_Key=cost.cost_Item where [item_Disable]=0 order by [item_Code] ";
                    break;
            }

            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selectstr;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Kind", SqlDbType.Int).Value = CurrentKind;
                com.Parameters.Add("@Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Category);
                com.Parameters.Add("@Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(UserPrefix);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Dynamic item = new Dynamic();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.xName = item.Name2 == "" ? item.Name1 : item.Name2;
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.Code + ")";
                            break;
                        case "ar":
                            item.xName = (item.Name1 == "" ? item.Name2 : item.Name1);
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.Code + ")";
                            break;
                        default:
                            item.xName = (item.Name1 == "" ? item.Name2 : item.Name1);
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.Code + ")";
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }

            return items;
        }
        public static List<Dynamic> UnionSelection(string DB, string xLan, PLenums.TransactionAccount[] TA, Guid? UserKey = null)
        {
            try
            {
                List<Dynamic> items = new List<Dynamic>();
                string selectstr = "";

                Guid? UserPrefix = null;
                if (UserKey != null)
                {
                    if (xConfig.UserPrefixFilter(UserKey) == true)
                    {
                        UserPrefix = xConfig.UserPrefix(UserKey);
                    }
                }

                if (TA.Contains(PLenums.TransactionAccount.CurrentAccount))
                    selectstr += "union all select top 100 percent [Key],[Code],[Name1],[Name2] from dbo.fnfinSelections_CurrentAccount(@Kind,@Category) where ([Prefix]=@Prefix or @Prefix is null)  ";

                if (TA.Contains(PLenums.TransactionAccount.Employee))
                    selectstr += "union all  select top 100 percent [emp_Key] as [Key],[emp_Code] as [Code],[emp_Name1] as [Name1],[emp_Name2] as [Name2] from [hrCard_Employee]   ";

                if (TA.Contains(PLenums.TransactionAccount.CashBox))
                    selectstr += "union all  select top 100 percent [cash_Key] as [Key],[cash_Code] as [Code],[cash_Name1] as [Name1],[cash_Name2] as [Name2] from [finCard_CashBox] where [cash_Disable]=0    ";

                if (TA.Contains(PLenums.TransactionAccount.Bank))
                    selectstr += "union all  select top 100 percent [bank_Key] as [Key],[bank_Code] as [Code],[bank_Name1] as [Name1],[bank_Name2] as [Name2] from [finCard_Bank] where [bank_Disable]=0     ";

                if (TA.Contains(PLenums.TransactionAccount.Revenue))
                    selectstr += "union all  select top 100 percent [rev_Key] as [Key],[rev_Code] as [Code],[rev_Name1] as [Name1],[rev_Name2] as [Name2] from [finCard_Revenue] where [rev_Disable]=0  ";

                if (TA.Contains(PLenums.TransactionAccount.Expenses))
                    selectstr += "union all select top 100 percent [exp_Key] as [Key],[exp_Code] as [Code],[exp_Name1] as [Name1],[exp_Name2] as [Name2] from [finCard_Expenses] where [exp_Disable]=0  ";

                if (TA.Contains(PLenums.TransactionAccount.Fixture))
                    selectstr += "union all select top 100 percent [fxd_Key] as [Key],[fxd_Code] as [Code],[fxd_Name1] as [Name1],[fxd_Name2] as [Name2] from [finFixedAssets_Fixture] where [fxd_Disable]=0     ";

                if (TA.Contains(PLenums.TransactionAccount.ChartofAccount))
                    selectstr += "union all select top 100 percent * from dbo.fnaccSelections_AccountsFinancial()  ";

                if (TA.Contains(PLenums.TransactionAccount.Stock))
                    selectstr += "union all select top 100 percent [item_Key] as [Key],[item_Code] as [Code],[item_Name1] as [Name1],[item_Name2] as [Name2] from [invCard_StockItem] where [item_Disable]=0   ";


                selectstr = selectstr.Remove(0, 9);
                using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand();
                    com.CommandText = selectstr;
                    com.CommandType = CommandType.Text;
                    com.Connection = con;
                    com.Parameters.Add("@Kind", SqlDbType.Int).Value = -1;
                    com.Parameters.Add("@Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                    com.Parameters.Add("@Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(UserPrefix);
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Dynamic item = new Dynamic();
                        item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                        item.Code = Convert.ToString(reader["Code"]);
                        item.Name1 = Convert.ToString(reader["Name1"]);
                        item.Name2 = Convert.ToString(reader["Name2"]);
                        switch (xLan)
                        {
                            case "en":
                                item.xName = (item.Name2 == "" ? item.Name1 : item.Name2);
                                item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.Code + ")";
                                break;
                            case "ar":
                                item.xName = (item.Name1 == "" ? item.Name2 : item.Name1);
                                item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.Code + ")";
                                break;
                            default:
                                item.xName = (item.Name1 == "" ? item.Name2 : item.Name1);
                                item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.Code + ")";
                                break;
                        }
                        items.Add(item);
                    }
                    reader.Close();
                }
                return items.OrderBy(x => x.Code).ToList();
            }
            catch(Exception ex)
            {

                return null;
            }
         
         
        }


        public static List<Dynamic> DocumentKind(string DB, string xLan,int documentKind)
        {
            List<Dynamic> items = new List<Dynamic>();
            string selectstr = "";

            switch (documentKind)
            {
                case (int)CLiCore.DocumentKind.finDebitNote:
                case (int)CLiCore.DocumentKind.finCreditNote:
                    selectstr = "select top 100 percent * from dbo.fnfinSelections_CurrentAccount(@Kind,@Category) order by [Code] ";
                    break;
                //case DocumentKind.Employee:
                //    selectstr = "";//"select top 100 percent [cash_Key] as [Key],[cash_Code] as [Code],[cash_Name1] as [Name1],[cash_Name1] as [Name2] from [finCard_CashBox] where [cash_Disable]=0";
                //    break;
                case (int)CLiCore.DocumentKind.finCashCollection:
                case (int)CLiCore.DocumentKind.finCashPayment:
                    selectstr = "select top 100 percent [cash_Key] as [Key],[cash_Code] as [Code],[cash_Name1] as [Name1],[cash_Name2] as [Name2] from [finCard_CashBox] where [cash_Disable]=0 order by [cash_Code] ";
                    break;
                case (int)CLiCore.DocumentKind.finBankCollection:
                case (int)CLiCore.DocumentKind.finBankPayment:
                    selectstr = "select top 100 percent [bank_Key] as [Key],[bank_Code] as [Code],[bank_Name1] as [Name1],[bank_Name2] as [Name2] from [finCard_Bank] where [bank_Disable]=0 order by [bank_Code] ";
                    break;
                    //case DocumentKind.Revenue:
                    //    selectstr = "select top 100 percent [rev_Key] as [Key],[rev_Code] as [Code],[rev_Name1] as [Name1],[rev_Name2] as [Name2] from [finCard_Revenue] where [rev_Disable]=0 order by [Code] ";
                    //    break;
                    //case DocumentKind.Expenses:
                    //    selectstr = "select top 100 percent [exp_Key] as [Key],[exp_Code] as [Code],[exp_Name1] as [Name1],[exp_Name2] as [Name2] from [finCard_Expenses] where [exp_Disable]=0 order by [Code] ";
                    //    break;
                    //case DocumentKind.Fixture:
                    //    selectstr = "select top 100 percent [bank_Key] as [Key],[bank_Code] as [Code],[bank_Name1] as [Name1],[bank_Name2] as [Name2] from [finFixedAssets_Fixture] where [bank_Disable]=0 order by [Code] ";
                    //    break;
                    //case DocumentKind.ChartofAccount:
                    //    selectstr = "select top 100 percent * from dbo.fnaccSelections_AccountsFinancial() order by [Code] ";
                    //    break;
                    //case DocumentKind.Stock:
                    //    selectstr = "select top 100 percent [bank_Key] as [Key],[bank_Code] as [Code],[bank_Name1] as [Name1],[bank_Name2] as [Name2] from [finCard_Bank] where [bank_Disable]=0 order by [Code] ";
                    //    break;
            }

            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selectstr;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Kind", SqlDbType.Int).Value = -1;
                com.Parameters.Add("@Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Dynamic item = new Dynamic();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.xName = item.Name2 == "" ? item.Name1 : item.Name2;
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.Code + ")";
                            break;
                        case "ar":
                            item.xName = item.Name1 == "" ? item.Name2 : item.Name1;
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.Code + ")";
                            break;
                        default:
                            item.xName = item.Name1 == "" ? item.Name2 : item.Name1;
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.Code + ")";
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }

            return items;
        }
    }
}
