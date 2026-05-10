using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Cards
{
    public class SalariesIntegration
    {
        public Guid? Key { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Guid? BasicSalary { get; set; }
        public Guid? Overtiem { get; set; }
        public Guid? Reward { get; set; }
        public Guid? Bonus { get; set; }
        public Guid? Commission { get; set; }
        public Guid? Tickets { get; set; }
        public Guid? AnnualVacation { get; set; }
        public Guid? EndService { get; set; }
        public Guid? Benefit1 { get; set; }
        public Guid? Benefit2 { get; set; }
        public Guid? Benefit3 { get; set; }
        public Guid? AllHome { get; set; }
        public Guid? AllTransportation { get; set; }
        public Guid? AllComunication { get; set; }
        public Guid? AllFood { get; set; }
        public Guid? AllOther { get; set; }
        public Guid? Insurance { get; set; }
        public Guid? Late { get; set; }
        public Guid? Absence { get; set; }
        public Guid? Penalty { get; set; }
        public Guid? Deduction1 { get; set; }
        public Guid? Deduction2 { get; set; }
        public Guid? Deduction3 { get; set; }


        public List<SalariesIntegration> GetList(string DB)
        {
            List<SalariesIntegration> items = new List<SalariesIntegration>();
            string selQuery = "select top 100 percent * from finCard_SalariesIntegration order by [SI_Code]";
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
                    SalariesIntegration item = new SalariesIntegration();
                    item.Key = iCore.IsDbNullRtNull(reader["SI_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["SI_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["SI_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["SI_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["SI_LastupDate"]);
                    item.Code = Convert.ToString(reader["SI_Code"]);
                    item.Name1 = Convert.ToString(reader["SI_Name1"]);
                    item.Name2 = Convert.ToString(reader["SI_Name2"]);
                    item.BasicSalary = iCore.IsDbNullRtNull(reader["SI_BasicSalary"]);
                    item.Overtiem = iCore.IsDbNullRtNull(reader["SI_Overtiem"]);
                    item.Reward = iCore.IsDbNullRtNull(reader["SI_Reward"]);
                    item.Bonus = iCore.IsDbNullRtNull(reader["SI_Bonus"]);
                    item.Commission = iCore.IsDbNullRtNull(reader["SI_Commission"]);
                    item.Tickets = iCore.IsDbNullRtNull(reader["SI_Tickets"]);
                    item.AnnualVacation = iCore.IsDbNullRtNull(reader["SI_AnnualVacation"]);
                    item.EndService = iCore.IsDbNullRtNull(reader["SI_EndService"]);
                    item.Benefit1 = iCore.IsDbNullRtNull(reader["SI_Benefit1"]);
                    item.Benefit2 = iCore.IsDbNullRtNull(reader["SI_Benefit2"]);
                    item.Benefit3 = iCore.IsDbNullRtNull(reader["SI_Benefit3"]);
                    item.AllHome = iCore.IsDbNullRtNull(reader["SI_AllHome"]);
                    item.AllTransportation = iCore.IsDbNullRtNull(reader["SI_AllTransportation"]);
                    item.AllComunication = iCore.IsDbNullRtNull(reader["SI_AllComunication"]);
                    item.AllFood = iCore.IsDbNullRtNull(reader["SI_AllFood"]);
                    item.AllOther = iCore.IsDbNullRtNull(reader["SI_AllOther"]);
                    item.Insurance = iCore.IsDbNullRtNull(reader["SI_Insurance"]);
                    item.Late = iCore.IsDbNullRtNull(reader["SI_Late"]);
                    item.Absence = iCore.IsDbNullRtNull(reader["SI_Absence"]);
                    item.Penalty = iCore.IsDbNullRtNull(reader["SI_Penalty"]);
                    item.Deduction1 = iCore.IsDbNullRtNull(reader["SC_Deduction1"]);
                    item.Deduction2 = iCore.IsDbNullRtNull(reader["SC_Deduction2"]);
                    item.Deduction3 = iCore.IsDbNullRtNull(reader["SC_Deduction3"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public SalariesIntegration GetItem(string DB, Guid? Key)
        {
            SalariesIntegration item = new SalariesIntegration();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from finCard_SalariesIntegration";
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
                    item.Key = iCore.IsDbNullRtNull(reader["SI_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["SI_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["SI_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["SI_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["SI_LastupDate"]);
                    item.Code = Convert.ToString(reader["SI_Code"]);
                    item.Name1 = Convert.ToString(reader["SI_Name1"]);
                    item.Name2 = Convert.ToString(reader["SI_Name2"]);
                    item.BasicSalary = iCore.IsDbNullRtNull(reader["SI_BasicSalary"]);
                    item.Overtiem = iCore.IsDbNullRtNull(reader["SI_Overtiem"]);
                    item.Reward = iCore.IsDbNullRtNull(reader["SI_Reward"]);
                    item.Bonus = iCore.IsDbNullRtNull(reader["SI_Bonus"]);
                    item.Commission = iCore.IsDbNullRtNull(reader["SI_Commission"]);
                    item.Tickets = iCore.IsDbNullRtNull(reader["SI_Tickets"]);
                    item.AnnualVacation = iCore.IsDbNullRtNull(reader["SI_AnnualVacation"]);
                    item.EndService = iCore.IsDbNullRtNull(reader["SI_EndService"]);
                    item.Benefit1 = iCore.IsDbNullRtNull(reader["SI_Benefit1"]);
                    item.Benefit2 = iCore.IsDbNullRtNull(reader["SI_Benefit2"]);
                    item.Benefit3 = iCore.IsDbNullRtNull(reader["SI_Benefit3"]);
                    item.AllHome = iCore.IsDbNullRtNull(reader["SI_AllHome"]);
                    item.AllTransportation = iCore.IsDbNullRtNull(reader["SI_AllTransportation"]);
                    item.AllComunication = iCore.IsDbNullRtNull(reader["SI_AllComunication"]);
                    item.AllFood = iCore.IsDbNullRtNull(reader["SI_AllFood"]);
                    item.AllOther = iCore.IsDbNullRtNull(reader["SI_AllOther"]);
                    item.Insurance = iCore.IsDbNullRtNull(reader["SI_Insurance"]);
                    item.Late = iCore.IsDbNullRtNull(reader["SI_Late"]);
                    item.Absence = iCore.IsDbNullRtNull(reader["SI_Absence"]);
                    item.Penalty = iCore.IsDbNullRtNull(reader["SI_Penalty"]);
                    item.Deduction1 = iCore.IsDbNullRtNull(reader["SC_Deduction1"]);
                    item.Deduction2 = iCore.IsDbNullRtNull(reader["SC_Deduction2"]);
                    item.Deduction3 = iCore.IsDbNullRtNull(reader["SC_Deduction3"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, SalariesIntegration item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO finCard_SalariesIntegration");
                str.Append("([SI_CreateUser]");
                str.Append(",[SI_CreateDate]");
                str.Append(",[SI_LastupUser]");
                str.Append(",[SI_LastupDate]");
                str.Append(",[SI_Code]");
                str.Append(",[SI_Name1]");
                str.Append(",[SI_Name2]");
                str.Append(",[SI_BasicSalary]");
                str.Append(",[SI_Overtiem]");
                str.Append(",[SI_Reward]");
                str.Append(",[SI_Bonus]");
                str.Append(",[SI_Commission]");
                str.Append(",[SI_Tickets]");
                str.Append(",[SI_AnnualVacation]");
                str.Append(",[SI_EndService]");
                str.Append(",[SI_Benefit1]");
                str.Append(",[SI_Benefit2]");
                str.Append(",[SI_Benefit3]");
                str.Append(",[SI_AllHome]");
                str.Append(",[SI_AllTransportation]");
                str.Append(",[SI_AllComunication]");
                str.Append(",[SI_AllFood]");
                str.Append(",[SI_AllOther]");
                str.Append(",[SI_Insurance]");
                str.Append(",[SI_Late]");
                str.Append(",[SI_Absence]");
                str.Append(",[SI_Penalty]");
                str.Append(",[SC_Deduction1]");
                str.Append(",[SC_Deduction2]");
                str.Append(",[SC_Deduction3])");
                str.Append(" VALUES ");
                str.Append("(@SI_CreateUser");
                str.Append(",@SI_CreateDate");
                str.Append(",@SI_LastupUser");
                str.Append(",@SI_LastupDate");
                str.Append(",@SI_Code");
                str.Append(",@SI_Name1");
                str.Append(",@SI_Name2");
                str.Append(",@SI_BasicSalary");
                str.Append(",@SI_Overtiem");
                str.Append(",@SI_Reward");
                str.Append(",@SI_Bonus");
                str.Append(",@SI_Commission");
                str.Append(",@SI_Tickets");
                str.Append(",@SI_AnnualVacation");
                str.Append(",@SI_EndService");
                str.Append(",@SI_Benefit1");
                str.Append(",@SI_Benefit2");
                str.Append(",@SI_Benefit3");
                str.Append(",@SI_AllHome");
                str.Append(",@SI_AllTransportation");
                str.Append(",@SI_AllComunication");
                str.Append(",@SI_AllFood");
                str.Append(",@SI_AllOther");
                str.Append(",@SI_Insurance");
                str.Append(",@SI_Late");
                str.Append(",@SI_Absence");
                str.Append(",@SI_Penalty");
                str.Append(",@SC_Deduction1");
                str.Append(",@SC_Deduction2");
                str.Append(",@SC_Deduction3)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@SI_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@SI_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@SI_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@SI_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@SI_Code", SqlDbType.NVarChar, 50).Value = item.Code ?? "";
                comm.Parameters.Add("@SI_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@SI_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@SI_BasicSalary", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.BasicSalary);
                comm.Parameters.Add("@SI_Overtiem", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Overtiem);
                comm.Parameters.Add("@SI_Reward", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Reward);
                comm.Parameters.Add("@SI_Bonus", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Bonus);
                comm.Parameters.Add("@SI_Commission", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Commission);
                comm.Parameters.Add("@SI_Tickets", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Tickets);
                comm.Parameters.Add("@SI_AnnualVacation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.AnnualVacation);
                comm.Parameters.Add("@SI_EndService", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.EndService);
                comm.Parameters.Add("@SI_Benefit1", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Benefit1);
                comm.Parameters.Add("@SI_Benefit2", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Benefit2);
                comm.Parameters.Add("@SI_Benefit3", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Benefit3);
                comm.Parameters.Add("@SI_AllHome", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.AllHome);
                comm.Parameters.Add("@SI_AllTransportation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.AllTransportation);
                comm.Parameters.Add("@SI_AllComunication", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.AllComunication);
                comm.Parameters.Add("@SI_AllFood", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.AllFood);
                comm.Parameters.Add("@SI_AllOther", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.AllOther);
                comm.Parameters.Add("@SI_Insurance", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Insurance);
                comm.Parameters.Add("@SI_Late", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Late);
                comm.Parameters.Add("@SI_Absence", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Absence);
                comm.Parameters.Add("@SI_Penalty", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Penalty);
                comm.Parameters.Add("@SC_Deduction1", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Deduction1);
                comm.Parameters.Add("@SC_Deduction2", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Deduction2);
                comm.Parameters.Add("@SC_Deduction3", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Deduction3);
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, SalariesIntegration item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update finCard_SalariesIntegration SET ");
                str.Append("[SI_CreateUser]=@SI_CreateUser");
                str.Append(",[SI_CreateDate]=@SI_CreateDate");
                str.Append(",[SI_LastupUser]=@SI_LastupUser");
                str.Append(",[SI_LastupDate]=@SI_LastupDate");
                str.Append(",[SI_Code]=@SI_Code");
                str.Append(",[SI_Name1]=@SI_Name1");
                str.Append(",[SI_Name2]=@SI_Name2");
                str.Append(",[SI_BasicSalary]=@SI_BasicSalary");
                str.Append(",[SI_Overtiem]=@SI_Overtiem");
                str.Append(",[SI_Reward]=@SI_Reward");
                str.Append(",[SI_Bonus]=@SI_Bonus");
                str.Append(",[SI_Commission]=@SI_Commission");
                str.Append(",[SI_Tickets]=@SI_Tickets");
                str.Append(",[SI_AnnualVacation]=@SI_AnnualVacation");
                str.Append(",[SI_EndService]=@SI_EndService");
                str.Append(",[SI_Benefit1]=@SI_Benefit1");
                str.Append(",[SI_Benefit2]=@SI_Benefit2");
                str.Append(",[SI_Benefit3]=@SI_Benefit3");
                str.Append(",[SI_AllHome]=@SI_AllHome");
                str.Append(",[SI_AllTransportation]=@SI_AllTransportation");
                str.Append(",[SI_AllComunication]=@SI_AllComunication");
                str.Append(",[SI_AllFood]=@SI_AllFood");
                str.Append(",[SI_AllOther]=@SI_AllOther");
                str.Append(",[SI_Insurance]=@SI_Insurance");
                str.Append(",[SI_Late]=@SI_Late");
                str.Append(",[SI_Absence]=@SI_Absence");
                str.Append(",[SI_Penalty]=@SI_Penalty");
                str.Append(",[SC_Deduction1]=@SC_Deduction1");
                str.Append(",[SC_Deduction2]=@SC_Deduction2");
                str.Append(",[SC_Deduction3]=@SC_Deduction3");
                str.Append(" WHERE SI_Key=@SI_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@SI_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@SI_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@SI_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@SI_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@SI_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@SI_Code", SqlDbType.NVarChar, 50).Value = item.Code ?? "";
                comm.Parameters.Add("@SI_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@SI_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@SI_BasicSalary", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.BasicSalary);
                comm.Parameters.Add("@SI_Overtiem", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Overtiem);
                comm.Parameters.Add("@SI_Reward", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Reward);
                comm.Parameters.Add("@SI_Bonus", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Bonus);
                comm.Parameters.Add("@SI_Commission", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Commission);
                comm.Parameters.Add("@SI_Tickets", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Tickets);
                comm.Parameters.Add("@SI_AnnualVacation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.AnnualVacation);
                comm.Parameters.Add("@SI_EndService", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.EndService);
                comm.Parameters.Add("@SI_Benefit1", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Benefit1);
                comm.Parameters.Add("@SI_Benefit2", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Benefit2);
                comm.Parameters.Add("@SI_Benefit3", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Benefit3);
                comm.Parameters.Add("@SI_AllHome", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.AllHome);
                comm.Parameters.Add("@SI_AllTransportation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.AllTransportation);
                comm.Parameters.Add("@SI_AllComunication", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.AllComunication);
                comm.Parameters.Add("@SI_AllFood", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.AllFood);
                comm.Parameters.Add("@SI_AllOther", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.AllOther);
                comm.Parameters.Add("@SI_Insurance", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Insurance);
                comm.Parameters.Add("@SI_Late", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Late);
                comm.Parameters.Add("@SI_Absence", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Absence);
                comm.Parameters.Add("@SI_Penalty", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Penalty);
                comm.Parameters.Add("@SC_Deduction1", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Deduction1);
                comm.Parameters.Add("@SC_Deduction2", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Deduction2);
                comm.Parameters.Add("@SC_Deduction3", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Deduction3);
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [finCard_SalariesIntegration] where [SI_Key]=@Key";
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
    }
}
