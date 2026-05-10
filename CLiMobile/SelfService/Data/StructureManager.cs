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
    public class StructureManager
    {
        public Guid? Key { get; set; }
        public Guid? Parent { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public int Kind { get; set; }
        public int Level { get; set; }
        public Boolean FullStructure { get; set; }
        public Boolean FinalConfirmation { get; set; }
        public Boolean cfmAdvance { get; set; }
        public Boolean cfmLoan { get; set; }
        public Boolean cfmHourLeave { get; set; }
        public Boolean cfmHourLeaveFinal { get; set; }
        public Boolean cfmAnnualLeave { get; set; }
        public Boolean cfmNormalLeave { get; set; }
        public Boolean cfmReward { get; set; }
        public Boolean cfmPenalty { get; set; }
        public Boolean RewardEdit { get; set; }
        public Boolean PenaltyEdit { get; set; }
        public Boolean HRDepartment { get; set; }
        public Boolean FinancialDepartment { get; set; }
        public Boolean AdminDepartment { get; set; }
        public int CloseDays { get; set; }
        public decimal CloseAdvance { get; set; }
        public Boolean StopAdvance { get; set; }
        public Boolean StopLoan { get; set; }
        public Boolean StopAnnualLeave { get; set; }
        public StructureManager GetItem(string DB,Guid? Key)
        {
            StructureManager item = new StructureManager();
            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_StructureManager(@Key) ";
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
                    item.Parent = iCore.IsDbNullRtNull(reader["Parent"]);
                    item.No = Convert.ToInt32(reader["No"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Level = Convert.ToInt32(reader["Level"]);
                    item.FullStructure = Convert.ToBoolean(reader["FullStructure"]);
                    item.FinalConfirmation = Convert.ToBoolean(reader["FinalConfirmation"]);
                    item.cfmAdvance = Convert.ToBoolean(reader["cfmAdvance"]);
                    item.cfmLoan = Convert.ToBoolean(reader["cfmLoan"]);
                    item.cfmHourLeave = Convert.ToBoolean(reader["cfmHourLeave"]);
                    item.cfmHourLeaveFinal = Convert.ToBoolean(reader["cfmHourLeaveFinal"]);
                    item.cfmAnnualLeave = Convert.ToBoolean(reader["cfmAnnualLeave"]);
                    item.cfmNormalLeave = Convert.ToBoolean(reader["cfmNormalLeave"]);
                    item.cfmReward = Convert.ToBoolean(reader["cfmReward"]);
                    item.cfmPenalty = Convert.ToBoolean(reader["cfmPenalty"]);
                    item.RewardEdit = Convert.ToBoolean(reader["RewardEdit"]);
                    item.PenaltyEdit = Convert.ToBoolean(reader["PenaltyEdit"]);
                    item.HRDepartment = Convert.ToBoolean(reader["HRDepartment"]);
                    item.FinancialDepartment = Convert.ToBoolean(reader["FinancialDepartment"]);
                    item.AdminDepartment = Convert.ToBoolean(reader["AdminDepartment"]);
                    item.CloseDays = Convert.ToInt32(reader["CloseDays"]);
                    item.CloseAdvance = Convert.ToDecimal(reader["CloseAdvance"]);
                    item.StopAdvance = Convert.ToBoolean(reader["StopAdvance"]);
                    item.StopLoan = Convert.ToBoolean(reader["StopLoan"]);
                    item.StopAnnualLeave = Convert.ToBoolean(reader["StopAnnualLeave"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
