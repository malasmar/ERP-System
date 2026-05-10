using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace CLiCore
{
    public class iCore
    {
        public static Guid vatDefault = Guid.Parse("D71AD938-5F99-48C4-94E8-9B6E5A38DD44"); 
        public static string Conn;
        public static string Server;
        public static string Username;
        public static string Password;
        public static string GetCon(string DB)
        {
            return "Data Source=" + Server + ";Initial Catalog=" + DB + ";User ID=" + Username + ";Password=" + Password + ";Connection Timeout=500000";
        }
        public static List<PLxLanguage> iLL;
        #region "Check Values"
        public static object IsNullRtDbNull(object value)
        {
            if (value == null) { return DBNull.Value; } else { return value; }
        }
        public static Guid? IsNullReturnNew(Guid? value)
        {
            if (value == null)
            {
                return Guid.NewGuid();
            }
            else
            {
                return value;
            }
        }
        public static object IsNullRtEmptyStr(object value)
        {
            if (value == null) { return string.Empty; } else { return value; }
        }
        public static string IsDbNullRtEmptyString(object value)
        {
            if (value == DBNull.Value) { return string.Empty; } else { return value.ToString(); }
        }
        public static Guid? IsDbNullRtNull(object value)
        {
            if (value == DBNull.Value) { return null; } else { return (Guid)value; }
        }
        public static DateTime? IsDbNullRtNullDate(object value)
        {
            if (value == DBNull.Value) { return null; } else { return (DateTime?)value; }
        }
        public static DateTime IsDbNullRtNow(object value)
        {
            if (value == DBNull.Value) { return DateTime.Now; } else { return (DateTime)value; }
        }
        public static TimeSpan? IsDbNullRtNullTime(object value)
        {
            if (value == DBNull.Value) { return null; } else { return (TimeSpan?)value; }
        }
        public static object IsNullDateReturnNow(object value)
        {
            if (value == null) { return DateTime.Now; } else { return value; }
        }
        #endregion
        public static bool IsValidEmail(string strIn)
        {
            bool invalid;
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;
            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            if (invalid)
                return false;
            // Return true if strIn is in valid email format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();
            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
            }
            return match.Groups[1].Value + domainName;
        }

        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }

        public static string DocumentsName(string xLan, int DocKind)
        {
            string res = "";
            string selQuery = "select top 100 percent * from [px_DocumentsName] where [sys_No]=@Key";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.Int).Value = DocKind;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                 

                    switch (xLan)
                    {
                        case "en":
                            res = Convert.ToString(reader["sys_EnglishName"]);
                            break;
                        case "ar":
                            res = Convert.ToString(reader["sys_ArabicName"]);
                            break;
                        default:
                            res = Convert.ToString(reader["sys_EnglishName"]);
                            break;
                    }
                  
                }
                reader.Close();
            }
            return res;
        }

        public static string QR(string DB, DateTime dTime, decimal Total, decimal Tax)
        {
           
         
            CLiCore.Selections.cvat cvat = new Selections.cvat().GetItem(DB);
            string getTLVFormat =
              $"{Convert.ToChar(1)}{Convert.ToChar(UnicodeEncoding.UTF8.GetByteCount(cvat.CompanyName))}{cvat.CompanyName}"
            + $"{Convert.ToChar(2)}{Convert.ToChar(cvat.vatRNo.Length)}{cvat.vatRNo}"
            + $"{Convert.ToChar(3)}{Convert.ToChar(dTime.ToString("yyyy-MM-dd'T'HH:mm:ssZ").Length)}{dTime.ToString("yyyy-MM-dd'T'HH:mm:ssZ")}"
            + $"{Convert.ToChar(4)}{Convert.ToChar(Total.ToString().Length)}{Total}"
            + $"{Convert.ToChar(5)}{Convert.ToChar(Tax.ToString().Length)}{Tax}";
            return Convert.ToBase64String(UnicodeEncoding.UTF8.GetBytes(getTLVFormat));

        }
    }
    public class OperationResult
    {
        public Guid? OperationKey { get; set; }
        public int VoucherNo { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
    public class xCore
    {
        public static int UpdatePassword( Guid? Key, string Password)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                StringBuilder str = new StringBuilder();
                str.Clear();
                str.Append(" Update px_SelfServiceUsers SET ");
                str.Append(" [ssu_Passwoard]=@ssu_Passwoard");
                str.Append(" WHERE ssu_Employee=@ssu_Employee");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@ssu_Employee", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                comm.Parameters.Add("@ssu_Passwoard", SqlDbType.NVarChar, 127).Value = Password ?? "";

                con.Open();
                res = comm.ExecuteNonQuery();
            }
            return res;
        }
        public static int GetIO(int DocKind)
        {
            int result = 1;
            switch (DocKind)
            {
                case (int)DocumentKind.invOpeningBalance:
                case (int)DocumentKind.PurchaseInvoice:
                case (int)DocumentKind.PurchaseOrder:
                case (int)DocumentKind.PurchaseReceipt:
                case (int)DocumentKind.PurchaseRequest:
                case (int)DocumentKind.ReceiptInWarehouse:
                case (int)DocumentKind.RetConsumptionStock:
                case (int)DocumentKind.ReturnSalesInvoice:
                    result = 1;
                    break;

                default:
                case (int)DocumentKind.SendToWarehouse:
                case (int)DocumentKind.SalesInvoice:
                case (int)DocumentKind.Quotation:
                case (int)DocumentKind.SalesSendMaterial:
                case (int)DocumentKind.ConsumptionStock:
                case (int)DocumentKind.ReturnPurchase:
                    result = -1;
                    break;
            }
            return result;
        }
        public static bool HeaderDebitOrCredit(int DocKind)
        {
            bool result = false;
            switch (DocKind)
            {
                case (int)DocumentKind.finBankCollection:
                case (int)DocumentKind.finCashCollection:
                case (int)DocumentKind.finDebitNote:
                case (int)DocumentKind.SalesInvoice:
                case (int)DocumentKind.ReturnPurchase:
                case (int)DocumentKind.ConsumptionStock:
                    result = false;
                    break;

                case (int)DocumentKind.finBankPayment:
                case (int)DocumentKind.finCashPayment:
                case (int)DocumentKind.finCreditNote:
                case (int)DocumentKind.PurchaseInvoice:
                case (int)DocumentKind.ReturnSalesInvoice:
                case (int)DocumentKind.RetConsumptionStock:
                    result = true;
                    break;
            }
            return result;
        }



    }
}
