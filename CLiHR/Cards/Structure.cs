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
    public class Structure
    {
        public Guid? Key { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public Guid? Parent { get; set; }
        public int No { get; set; }
        public string Name2 { get; set; }
        public string Name1 { get; set; }
        public string Description { get; set; }
        public int Kind { get; set; }
        public int Level { get; set; }
        public int Order { get; set; }
        public int Max { get; set; }
        public int Min { get; set; }
        public int Target { get; set; }
        public Boolean FullStructre { get; set; }
        public Boolean FinalConfirmation { get; set; }
        public Boolean ConfirmAdvance { get; set; }
        public Boolean ConfirmLoan { get; set; }
        public Boolean ConfirmHourLeave { get; set; }
        public Boolean ConfirmHourLeaveFinal { get; set; }
        public Boolean ConfirmAnnualLeave { get; set; }
        public Boolean ConfirmNormalLeave { get; set; }
        public Boolean ConfirmReward { get; set; }
        public Boolean ConfirmPenalty { get; set; }
        public Boolean RewardEdit { get; set; }
        public Boolean PenaltyEdit { get; set; }
        public Boolean HRDepartment { get; set; }
        public Boolean FinancialDepartment { get; set; }
        public Boolean AdministrativeDepartment { get; set; }
        public int CloseDays { get; set; }
        public decimal CloseAdvance { get; set; }
        public Guid? Manager { get; set; }
        public Boolean StopAdvance { get; set; }
        public Boolean StopLeave { get; set; }
        public Boolean StopAnnualLeave { get; set; }
        public Boolean Disable { get; set; }
        public List<Structure> GetList(string DB)
        {
            List<Structure> items = new List<Structure>();
            string selQuery = "select top 100 percent * from HRStructure_Organizational order by [str_Level],[str_Order] ";
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
                    Structure item = new Structure();
                    item.Key = iCore.IsDbNullRtNull(reader["str_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["str_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["str_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["str_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["str_LastupDate"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["str_Parent"]);
                    item.No = Convert.ToInt32(reader["str_No"]);
                    item.Name2 = Convert.ToString(reader["str_Name2"]);
                    item.Name1 = Convert.ToString(reader["str_Name1"]);
                    item.Description = Convert.ToString(reader["str_Description"]);
                    item.Kind = Convert.ToInt32(reader["str_Kind"]);
                    item.Level = Convert.ToInt32(reader["str_Level"]);
                    item.Order = Convert.ToInt32(reader["str_Order"]);
                    item.Max = Convert.ToInt32(reader["str_Max"]);
                    item.Min = Convert.ToInt32(reader["str_Min"]);
                    item.Target = Convert.ToInt32(reader["str_Target"]);
                    item.FullStructre = Convert.ToBoolean(reader["str_FullStructre"]);
                    item.FinalConfirmation = Convert.ToBoolean(reader["str_FinalConfirmation"]);
                    item.ConfirmAdvance = Convert.ToBoolean(reader["str_ConfirmAdvance"]);
                    item.ConfirmLoan = Convert.ToBoolean(reader["str_ConfirmLoan"]);
                    item.ConfirmHourLeave = Convert.ToBoolean(reader["str_ConfirmHourLeave"]);
                    item.ConfirmHourLeaveFinal = Convert.ToBoolean(reader["str_ConfirmHourLeaveFinal"]);
                    item.ConfirmAnnualLeave = Convert.ToBoolean(reader["str_ConfirmAnnualLeave"]);
                    item.ConfirmNormalLeave = Convert.ToBoolean(reader["str_ConfirmNormalLeave"]);
                    item.ConfirmReward = Convert.ToBoolean(reader["str_ConfirmReward"]);
                    item.ConfirmPenalty = Convert.ToBoolean(reader["str_ConfirmPenalty"]);
                    item.RewardEdit = Convert.ToBoolean(reader["str_RewardEdit"]);
                    item.PenaltyEdit = Convert.ToBoolean(reader["str_PenaltyEdit"]);
                    item.HRDepartment = Convert.ToBoolean(reader["str_HRDepartment"]);
                    item.FinancialDepartment = Convert.ToBoolean(reader["str_FinancialDepartment"]);
                    item.AdministrativeDepartment = Convert.ToBoolean(reader["str_AdministrativeDepartment"]);
                    item.CloseDays = Convert.ToInt32(reader["str_CloseDays"]);
                    item.CloseAdvance = Convert.ToDecimal(reader["str_CloseAdvance"]);
                    item.Manager = iCore.IsDbNullRtNull(reader["str_Manager"]);
                    item.StopAdvance = Convert.ToBoolean(reader["str_StopAdvance"]);
                    item.StopLeave = Convert.ToBoolean(reader["str_StopLeave"]);
                    item.StopAnnualLeave = Convert.ToBoolean(reader["str_StopAnnualLeave"]);
                    item.Disable = Convert.ToBoolean(reader["str_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public Structure GetItem(string DB,Guid? Key)
        {
            Structure item = new Structure();
            item.No = MaxNo(DB);
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from HRStructure_Organizational where [str_Key]=@Key";
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
                    item.Key = iCore.IsDbNullRtNull(reader["str_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["str_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["str_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["str_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["str_LastupDate"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["str_Parent"]);
                    item.No = Convert.ToInt32(reader["str_No"]);
                    item.Name2 = Convert.ToString(reader["str_Name2"]);
                    item.Name1 = Convert.ToString(reader["str_Name1"]);
                    item.Description = Convert.ToString(reader["str_Description"]);
                    item.Kind = Convert.ToInt32(reader["str_Kind"]);
                    item.Level = Convert.ToInt32(reader["str_Level"]);
                    item.Order = Convert.ToInt32(reader["str_Order"]);
                    item.Max = Convert.ToInt32(reader["str_Max"]);
                    item.Min = Convert.ToInt32(reader["str_Min"]);
                    item.Target = Convert.ToInt32(reader["str_Target"]);
                    item.FullStructre = Convert.ToBoolean(reader["str_FullStructre"]);
                    item.FinalConfirmation = Convert.ToBoolean(reader["str_FinalConfirmation"]);
                    item.ConfirmAdvance = Convert.ToBoolean(reader["str_ConfirmAdvance"]);
                    item.ConfirmLoan = Convert.ToBoolean(reader["str_ConfirmLoan"]);
                    item.ConfirmHourLeave = Convert.ToBoolean(reader["str_ConfirmHourLeave"]);
                    item.ConfirmHourLeaveFinal = Convert.ToBoolean(reader["str_ConfirmHourLeaveFinal"]);
                    item.ConfirmAnnualLeave = Convert.ToBoolean(reader["str_ConfirmAnnualLeave"]);
                    item.ConfirmNormalLeave = Convert.ToBoolean(reader["str_ConfirmNormalLeave"]);
                    item.ConfirmReward = Convert.ToBoolean(reader["str_ConfirmReward"]);
                    item.ConfirmPenalty = Convert.ToBoolean(reader["str_ConfirmPenalty"]);
                    item.RewardEdit = Convert.ToBoolean(reader["str_RewardEdit"]);
                    item.PenaltyEdit = Convert.ToBoolean(reader["str_PenaltyEdit"]);
                    item.HRDepartment = Convert.ToBoolean(reader["str_HRDepartment"]);
                    item.FinancialDepartment = Convert.ToBoolean(reader["str_FinancialDepartment"]);
                    item.AdministrativeDepartment = Convert.ToBoolean(reader["str_AdministrativeDepartment"]);
                    item.CloseDays = Convert.ToInt32(reader["str_CloseDays"]);
                    item.CloseAdvance = Convert.ToDecimal(reader["str_CloseAdvance"]);
                    item.Manager = iCore.IsDbNullRtNull(reader["str_Manager"]);
                    item.StopAdvance = Convert.ToBoolean(reader["str_StopAdvance"]);
                    item.StopLeave = Convert.ToBoolean(reader["str_StopLeave"]);
                    item.StopAnnualLeave = Convert.ToBoolean(reader["str_StopAnnualLeave"]);
                    item.Disable = Convert.ToBoolean(reader["str_Disable"]);
                }
                reader.Close();
            }
            return item;
        }
        public static void Insert(string DB, Structure item)
        {
            Guid? key= Guid.NewGuid();
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO HRStructure_Organizational");
                str.Append("([str_Key]");
                str.Append(",[str_CreateUser]");
                str.Append(",[str_CreateDate]");
                str.Append(",[str_LastupUser]");
                str.Append(",[str_LastupDate]");
                str.Append(",[str_Parent]");
                str.Append(",[str_No]");
                str.Append(",[str_Name2]");
                str.Append(",[str_Name1]");
                str.Append(",[str_Description]");
                str.Append(",[str_Kind]");
                str.Append(",[str_Level]");
                str.Append(",[str_Order]");
                str.Append(",[str_Max]");
                str.Append(",[str_Min]");
                str.Append(",[str_Target]");
                str.Append(",[str_FullStructre]");
                str.Append(",[str_FinalConfirmation]");
                str.Append(",[str_ConfirmAdvance]");
                str.Append(",[str_ConfirmLoan]");
                str.Append(",[str_ConfirmHourLeave]");
                str.Append(",[str_ConfirmHourLeaveFinal]");
                str.Append(",[str_ConfirmAnnualLeave]");
                str.Append(",[str_ConfirmNormalLeave]");
                str.Append(",[str_ConfirmReward]");
                str.Append(",[str_ConfirmPenalty]");
                str.Append(",[str_RewardEdit]");
                str.Append(",[str_PenaltyEdit]");
                str.Append(",[str_HRDepartment]");
                str.Append(",[str_FinancialDepartment]");
                str.Append(",[str_AdministrativeDepartment]");
                str.Append(",[str_CloseDays]");
                str.Append(",[str_CloseAdvance]");
                str.Append(",[str_Manager]");
                str.Append(",[str_StopAdvance]");
                str.Append(",[str_StopLeave]");
                str.Append(",[str_StopAnnualLeave]");
                str.Append(",[str_Disable])");
                str.Append(" VALUES ");
                str.Append("(@str_Key");
                str.Append(",@str_CreateUser");
                str.Append(",@str_CreateDate");
                str.Append(",@str_LastupUser");
                str.Append(",@str_LastupDate");
                str.Append(",@str_Parent");
                str.Append(",@str_No");
                str.Append(",@str_Name2");
                str.Append(",@str_Name1");
                str.Append(",@str_Description");
                str.Append(",@str_Kind");
                str.Append(",@str_Level");
                str.Append(",@str_Order");
                str.Append(",@str_Max");
                str.Append(",@str_Min");
                str.Append(",@str_Target");
                str.Append(",@str_FullStructre");
                str.Append(",@str_FinalConfirmation");
                str.Append(",@str_ConfirmAdvance");
                str.Append(",@str_ConfirmLoan");
                str.Append(",@str_ConfirmHourLeave");
                str.Append(",@str_ConfirmHourLeaveFinal");
                str.Append(",@str_ConfirmAnnualLeave");
                str.Append(",@str_ConfirmNormalLeave");
                str.Append(",@str_ConfirmReward");
                str.Append(",@str_ConfirmPenalty");
                str.Append(",@str_RewardEdit");
                str.Append(",@str_PenaltyEdit");
                str.Append(",@str_HRDepartment");
                str.Append(",@str_FinancialDepartment");
                str.Append(",@str_AdministrativeDepartment");
                str.Append(",@str_CloseDays");
                str.Append(",@str_CloseAdvance");
                str.Append(",@str_Manager");
                str.Append(",@str_StopAdvance");
                str.Append(",@str_StopLeave");
                str.Append(",@str_StopAnnualLeave");
                str.Append(",@str_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@str_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(key);
                comm.Parameters.Add("@str_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@str_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@str_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@str_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@str_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Parent);
                comm.Parameters.Add("@str_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@str_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@str_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@str_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                comm.Parameters.Add("@str_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@str_Level", SqlDbType.Int).Value = StructureLevel(DB, item.Parent);
                comm.Parameters.Add("@str_Order", SqlDbType.Int).Value = StructureOrder(DB, item.Parent);
                comm.Parameters.Add("@str_Max", SqlDbType.Int).Value = item.Max;
                comm.Parameters.Add("@str_Min", SqlDbType.Int).Value = item.Min;
                comm.Parameters.Add("@str_Target", SqlDbType.Int).Value = item.Target;
                comm.Parameters.Add("@str_FullStructre", SqlDbType.Bit).Value = item.FullStructre;
                comm.Parameters.Add("@str_FinalConfirmation", SqlDbType.Bit).Value = item.FinalConfirmation;
                comm.Parameters.Add("@str_ConfirmAdvance", SqlDbType.Bit).Value = item.ConfirmAdvance;
                comm.Parameters.Add("@str_ConfirmLoan", SqlDbType.Bit).Value = item.ConfirmLoan;
                comm.Parameters.Add("@str_ConfirmHourLeave", SqlDbType.Bit).Value = item.ConfirmHourLeave;
                comm.Parameters.Add("@str_ConfirmHourLeaveFinal", SqlDbType.Bit).Value = item.ConfirmHourLeaveFinal;
                comm.Parameters.Add("@str_ConfirmAnnualLeave", SqlDbType.Bit).Value = item.ConfirmAnnualLeave;
                comm.Parameters.Add("@str_ConfirmNormalLeave", SqlDbType.Bit).Value = item.ConfirmNormalLeave;
                comm.Parameters.Add("@str_ConfirmReward", SqlDbType.Bit).Value = item.ConfirmReward;
                comm.Parameters.Add("@str_ConfirmPenalty", SqlDbType.Bit).Value = item.ConfirmPenalty;
                comm.Parameters.Add("@str_RewardEdit", SqlDbType.Bit).Value = item.RewardEdit;
                comm.Parameters.Add("@str_PenaltyEdit", SqlDbType.Bit).Value = item.PenaltyEdit;
                comm.Parameters.Add("@str_HRDepartment", SqlDbType.Bit).Value = item.HRDepartment;
                comm.Parameters.Add("@str_FinancialDepartment", SqlDbType.Bit).Value = item.FinancialDepartment;
                comm.Parameters.Add("@str_AdministrativeDepartment", SqlDbType.Bit).Value = item.AdministrativeDepartment;
                comm.Parameters.Add("@str_CloseDays", SqlDbType.Int).Value = item.CloseDays;
                comm.Parameters.Add("@str_CloseAdvance", SqlDbType.Decimal).Value = item.CloseAdvance;
                comm.Parameters.Add("@str_Manager", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Manager);
                comm.Parameters.Add("@str_StopAdvance", SqlDbType.Bit).Value = item.StopAdvance;
                comm.Parameters.Add("@str_StopLeave", SqlDbType.Bit).Value = item.StopLeave;
                comm.Parameters.Add("@str_StopAnnualLeave", SqlDbType.Bit).Value = item.StopAnnualLeave;
                comm.Parameters.Add("@str_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();

                str.Clear();
                str.Append("Update hrCard_Employee set ");
                str.Append("[emp_Structure]=@emp_Structure");
                str.Append(" WHERE emp_Key=@emp_Key");
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@emp_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Manager);
                comm.Parameters.Add("@emp_Structure", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(key);
                comm.ExecuteNonQuery();
            }
        }
        public static void Update(string DB, Structure item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update HRStructure_Organizational set ");
                str.Append("[str_CreateUser]=@str_CreateUser");
                str.Append(",[str_CreateDate]=@str_CreateDate");
                str.Append(",[str_LastupUser]=@str_LastupUser");
                str.Append(",[str_LastupDate]=@str_LastupDate");
                str.Append(",[str_Parent]=@str_Parent");
                str.Append(",[str_No]=@str_No");
                str.Append(",[str_Name2]=@str_Name2");
                str.Append(",[str_Name1]=@str_Name1");
                str.Append(",[str_Description]=@str_Description");
                str.Append(",[str_Kind]=@str_Kind");
                str.Append(",[str_Level]=@str_Level");
                str.Append(",[str_Order]=@str_Order");
                str.Append(",[str_Max]=@str_Max");
                str.Append(",[str_Min]=@str_Min");
                str.Append(",[str_Target]=@str_Target");
                str.Append(",[str_FullStructre]=@str_FullStructre");
                str.Append(",[str_FinalConfirmation]=@str_FinalConfirmation");
                str.Append(",[str_ConfirmAdvance]=@str_ConfirmAdvance");
                str.Append(",[str_ConfirmLoan]=@str_ConfirmLoan");
                str.Append(",[str_ConfirmHourLeave]=@str_ConfirmHourLeave");
                str.Append(",[str_ConfirmHourLeaveFinal]=@str_ConfirmHourLeaveFinal");
                str.Append(",[str_ConfirmAnnualLeave]=@str_ConfirmAnnualLeave");
                str.Append(",[str_ConfirmNormalLeave]=@str_ConfirmNormalLeave");
                str.Append(",[str_ConfirmReward]=@str_ConfirmReward");
                str.Append(",[str_ConfirmPenalty]=@str_ConfirmPenalty");
                str.Append(",[str_RewardEdit]=@str_RewardEdit");
                str.Append(",[str_PenaltyEdit]=@str_PenaltyEdit");
                str.Append(",[str_HRDepartment]=@str_HRDepartment");
                str.Append(",[str_FinancialDepartment]=@str_FinancialDepartment");
                str.Append(",[str_AdministrativeDepartment]=@str_AdministrativeDepartment");
                str.Append(",[str_CloseDays]=@str_CloseDays");
                str.Append(",[str_CloseAdvance]=@str_CloseAdvance");
                str.Append(",[str_Manager]=@str_Manager");
                str.Append(",[str_StopAdvance]=@str_StopAdvance");
                str.Append(",[str_StopLeave]=@str_StopLeave");
                str.Append(",[str_StopAnnualLeave]=@str_StopAnnualLeave");
                str.Append(",[str_Disable]=@str_Disable");
                str.Append(" WHERE str_Key=@str_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@str_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@str_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@str_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@str_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@str_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@str_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Parent);
                comm.Parameters.Add("@str_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@str_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@str_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@str_Description", SqlDbType.NVarChar, 500).Value = item.Description ?? "";
                comm.Parameters.Add("@str_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@str_Level", SqlDbType.Int).Value = StructureLevel(DB, item.Parent);
                comm.Parameters.Add("@str_Order", SqlDbType.Int).Value = StructureOrder(DB, item.Parent);
                comm.Parameters.Add("@str_Max", SqlDbType.Int).Value = item.Max;
                comm.Parameters.Add("@str_Min", SqlDbType.Int).Value = item.Min;
                comm.Parameters.Add("@str_Target", SqlDbType.Int).Value = item.Target;
                comm.Parameters.Add("@str_FullStructre", SqlDbType.Bit).Value = item.FullStructre;
                comm.Parameters.Add("@str_FinalConfirmation", SqlDbType.Bit).Value = item.FinalConfirmation;
                comm.Parameters.Add("@str_ConfirmAdvance", SqlDbType.Bit).Value = item.ConfirmAdvance;
                comm.Parameters.Add("@str_ConfirmLoan", SqlDbType.Bit).Value = item.ConfirmLoan;
                comm.Parameters.Add("@str_ConfirmHourLeave", SqlDbType.Bit).Value = item.ConfirmHourLeave;
                comm.Parameters.Add("@str_ConfirmHourLeaveFinal", SqlDbType.Bit).Value = item.ConfirmHourLeaveFinal;
                comm.Parameters.Add("@str_ConfirmAnnualLeave", SqlDbType.Bit).Value = item.ConfirmAnnualLeave;
                comm.Parameters.Add("@str_ConfirmNormalLeave", SqlDbType.Bit).Value = item.ConfirmNormalLeave;
                comm.Parameters.Add("@str_ConfirmReward", SqlDbType.Bit).Value = item.ConfirmReward;
                comm.Parameters.Add("@str_ConfirmPenalty", SqlDbType.Bit).Value = item.ConfirmPenalty;
                comm.Parameters.Add("@str_RewardEdit", SqlDbType.Bit).Value = item.RewardEdit;
                comm.Parameters.Add("@str_PenaltyEdit", SqlDbType.Bit).Value = item.PenaltyEdit;
                comm.Parameters.Add("@str_HRDepartment", SqlDbType.Bit).Value = item.HRDepartment;
                comm.Parameters.Add("@str_FinancialDepartment", SqlDbType.Bit).Value = item.FinancialDepartment;
                comm.Parameters.Add("@str_AdministrativeDepartment", SqlDbType.Bit).Value = item.AdministrativeDepartment;
                comm.Parameters.Add("@str_CloseDays", SqlDbType.Int).Value = item.CloseDays;
                comm.Parameters.Add("@str_CloseAdvance", SqlDbType.Decimal).Value = item.CloseAdvance;
                comm.Parameters.Add("@str_Manager", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Manager);
                comm.Parameters.Add("@str_StopAdvance", SqlDbType.Bit).Value = item.StopAdvance;
                comm.Parameters.Add("@str_StopLeave", SqlDbType.Bit).Value = item.StopLeave;
                comm.Parameters.Add("@str_StopAnnualLeave", SqlDbType.Bit).Value = item.StopAnnualLeave;
                comm.Parameters.Add("@str_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();

                str.Clear();
                str.Append("Update hrCard_Employee set ");
                str.Append("[emp_Structure]=@emp_Structure");
                str.Append(" WHERE emp_Key=@emp_Key");
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@emp_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Manager);
                comm.Parameters.Add("@emp_Structure", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.ExecuteNonQuery();
            }
        }
        private int MaxNo(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([str_No])+1,1) from [HRStructure_Organizational]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand comm = new SqlCommand();
                comm.CommandText = selQuery;
                comm.CommandType = CommandType.Text;
                comm.Connection = con;
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    res = (int)reader[0];
                }
                reader.Close();
            }
            return res;
        }
        private static  int StructureLevel(string DB, Guid? Key)
        {
            if (Key == null)
                return 1;
            int Res = 0;
            string selQuery = "select top 100 percent isnull(str_Level,1) from HRStructure_Organizational  where [str_Key]=@Key ";
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
                    Res = Convert.ToInt32(reader[0]) + 1;
                }
                reader.Close();
            }
            return Res;
        }
        private static  int StructureOrder(string DB, Guid? Key)
        {
            if (Key == null)
                return 1;
            int Res = 0;
            string selQuery = "select top 100 percent count(str_Key) + 1 from HRStructure_Organizational  where str_Parent=@Key ";
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
                    Res = Convert.ToInt32(reader[0]);
                }
                reader.Close();
            }
            return Res;
        }
        public static int Delete(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [HRStructure_Organizational] where [str_Key]=@Key";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
               res= comm.ExecuteNonQuery();
            }
            return res;
        }

    }
}
