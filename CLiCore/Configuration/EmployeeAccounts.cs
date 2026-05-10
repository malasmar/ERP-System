using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore.Configuration
{
    public class EmployeeAccounts
    {
        public Guid? Key { get; set; }
        public Guid? Account { get; set; }
        public Guid? Advance { get; set; }
        public Guid? Loan { get; set; }
        public Guid? EosProvision { get; set; }
        public Guid? VacProvision { get; set; }
        public Boolean DetAdvance { get; set; }
        public Boolean DetLoan { get; set; }
        public Boolean DetEosProvision { get; set; }
        public Boolean DetVacProvision { get; set; }
        public EmployeeAccounts GetItem(string DB)
        {
            EmployeeAccounts item = new EmployeeAccounts();
            item.DetAdvance = true;
            string selQuery = "select top(1) * from com_Settings_EmployeeAccounts";
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
                    item.Key = iCore.IsDbNullRtNull(reader["set_Key"]);
                    item.Account = iCore.IsDbNullRtNull(reader["set_Account"]);
                    item.Advance = iCore.IsDbNullRtNull(reader["set_Advance"]);
                    item.Loan = iCore.IsDbNullRtNull(reader["set_Loan"]);
                    item.EosProvision = iCore.IsDbNullRtNull(reader["set_EosProvision"]);
                    item.VacProvision = iCore.IsDbNullRtNull(reader["set_VacProvision"]);
                    item.DetAdvance = Convert.ToBoolean(reader["set_DetAdvance"]);
                    item.DetLoan = Convert.ToBoolean(reader["set_DetLoan"]);
                    item.DetEosProvision = Convert.ToBoolean(reader["set_DetEosProvision"]);
                    item.DetVacProvision = Convert.ToBoolean(reader["set_DetVacProvision"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, EmployeeAccounts item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("delete from com_Settings_EmployeeAccounts ");
                str.Append("INSERT INTO com_Settings_EmployeeAccounts ");
                str.Append("([set_Account]");
                str.Append(",[set_Advance]");
                str.Append(",[set_Loan]");
                str.Append(",[set_EosProvision]");
                str.Append(",[set_VacProvision]");
                str.Append(",[set_DetAdvance]");
                str.Append(",[set_DetLoan]");
                str.Append(",[set_DetEosProvision]");
                str.Append(",[set_DetVacProvision])");
                str.Append(" VALUES ");
                str.Append("(@set_Account");
                str.Append(",@set_Advance");
                str.Append(",@set_Loan");
                str.Append(",@set_EosProvision");
                str.Append(",@set_VacProvision");
                str.Append(",@set_DetAdvance");
                str.Append(",@set_DetLoan");
                str.Append(",@set_DetEosProvision");
                str.Append(",@set_DetVacProvision)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@set_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Account);
                comm.Parameters.Add("@set_Advance", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Advance);
                comm.Parameters.Add("@set_Loan", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Loan);
                comm.Parameters.Add("@set_EosProvision", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.EosProvision);
                comm.Parameters.Add("@set_VacProvision", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.VacProvision);
                comm.Parameters.Add("@set_DetAdvance", SqlDbType.Bit).Value = item.DetAdvance;
                comm.Parameters.Add("@set_DetLoan", SqlDbType.Bit).Value = item.DetLoan;
                comm.Parameters.Add("@set_DetEosProvision", SqlDbType.Bit).Value = item.DetEosProvision;
                comm.Parameters.Add("@set_DetVacProvision", SqlDbType.Bit).Value = item.DetVacProvision;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
    }
}
