using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.Data
{
    public class PersonDetails
    {
        public Guid? Key { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public int NationalityKind { get; set; }
        public string NationalityID { get; set; }
        public string Nationality { get; set; }
        public int PaymentType { get; set; }
        public string Bank { get; set; }
        public string IBAN { get; set; }
        public Structure Structure { get; set; }
        public Department Department { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public JobTitle JobTitle { get; set; }
        public decimal DailyHour { get; set; }
        public int AnnualDays { get; set; }
        public decimal AnnualBalance { get; set; }
        public DateTime? LastReturn { get; set; }
        public int VacationKind { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string xPhone { get; set; }
        public string xMobile { get; set; }
        public string xAddress { get; set; }
        public int Status { get; set; }
        public Guid? Location { get; set; }
        public Boolean Tracking { get; set; }
        public Boolean GPSAttendance { get; set; }
        public string Image { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HomeAllowances { get; set; }
        public decimal Allowances { get; set; }
        public PersonDetails GetItem(string DB,Guid? Key)
        {
            PersonDetails item = new PersonDetails();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_EmployeeDetails(@Key) ";
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
                    item.Email = Convert.ToString(reader["Email"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.NationalityKind = Convert.ToInt32(reader["NationalityKind"]);
                    item.NationalityID = Convert.ToString(reader["NationalityID"]);
                    item.Nationality = Convert.ToString(reader["Nationality"]);
                    item.PaymentType = Convert.ToInt32(reader["PaymentType"]);
                    item.Bank = Convert.ToString(reader["Bank"]);
                    item.IBAN = Convert.ToString(reader["IBAN"]);
                    item.Structure =new Structure().GetItem(DB,iCore.IsDbNullRtNull(reader["Structure"]));
                    item.Department = new Department().GetItem(DB, iCore.IsDbNullRtNull(reader["Department"]));
                    item.StartDate = iCore.IsDbNullRtNullDate(reader["StartDate"]);
                    item.EndDate = iCore.IsDbNullRtNullDate(reader["EndDate"]);
                    item.JobTitle = new JobTitle().GetItem(DB, iCore.IsDbNullRtNull(reader["JobTitle"]));
                    item.DailyHour = Convert.ToDecimal(reader["DailyHour"]);
                    item.AnnualDays = Convert.ToInt32(reader["AnnualDays"]);
                    item.AnnualBalance = Convert.ToDecimal(reader["AnnualBalance"]);
                    item.LastReturn = iCore.IsDbNullRtNullDate(reader["LastReturn"]);
                    item.VacationKind = Convert.ToInt32(reader["VacationKind"]);
                    item.Phone = Convert.ToString(reader["Phone"]);
                    item.Mobile = Convert.ToString(reader["Mobile"]);
                    item.Address = Convert.ToString(reader["Address"]);
                    item.xPhone = Convert.ToString(reader["xPhone"]);
                    item.xMobile = Convert.ToString(reader["xMobile"]);
                    item.xAddress = Convert.ToString(reader["xAddress"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Location = iCore.IsDbNullRtNull(reader["Location"]);
                    item.Tracking = Convert.ToBoolean(reader["Tracking"]);
                    item.GPSAttendance = Convert.ToBoolean(reader["GPSAttendance"]);
                    item.Image = Convert.ToString(reader["Image"]);
                    item.BasicSalary = Convert.ToDecimal(reader["BasicSalary"]);
                    item.HomeAllowances = Convert.ToDecimal(reader["HomeAllowances"]);
                    item.Allowances = Convert.ToDecimal(reader["Allowances"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
