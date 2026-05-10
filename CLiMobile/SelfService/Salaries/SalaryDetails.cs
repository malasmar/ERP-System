using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.Salaries
{
    public class SalaryDetails
    {
        public Guid? Key { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Absence { get; set; }
        public decimal LateMinutes { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal BenBasicSalary { get; set; }
        public decimal BenOvertime { get; set; }
        public decimal BenRewards { get; set; }
        public decimal BenBonuses { get; set; }
        public decimal BenComissions { get; set; }
        public decimal Benefit { get; set; }
        public decimal BenEndService { get; set; }
        public decimal BenVacation { get; set; }
        public decimal BenTicket { get; set; }
        public decimal AllHousing { get; set; }
        public decimal AllTransportation { get; set; }
        public decimal AllCommunication { get; set; }
        public decimal AllFood { get; set; }
        public decimal AllOther { get; set; }
        public decimal DedLate { get; set; }
        public decimal DedAbsenceBasic { get; set; }
        public decimal DedAbsenceAllowances { get; set; }
        public decimal DedPenalty { get; set; }
        public decimal DedInsurance { get; set; }
        public decimal Deduction { get; set; }
        public decimal DedAdvance { get; set; }
        public decimal DedLoan { get; set; }
        public decimal TotBenefit { get; set; }
        public decimal TotDeduction { get; set; }
        public decimal Total { get; set; }
        public SalaryDetails GetItem(string DB,Guid? Key)
        {
            SalaryDetails item = new SalaryDetails();
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_SalaryDetails(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Year = Convert.ToInt32(reader["Year"]);
                    item.Month = Convert.ToInt32(reader["Month"]);
                    item.Absence = Convert.ToInt32(reader["Absence"]);
                    item.LateMinutes = Convert.ToDecimal(reader["LateMinutes"]);
                    item.OvertimeHours = Convert.ToDecimal(reader["OvertimeHours"]);
                    item.BenBasicSalary = Convert.ToDecimal(reader["BenBasicSalary"]);
                    item.BenOvertime = Convert.ToDecimal(reader["BenOvertime"]);
                    item.BenRewards = Convert.ToDecimal(reader["BenRewards"]);
                    item.BenBonuses = Convert.ToDecimal(reader["BenBonuses"]);
                    item.BenComissions = Convert.ToDecimal(reader["BenComissions"]);
                    item.Benefit = Convert.ToDecimal(reader["Benefit"]);
                    item.BenEndService = Convert.ToDecimal(reader["BenEndService"]);
                    item.BenVacation = Convert.ToDecimal(reader["BenVacation"]);
                    item.BenTicket = Convert.ToDecimal(reader["BenTicket"]);
                    item.AllHousing = Convert.ToDecimal(reader["AllHousing"]);
                    item.AllTransportation = Convert.ToDecimal(reader["AllTransportation"]);
                    item.AllCommunication = Convert.ToDecimal(reader["AllCommunication"]);
                    item.AllFood = Convert.ToDecimal(reader["AllFood"]);
                    item.AllOther = Convert.ToDecimal(reader["AllOther"]);
                    item.DedLate = Convert.ToDecimal(reader["DedLate"]);
                    item.DedAbsenceBasic = Convert.ToDecimal(reader["DedAbsenceBasic"]);
                    item.DedAbsenceAllowances = Convert.ToDecimal(reader["DedAbsenceAllowances"]);
                    item.DedPenalty = Convert.ToDecimal(reader["DedPenalty"]);
                    item.DedInsurance = Convert.ToDecimal(reader["DedInsurance"]);
                    item.Deduction = Convert.ToDecimal(reader["Deduction"]);
                    item.DedAdvance = Convert.ToDecimal(reader["DedAdvance"]);
                    item.DedLoan = Convert.ToDecimal(reader["DedLoan"]);
                    item.TotBenefit = Convert.ToDecimal(reader["TotBenefit"]);
                    item.TotDeduction = Convert.ToDecimal(reader["TotDeduction"]);
                    item.Total = item.TotBenefit - item.TotDeduction;
                }
                reader.Close();
            }
            return item;
        }
    }
}
