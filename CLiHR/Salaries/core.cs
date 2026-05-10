using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiHR.Salaries
{
    public class core
    {
        private readonly static object Locker = new object();
        public static void CreateSalary(string DB,Guid Key,int Year,int Month)
        {
            lock (Locker)
            {
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

                    str.Clear();
                    //Header Data
                    str.Append("exec dbo.spPayroll_CreateSalaries @Key,@Year,@Month ");
              
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    comm.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                    comm.Parameters.Add("@Month", SqlDbType.Int).Value = Month;
    
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
            }
        }
        public static void UpdateAttendance(string DB, Guid? Key, int Absence, decimal Late,decimal Overtime)
        {
            lock (Locker)
            {
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

                    str.Clear();
                    //Header Data
                    str.Append("exec dbo.spPayroll_UpdateAttendance @Key,@Absence,@Late,@Overtime ");

                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    comm.Parameters.Add("@Absence", SqlDbType.Int).Value = Absence;
                    comm.Parameters.Add("@Late", SqlDbType.Decimal).Value = Late;
                    comm.Parameters.Add("@Overtime", SqlDbType.Decimal).Value = Overtime;

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
            }
        }
        public static void UpdateBenefit(string DB,Benefit item)
        {
            lock (Locker)
            {
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
                    str.Clear();
                    str.Append("Update hrDocument_Salaries SET ");
                    str.Append(" [sal_BenRewards]=@sal_BenRewards");
                    str.Append(",[sal_BenBonuses]=@sal_BenBonuses");
                    str.Append(",[sal_BenComissions]=@sal_BenComissions");
                    str.Append(",[Sal_Benefit]=@Sal_Benefit");
                    str.Append(",[sal_Benefit1]=@sal_Benefit1");
                    str.Append(",[sal_Benefit2]=@sal_Benefit2");
                    str.Append(",[sal_Benefit3]=@sal_Benefit3");
                    str.Append(",[sal_BenEndService]=@sal_BenEndService");
                    str.Append(",[sal_BenVacation]=@sal_BenVacation");
                    str.Append(",[sal_BenTicket]=@sal_BenTicket");
                    str.Append(" WHERE sal_Key=@sal_Key");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@sal_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                    comm.Parameters.Add("@sal_BenRewards", SqlDbType.Decimal).Value = item.Rewards;
                    comm.Parameters.Add("@sal_BenBonuses", SqlDbType.Decimal).Value = item.Bonuses;
                    comm.Parameters.Add("@sal_BenComissions", SqlDbType.Decimal).Value = item.Commission;
                    comm.Parameters.Add("@Sal_Benefit", SqlDbType.Decimal).Value = item.BenefitAmount;
                    comm.Parameters.Add("@sal_Benefit1", SqlDbType.Decimal).Value = item.Benefit1;
                    comm.Parameters.Add("@sal_Benefit2", SqlDbType.Decimal).Value = item.Benefit2;
                    comm.Parameters.Add("@sal_Benefit3", SqlDbType.Decimal).Value = item.Benefit3;
                    comm.Parameters.Add("@sal_BenEndService", SqlDbType.Decimal).Value = item.EndService;
                    comm.Parameters.Add("@sal_BenVacation", SqlDbType.Decimal).Value = item.Vacation;
                    comm.Parameters.Add("@sal_BenTicket", SqlDbType.Decimal).Value = item.Ticket;
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
            }
        }
        public static void UpdateDeduction(string DB, Deduction item)
        {
            lock (Locker)
            {
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
                    str.Clear();
                    str.Append("Update hrDocument_Salaries SET ");
                    str.Append(" [sal_DedPenalty]=@sal_DedPenalty");
                    str.Append(",[sal_DedInsurance]=@sal_DedInsurance");
                    str.Append(",[Sal_Deduction]=@Sal_Deduction");
                    str.Append(",[sal_Deduction1]=@sal_Deduction1");
                    str.Append(",[sal_Deduction2]=@sal_Deduction2");
                    str.Append(",[sal_Deduction3]=@sal_Deduction3");
                    str.Append(",[sal_DedAdvance]=@sal_DedAdvance");
                    str.Append(",[sal_DedLoan]=@sal_DedLoan");
                    str.Append(" WHERE sal_Key=@sal_Key");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@sal_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                    comm.Parameters.Add("@sal_DedPenalty", SqlDbType.Decimal).Value = item.Penalty;
                    comm.Parameters.Add("@sal_DedInsurance", SqlDbType.Decimal).Value = item.Insurance;
                    comm.Parameters.Add("@Sal_Deduction", SqlDbType.Decimal).Value = item.DeductionAmount;
                    comm.Parameters.Add("@sal_Deduction1", SqlDbType.Decimal).Value = item.Deduction1;
                    comm.Parameters.Add("@sal_Deduction2", SqlDbType.Decimal).Value = item.Deduction2;
                    comm.Parameters.Add("@sal_Deduction3", SqlDbType.Decimal).Value = item.Deduction3;
                    comm.Parameters.Add("@sal_DedAdvance", SqlDbType.Decimal).Value = item.Advance;
                    comm.Parameters.Add("@sal_DedLoan", SqlDbType.Decimal).Value = item.Loan;
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
            }
        }
        public static void DeleteSalary(string DB, Guid? Key)
        {
            lock (Locker)
            {
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
                    str.Clear();
                    str.Append(" delete from hrDocument_Salaries where [sal_Key]=@Key and [sal_Posted]=0 ");
                    //str.Append(" delete from hrDocument_PrepareSalaries where [sal_Key]=@Key ");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
 
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
            }
        }
    }
}
