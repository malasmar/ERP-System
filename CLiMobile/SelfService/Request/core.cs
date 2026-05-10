using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace CLiMobile.SelfService.Request
{
    public class core
    {
        public static bool InsertAdvance(string DB, Advance item)
        {
         
            Guid? Key=Guid.NewGuid();
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
                str.Clear();
                str.Append("INSERT INTO hrRequest_Advance");
                str.Append("([req_Key]");
                str.Append(",[req_Create]");
                str.Append(",[req_Employee]");
                str.Append(",[req_Year]");
                str.Append(",[req_Month]");
                str.Append(",[req_Amount]");
                str.Append(",[req_Description]");
                str.Append(",[req_Status]");
                str.Append(",[req_Comment]");
                str.Append(",[req_Payment])");
                str.Append(" VALUES ");
                str.Append("(@req_Key");
                str.Append(",@req_Create");
                str.Append(",@req_Employee");
                str.Append(",@req_Year");
                str.Append(",@req_Month");
                str.Append(",@req_Amount");
                str.Append(",@req_Description");
                str.Append(",@req_Status");
                str.Append(",@req_Comment");
                str.Append(",@req_Payment)");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@req_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                comm.Parameters.Add("@req_Create", SqlDbType.DateTime).Value = DateTime.Now;
                comm.Parameters.Add("@req_Employee", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Employee);
                comm.Parameters.Add("@req_Year", SqlDbType.Int).Value = item.Year;
                comm.Parameters.Add("@req_Month", SqlDbType.Int).Value = item.Month;
                comm.Parameters.Add("@req_Amount", SqlDbType.Decimal).Value = item.Amount;
                comm.Parameters.Add("@req_Description", SqlDbType.NVarChar, 200).Value = item.Description ?? "";
                comm.Parameters.Add("@req_Status", SqlDbType.Int).Value = 0;
                comm.Parameters.Add("@req_Comment", SqlDbType.NVarChar).Value = "";
                comm.Parameters.Add("@req_Payment", SqlDbType.Bit).Value = false;
                comm.ExecuteNonQuery();

                str.Clear();
                str.Append("EXEC dbo.spRequest_Confirmation_Advance @Key,@Employee");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                comm.Parameters.Add("@Employee", SqlDbType.UniqueIdentifier).Value = item.Employee;
                comm.ExecuteNonQuery();

                try
                {
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public static bool InsertLoan(string DB, Loan item)
        {
            Guid? Key = Guid.NewGuid();
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
                str.Clear();
                str.Append("INSERT INTO hrRequest_Loan");
                str.Append("([req_Key]");
                str.Append(",[req_Create]");
                str.Append(",[req_Employee]");
                str.Append(",[req_Year]");
                str.Append(",[req_Month]");
                str.Append(",[req_Amount]");
                str.Append(",[req_MonthlyAmount]");
                str.Append(",[req_Description]");
                str.Append(",[req_Comment]");
                str.Append(",[req_Status]");
                str.Append(",[req_Payment])");
                str.Append(" VALUES ");
                str.Append("(@req_Key");
                str.Append(",@req_Create");
                str.Append(",@req_Employee");
                str.Append(",@req_Year");
                str.Append(",@req_Month");
                str.Append(",@req_Amount");
                str.Append(",@req_MonthlyAmount");
                str.Append(",@req_Description");
                str.Append(",@req_Comment");
                str.Append(",@req_Status");
                str.Append(",@req_Payment)");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@req_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                comm.Parameters.Add("@req_Create", SqlDbType.DateTime).Value = DateTime.Now;
                comm.Parameters.Add("@req_Employee", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Employee);
                comm.Parameters.Add("@req_Year", SqlDbType.Int).Value = item.Year;
                comm.Parameters.Add("@req_Month", SqlDbType.Int).Value = item.Month;
                comm.Parameters.Add("@req_Amount", SqlDbType.Decimal).Value = item.Amount;
                comm.Parameters.Add("@req_MonthlyAmount", SqlDbType.Decimal).Value = item.MonthlyAmount;
                comm.Parameters.Add("@req_Description", SqlDbType.NVarChar, 200).Value = item.Description ?? "";
                comm.Parameters.Add("@req_Status", SqlDbType.Int).Value = 0;
                comm.Parameters.Add("@req_Payment", SqlDbType.Bit).Value = false;
                comm.Parameters.Add("@req_Comment", SqlDbType.NVarChar).Value = "";
                comm.ExecuteNonQuery();

                List<LoanSchedule> items = new LoanSchedule().SplitAdvanceByAmount(item.Amount, item.MonthlyAmount, item.Year, item.Month);
                foreach(LoanSchedule schedule in items)
                {
                    if (item.MonthlyAmount == 0)
                        continue;

                    str.Clear();
                    str.Append("INSERT INTO hrRequest_LoanSechulding");
                    str.Append("([sech_LoanKey]");
                    str.Append(",[sech_Employee]");
                    str.Append(",[sech_Year]");
                    str.Append(",[sech_Month]");
                    str.Append(",[sech_Amount])");
                    str.Append(" VALUES ");
                    str.Append("(@sech_LoanKey");
                    str.Append(",@sech_Employee");
                    str.Append(",@sech_Year");
                    str.Append(",@sech_Month");
                    str.Append(",@sech_Amount)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@sech_LoanKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                    comm.Parameters.Add("@sech_Employee", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Employee);
                    comm.Parameters.Add("@sech_Year", SqlDbType.Int).Value = schedule.Year;
                    comm.Parameters.Add("@sech_Month", SqlDbType.Int).Value = schedule.Month;
                    comm.Parameters.Add("@sech_Amount", SqlDbType.Decimal).Value = schedule.Amount;
                    comm.ExecuteNonQuery();
                }

                str.Clear();
                str.Append("EXEC dbo.spRequest_Confirmation_Loan @Key,@Employee");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                comm.Parameters.Add("@Employee", SqlDbType.UniqueIdentifier).Value = item.Employee;
                comm.ExecuteNonQuery();

                try
                {
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public static bool InsertNormalLeave(string DB, NormalLeave item)
        {

            Guid? Key = Guid.NewGuid();
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
                str.Clear();
                str.Append("INSERT INTO hrRequest_NormalLeave");
                str.Append("([req_Key]");
                str.Append(",[Req_Create]");
                str.Append(",[Req_Employee]");
                str.Append(",[Req_Kind]");
                str.Append(",[Req_LeaveDate]");
                str.Append(",[Req_LeaveDays]");
                str.Append(",[Req_Description]");
                str.Append(",[req_Comment]");
                str.Append(",[Req_Status]");
                str.Append(",[Req_FileName])");
                str.Append(" VALUES ");
                str.Append("(@req_Key");
                str.Append(",@Req_Create");
                str.Append(",@Req_Employee");
                str.Append(",@Req_Kind");
                str.Append(",@Req_LeaveDate");
                str.Append(",@Req_LeaveDays");
                str.Append(",@Req_Description");
                str.Append(",@req_Comment");
                str.Append(",@Req_Status");
                str.Append(",@Req_FileName)");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@req_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                comm.Parameters.Add("@Req_Create", SqlDbType.DateTime).Value =DateTime.UtcNow;
                comm.Parameters.Add("@Req_Employee", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Employee);
                comm.Parameters.Add("@Req_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@Req_LeaveDate", SqlDbType.Date).Value = item.LeaveDate;
                comm.Parameters.Add("@Req_LeaveDays", SqlDbType.Int).Value = item.LeaveDays;
                comm.Parameters.Add("@Req_Description", SqlDbType.NVarChar, 200).Value = item.Description ?? "";
                comm.Parameters.Add("@Req_Status", SqlDbType.Int).Value =0;
                comm.Parameters.Add("@Req_FileName", SqlDbType.NVarChar, 500).Value =item.FileName;
                comm.Parameters.Add("@req_Comment", SqlDbType.NVarChar).Value = "";
                comm.ExecuteNonQuery();

                str.Clear();
                str.Append("EXEC dbo.spRequest_Confirmation_NormalLeave @Key,@Employee");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                comm.Parameters.Add("@Employee", SqlDbType.UniqueIdentifier).Value = item.Employee;
                comm.ExecuteNonQuery();

                try
                {
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public static bool InsertHourLeave(string DB, HourLeave item)
        {

            Guid? Key = Guid.NewGuid();
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
                str.Clear();
                str.Append("INSERT INTO hrRequest_HourLeave");
                str.Append("([req_Key]");
                str.Append(",[Req_Create]");
                str.Append(",[Req_Employee]");
                str.Append(",[Req_Kind]");
                str.Append(",[Req_LeaveDate]");
                str.Append(",[Req_LeaveHour]");
                str.Append(",[Req_ReturnHour]");
                str.Append(",[Req_EndDay]");
                str.Append(",[Req_Description]");
                str.Append(",[req_Comment]");
                str.Append(",[Req_Status])");
                str.Append(" VALUES ");
                str.Append("(@req_Key");
                str.Append(",@Req_Create");
                str.Append(",@Req_Employee");
                str.Append(",@Req_Kind");
                str.Append(",@Req_LeaveDate");
                str.Append(",@Req_LeaveHour");
                str.Append(",@Req_ReturnHour");
                str.Append(",@Req_EndDay");
                str.Append(",@Req_Description");
                str.Append(",@req_Comment");
                str.Append(",@Req_Status)");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@req_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                comm.Parameters.Add("@Req_Create", SqlDbType.DateTime).Value = DateTime.UtcNow;
                comm.Parameters.Add("@Req_Employee", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Employee);
                comm.Parameters.Add("@Req_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@Req_LeaveDate", SqlDbType.Date).Value = item.LeaveDate;
                comm.Parameters.Add("@Req_LeaveHour", SqlDbType.Time).Value = iCore.IsNullRtDbNull(item.LeaveHour);
                comm.Parameters.Add("@Req_ReturnHour", SqlDbType.Time).Value = iCore.IsNullRtDbNull(item.ReturnHour);
                comm.Parameters.Add("@Req_EndDay", SqlDbType.Bit).Value = item.EndDay;
                comm.Parameters.Add("@Req_Description", SqlDbType.NVarChar, 200).Value = item.Description ?? "";
                comm.Parameters.Add("@Req_Status", SqlDbType.Int).Value = 0;
                comm.Parameters.Add("@req_Comment", SqlDbType.NVarChar).Value = "";
                comm.ExecuteNonQuery();

                str.Clear();
                str.Append("EXEC dbo.spRequest_Confirmation_HourlyLeave @Key,@Employee");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                comm.Parameters.Add("@Employee", SqlDbType.UniqueIdentifier).Value = item.Employee;
                comm.ExecuteNonQuery();

                try
                {
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public static bool InsertAnnualLeave(string DB, AnnualLeave item)
        {

            Guid? Key = Guid.NewGuid();
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
                str.Clear();
                str.Append("INSERT INTO hrRequest_AnnualLeave");
                str.Append("([req_Key]");
                str.Append(",[Req_Create]");
                str.Append(",[Req_Employee]");
                str.Append(",[Req_Kind]");
                str.Append(",[Req_LeaveDate]");
                str.Append(",[Req_LeaveDays]");
                str.Append(",[Req_Description]");
                str.Append(",[req_Comment]");
                str.Append(",[Req_Status]");
                str.Append(",[Req_AttachmentStatus]");
                str.Append(",[Req_FileName]");
                str.Append(",[Req_Year]");
                str.Append(",[Req_Month]");
                str.Append(",[Req_ReturnStatus]");
                str.Append(",[Req_ReturnDate]");
                str.Append(",[Req_StartDate]");
                str.Append(",[Req_ReturnDescription]");
                str.Append(",[Req_ConfirmReturn])");
                str.Append(" VALUES ");
                str.Append("(@req_Key");
                str.Append(",@Req_Create");
                str.Append(",@Req_Employee");
                str.Append(",@Req_Kind");
                str.Append(",@Req_LeaveDate");
                str.Append(",@Req_LeaveDays");
                str.Append(",@Req_Description");
                str.Append(",@req_Comment");
                str.Append(",@Req_Status");
                str.Append(",@Req_AttachmentStatus");
                str.Append(",@Req_FileName");
                str.Append(",@Req_Year");
                str.Append(",@Req_Month");
                str.Append(",@Req_ReturnStatus");
                str.Append(",@Req_ReturnDate");
                str.Append(",@Req_StartDate");
                str.Append(",@Req_ReturnDescription");
                str.Append(",@Req_ConfirmReturn)");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@req_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                comm.Parameters.Add("@Req_Create", SqlDbType.DateTime).Value =DateTime.UtcNow;
                comm.Parameters.Add("@Req_Employee", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Employee);
                comm.Parameters.Add("@Req_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@Req_LeaveDate", SqlDbType.Date).Value = item.LeaveDate;
                comm.Parameters.Add("@Req_LeaveDays", SqlDbType.Int).Value = item.LeaveDays;
                comm.Parameters.Add("@Req_Description", SqlDbType.NVarChar, 200).Value = item.Description ?? "";
                comm.Parameters.Add("@Req_Status", SqlDbType.Int).Value = false;
                comm.Parameters.Add("@Req_AttachmentStatus", SqlDbType.Bit).Value = item.AttachmentStatus;
                comm.Parameters.Add("@Req_FileName", SqlDbType.NVarChar, 500).Value = item.FileName ?? "";
                comm.Parameters.Add("@Req_Year", SqlDbType.Int).Value = 0;
                comm.Parameters.Add("@Req_Month", SqlDbType.Int).Value = 0;
                comm.Parameters.Add("@Req_ReturnStatus", SqlDbType.Bit).Value = false;
                comm.Parameters.Add("@Req_ReturnDate", SqlDbType.Date).Value = DBNull.Value;
                comm.Parameters.Add("@Req_StartDate", SqlDbType.Date).Value = DBNull.Value;
                comm.Parameters.Add("@Req_ReturnDescription", SqlDbType.NVarChar, 500).Value =  "";
                comm.Parameters.Add("@Req_ConfirmReturn", SqlDbType.Bit).Value = false;
                comm.Parameters.Add("@req_Comment", SqlDbType.NVarChar).Value = "";
                comm.ExecuteNonQuery();

                str.Clear();
                str.Append("EXEC dbo.spRequest_Confirmation_AnnualLeave @Key,@Employee");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                comm.Parameters.Add("@Employee", SqlDbType.UniqueIdentifier).Value = item.Employee;
                comm.ExecuteNonQuery();

                try
                {
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public static bool InsertReward(string DB, Reward item)
        {

            Guid? Key = Guid.NewGuid();
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
                str.Clear();
                str.Append("INSERT INTO hrRequest_Reward");
                str.Append("([req_Key]");
                str.Append(",[req_Create]");
                str.Append(",[req_RewardKey]");
                str.Append(",[req_Supervisor]");
                str.Append(",[req_Year]");
                str.Append(",[req_Month]");
                str.Append(",[req_Kind]");
                str.Append(",[req_Value]");
                str.Append(",[req_Description]");
                str.Append(",[req_Comment]");
                str.Append(",[req_Status])");
                str.Append(" VALUES ");
                str.Append("(@req_Key");
                str.Append(",@req_Create");
                str.Append(",@req_RewardKey");
                str.Append(",@req_Supervisor");
                str.Append(",@req_Year");
                str.Append(",@req_Month");
                str.Append(",@req_Kind");
                str.Append(",@req_Value");
                str.Append(",@req_Description");
                str.Append(",@req_Comment");
                str.Append(",@req_Status)");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@req_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                comm.Parameters.Add("@req_Create", SqlDbType.DateTime).Value = DateTime.Now;
                comm.Parameters.Add("@req_RewardKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.RewardKey);
                comm.Parameters.Add("@req_Supervisor", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Supervisor);
                comm.Parameters.Add("@req_Year", SqlDbType.Int).Value = item.Year;
                comm.Parameters.Add("@req_Month", SqlDbType.Int).Value = item.Month;
                comm.Parameters.Add("@req_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@req_Value", SqlDbType.Decimal).Value = item.Value;
                comm.Parameters.Add("@req_Description", SqlDbType.NVarChar, 200).Value = item.Description ?? "";
                comm.Parameters.Add("@req_Status", SqlDbType.Int).Value = false;
                comm.Parameters.Add("@req_Comment", SqlDbType.NVarChar).Value = "";
                comm.ExecuteNonQuery();

                int i = 0;
                foreach(Guid k in item.Employee)
                {
                    str.Clear();
                    str.Append("INSERT INTO hrRequest_Reward_Employees");
                    str.Append("([req_Order]");
                    str.Append(",[req_Request]");
                    str.Append(",[req_Employee])");
                    str.Append(" VALUES ");
                    str.Append("(@req_Order");
                    str.Append(",@req_Request");
                    str.Append(",@req_Employee)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@req_Order", SqlDbType.Int).Value = i;
                    comm.Parameters.Add("@req_Request", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                    comm.Parameters.Add("@req_Employee", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(k);
                    comm.ExecuteNonQuery();
                    ++i;
                }

                str.Clear();
                str.Append("EXEC dbo.spRequest_Confirmation_Reward @Key,@Employee");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                comm.Parameters.Add("@Employee", SqlDbType.UniqueIdentifier).Value = item.Supervisor;
                comm.ExecuteNonQuery();

                try
                {
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public static bool InsertPenalty(string DB, Penalty item)
        {

            Guid? Key = Guid.NewGuid();
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
                str.Clear();
                str.Append("INSERT INTO hrRequest_Penalty");
                str.Append("([req_Key]");
                str.Append(",[req_Create]");
                str.Append(",[req_PenaltyKey]");
                str.Append(",[req_Supervisor]");
                str.Append(",[req_Year]");
                str.Append(",[req_Month]");
                str.Append(",[req_Kind]");
                str.Append(",[req_Value]");
                str.Append(",[req_Description]");
                str.Append(",[req_Comment]");
                str.Append(",[req_Status])");
                str.Append(" VALUES ");
                str.Append("(@req_Key");
                str.Append(",@req_Create");
                str.Append(",@req_PenaltyKey");
                str.Append(",@req_Supervisor");
                str.Append(",@req_Year");
                str.Append(",@req_Month");
                str.Append(",@req_Kind");
                str.Append(",@req_Value");
                str.Append(",@req_Description");
                str.Append(",@req_Comment");
                str.Append(",@req_Status)");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@req_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                comm.Parameters.Add("@req_Create", SqlDbType.DateTime).Value = DateTime.Now;
                comm.Parameters.Add("@req_PenaltyKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.PenaltyKey);
                comm.Parameters.Add("@req_Supervisor", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Supervisor);
                comm.Parameters.Add("@req_Year", SqlDbType.Int).Value = item.Year;
                comm.Parameters.Add("@req_Month", SqlDbType.Int).Value = item.Month;
                comm.Parameters.Add("@req_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@req_Value", SqlDbType.Decimal).Value = item.Value;
                comm.Parameters.Add("@req_Description", SqlDbType.NVarChar, 200).Value = item.Description ?? "";
                comm.Parameters.Add("@req_Status", SqlDbType.Int).Value = false;
                comm.Parameters.Add("@req_Comment", SqlDbType.NVarChar).Value = "";
                comm.ExecuteNonQuery();

                int i = 0;
                foreach (Guid k in item.Employee)
                {
                    str.Clear();
                    str.Append("INSERT INTO hrRequest_Penalty_Employees");
                    str.Append("([req_Order]");
                    str.Append(",[req_Request]");
                    str.Append(",[req_Employee])");
                    str.Append(" VALUES ");
                    str.Append("(@req_Order");
                    str.Append(",@req_Request");
                    str.Append(",@req_Employee)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@req_Order", SqlDbType.Int).Value = i;
                    comm.Parameters.Add("@req_Request", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                    comm.Parameters.Add("@req_Employee", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(k);
                    comm.ExecuteNonQuery();
                    ++i;
                }


                str.Clear();
                str.Append("EXEC dbo.spRequest_Confirmation_Penalty @Key,@Employee");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                comm.Parameters.Add("@Employee", SqlDbType.UniqueIdentifier).Value = item.Supervisor;
                comm.ExecuteNonQuery();

                try
                {
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }


        //Confirmation
        public static bool ConfirmAdvance(string DB, Confirmation item)
        {
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
                str.Clear();
                str.Append("Update hrRequest_Confirmation SET ");
                str.Append(" [conf_User]=@conf_User");
                str.Append(",[conf_Date]=@conf_Date");
                str.Append(",[conf_Description]=@conf_Description");
                str.Append(",[conf_Status]=@conf_Status");
                str.Append(" WHERE conf_Key=@conf_Key");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@conf_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@conf_User", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Person);
                comm.Parameters.Add("@conf_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                comm.Parameters.Add("@conf_Description", SqlDbType.NVarChar, 250).Value = item.Comment ?? "";
                comm.Parameters.Add("@conf_Status", SqlDbType.Int).Value = item.Status;

                comm.ExecuteNonQuery();

                if (item.Final == true)
                {
                    str.Clear();
                    str.Append("update [hrRequest_Advance] set ");
                    str.Append(" [req_Status]=@req_Status");
                    str.Append(" ,[req_Comment]=@req_Comment");
                    str.Append(" WHERE req_Key=@req_Key");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@req_Key", SqlDbType.UniqueIdentifier).Value = item.Request;
                    comm.Parameters.Add("@req_Status", SqlDbType.Int).Value = item.Status;
                    comm.Parameters.Add("@req_Comment", SqlDbType.NVarChar).Value = item.Comment??"";
                    comm.ExecuteNonQuery();
                }
             

                try
                {
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public static bool ConfirmLoan(string DB, Confirmation item)
        {
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
                str.Clear();
                str.Append("Update hrRequest_Confirmation SET ");
                str.Append(" [conf_User]=@conf_User");
                str.Append(",[conf_Date]=@conf_Date");
                str.Append(",[conf_Description]=@conf_Description");
                str.Append(",[conf_Status]=@conf_Status");
                str.Append(" WHERE conf_Key=@conf_Key");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@conf_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@conf_User", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Person);
                comm.Parameters.Add("@conf_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                comm.Parameters.Add("@conf_Description", SqlDbType.NVarChar, 250).Value = item.Comment ?? "";
                comm.Parameters.Add("@conf_Status", SqlDbType.Int).Value = item.Status;

                comm.ExecuteNonQuery();

                if (item.Final == true)
                {
                    str.Clear();
                    str.Append("update [hrRequest_Loan] set ");
                    str.Append(" [req_Status]=@req_Status");
                    str.Append(" ,[req_Comment]=@req_Comment");
                    str.Append(" WHERE req_Key=@req_Key");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@req_Key", SqlDbType.UniqueIdentifier).Value = item.Request;
                    comm.Parameters.Add("@req_Status", SqlDbType.Int).Value = item.Status;
                    comm.Parameters.Add("@req_Comment", SqlDbType.NVarChar).Value = item.Comment??"";
                    comm.ExecuteNonQuery();
                }


                try
                {
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public static bool ConfirmNormalLeave(string DB, Confirmation item)
        {
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
                str.Clear();
                str.Append("Update hrRequest_Confirmation SET ");
                str.Append(" [conf_User]=@conf_User");
                str.Append(",[conf_Date]=@conf_Date");
                str.Append(",[conf_Description]=@conf_Description");
                str.Append(",[conf_Status]=@conf_Status");
                str.Append(" WHERE conf_Key=@conf_Key");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@conf_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@conf_User", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Person);
                comm.Parameters.Add("@conf_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                comm.Parameters.Add("@conf_Description", SqlDbType.NVarChar, 250).Value = item.Comment ?? "";
                comm.Parameters.Add("@conf_Status", SqlDbType.Int).Value = item.Status;
                comm.ExecuteNonQuery();

                if (item.Final == true)
                {
                    str.Clear();
                    str.Append("update [hrRequest_NormalLeave] set ");
                    str.Append(" [req_Status]=@req_Status");
                    str.Append(" ,[req_Comment]=@req_Comment");
                    str.Append(" WHERE req_Key=@req_Key");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@req_Key", SqlDbType.UniqueIdentifier).Value = item.Request;
                    comm.Parameters.Add("@req_Status", SqlDbType.Int).Value = item.Status;
                    comm.Parameters.Add("@req_Comment", SqlDbType.NVarChar).Value = item.Comment??"";
                    comm.ExecuteNonQuery();
                }


                try
                {
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public static bool ConfirmAnnualLeave(string DB, Confirmation item)
        {
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
                str.Clear();
                str.Append("Update hrRequest_Confirmation SET ");
                str.Append(" [conf_User]=@conf_User");
                str.Append(",[conf_Date]=@conf_Date");
                str.Append(",[conf_Description]=@conf_Description");
                str.Append(",[conf_Status]=@conf_Status");
                str.Append(" WHERE conf_Key=@conf_Key");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@conf_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@conf_User", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Person);
                comm.Parameters.Add("@conf_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                comm.Parameters.Add("@conf_Description", SqlDbType.NVarChar, 250).Value = item.Comment ?? "";
                comm.Parameters.Add("@conf_Status", SqlDbType.Int).Value = item.Status;
                comm.ExecuteNonQuery();

                if (item.Final == true)
                {
                    str.Clear();
                    str.Append("update [hrRequest_AnnualLeave] set ");
                    str.Append(" [req_Status]=@req_Status");
                    str.Append(" ,[req_Comment]=@req_Comment");
                    str.Append(" WHERE req_Key=@req_Key");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@req_Key", SqlDbType.UniqueIdentifier).Value = item.Request;
                    comm.Parameters.Add("@req_Status", SqlDbType.Int).Value = item.Status;
                    comm.Parameters.Add("@req_Comment", SqlDbType.NVarChar).Value = item.Comment??"";
                    comm.ExecuteNonQuery();
                }


                try
                {
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public static bool ConfirmHourLeave(string DB, Confirmation item)
        {
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
                str.Clear();
                str.Append("Update hrRequest_Confirmation SET ");
                str.Append(" [conf_User]=@conf_User");
                str.Append(",[conf_Date]=@conf_Date");
                str.Append(",[conf_Description]=@conf_Description");
                str.Append(",[conf_Status]=@conf_Status");
                str.Append(" WHERE conf_Key=@conf_Key");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@conf_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@conf_User", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Person);
                comm.Parameters.Add("@conf_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                comm.Parameters.Add("@conf_Description", SqlDbType.NVarChar, 250).Value = item.Comment ?? "";
                comm.Parameters.Add("@conf_Status", SqlDbType.Int).Value = item.Status;
                comm.ExecuteNonQuery();

                if (item.Final == true)
                {
                    str.Clear();
                    str.Append("update [hrRequest_HourLeave] set ");
                    str.Append(" [req_Status]=@req_Status");
                    str.Append(" ,[req_Comment]=@req_Comment");
                    str.Append(" WHERE req_Key=@req_Key");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@req_Key", SqlDbType.UniqueIdentifier).Value = item.Request;
                    comm.Parameters.Add("@req_Status", SqlDbType.Int).Value = item.Status;
                    comm.Parameters.Add("@req_Comment", SqlDbType.NVarChar).Value = item.Comment??"";
                    comm.ExecuteNonQuery();
                }


                try
                {
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public static bool ConfirmPenalty(string DB, PRConfirmation item)
        {
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
                str.Clear();
                str.Append("Update hrRequest_Confirmation SET ");
                str.Append(" [conf_User]=@conf_User");
                str.Append(",[conf_Date]=@conf_Date");
                str.Append(",[conf_Description]=@conf_Description");
                str.Append(",[conf_Status]=@conf_Status");
                str.Append(" WHERE conf_Key=@conf_Key");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@conf_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@conf_User", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Person);
                comm.Parameters.Add("@conf_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                comm.Parameters.Add("@conf_Description", SqlDbType.NVarChar, 250).Value = item.Comment ?? "";
                comm.Parameters.Add("@conf_Status", SqlDbType.Int).Value = item.Status;
                comm.ExecuteNonQuery();

                if (item.Editable == true)
                {
                    str.Clear();
                    str.Append("update [hrRequest_Penalty] set ");
                    str.Append(" [req_Year]=@req_Year");
                    str.Append(",[req_Month]=@req_Month");
                    str.Append(",[req_Kind]=@req_Kind");
                    str.Append(",[req_Value]=@req_Value");
                    str.Append(" WHERE req_Key=@req_Key");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@req_Key", SqlDbType.UniqueIdentifier).Value = item.Request;
                    comm.Parameters.Add("@req_Year", SqlDbType.Int).Value = item.Year;
                    comm.Parameters.Add("@req_Month", SqlDbType.Int).Value = item.Month;
                    comm.Parameters.Add("@req_Kind", SqlDbType.Int).Value = item.Kind;
                    comm.Parameters.Add("@req_Value", SqlDbType.Decimal).Value = item.Value;
                    comm.ExecuteNonQuery();
                }

                if (item.Final == true)
                {
                    str.Clear();
                    str.Append("update [hrRequest_Penalty] set ");
                    str.Append(" [req_Status]=@req_Status");
                    str.Append(" ,[req_Comment]=@req_Comment");
                    str.Append(" WHERE req_Key=@req_Key");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@req_Key", SqlDbType.UniqueIdentifier).Value = item.Request;
                    comm.Parameters.Add("@req_Status", SqlDbType.Int).Value = item.Status;
                    comm.Parameters.Add("@req_Comment", SqlDbType.NVarChar).Value = item.Comment;
                    comm.ExecuteNonQuery();
                }


                try
                {
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public static bool ConfirmReward(string DB, PRConfirmation item)
        {
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
                str.Clear();
                str.Append("Update hrRequest_Confirmation SET ");
                str.Append(" [conf_User]=@conf_User");
                str.Append(",[conf_Date]=@conf_Date");
                str.Append(",[conf_Description]=@conf_Description");
                str.Append(",[conf_Status]=@conf_Status");
                str.Append(" WHERE conf_Key=@conf_Key");
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@conf_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@conf_User", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Person);
                comm.Parameters.Add("@conf_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                comm.Parameters.Add("@conf_Description", SqlDbType.NVarChar, 250).Value = item.Comment ?? "";
                comm.Parameters.Add("@conf_Status", SqlDbType.Int).Value = item.Status;
                comm.ExecuteNonQuery();

                if (item.Editable == true)
                {
                    str.Clear();
                    str.Append("update [hrRequest_Reward] set ");
                    str.Append(" [req_Year]=@req_Year");
                    str.Append(",[req_Month]=@req_Month");
                    str.Append(",[req_Kind]=@req_Kind");
                    str.Append(",[req_Value]=@req_Value");
                    str.Append(" WHERE req_Key=@req_Key");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@req_Key", SqlDbType.UniqueIdentifier).Value = item.Request;
                    comm.Parameters.Add("@req_Year", SqlDbType.Int).Value = item.Year;
                    comm.Parameters.Add("@req_Month", SqlDbType.Int).Value = item.Month;
                    comm.Parameters.Add("@req_Kind", SqlDbType.Int).Value = item.Kind;
                    comm.Parameters.Add("@req_Value", SqlDbType.Decimal).Value = item.Value;
                    comm.ExecuteNonQuery();
                }

                if (item.Final == true)
                {
                    str.Clear();
                    str.Append("update [hrRequest_Reward] set ");
                    str.Append(" [req_Status]=@req_Status");
                    str.Append(" ,[req_Comment]=@req_Comment");
                    str.Append(" WHERE req_Key=@req_Key");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@req_Key", SqlDbType.UniqueIdentifier).Value = item.Request;
                    comm.Parameters.Add("@req_Status", SqlDbType.Int).Value = item.Status;
                    comm.Parameters.Add("@req_Comment", SqlDbType.NVarChar).Value = item.Comment;
                    comm.ExecuteNonQuery();
                }


                try
                {
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}
