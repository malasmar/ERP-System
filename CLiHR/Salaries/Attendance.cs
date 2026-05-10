using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiHR.Salaries
{
    public class Attendance
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public int Absence { get; set; }
        public decimal LateMinutes { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Allowance { get; set; }
        public decimal Overtime { get; set; }
        public decimal DedLate { get; set; }
        public decimal DedAbsence { get; set; }
        public decimal DedAbsenceAll { get; set; }
        public Boolean Status { get; set; }
        public decimal Total { get; set; }
        public decimal DedAbsenceTotal { get; set; }

        public List<Attendance> GetList(string DB, int Year, int Month, string PaymentKind)
        {
            List<Attendance> items = new List<Attendance>();
            string selQuery = "select top 100 percent * from dbo.fnPayroll_UpdateAttendance(@Year,@Month,@PaymentKind) order by [Code] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                com.Parameters.Add("@Month", SqlDbType.Int).Value = Month;
                com.Parameters.Add("@PaymentKind", SqlDbType.NVarChar, 255).Value = PaymentKind ?? "";
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Attendance item = new Attendance();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Absence = Convert.ToInt32(reader["Absence"]);
                    item.LateMinutes = Convert.ToDecimal(reader["LateMinutes"]);
                    item.OvertimeHours = Convert.ToDecimal(reader["OvertimeHours"]);
                    item.BasicSalary = Convert.ToDecimal(reader["BasicSalary"]);
                    item.Allowance = Convert.ToDecimal(reader["Allowance"]);
                    item.Overtime = Convert.ToDecimal(reader["Overtime"]);
                    item.DedLate = Convert.ToDecimal(reader["DedLate"]);
                    item.DedAbsence = Convert.ToDecimal(reader["DedAbsence"]);
                    item.DedAbsenceAll = Convert.ToDecimal(reader["DedAbsenceAll"]);
                    item.Status = Convert.ToBoolean(reader["Status"]);
                    item.Total = item.Allowance + item.BasicSalary;
                    item.DedAbsenceTotal = item.DedAbsence + item.DedAbsenceAll;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
