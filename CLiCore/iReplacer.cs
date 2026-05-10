using CLiCore.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CLiCore
{
    public class iReplacer
    {
        public static List<ObjectDisplay> DocumentsName(string xLan, bool Financial = true)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            string selQuery = "select top 100 percent * from [px_DocumentsName] where [sys_Financial]=@Kind order by [sys_No] ";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Kind", SqlDbType.Bit).Value = Financial;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ObjectDisplay item = new ObjectDisplay();
                    item.Value = Convert.ToInt32(reader["sys_No"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = Convert.ToString(reader["sys_EnglishName"]);
                            break;
                        case "ar":
                            item.Display = Convert.ToString(reader["sys_ArabicName"]);
                            break;
                        default:
                            item.Display = Convert.ToString(reader["sys_EnglishName"]);
                            break;
                    }
                    Result.Add(item);
                }
                reader.Close();
            }
            return Result;
        }
        public static List<ObjectDisplay> BoolTF(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "", "False") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "", "True") });
            return Result;
        }
        public static List<ObjectDisplay> accGeneralLedgerKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2040", "Opening") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2041", "Transaction") });
            return Result;
        }
        public static List<ObjectDisplay> accAccountKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2042", "Assets") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2043", "Liability") });
            Result.Add(new ObjectDisplay() { Value = 2, Display = T(xLan, "2044", "Equity") });
            Result.Add(new ObjectDisplay() { Value = 3, Display = T(xLan, "2045", "Revenue") });
            Result.Add(new ObjectDisplay() { Value = 4, Display = T(xLan, "2046", "Expenses") });
            return Result;
        }
        public static List<ObjectDisplay> accFinancial(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2047", "Closed") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2040", "Opening") });
            return Result;
        }
        public static List<ObjectDisplay> accLevelKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2041", "Transaction") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2048", "Parent") });
            return Result;
        }
        public static List<ObjectDisplay> accDCKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "", "Debit Kind") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "", "Credit Kind") });
            return Result;
        }
        public static List<ObjectDisplay> accParent(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2041", "Transaction") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2048", "Parent") });
            return Result;
        }
        public static List<ObjectDisplay> accTransaction(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2041", "Transaction") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2048", "Parent") });
            return Result;
        }
        public static List<ObjectDisplay> accExpKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = -1, Display = T(xLan, "", "None") });
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "", "General") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "", "Marketing") });
            Result.Add(new ObjectDisplay() { Value = 2, Display = T(xLan, "", "Operation") });
            return Result;
        }
        public static List<ObjectDisplay> accCategory(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2049", "General Account") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2050", "Current Account") });
            Result.Add(new ObjectDisplay() { Value = 2, Display = T(xLan, "2051", "Cashbox category") });
            Result.Add(new ObjectDisplay() { Value = 3, Display = T(xLan, "2052", "Bank category") });
            Result.Add(new ObjectDisplay() { Value = 4, Display = T(xLan, "2053", "Revenue category") });
            Result.Add(new ObjectDisplay() { Value = 5, Display = T(xLan, "2054", "Expenses category") });
            return Result;
        }
        public static List<ObjectDisplay> accBalanceItemKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2048", "Parent") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2041", "Transaction") });
            return Result;
        }
        public static List<ObjectDisplay> finCostCenterKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2055", "General") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2056", "Sales") });
            Result.Add(new ObjectDisplay() { Value = 2, Display = T(xLan, "2057", "Operation") });
            return Result;
        }
        public static List<ObjectDisplay> finAccountsFull(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CurrentAccount, Display = T(xLan, "2050", "Current Account") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Employee, Display = T(xLan, "2058", "Employee") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CashBox, Display = T(xLan, "2059", "Cash Box") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Bank, Display = T(xLan, "2060", "Bank") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Fixture, Display = T(xLan, "2061", "Fixed Assets") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Revenue, Display = T(xLan, "2045", "Revenue") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Expenses, Display = T(xLan, "2046", "Expenses") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.ChartofAccount, Display = T(xLan, "2062", "Chart of Account") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Stock, Display = T(xLan, "2063", "Stock") });
            return Result;
        }
        public static List<ObjectDisplay> finAccounts(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CurrentAccount, Display = T(xLan, "2050", "Current Account") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Employee, Display = T(xLan, "2058", "Employee") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CashBox, Display = T(xLan, "2059", "Cash Box") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Bank, Display = T(xLan, "2060", "Bank") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Fixture, Display = T(xLan, "2061", "Fixed Assets") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Revenue, Display = T(xLan, "2045", "Revenue") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Expenses, Display = T(xLan, "2046", "Expenses") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.ChartofAccount, Display = T(xLan, "2062", "Chart of Account") });
            return Result;
        }
        public static List<ObjectDisplay> finPaymentsAccounts(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CurrentAccount, Display = T(xLan, "2050", "Current Account") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Employee, Display = T(xLan, "2058", "Employee") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CashBox, Display = T(xLan, "2059", "Cash Box") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Bank, Display = T(xLan, "2060", "Bank") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Fixture, Display = T(xLan, "2061", "Fixed Assets") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Expenses, Display = T(xLan, "2046", "Expenses") });
            return Result;
        }
        public static List<ObjectDisplay> finCollectionsAccounts(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CurrentAccount, Display = T(xLan, "2050", "Current Account") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Employee, Display = T(xLan, "2058", "Employee") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CashBox, Display = T(xLan, "2059", "Cash Box") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Bank, Display = T(xLan, "2060", "Bank") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.ChartofAccount, Display = T(xLan, "2062", "Chart of Account") });
            return Result;
        }
        public static List<ObjectDisplay> finTransactionAccByDocument(string xLan, int Doc)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            switch (Doc)
            {
                case (int)DocumentKind.finCashCollection:
                case (int)DocumentKind.finBankCollection:
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CurrentAccount, Display = T(xLan, "2050", "Current Account") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Employee, Display = T(xLan, "2058", "Employee") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CashBox, Display = T(xLan, "2059", "Cash Box") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Bank, Display = T(xLan, "2060", "Bank") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Expenses, Display = T(xLan, "2046", "Expenses") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.ChartofAccount, Display = T(xLan, "2062", "Chart of Account") });
                    break;
                case (int)DocumentKind.finCashPayment:
                case (int)DocumentKind.finBankPayment:
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CurrentAccount, Display = T(xLan, "2050", "Current Account") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Employee, Display = T(xLan, "2058", "Employee") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CashBox, Display = T(xLan, "2059", "Cash Box") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Bank, Display = T(xLan, "2060", "Bank") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Fixture, Display = T(xLan, "2061", "Fixed Assets") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Expenses, Display = T(xLan, "2046", "Expenses") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.ChartofAccount, Display = T(xLan, "2062", "Chart of Account") });
                    break;
                case (int)DocumentKind.finDebitNote:
                case (int)DocumentKind.finCreditNote:
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CurrentAccount, Display = T(xLan, "2050", "Current Account") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Employee, Display = T(xLan, "2058", "Employee") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Revenue, Display = T(xLan, "2045", "Revenue") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Expenses, Display = T(xLan, "2046", "Expenses") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.ChartofAccount, Display = T(xLan, "2062", "Chart of Account") });
                    break;
                case (int)DocumentKind.PurchaseInvoice:
                case (int)DocumentKind.SalesInvoice:
                case (int)DocumentKind.ReturnPurchase:
                case (int)DocumentKind.ReturnSalesInvoice:
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CurrentAccount, Display = T(xLan, "2152", "Credit") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CashBox, Display = T(xLan, "2065", "Cash") });
                    Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Bank, Display = T(xLan, "2060", "Bank") });
                    break;
            }
            return Result;
        }
        public static List<ObjectDisplay> finMethodsAccounts(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CurrentAccount, Display = T(xLan, "2050", "Current Account") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.CashBox, Display = T(xLan, "2059", "Cash Box") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Bank, Display = T(xLan, "2060", "Bank") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Expenses, Display = T(xLan, "2046", "Expenses") });
            return Result;
        }
        public static List<ObjectDisplay> finCurrentAccountKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2055", "General") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2066", "Supplier") });
            Result.Add(new ObjectDisplay() { Value = 2, Display = T(xLan, "2067", "Client") });
            Result.Add(new ObjectDisplay() { Value = 3, Display = T(xLan, "2058", "Employee") });
            Result.Add(new ObjectDisplay() { Value = 4, Display = T(xLan, "2068", "Owner") });
            Result.Add(new ObjectDisplay() { Value = 5, Display = T(xLan, "2069", "Sister Company") });
            Result.Add(new ObjectDisplay() { Value = 6, Display = T(xLan, "2070", "Adjustment") });
            return Result;
        }
        public static List<ObjectDisplay> finCashKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2059", "Cash Box") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2071", "Cash Hand") });
            return Result;
        }
        public static List<ObjectDisplay> finEmployeeAcccountTypes(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2072", "Salary") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2073", "Advance") });
            Result.Add(new ObjectDisplay() { Value = 2, Display = T(xLan, "2074", "Loan") });
            Result.Add(new ObjectDisplay() { Value = 3, Display = T(xLan, "2075", "End Service") });
            Result.Add(new ObjectDisplay() { Value = 4, Display = T(xLan, "2076", "Vacation") });
            return Result;
        }
        public static List<ObjectDisplay> invTransactionKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 77, Display = T(xLan, "", "Consumption") });
            Result.Add(new ObjectDisplay() { Value = 78, Display = T(xLan, "", "Retrun Consumption") });
            Result.Add(new ObjectDisplay() { Value = 76, Display = T(xLan, "", "Transfer Out") });
            Result.Add(new ObjectDisplay() { Value = 79, Display = T(xLan, "", "Income Transfer") });
 
            return Result;
        }
        public static List<ObjectDisplay> finFixtureAcccountTypes(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2042", "Assets") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2077", "Amortization") });
            return Result;
        }
        public static List<ObjectDisplay> FinGeneralAccountTypes(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2055", "General") });
            return Result;
        }
        public static List<ObjectDisplay> CardStatus(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2078", "Active") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2079", "Disable") });
            return Result;
        }
        public static List<ObjectDisplay> fxdIntegrationMethod(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "", "Grouping") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "", "Details") });
            return Result;
        }
        public static List<ObjectDisplay> finExpDistribution(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2079", "Disable") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2080", "Distribution") });
            return Result;
        }
        public static List<ObjectDisplay> finItemKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Revenue, Display = T(xLan, "2045", "Revenue") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Stock, Display = T(xLan, "2110", "Stock Item") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Expenses, Display = T(xLan, "2046", "Expenses") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Fixture, Display = T(xLan, "2061", "Fixed Assets") });
            return Result;
        }
        public static List<ObjectDisplay> finIncomeKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 6, Display = T(xLan, "2045", "Revenue") });
            Result.Add(new ObjectDisplay() { Value = 5, Display = T(xLan, "2046", "Expenses") });
            return Result;
        }
        public static List<ObjectDisplay> finExpControlled(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2079", "Disable") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2081", "Controlled") });
            return Result;
        }
        public static List<ObjectDisplay> finCurrencyRate(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2082", "To Main Currency") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2083", "From Main Currency") });
            return Result;
        }
        public static List<ObjectDisplay> FinancialYearStatus(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2040", "Opening") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2047", "Closed") });
            return Result;
        }
        public static List<ObjectDisplay> fxdDetailsMethod(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2084", "Grouping") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2085", "Details") });
            return Result;
        }
        public static List<ObjectDisplay> fxdKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2055", "General") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2086", "bulding") });
            Result.Add(new ObjectDisplay() { Value = 2, Display = T(xLan, "2087", "Land") });
            Result.Add(new ObjectDisplay() { Value = 3, Display = T(xLan, "2088", "Vehicles") });
            Result.Add(new ObjectDisplay() { Value = 4, Display = T(xLan, "2089", "Furniture") });
            Result.Add(new ObjectDisplay() { Value = 5, Display = T(xLan, "2090", "Computer") });
            Result.Add(new ObjectDisplay() { Value = 6, Display = T(xLan, "2091", "Software") });
            Result.Add(new ObjectDisplay() { Value = 7, Display = T(xLan, "2092", "Tools") });
            return Result;
        }
        public static List<ObjectDisplay> fxdTangible(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2093", "Tangible") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2094", "Intangible") });
            return Result;
        }
        public static List<ObjectDisplay> fxdBatchEnable(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2079", "Disable") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2095", "Enable") });
            return Result;
        }
        public static List<ObjectDisplay> invWarehouseKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2055", "General") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2096", "Booking") });
            Result.Add(new ObjectDisplay() { Value = 2, Display = T(xLan, "2097", "Production") });
            return Result;
        }
        public static List<ObjectDisplay> invUnitRate(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2098", "To Main") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2099", "From Main") });
            return Result;
        }
        public static List<ObjectDisplay> finPaymentKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2055", "General") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2100", "Close Invoice") });
            Result.Add(new ObjectDisplay() { Value = 2, Display = T(xLan, "2101", "Advance Payment") });
            return Result;
        }
        public static List<ObjectDisplay> finAudit(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2102", "Pending") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2103", "Seen") });
            Result.Add(new ObjectDisplay() { Value = 2, Display = T(xLan, "2047", "Closed") });
            Result.Add(new ObjectDisplay() { Value = 3, Display = T(xLan, "2104", "Reaudit") });
            return Result;
        }
        public static List<ObjectDisplay> SalesQuotationStatus(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2102", "Pending") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "", "Confirmed +DP") });
            Result.Add(new ObjectDisplay() { Value = 2, Display = T(xLan, "2105", "Confirmed -DP") });
            Result.Add(new ObjectDisplay() { Value = 3, Display = T(xLan, "2104", "Reaudit") });
            return Result;
        }
        public static List<ObjectDisplay> finPosted(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2102", "Pending") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2106", "Posted") });
            return Result;
        }
        public static List<ObjectDisplay> finVATKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2056", "Sales") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2107", "Purchase") });
            return Result;
        }
        public static List<ObjectDisplay> finISvat(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2108", "Out of VAT Report") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2109", "Incloud VAT Report") });
            return Result;
        }
        public static List<ObjectDisplay> invPurchaseItems(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Stock, Display = T(xLan, "2110", "Stock Item") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Expenses, Display = T(xLan, "2046", "Expenses") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Fixture, Display = T(xLan, "2061", "Fixed Assets") });
            return Result;
        }
        public static List<ObjectDisplay> invSalesItems(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Stock, Display = T(xLan, "2110", "Stock Item") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Revenue, Display = T(xLan, "2045", "Revenue") });
            Result.Add(new ObjectDisplay() { Value = (int)PLenums.TransactionAccount.Fixture, Display = T(xLan, "2061", "Fixed Assets") });
            return Result;
        }
        public static List<ObjectDisplay> invImportationInvoice(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2111", "Normal") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2112", "Importation") });
            return Result;
        }
        public static List<ObjectDisplay> invReturnedInvoice(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2113", "Invoice") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2114", "Returned") });
            return Result;
        }
        public static List<ObjectDisplay> salProformaInvoiced(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "", "Waiting") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "", "Invoiced") });
            return Result;
        }
        public static List<ObjectDisplay> UserKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2115", "Web") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2116", "Application") });
            return Result;
        }
        public static List<ObjectDisplay> WebAppSource(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2115", "Web") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2116", "Application") });
            return Result;
        }
        public static List<ObjectDisplay> invStcoktakingStatus(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2102", "Pending") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2047", "Closed") });
            return Result;
        }
        //HR
        public static List<ObjectDisplay> hrSelfServiceStatus(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "", "Active") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "", "Disable") });
            return Result;
        }
        public static List<ObjectDisplay> hrGender(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2117", "Male") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2118", "Female") });
            return Result;
        }
        public static List<ObjectDisplay> hrCitizen(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2119", "citizen") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2120", "Resident") });
            return Result;
        }
        public static List<ObjectDisplay> hrMaritalStatus(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2121", "Single") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2122", "Married") });
            return Result;
        }
        public static List<ObjectDisplay> hrInsurance(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2123", "No Insurance") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2124", "Insurance") });
            return Result;
        }
        public static List<ObjectDisplay> hrPaymentType(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2125", "WPS") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2060", "Bank") });
            Result.Add(new ObjectDisplay() { Value = 2, Display = T(xLan, "2065", "Cash") });
            Result.Add(new ObjectDisplay() { Value = 3, Display = T(xLan, "2126", "Other Side") });
            Result.Add(new ObjectDisplay() { Value = 4, Display = T(xLan, "2127", "Other") });
            return Result;
        }
        public static List<ObjectDisplay> hrInsuranceKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "2128", "Percent") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "2129", "Amount") });
            return Result;
        }
        public static List<ObjectDisplay> hrTicketKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2130", "No Ticket") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2131", "One Way") });
            Result.Add(new ObjectDisplay() { Value = 2, Display = T(xLan, "2132", "Tow Way") });
            return Result;
        }
        public static List<ObjectDisplay> hrJobKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2133", "Officer") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2134", "Manager") });
            Result.Add(new ObjectDisplay() { Value = 2, Display = T(xLan, "2135", "Sales Person") });
            Result.Add(new ObjectDisplay() { Value = 3, Display = T(xLan, "2136", "Purchaser") });
            Result.Add(new ObjectDisplay() { Value = 4, Display = T(xLan, "2127", "Other") });
            return Result;
        }
        public static List<ObjectDisplay> MonthList(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 1, Display = " 1- Jan" });
            Result.Add(new ObjectDisplay() { Value = 2, Display = " 2- Feb" });
            Result.Add(new ObjectDisplay() { Value = 3, Display = " 3- Mar" });
            Result.Add(new ObjectDisplay() { Value = 4, Display = " 4- Apr" });
            Result.Add(new ObjectDisplay() { Value = 5, Display = " 5- May" });
            Result.Add(new ObjectDisplay() { Value = 6, Display = " 6- Jun" });
            Result.Add(new ObjectDisplay() { Value = 7, Display = " 7- Jul" });
            Result.Add(new ObjectDisplay() { Value = 8, Display = " 8- Aug" });
            Result.Add(new ObjectDisplay() { Value = 9, Display = " 9- Sep" });
            Result.Add(new ObjectDisplay() { Value = 10, Display = "10- Oct" });
            Result.Add(new ObjectDisplay() { Value = 11, Display = "11- Nov" });
            Result.Add(new ObjectDisplay() { Value = 12, Display = "12- Dec" });
            return Result;
        }
        public static List<ObjectDisplay> TrackVehiclesStatus(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "2102", "Pending") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "2137", "Handled") });
            Result.Add(new ObjectDisplay() { Value = 2, Display = T(xLan, "2138", "Receipted") });
            Result.Add(new ObjectDisplay() { Value = 3, Display = T(xLan, "2139", "Installed") });
            return Result;
        }
        public static List<ObjectDisplay> InvStockPkg(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "", "Normal") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "", "Package") });
            return Result;
        }
        public static List<ObjectDisplay> crmKind(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "", "Visit") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "", "Calling") });
            return Result;
        }
        public static List<ObjectDisplay> crmCompetitor(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "", "No") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "", "Yes") });
            return Result;
        }
        public static List<ObjectDisplay> crmcompetitorStatus(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "", "Satisfied") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "", "UnSatisfied") });
            return Result;
        }
        public static List<ObjectDisplay> crmQuotationed(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "", "No") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "", "Yes") });
            return Result;
        }
        public static List<ObjectDisplay> crmExpect(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "", "High") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "", "Medium") });
            Result.Add(new ObjectDisplay() { Value = 2, Display = T(xLan, "", "Low") });
            Result.Add(new ObjectDisplay() { Value = 3, Display = T(xLan, "", "Not Interested") });
            return Result;
        }
        public static List<ObjectDisplay> crmContracted(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = false, Display = T(xLan, "", "No") });
            Result.Add(new ObjectDisplay() { Value = true, Display = T(xLan, "", "Yes") });
            return Result;
        }
        public static List<ObjectDisplay> hrRequestStatus(string xLan)
        {
            List<ObjectDisplay> Result = new List<ObjectDisplay>();
            Result.Add(new ObjectDisplay() { Value = 100, Display = T(xLan, "", "All Status") });
            Result.Add(new ObjectDisplay() { Value = 0, Display = T(xLan, "", "Pending") });
            Result.Add(new ObjectDisplay() { Value = 1, Display = T(xLan, "", "Confirmed") });
            Result.Add(new ObjectDisplay() { Value = -1, Display = T(xLan, "", "Rejected") });

            return Result;
        }
        private static string T(string Language, string Key, string Default = "No Label")
        {
            string? result = "No Label";
            if (Key == "")
                return Default;

            try
            {
                if (Language == null || Language == "" || Language == "en")
                {
                    if (iCore.iLL.Count(x => x.Key == Key) > 0)
                    {
                        result = iCore.iLL.Where(x => x.Key == Key).FirstOrDefault().English;
                    }
                }
                else
                {
                    if (iCore.iLL.Count(x => x.Key == Key) > 0)
                    {
                        result = iCore.iLL.Where(x => x.Key == Key).FirstOrDefault().Arabic;
                    }

                }
                return result ?? "No Label";
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
    public class ObjectDisplay
    {
        public object? Value { get; set; }
        public string? Display { get; set; }
    }
}