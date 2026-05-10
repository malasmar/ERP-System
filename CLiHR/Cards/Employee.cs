using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
 

namespace CLiHR.Cards
{
    public class Employee
    {
        public Guid? Key { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public string Email { get; set; }
        public int Serial { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Boolean Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public int NationalityKind { get; set; }
        public string NationalityID { get; set; }
        public string Nationality { get; set; }
        public int MaritalStatus { get; set; }
        public int AdultFollower { get; set; }
        public int ChildFollower { get; set; }
        public int PaymentType { get; set; }
        public string Bank { get; set; }
        public string IBAN { get; set; }
        public Guid? Biometric { get; set; }
        public int EnrollID { get; set; }
        public Guid? Structure { get; set; }
        public Guid? Department { get; set; }
        public Guid? City { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal AllHome { get; set; }
        public decimal AllTransportation { get; set; }
        public decimal AllComunication { get; set; }
        public decimal AllFood { get; set; }
        public decimal AllOther1 { get; set; }
        public decimal AllOther2 { get; set; }
        public decimal AllOther3 { get; set; }
        public Boolean Insurance { get; set; }
        public string? InsuranceNo { get; set; }
        public decimal InsuranceSalary { get; set; }
        public decimal InsuranceHome { get; set; }
        public decimal InsuranceOther { get; set; }
        public Boolean InsuranceKind { get; set; }
        public decimal InsuranceValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int JobKind { get; set; }
        public Guid? JobTitle { get; set; }
        public string JobDescription { get; set; }
        public decimal DailyHour { get; set; }
        public string Shift { get; set; }
        public int AnnualDays { get; set; }
        public decimal AnnualBalance { get; set; }
        public DateTime? LastReturn { get; set; }
        public int ContractKind { get; set; }
        public int VacationKind { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string xPhone { get; set; }
        public string xMobile { get; set; }
        public string xAddress { get; set; }
        public string xEmail { get; set; }
        public Boolean LockCard { get; set; }
        public Boolean LockSalary { get; set; }
        public int TicketKind { get; set; }
        public int TicketCount { get; set; }
        public int Status { get; set; }
        public Guid? Location { get; set; }
        public Boolean Tracking { get; set; }
        public Boolean GPSAttendance { get; set; }
        public Guid? Branch { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public Guid? accIntegration { get; set; }
        public Guid? accAccount { get; set; }
        public Guid? accAdvance { get; set; }
        public Guid? accLoan { get; set; }
        public Guid? accEosProvision { get; set; }
        public Guid? accVacProvision { get; set; }
        public string Image { get; set; }

        public bool SelfService { get; set; }


        public List<Employee> GetList(string DB)
        {
            List<Employee> items = new List<Employee>();
            string selQuery = "select top 100 percent * from hrCard_Employee order by [emp_Code] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Employee item = new Employee();
                    item.Key = iCore.IsDbNullRtNull(reader["emp_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["emp_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["emp_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["emp_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["emp_LastupDate"]);
                    item.Email = Convert.ToString(reader["emp_Email"]);
                    item.Serial = Convert.ToInt32(reader["emp_Serial"]);
                    item.Code = Convert.ToString(reader["emp_Code"]);
                    item.Name1 = Convert.ToString(reader["emp_Name1"]);
                    item.Name2 = Convert.ToString(reader["emp_Name2"]);
                    item.Gender = Convert.ToBoolean(reader["emp_Gender"]);
                    item.BirthDate = iCore.IsDbNullRtNullDate(reader["emp_BirthDate"]);
                    item.NationalityKind = Convert.ToInt32(reader["emp_NationalityKind"]);
                    item.NationalityID = Convert.ToString(reader["emp_NationalityID"]);
                    item.Nationality = Convert.ToString(reader["emp_Nationality"]);
                    item.MaritalStatus = Convert.ToInt32(reader["emp_MaritalStatus"]);
                    item.AdultFollower = Convert.ToInt32(reader["emp_AdultFollower"]);
                    item.ChildFollower = Convert.ToInt32(reader["emp_ChildFollower"]);
                    item.PaymentType = Convert.ToInt32(reader["emp_PaymentType"]);
                    item.Bank = Convert.ToString(reader["emp_Bank"]);
                    item.IBAN = Convert.ToString(reader["emp_IBAN"]);
                    item.Biometric = iCore.IsDbNullRtNull(reader["emp_Biometric"]);
                    item.EnrollID = Convert.ToInt32(reader["emp_EnrollID"]);
                    item.Structure = iCore.IsDbNullRtNull(reader["emp_Structure"]);
                    item.Department = iCore.IsDbNullRtNull(reader["emp_Department"]);
                    item.City = iCore.IsDbNullRtNull(reader["emp_City"]);
                    item.BasicSalary = Convert.ToDecimal(reader["emp_BasicSalary"]);
                    item.AllHome = Convert.ToDecimal(reader["emp_AllHome"]);
                    item.AllTransportation = Convert.ToDecimal(reader["emp_AllTransportation"]);
                    item.AllComunication = Convert.ToDecimal(reader["emp_AllComunication"]);
                    item.AllFood = Convert.ToDecimal(reader["emp_AllFood"]);
                    item.AllOther1 = Convert.ToDecimal(reader["emp_AllOther1"]);
                    item.AllOther2 = Convert.ToDecimal(reader["emp_AllOther2"]);
                    item.AllOther3 = Convert.ToDecimal(reader["emp_AllOther3"]);
                    item.Insurance = Convert.ToBoolean(reader["emp_Insurance"]);
                    item.InsuranceNo = Convert.ToString(reader["emp_InsuranceNo"]);
                    item.InsuranceSalary = Convert.ToDecimal(reader["emp_InsuranceSalary"]);
                    item.InsuranceHome = Convert.ToDecimal(reader["emp_InsuranceHome"]);
                    item.InsuranceOther = Convert.ToDecimal(reader["emp_InsuranceOther"]);
                    item.InsuranceKind = Convert.ToBoolean(reader["emp_InsuranceKind"]);
                    item.InsuranceValue = Convert.ToDecimal(reader["emp_InsuranceValue"]);
                    item.StartDate = iCore.IsDbNullRtNullDate(reader["emp_StartDate"]);
                    item.EndDate = iCore.IsDbNullRtNullDate(reader["emp_EndDate"]);
                    item.JobKind = Convert.ToInt32(reader["emp_JobKind"]);
                    item.JobTitle = iCore.IsDbNullRtNull(reader["emp_JobTitle"]);
                    item.JobDescription = Convert.ToString(reader["emp_JobDescription"]);
                    item.DailyHour = Convert.ToDecimal(reader["emp_DailyHour"]);
                    item.Shift = Convert.ToString(reader["emp_Shift"]);
                    item.AnnualDays = Convert.ToInt32(reader["emp_AnnualDays"]);
                    item.AnnualBalance = Convert.ToDecimal(reader["emp_AnnualBalance"]);
                    item.LastReturn = iCore.IsDbNullRtNullDate(reader["emp_LastReturn"]);
                    item.ContractKind = Convert.ToInt32(reader["emp_ContractKind"]);
                    item.VacationKind = Convert.ToInt32(reader["emp_VacationKind"]);
                    item.Phone = Convert.ToString(reader["emp_Phone"]);
                    item.Mobile = Convert.ToString(reader["emp_Mobile"]);
                    item.Address = Convert.ToString(reader["emp_Address"]);
                    item.xPhone = Convert.ToString(reader["emp_xPhone"]);
                    item.xMobile = Convert.ToString(reader["emp_xMobile"]);
                    item.xAddress = Convert.ToString(reader["emp_xAddress"]);
                    item.xEmail = Convert.ToString(reader["emp_xEmail"]);
                    item.LockCard = Convert.ToBoolean(reader["emp_LockCard"]);
                    item.LockSalary = Convert.ToBoolean(reader["emp_LockSalary"]);
                    item.TicketKind = Convert.ToInt32(reader["emp_TicketKind"]);
                    item.TicketCount = Convert.ToInt32(reader["emp_TicketCount"]);
                    item.Status = Convert.ToInt32(reader["emp_Status"]);
                    item.Location = iCore.IsDbNullRtNull(reader["emp_Location"]);
                    item.Tracking = Convert.ToBoolean(reader["emp_Tracking"]);
                    item.GPSAttendance = Convert.ToBoolean(reader["emp_GPSAttendance"]);
                    item.Branch = iCore.IsDbNullRtNull(reader["emp_Branch"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["emp_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["emp_Project"]);
                    item.accIntegration = iCore.IsDbNullRtNull(reader["emp_accIntegration"]);
                    item.accAccount = iCore.IsDbNullRtNull(reader["emp_accAccount"]);
                    item.accAdvance = iCore.IsDbNullRtNull(reader["emp_accAdvance"]);
                    item.accLoan = iCore.IsDbNullRtNull(reader["emp_accLoan"]);
                    item.accEosProvision = iCore.IsDbNullRtNull(reader["emp_accEosProvision"]);
                    item.accVacProvision = iCore.IsDbNullRtNull(reader["emp_accVacProvision"]);
                    item.Image = Convert.ToString(reader["emp_Image"]);
                    item.SelfService = Convert.ToBoolean(reader["emp_SelfService"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public Employee GetItem(string DB, Guid? Key)
        {
            Employee item = new Employee();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from hrCard_Employee where [emp_Key]=@Key";
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
                    item.Key = iCore.IsDbNullRtNull(reader["emp_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["emp_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["emp_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["emp_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["emp_LastupDate"]);
                    item.Email = Convert.ToString(reader["emp_Email"]);
                    item.Serial = Convert.ToInt32(reader["emp_Serial"]);
                    item.Code = Convert.ToString(reader["emp_Code"]);
                    item.Name1 = Convert.ToString(reader["emp_Name1"]);
                    item.Name2 = Convert.ToString(reader["emp_Name2"]);
                    item.Gender = Convert.ToBoolean(reader["emp_Gender"]);
                    item.BirthDate = iCore.IsDbNullRtNullDate(reader["emp_BirthDate"]);
                    item.NationalityKind = Convert.ToInt32(reader["emp_NationalityKind"]);
                    item.NationalityID = Convert.ToString(reader["emp_NationalityID"]);
                    item.Nationality = Convert.ToString(reader["emp_Nationality"]);
                    item.MaritalStatus = Convert.ToInt32(reader["emp_MaritalStatus"]);
                    item.AdultFollower = Convert.ToInt32(reader["emp_AdultFollower"]);
                    item.ChildFollower = Convert.ToInt32(reader["emp_ChildFollower"]);
                    item.PaymentType = Convert.ToInt32(reader["emp_PaymentType"]);
                    item.Bank = Convert.ToString(reader["emp_Bank"]);
                    item.IBAN = Convert.ToString(reader["emp_IBAN"]);
                    item.Biometric = iCore.IsDbNullRtNull(reader["emp_Biometric"]);
                    item.EnrollID = Convert.ToInt32(reader["emp_EnrollID"]);
                    item.Structure = iCore.IsDbNullRtNull(reader["emp_Structure"]);
                    item.Department = iCore.IsDbNullRtNull(reader["emp_Department"]);
                    item.City = iCore.IsDbNullRtNull(reader["emp_City"]);
                    item.BasicSalary = Convert.ToDecimal(reader["emp_BasicSalary"]);
                    item.AllHome = Convert.ToDecimal(reader["emp_AllHome"]);
                    item.AllTransportation = Convert.ToDecimal(reader["emp_AllTransportation"]);
                    item.AllComunication = Convert.ToDecimal(reader["emp_AllComunication"]);
                    item.AllFood = Convert.ToDecimal(reader["emp_AllFood"]);
                    item.AllOther1 = Convert.ToDecimal(reader["emp_AllOther1"]);
                    item.AllOther2 = Convert.ToDecimal(reader["emp_AllOther2"]);
                    item.AllOther3 = Convert.ToDecimal(reader["emp_AllOther3"]);
                    item.Insurance = Convert.ToBoolean(reader["emp_Insurance"]);
                    item.InsuranceNo = Convert.ToString(reader["emp_InsuranceNo"]);
                    item.InsuranceSalary = Convert.ToDecimal(reader["emp_InsuranceSalary"]);
                    item.InsuranceHome = Convert.ToDecimal(reader["emp_InsuranceHome"]);
                    item.InsuranceOther = Convert.ToDecimal(reader["emp_InsuranceOther"]);
                    item.InsuranceKind = Convert.ToBoolean(reader["emp_InsuranceKind"]);
                    item.InsuranceValue = Convert.ToDecimal(reader["emp_InsuranceValue"]);
                    item.StartDate = iCore.IsDbNullRtNullDate(reader["emp_StartDate"]);
                    item.EndDate = iCore.IsDbNullRtNullDate(reader["emp_EndDate"]);
                    item.JobKind = Convert.ToInt32(reader["emp_JobKind"]);
                    item.JobTitle = iCore.IsDbNullRtNull(reader["emp_JobTitle"]);
                    item.JobDescription = Convert.ToString(reader["emp_JobDescription"]);
                    item.DailyHour = Convert.ToDecimal(reader["emp_DailyHour"]);
                    item.Shift = Convert.ToString(reader["emp_Shift"]);
                    item.AnnualDays = Convert.ToInt32(reader["emp_AnnualDays"]);
                    item.AnnualBalance = Convert.ToDecimal(reader["emp_AnnualBalance"]);
                    item.LastReturn = iCore.IsDbNullRtNullDate(reader["emp_LastReturn"]);
                    item.ContractKind = Convert.ToInt32(reader["emp_ContractKind"]);
                    item.VacationKind = Convert.ToInt32(reader["emp_VacationKind"]);
                    item.Phone = Convert.ToString(reader["emp_Phone"]);
                    item.Mobile = Convert.ToString(reader["emp_Mobile"]);
                    item.Address = Convert.ToString(reader["emp_Address"]);
                    item.xPhone = Convert.ToString(reader["emp_xPhone"]);
                    item.xMobile = Convert.ToString(reader["emp_xMobile"]);
                    item.xAddress = Convert.ToString(reader["emp_xAddress"]);
                    item.xEmail = Convert.ToString(reader["emp_xEmail"]);
                    item.LockCard = Convert.ToBoolean(reader["emp_LockCard"]);
                    item.LockSalary = Convert.ToBoolean(reader["emp_LockSalary"]);
                    item.TicketKind = Convert.ToInt32(reader["emp_TicketKind"]);
                    item.TicketCount = Convert.ToInt32(reader["emp_TicketCount"]);
                    item.Status = Convert.ToInt32(reader["emp_Status"]);
                    item.Location = iCore.IsDbNullRtNull(reader["emp_Location"]);
                    item.Tracking = Convert.ToBoolean(reader["emp_Tracking"]);
                    item.GPSAttendance = Convert.ToBoolean(reader["emp_GPSAttendance"]);
                    item.Branch = iCore.IsDbNullRtNull(reader["emp_Branch"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["emp_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["emp_Project"]);
                    item.accIntegration = iCore.IsDbNullRtNull(reader["emp_accIntegration"]);
                    item.accAccount = iCore.IsDbNullRtNull(reader["emp_accAccount"]);
                    item.accAdvance = iCore.IsDbNullRtNull(reader["emp_accAdvance"]);
                    item.accLoan = iCore.IsDbNullRtNull(reader["emp_accLoan"]);
                    item.accEosProvision = iCore.IsDbNullRtNull(reader["emp_accEosProvision"]);
                    item.accVacProvision = iCore.IsDbNullRtNull(reader["emp_accVacProvision"]);
                    item.Image = Convert.ToString(reader["emp_Image"]);
                    item.SelfService = Convert.ToBoolean(reader["emp_SelfService"]);
                }
                reader.Close();
            }
            return item;
        }

        public static Guid? Insert(string DB, Employee item, Guid? Subscribe)
        {
            Guid? key = Guid.NewGuid();
            CLiCore.Configuration.EmployeeAccounts accounts = new CLiCore.Configuration.EmployeeAccounts().GetItem(DB);
            Guid? iAccount;
            Guid? iAdvance;
            Guid? iLoan;
            Guid? iVacProv;
            Guid? iEosProv;
            string iaccCode = xConfig.AccountCode(DB, accounts.Account);
            iAccount = CLiCore.Platx.Account.Insert(DB, accounts.Account, iaccCode, item.Name1, item.Name2);
            if (accounts.DetAdvance == true)
            {
                string code = xConfig.AccountCode(DB, accounts.Advance);
                iAdvance = CLiCore.Platx.Account.Insert(DB, accounts.Account, code, item.Name1, item.Name2);
            }
            else
            {
                iAdvance = accounts.Advance;
            }

            if (accounts.DetLoan == true)
            {
                string code = xConfig.AccountCode(DB, accounts.Loan);
                iLoan = CLiCore.Platx.Account.Insert(DB, accounts.Loan, code, item.Name1, item.Name2);
            }
            else
            {
                iLoan = accounts.Loan;
            }

            if (accounts.DetVacProvision == true)
            {
                string code = xConfig.AccountCode(DB, accounts.VacProvision);
                iVacProv = CLiCore.Platx.Account.Insert(DB, accounts.VacProvision, code, item.Name1, item.Name2);
            }
            else
            {
                iVacProv = accounts.VacProvision;
            }

            if (accounts.DetEosProvision == true)
            {
                string code = xConfig.AccountCode(DB, accounts.EosProvision);
                iEosProv = CLiCore.Platx.Account.Insert(DB, accounts.EosProvision, code, item.Name1, item.Name2);
            }
            else
            {
                iEosProv = accounts.EosProvision;
            }
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO hrCard_Employee");
                str.Append("([emp_Key]");
                str.Append(",[emp_CreateUser]");
                str.Append(",[emp_CreateDate]");
                str.Append(",[emp_LastupUser]");
                str.Append(",[emp_LastupDate]");
                str.Append(",[emp_Email]");
                str.Append(",[emp_Serial]");
                str.Append(",[emp_Code]");
                str.Append(",[emp_Name1]");
                str.Append(",[emp_Name2]");
                str.Append(",[emp_Gender]");
                str.Append(",[emp_BirthDate]");
                str.Append(",[emp_NationalityKind]");
                str.Append(",[emp_NationalityID]");
                str.Append(",[emp_Nationality]");
                str.Append(",[emp_MaritalStatus]");
                str.Append(",[emp_AdultFollower]");
                str.Append(",[emp_ChildFollower]");
                str.Append(",[emp_PaymentType]");
                str.Append(",[emp_Bank]");
                str.Append(",[emp_IBAN]");
                str.Append(",[emp_Biometric]");
                str.Append(",[emp_EnrollID]");
                str.Append(",[emp_Structure]");
                str.Append(",[emp_Department]");
                str.Append(",[emp_City]");
                str.Append(",[emp_BasicSalary]");
                str.Append(",[emp_AllHome]");
                str.Append(",[emp_AllTransportation]");
                str.Append(",[emp_AllComunication]");
                str.Append(",[emp_AllFood]");
                str.Append(",[emp_AllOther1]");
                str.Append(",[emp_AllOther2]");
                str.Append(",[emp_AllOther3]");
                str.Append(",[emp_Insurance]");
                str.Append(",[emp_InsuranceNo]");
                str.Append(",[emp_InsuranceSalary]");
                str.Append(",[emp_InsuranceHome]");
                str.Append(",[emp_InsuranceOther]");
                str.Append(",[emp_InsuranceKind]");
                str.Append(",[emp_InsuranceValue]");
                str.Append(",[emp_StartDate]");
                str.Append(",[emp_EndDate]");
                str.Append(",[emp_JobKind]");
                str.Append(",[emp_JobTitle]");
                str.Append(",[emp_JobDescription]");
                str.Append(",[emp_DailyHour]");
                str.Append(",[emp_Shift]");
                str.Append(",[emp_AnnualDays]");
                str.Append(",[emp_AnnualBalance]");
                str.Append(",[emp_LastReturn]");
                str.Append(",[emp_ContractKind]");
                str.Append(",[emp_VacationKind]");
                str.Append(",[emp_Phone]");
                str.Append(",[emp_Mobile]");
                str.Append(",[emp_Address]");
                str.Append(",[emp_xPhone]");
                str.Append(",[emp_xMobile]");
                str.Append(",[emp_xAddress]");
                str.Append(",[emp_xEmail]");
                str.Append(",[emp_LockCard]");
                str.Append(",[emp_LockSalary]");
                str.Append(",[emp_TicketKind]");
                str.Append(",[emp_TicketCount]");
                str.Append(",[emp_Status]");
                str.Append(",[emp_Location]");
                str.Append(",[emp_Tracking]");
                str.Append(",[emp_GPSAttendance]");
                str.Append(",[emp_Branch]");
                str.Append(",[emp_CostCenter]");
                str.Append(",[emp_Project]");
                str.Append(",[emp_accIntegration]");
                str.Append(",[emp_accAccount]");
                str.Append(",[emp_accAdvance]");
                str.Append(",[emp_accLoan]");
                str.Append(",[emp_accEosProvision]");
                str.Append(",[emp_accVacProvision]");
                str.Append(",[emp_SelfService]");
                str.Append(",[emp_Image])");
                str.Append(" VALUES ");
                str.Append("(@emp_Key");
                str.Append(",@emp_CreateUser");
                str.Append(",@emp_CreateDate");
                str.Append(",@emp_LastupUser");
                str.Append(",@emp_LastupDate");
                str.Append(",@emp_Email");
                str.Append(",@emp_Serial");
                str.Append(",@emp_Code");
                str.Append(",@emp_Name1");
                str.Append(",@emp_Name2");
                str.Append(",@emp_Gender");
                str.Append(",@emp_BirthDate");
                str.Append(",@emp_NationalityKind");
                str.Append(",@emp_NationalityID");
                str.Append(",@emp_Nationality");
                str.Append(",@emp_MaritalStatus");
                str.Append(",@emp_AdultFollower");
                str.Append(",@emp_ChildFollower");
                str.Append(",@emp_PaymentType");
                str.Append(",@emp_Bank");
                str.Append(",@emp_IBAN");
                str.Append(",@emp_Biometric");
                str.Append(",@emp_EnrollID");
                str.Append(",@emp_Structure");
                str.Append(",@emp_Department");
                str.Append(",@emp_City");
                str.Append(",@emp_BasicSalary");
                str.Append(",@emp_AllHome");
                str.Append(",@emp_AllTransportation");
                str.Append(",@emp_AllComunication");
                str.Append(",@emp_AllFood");
                str.Append(",@emp_AllOther1");
                str.Append(",@emp_AllOther2");
                str.Append(",@emp_AllOther3");
                str.Append(",@emp_Insurance");
                str.Append(",@emp_InsuranceNo");
                str.Append(",@emp_InsuranceSalary");
                str.Append(",@emp_InsuranceHome");
                str.Append(",@emp_InsuranceOther");
                str.Append(",@emp_InsuranceKind");
                str.Append(",@emp_InsuranceValue");
                str.Append(",@emp_StartDate");
                str.Append(",@emp_EndDate");
                str.Append(",@emp_JobKind");
                str.Append(",@emp_JobTitle");
                str.Append(",@emp_JobDescription");
                str.Append(",@emp_DailyHour");
                str.Append(",@emp_Shift");
                str.Append(",@emp_AnnualDays");
                str.Append(",@emp_AnnualBalance");
                str.Append(",@emp_LastReturn");
                str.Append(",@emp_ContractKind");
                str.Append(",@emp_VacationKind");
                str.Append(",@emp_Phone");
                str.Append(",@emp_Mobile");
                str.Append(",@emp_Address");
                str.Append(",@emp_xPhone");
                str.Append(",@emp_xMobile");
                str.Append(",@emp_xAddress");
                str.Append(",@emp_xEmail");
                str.Append(",@emp_LockCard");
                str.Append(",@emp_LockSalary");
                str.Append(",@emp_TicketKind");
                str.Append(",@emp_TicketCount");
                str.Append(",@emp_Status");
                str.Append(",@emp_Location");
                str.Append(",@emp_Tracking");
                str.Append(",@emp_GPSAttendance");
                str.Append(",@emp_Branch");
                str.Append(",@emp_CostCenter");
                str.Append(",@emp_Project");
                str.Append(",@emp_accIntegration");
                str.Append(",@emp_accAccount");
                str.Append(",@emp_accAdvance");
                str.Append(",@emp_accLoan");
                str.Append(",@emp_accEosProvision");
                str.Append(",@emp_accVacProvision");
                str.Append(",@emp_SelfService");
                str.Append(",@emp_Image)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@emp_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(key);
                comm.Parameters.Add("@emp_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@emp_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@emp_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@emp_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@emp_Email", SqlDbType.NVarChar, 500).Value = item.Email ?? "";
                comm.Parameters.Add("@emp_Serial", SqlDbType.Int).Value = item.Serial;
                comm.Parameters.Add("@emp_Code", SqlDbType.NVarChar, 25).Value = item.Code ?? "";
                comm.Parameters.Add("@emp_Name1", SqlDbType.NVarChar, 100).Value = item.Name1 ?? "";
                comm.Parameters.Add("@emp_Name2", SqlDbType.NVarChar, 100).Value = item.Name2 ?? "";
                comm.Parameters.Add("@emp_Gender", SqlDbType.Bit).Value = item.Gender;
                comm.Parameters.Add("@emp_BirthDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.BirthDate);
                comm.Parameters.Add("@emp_NationalityKind", SqlDbType.Int).Value = item.NationalityKind;
                comm.Parameters.Add("@emp_NationalityID", SqlDbType.NVarChar, 25).Value = item.NationalityID ?? "";
                comm.Parameters.Add("@emp_Nationality", SqlDbType.NVarChar, 50).Value = item.Nationality ?? "";
                comm.Parameters.Add("@emp_MaritalStatus", SqlDbType.Int).Value = item.MaritalStatus;
                comm.Parameters.Add("@emp_AdultFollower", SqlDbType.Int).Value = item.AdultFollower;
                comm.Parameters.Add("@emp_ChildFollower", SqlDbType.Int).Value = item.ChildFollower;
                comm.Parameters.Add("@emp_PaymentType", SqlDbType.Int).Value = item.PaymentType;
                comm.Parameters.Add("@emp_Bank", SqlDbType.NVarChar, 50).Value = item.Bank ?? "";
                comm.Parameters.Add("@emp_IBAN", SqlDbType.NVarChar, 28).Value = item.IBAN ?? "";
                comm.Parameters.Add("@emp_Biometric", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Biometric);
                comm.Parameters.Add("@emp_EnrollID", SqlDbType.Int).Value = item.EnrollID;
                comm.Parameters.Add("@emp_Structure", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Structure);
                comm.Parameters.Add("@emp_Department", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Department);
                comm.Parameters.Add("@emp_City", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.City);
                comm.Parameters.Add("@emp_BasicSalary", SqlDbType.Decimal).Value = item.BasicSalary;
                comm.Parameters.Add("@emp_AllHome", SqlDbType.Decimal).Value = item.AllHome;
                comm.Parameters.Add("@emp_AllTransportation", SqlDbType.Decimal).Value = item.AllTransportation;
                comm.Parameters.Add("@emp_AllComunication", SqlDbType.Decimal).Value = item.AllComunication;
                comm.Parameters.Add("@emp_AllFood", SqlDbType.Decimal).Value = item.AllFood;
                comm.Parameters.Add("@emp_AllOther1", SqlDbType.Decimal).Value = item.AllOther1;
                comm.Parameters.Add("@emp_AllOther2", SqlDbType.Decimal).Value = item.AllOther2;
                comm.Parameters.Add("@emp_AllOther3", SqlDbType.Decimal).Value = item.AllOther3;
                comm.Parameters.Add("@emp_Insurance", SqlDbType.Bit).Value = item.Insurance;
                comm.Parameters.Add("@emp_InsuranceNo", SqlDbType.NVarChar, 10).Value = item.InsuranceNo ?? "";
                comm.Parameters.Add("@emp_InsuranceSalary", SqlDbType.Decimal).Value = item.InsuranceSalary;
                comm.Parameters.Add("@emp_InsuranceHome", SqlDbType.Decimal).Value = item.InsuranceHome;
                comm.Parameters.Add("@emp_InsuranceOther", SqlDbType.Decimal).Value = item.InsuranceOther;
                comm.Parameters.Add("@emp_InsuranceKind", SqlDbType.Bit).Value = item.InsuranceKind;
                comm.Parameters.Add("@emp_InsuranceValue", SqlDbType.Decimal).Value = item.InsuranceValue;
                comm.Parameters.Add("@emp_StartDate", SqlDbType.Date).Value = item.StartDate;
                comm.Parameters.Add("@emp_EndDate", SqlDbType.Date).Value = item.EndDate;
                comm.Parameters.Add("@emp_JobKind", SqlDbType.Int).Value = item.JobKind;
                comm.Parameters.Add("@emp_JobTitle", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.JobTitle);
                comm.Parameters.Add("@emp_JobDescription", SqlDbType.NVarChar, 500).Value = item.JobDescription ?? "";
                comm.Parameters.Add("@emp_DailyHour", SqlDbType.Decimal).Value = item.DailyHour;
                comm.Parameters.Add("@emp_Shift", SqlDbType.NVarChar, 25).Value = item.Shift ?? "";
                comm.Parameters.Add("@emp_AnnualDays", SqlDbType.Int).Value = item.AnnualDays;
                comm.Parameters.Add("@emp_AnnualBalance", SqlDbType.Decimal).Value = item.AnnualBalance;
                comm.Parameters.Add("@emp_LastReturn", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastReturn);
                comm.Parameters.Add("@emp_ContractKind", SqlDbType.Int).Value = item.ContractKind;
                comm.Parameters.Add("@emp_VacationKind", SqlDbType.Int).Value = item.VacationKind;
                comm.Parameters.Add("@emp_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@emp_Mobile", SqlDbType.NVarChar, 15).Value = item.Mobile ?? "";
                comm.Parameters.Add("@emp_Address", SqlDbType.NVarChar, 200).Value = item.Address ?? "";
                comm.Parameters.Add("@emp_xPhone", SqlDbType.NVarChar, 15).Value = item.xPhone ?? "";
                comm.Parameters.Add("@emp_xMobile", SqlDbType.NVarChar, 15).Value = item.xMobile ?? "";
                comm.Parameters.Add("@emp_xAddress", SqlDbType.NVarChar, 200).Value = item.xAddress ?? "";
                comm.Parameters.Add("@emp_xEmail", SqlDbType.NVarChar, 200).Value = item.xEmail ?? "";
                comm.Parameters.Add("@emp_LockCard", SqlDbType.Bit).Value = item.LockCard;
                comm.Parameters.Add("@emp_LockSalary", SqlDbType.Bit).Value = item.LockSalary;
                comm.Parameters.Add("@emp_TicketKind", SqlDbType.Int).Value = item.TicketKind;
                comm.Parameters.Add("@emp_TicketCount", SqlDbType.Int).Value = item.TicketCount;
                comm.Parameters.Add("@emp_Status", SqlDbType.Int).Value = item.Status;
                comm.Parameters.Add("@emp_Location", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Location);
                comm.Parameters.Add("@emp_Tracking", SqlDbType.Bit).Value = item.Tracking;
                comm.Parameters.Add("@emp_GPSAttendance", SqlDbType.Bit).Value = item.GPSAttendance;
                comm.Parameters.Add("@emp_Branch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Branch);
                comm.Parameters.Add("@emp_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                comm.Parameters.Add("@emp_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                comm.Parameters.Add("@emp_accIntegration", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.accIntegration);
                comm.Parameters.Add("@emp_accAccount", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(iAccount);
                comm.Parameters.Add("@emp_accAdvance", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(iAdvance);
                comm.Parameters.Add("@emp_accLoan", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(iLoan);
                comm.Parameters.Add("@emp_accEosProvision", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(iVacProv);
                comm.Parameters.Add("@emp_accVacProvision", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(iEosProv);
                comm.Parameters.Add("@emp_SelfService", SqlDbType.Bit).Value = item.SelfService;
                comm.Parameters.Add("@emp_Image", SqlDbType.NVarChar, 500).Value = item.Image ?? "";
                con.Open();
                comm.ExecuteNonQuery();

                
            }
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                StringBuilder str = new StringBuilder();
                str.Clear();
                str.Append("INSERT INTO px_SelfServiceUsers");
                str.Append("([ssu_Employee]");
                str.Append(",[ssu_Name1]");
                str.Append(",[ssu_Name2]");
                str.Append(",[ssu_Phone]");
                str.Append(",[ssu_Email]");
                str.Append(",[ssu_Passwoard]");
                str.Append(",[ssu_Subscribe]");
                str.Append(",[ssu_DataBase]");
                str.Append(",[ssu_Disable])");
                str.Append(" VALUES ");
                str.Append("(@ssu_Employee");
                str.Append(",@ssu_Name1");
                str.Append(",@ssu_Name2");
                str.Append(",@ssu_Phone");
                str.Append(",@ssu_Email");
                str.Append(",@ssu_Passwoard");
                str.Append(",@ssu_Subscribe");
                str.Append(",@ssu_DataBase");
                str.Append(",@ssu_Disable)");

                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@ssu_Employee", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(key);
                comm.Parameters.Add("@ssu_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@ssu_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@ssu_Phone", SqlDbType.NVarChar, 15).Value = item.Mobile ?? "";
                comm.Parameters.Add("@ssu_Email", SqlDbType.NVarChar, 500).Value = item.Email ?? "";
                comm.Parameters.Add("@ssu_Passwoard", SqlDbType.NVarChar, 127).Value = "";
                comm.Parameters.Add("@ssu_Subscribe", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Subscribe);
                comm.Parameters.Add("@ssu_DataBase", SqlDbType.NVarChar, 50).Value = DB ?? "";
                comm.Parameters.Add("@ssu_Disable", SqlDbType.Bit).Value = item.SelfService;
                con.Open();
                comm.ExecuteNonQuery();
            }
            return key;
        }

        public static void Update(string DB, Employee item,Guid? Subscribe)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update hrCard_Employee SET ");
                str.Append(" [emp_LastupUser]=@emp_LastupUser");
                str.Append(",[emp_LastupDate]=@emp_LastupDate");
                str.Append(",[emp_Email]=@emp_Email");
                str.Append(",[emp_Serial]=@emp_Serial");
                str.Append(",[emp_Code]=@emp_Code");
                str.Append(",[emp_Name1]=@emp_Name1");
                str.Append(",[emp_Name2]=@emp_Name2");
                str.Append(",[emp_Gender]=@emp_Gender");
                str.Append(",[emp_BirthDate]=@emp_BirthDate");
                str.Append(",[emp_NationalityKind]=@emp_NationalityKind");
                str.Append(",[emp_NationalityID]=@emp_NationalityID");
                str.Append(",[emp_Nationality]=@emp_Nationality");
                str.Append(",[emp_MaritalStatus]=@emp_MaritalStatus");
                str.Append(",[emp_AdultFollower]=@emp_AdultFollower");
                str.Append(",[emp_ChildFollower]=@emp_ChildFollower");
                str.Append(",[emp_PaymentType]=@emp_PaymentType");
                str.Append(",[emp_Bank]=@emp_Bank");
                str.Append(",[emp_IBAN]=@emp_IBAN");
                str.Append(",[emp_Biometric]=@emp_Biometric");
                str.Append(",[emp_EnrollID]=@emp_EnrollID");
                str.Append(",[emp_Structure]=@emp_Structure");
                str.Append(",[emp_Department]=@emp_Department");
                str.Append(",[emp_City]=@emp_City");
                str.Append(",[emp_BasicSalary]=@emp_BasicSalary");
                str.Append(",[emp_AllHome]=@emp_AllHome");
                str.Append(",[emp_AllTransportation]=@emp_AllTransportation");
                str.Append(",[emp_AllComunication]=@emp_AllComunication");
                str.Append(",[emp_AllFood]=@emp_AllFood");
                str.Append(",[emp_AllOther1]=@emp_AllOther1");
                str.Append(",[emp_AllOther2]=@emp_AllOther2");
                str.Append(",[emp_AllOther3]=@emp_AllOther3");
                str.Append(",[emp_Insurance]=@emp_Insurance");
                str.Append(",[emp_InsuranceNo]=@emp_InsuranceNo");
                str.Append(",[emp_InsuranceSalary]=@emp_InsuranceSalary");
                str.Append(",[emp_InsuranceHome]=@emp_InsuranceHome");
                str.Append(",[emp_InsuranceOther]=@emp_InsuranceOther");
                str.Append(",[emp_InsuranceKind]=@emp_InsuranceKind");
                str.Append(",[emp_InsuranceValue]=@emp_InsuranceValue");
                str.Append(",[emp_StartDate]=@emp_StartDate");
                str.Append(",[emp_EndDate]=@emp_EndDate");
                str.Append(",[emp_JobKind]=@emp_JobKind");
                str.Append(",[emp_JobTitle]=@emp_JobTitle");
                str.Append(",[emp_JobDescription]=@emp_JobDescription");
                str.Append(",[emp_DailyHour]=@emp_DailyHour");
                str.Append(",[emp_Shift]=@emp_Shift");
                str.Append(",[emp_AnnualDays]=@emp_AnnualDays");
                str.Append(",[emp_AnnualBalance]=@emp_AnnualBalance");
                str.Append(",[emp_LastReturn]=@emp_LastReturn");
                str.Append(",[emp_ContractKind]=@emp_ContractKind");
                str.Append(",[emp_VacationKind]=@emp_VacationKind");
                str.Append(",[emp_Phone]=@emp_Phone");
                str.Append(",[emp_Mobile]=@emp_Mobile");
                str.Append(",[emp_Address]=@emp_Address");
                str.Append(",[emp_xPhone]=@emp_xPhone");
                str.Append(",[emp_xMobile]=@emp_xMobile");
                str.Append(",[emp_xAddress]=@emp_xAddress");
                str.Append(",[emp_xEmail]=@emp_xEmail");
                str.Append(",[emp_LockCard]=@emp_LockCard");
                str.Append(",[emp_LockSalary]=@emp_LockSalary");
                str.Append(",[emp_TicketKind]=@emp_TicketKind");
                str.Append(",[emp_TicketCount]=@emp_TicketCount");
                str.Append(",[emp_Status]=@emp_Status");
                str.Append(",[emp_Location]=@emp_Location");
                str.Append(",[emp_Tracking]=@emp_Tracking");
                str.Append(",[emp_GPSAttendance]=@emp_GPSAttendance");
                str.Append(",[emp_Branch]=@emp_Branch");
                str.Append(",[emp_CostCenter]=@emp_CostCenter");
                str.Append(",[emp_Project]=@emp_Project");
                str.Append(",[emp_SelfService]=@emp_SelfService");
                str.Append(",[emp_accIntegration]=@emp_accIntegration");
                str.Append(" WHERE emp_Key=@emp_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@emp_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@emp_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@emp_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@emp_Email", SqlDbType.NVarChar, 500).Value = item.Email ?? "";
                comm.Parameters.Add("@emp_Serial", SqlDbType.Int).Value = item.Serial;
                comm.Parameters.Add("@emp_Code", SqlDbType.NVarChar, 25).Value = item.Code ?? "";
                comm.Parameters.Add("@emp_Name1", SqlDbType.NVarChar, 100).Value = item.Name1 ?? "";
                comm.Parameters.Add("@emp_Name2", SqlDbType.NVarChar, 100).Value = item.Name2 ?? "";
                comm.Parameters.Add("@emp_Gender", SqlDbType.Bit).Value = item.Gender;
                comm.Parameters.Add("@emp_BirthDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.BirthDate);
                comm.Parameters.Add("@emp_NationalityKind", SqlDbType.Int).Value = item.NationalityKind;
                comm.Parameters.Add("@emp_NationalityID", SqlDbType.NVarChar, 25).Value = item.NationalityID ?? "";
                comm.Parameters.Add("@emp_Nationality", SqlDbType.NVarChar, 50).Value = item.Nationality ?? "";
                comm.Parameters.Add("@emp_MaritalStatus", SqlDbType.Int).Value = item.MaritalStatus;
                comm.Parameters.Add("@emp_AdultFollower", SqlDbType.Int).Value = item.AdultFollower;
                comm.Parameters.Add("@emp_ChildFollower", SqlDbType.Int).Value = item.ChildFollower;
                comm.Parameters.Add("@emp_PaymentType", SqlDbType.Int).Value = item.PaymentType;
                comm.Parameters.Add("@emp_Bank", SqlDbType.NVarChar, 50).Value = item.Bank ?? "";
                comm.Parameters.Add("@emp_IBAN", SqlDbType.NVarChar, 28).Value = item.IBAN ?? "";
                comm.Parameters.Add("@emp_Biometric", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Biometric);
                comm.Parameters.Add("@emp_EnrollID", SqlDbType.Int).Value = item.EnrollID;
                comm.Parameters.Add("@emp_Structure", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Structure);
                comm.Parameters.Add("@emp_Department", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Department);
                comm.Parameters.Add("@emp_City", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.City);
                comm.Parameters.Add("@emp_BasicSalary", SqlDbType.Decimal).Value = item.BasicSalary;
                comm.Parameters.Add("@emp_AllHome", SqlDbType.Decimal).Value = item.AllHome;
                comm.Parameters.Add("@emp_AllTransportation", SqlDbType.Decimal).Value = item.AllTransportation;
                comm.Parameters.Add("@emp_AllComunication", SqlDbType.Decimal).Value = item.AllComunication;
                comm.Parameters.Add("@emp_AllFood", SqlDbType.Decimal).Value = item.AllFood;
                comm.Parameters.Add("@emp_AllOther1", SqlDbType.Decimal).Value = item.AllOther1;
                comm.Parameters.Add("@emp_AllOther2", SqlDbType.Decimal).Value = item.AllOther2;
                comm.Parameters.Add("@emp_AllOther3", SqlDbType.Decimal).Value = item.AllOther3;
                comm.Parameters.Add("@emp_Insurance", SqlDbType.Bit).Value = item.Insurance;
                comm.Parameters.Add("@emp_InsuranceNo", SqlDbType.NVarChar, 10).Value = item.InsuranceNo ?? "";
                comm.Parameters.Add("@emp_InsuranceSalary", SqlDbType.Decimal).Value = item.InsuranceSalary;
                comm.Parameters.Add("@emp_InsuranceHome", SqlDbType.Decimal).Value = item.InsuranceHome;
                comm.Parameters.Add("@emp_InsuranceOther", SqlDbType.Decimal).Value = item.InsuranceOther;
                comm.Parameters.Add("@emp_InsuranceKind", SqlDbType.Bit).Value = item.InsuranceKind;
                comm.Parameters.Add("@emp_InsuranceValue", SqlDbType.Decimal).Value = item.InsuranceValue;
                comm.Parameters.Add("@emp_StartDate", SqlDbType.Date).Value = item.StartDate;
                comm.Parameters.Add("@emp_EndDate", SqlDbType.Date).Value = item.EndDate;
                comm.Parameters.Add("@emp_JobKind", SqlDbType.Int).Value = item.JobKind;
                comm.Parameters.Add("@emp_JobTitle", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.JobTitle);
                comm.Parameters.Add("@emp_JobDescription", SqlDbType.NVarChar, 500).Value = item.JobDescription ?? "";
                comm.Parameters.Add("@emp_DailyHour", SqlDbType.Decimal).Value = item.DailyHour;
                comm.Parameters.Add("@emp_Shift", SqlDbType.NVarChar, 25).Value = item.Shift ?? "";
                comm.Parameters.Add("@emp_AnnualDays", SqlDbType.Int).Value = item.AnnualDays;
                comm.Parameters.Add("@emp_AnnualBalance", SqlDbType.Decimal).Value = item.AnnualBalance;
                comm.Parameters.Add("@emp_LastReturn", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastReturn);
                comm.Parameters.Add("@emp_ContractKind", SqlDbType.Int).Value = item.ContractKind;
                comm.Parameters.Add("@emp_VacationKind", SqlDbType.Int).Value = item.VacationKind;
                comm.Parameters.Add("@emp_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@emp_Mobile", SqlDbType.NVarChar, 15).Value = item.Mobile ?? "";
                comm.Parameters.Add("@emp_Address", SqlDbType.NVarChar, 200).Value = item.Address ?? "";
                comm.Parameters.Add("@emp_xPhone", SqlDbType.NVarChar, 15).Value = item.xPhone ?? "";
                comm.Parameters.Add("@emp_xMobile", SqlDbType.NVarChar, 15).Value = item.xMobile ?? "";
                comm.Parameters.Add("@emp_xAddress", SqlDbType.NVarChar, 200).Value = item.xAddress ?? "";
                comm.Parameters.Add("@emp_xEmail", SqlDbType.NVarChar, 200).Value = item.xEmail ?? "";
                comm.Parameters.Add("@emp_LockCard", SqlDbType.Bit).Value = item.LockCard;
                comm.Parameters.Add("@emp_LockSalary", SqlDbType.Bit).Value = item.LockSalary;
                comm.Parameters.Add("@emp_TicketKind", SqlDbType.Int).Value = item.TicketKind;
                comm.Parameters.Add("@emp_TicketCount", SqlDbType.Int).Value = item.TicketCount;
                comm.Parameters.Add("@emp_Status", SqlDbType.Int).Value = item.Status;
                comm.Parameters.Add("@emp_Location", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Location);
                comm.Parameters.Add("@emp_Tracking", SqlDbType.Bit).Value = item.Tracking;
                comm.Parameters.Add("@emp_GPSAttendance", SqlDbType.Bit).Value = item.GPSAttendance;
                comm.Parameters.Add("@emp_Branch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Branch);
                comm.Parameters.Add("@emp_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                comm.Parameters.Add("@emp_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                comm.Parameters.Add("@emp_SelfService", SqlDbType.Bit).Value = item.SelfService;
                comm.Parameters.Add("@emp_accIntegration", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.accIntegration);
                con.Open();
                comm.ExecuteNonQuery();
            }

            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                StringBuilder str = new StringBuilder();
                str.Clear();
                str.Append("IF ((select top 100 percent count(*)  from px_SelfServiceUsers where [ssu_Employee]=@ssu_Employee)=0 ) ");
                str.Append(" BEGIN ");
                str.Append(" INSERT INTO px_SelfServiceUsers");
                str.Append("([ssu_Employee]");
                str.Append(",[ssu_Name1]");
                str.Append(",[ssu_Name2]");
                str.Append(",[ssu_Phone]");
                str.Append(",[ssu_Email]");
                str.Append(",[ssu_Passwoard]");
                str.Append(",[ssu_Subscribe]");
                str.Append(",[ssu_DataBase]");
                str.Append(",[ssu_Disable])");
                str.Append(" VALUES ");
                str.Append("(@ssu_Employee");
                str.Append(",@ssu_Name1");
                str.Append(",@ssu_Name2");
                str.Append(",@ssu_Phone");
                str.Append(",@ssu_Email");
                str.Append(",@ssu_Passwoard");
                str.Append(",@ssu_Subscribe");
                str.Append(",@ssu_DataBase");
                str.Append(",@ssu_Disable)");
                str.Append(" END ");
                str.Append("IF ((select top 100 percent count(*)  from px_SelfServiceUsers where [ssu_Employee]=@ssu_Employee)>0 ) ");
                str.Append(" BEGIN ");
                str.Append(" Update px_SelfServiceUsers SET ");
                str.Append(" [ssu_Name1]=@ssu_Name1");
                str.Append(",[ssu_Name2]=@ssu_Name2");
                str.Append(",[ssu_Phone]=@ssu_Phone");
                str.Append(",[ssu_Email]=@ssu_Email");
                str.Append(",[ssu_Disable]=@ssu_Disable");
                str.Append(" WHERE ssu_Employee=@ssu_Employee");
                str.Append(" END ");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@ssu_Employee", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@ssu_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@ssu_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@ssu_Phone", SqlDbType.NVarChar, 15).Value = item.Mobile ?? "";
                comm.Parameters.Add("@ssu_Email", SqlDbType.NVarChar, 500).Value = item.Email ?? "";
                comm.Parameters.Add("@ssu_Passwoard", SqlDbType.NVarChar, 127).Value = "";
                comm.Parameters.Add("@ssu_Subscribe", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Subscribe);
                comm.Parameters.Add("@ssu_DataBase", SqlDbType.NVarChar, 50).Value = DB ?? "";
                comm.Parameters.Add("@ssu_Disable", SqlDbType.Bit).Value = item.SelfService;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [hrCard_Employee] where [emp_Key]=@Key";
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
        public static int UpdatePassword(Guid? Key,string Password)
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
                comm.Parameters.Add("@ssu_Passwoard", SqlDbType.NVarChar, 127).Value = Password??"";
 
                con.Open();
                res= comm.ExecuteNonQuery();
            }
            return res;
        }

    }
}
