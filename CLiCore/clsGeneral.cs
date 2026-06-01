using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CLiCore
{
    public static class clsGeneral
    {

        public static int UTCOffset = 180;

        public static string UploadFilePath = "";

        public static List<clsLanguage> LangList { get; set; }

        public static string Trans(enumLanguage Language, string Key)
        {
            Key = Key.ToLower().Replace(" ", "").Trim();

            if (LangList == null)
            {
                clsGeneral.LangList = clsLanguage.GetList("");
            }
            if (LangList.Count == 0)
            {
                clsGeneral.LangList = clsLanguage.GetList("");
            }


            string strDisplay = "";
            try
            {
                var obj = clsGeneral.LangList.Where(x => x.LanKey == Key || x.LanCode == Key).FirstOrDefault();

                if (Language == enumLanguage.English)
                {
                    strDisplay = obj == null ? "ErrorT(" + Key + ")" : obj.LanEnglish;
                }
                else if (Language == enumLanguage.Arabic)
                {
                    strDisplay = obj == null ? "ErrorT(" + Key + ")" : obj.LanArabic;
                }
                else if (Language == enumLanguage.Russian)
                {
                    strDisplay = obj == null ? "ErrorT(" + Key + ")" : obj.LanRussian;
                }
                else if (Language == enumLanguage.Indian)
                {
                    strDisplay = obj == null ? "ErrorT(" + Key + ")" : obj.LanIndian;
                }
                else if (Language == enumLanguage.French)
                {
                    strDisplay = obj == null ? "ErrorT(" + Key + ")" : obj.LanFrench;
                }
                else if (Language == enumLanguage.Turkish)
                {
                    strDisplay = obj == null ? "ErrorT(" + Key + ")" : obj.LanTurkish;
                }


                return strDisplay ?? "";
            }
            catch (Exception ex)
            {
                return "Error Translate";
            }
        }

        public static string ConnectionDB { get; set; }

        public static string ScheduleURL { get; set; }

        public static string ClassURL { get; set; }

        public static string BrainCertApiKey { get; set; }

        public static string MailServer { get; set; }

        public static string MailPort { get; set; }

        public static string SenderName { get; set; }

        public static string Sender { get; set; }

        public static string Password { get; set; }

        public static string GetCon(string db)
        {
            return ConnectionDB;
        }

        public static string GetNewCookieId()
        {
            Random random = new Random(90);

            string val = DateTime.Now.ToString("yyyyMMddhhmmss") + random.Next(999).ToString();

            return val;
        }

        public static int? IsNullReturnZero(string value)
        {
            if (value.Equals("null"))
            {
                return 0;
            }
            else
            {
                int val = 0;
                int.TryParse(value, out val);

                return val;
            }
        }

        public static int IsNullReturnZeroValue(string value)
        {
            if (value.Equals("null"))
            {
                return 0;
            }
            else
            {
                int val = 0;
                int.TryParse(value, out val);

                return val;
            }
        }

        public static int IsDbNullRtZero(object value)
        {
            if (value == DBNull.Value) { return 0; } else { return (int)value; }
        }


        public static DateTime IsDbNullRtNow(object value)
        {
            if (value == DBNull.Value) { return DateTime.UtcNow.AddMinutes(UTCOffset); } else { return (DateTime)value; }
        }

        public static Int64 IsDbNullRtZero64(object value)
        {
            if (value == DBNull.Value) { return 0; } else { return (Int64)value; }
        }

        public static DateTime IsDbNullRtToday(object value)
        {
            if (value == DBNull.Value) { return DateTime.Now; } else { return (DateTime)value; }
        }

        public static string IsDbNullRtEmpty(object value)
        {
            if (value == DBNull.Value) { return ""; } else { return value == null ? "" : value.ToString(); }
        }

        public static bool IsDbNullRtFalse(object value)
        {
            if (value == DBNull.Value) { return false; } else { return value == null ? false : (bool)value; }
        }

        public static string IsDateTimeDbNullRtEmpty(object value)
        {
            if (value == DBNull.Value) { return ""; } else { return (value == null || value.ToString().Length < 19) ? "" : ((DateTime)value).ToShortTimeString(); }
        }

        public static DateTime? IsDateTimeDbNullRtNull(object value)
        {
            if (value == DBNull.Value) { return null; } else { return (DateTime)value; }
        }

        public static object ConvertTimeToDate(string _Time)
        {
            if (_Time.Contains("AM") || _Time.Contains("PM"))
            {
                string time = _Time.Replace(" AM", ":00 AM").Replace(" PM", ":00 PM");
                DateTime obj2 = DateTime.ParseExact(DateTime.Now.ToString("M/d/yyyy") + " " + time, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                return obj2;
            }

            return ((object)DBNull.Value);
        }

        public static decimal IsDbNullRtZeroDecimal(object value)
        {
            if (value == DBNull.Value) { return 0; } else { return (decimal)value; }
        }

        public static object IsDateNullrtDbNull(object value)
        {

            if (value == null) { return ((object)DBNull.Value); } else { return (DateTime)value; }
        }



        public enum enumComplaintStatus
        {
            New = 1,
            WaitingForReply = 2,
            Replied = 3
        }
        public enum enumTimeStatus
        {
            Available = 1,
            Reserved = 2,
            Required_For_Reservation = 3
        }






        public enum enumLookupHDR
        {
            Gender = 1,
            Relation = 2,
            UserType = 3,
            Religion = 4,
            DocumentType = 5,
            knowledgLevel = 6,
            Language = 7,
            StudentType = 8,
            EducationalLevel = 9,
            RegistrationStatus = 10,
            StudentStatus = 11,
            WeeklyPlanStatus = 12,
            WeekDays = 13,
            ComplaintStatus = 14,
            SuggestionStatus = 15,
            AccountType = 16,
            QuestionsType = 17
        }

        public enum enumDays
        {
            Saturday = 1,
            Sunday = 2,
            Monday = 3,
            Tuesday = 4,
            Wednesday = 5,
            Thursday = 6,
            Friday = 7,
        }


        public static List<object> GetDaysOfWeek()
        {
            var daysOfWeek = new List<object>
        {
            new { Key = enumDays.Saturday, subName = "Saturday", subNameA = "السبت" },
            new { Key = enumDays.Sunday, subName = "Sunday", subNameA = "الأحد" },
            new { Key = enumDays.Monday, subName = "Monday", subNameA = "الإثنين" },
            new { Key = enumDays.Tuesday, subName = "Tuesday", subNameA = "الثلاثاء" },
            new { Key = enumDays.Wednesday, subName = "Wednesday", subNameA = "الأربعاء" },
            new { Key = enumDays.Thursday, subName = "Thursday", subNameA = "الخميس" },
            new { Key = enumDays.Friday, subName = "Friday", subNameA = "الجمعة" }
        };

            return daysOfWeek;
        }
        public enum enumMonths
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12,
        }

        public enum enumLanguage
        {
            Arabic = 1,
            English = 2,
            Turkish = 3,
            Indian = 4,
            French = 5,
            Russian = 6,
        }

        public enum enumDescriptionHDR
        {
            Gender = 1,
            Language = 2,
            TutoringMethod = 3,
            SessionType = 4,
            SubjectType = 5

        }

        public enum enumSubjectType
        {
            Session = 12,
            Course = 13,

        }

        public enum enumNotificationsType
        {
            General = 1,
            ERP = 2,
            WeeklyPlan = 3,
            Absence = 4,
            Chat = 5,
            Bus = 6,
            CallStudent = 7,
            Exam = 8,
        }
        public enum enumQuestionsType
        {
            EssayQuestion = 1,
            MultipleChoiceQuestion = 2,
            True_FalseQuestion = 3
        }

        public static string DataTableToJSON(DataTable table)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            foreach (DataRow dr in table.Rows)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    row[col.ColumnName] = dr[col];
                }
                rows.Add(row);
            }
            return JsonSerializer.Serialize(rows);
        }





    }
}
