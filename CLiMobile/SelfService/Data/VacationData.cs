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
    public class VacationData
    {
        public Guid? Key { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int AnnualDays { get; set; }
        public decimal AnnualBalance { get; set; }
        public DateTime? LastReturn { get; set; }
        public int VacationKind { get; set; }
        public int TichetKind { get; set; }
        public int TicketCount { get; set; }
        public int Days { get; set; }
        public int Service { get; set; }
        public VacationData GetItem(string DB,Guid? Key)
        {
            VacationData item = new VacationData();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_EmployeeVacation(@Key) ";
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
                    item.StartDate = iCore.IsDbNullRtNullDate(reader["StartDate"]);
                    item.EndDate = iCore.IsDbNullRtNullDate(reader["EndDate"]);
                    item.AnnualDays = Convert.ToInt32(reader["AnnualDays"]);
                    item.AnnualBalance = Convert.ToDecimal(reader["AnnualBalance"]);
                    item.LastReturn = iCore.IsDbNullRtNullDate(reader["LastReturn"]);
                    item.VacationKind = Convert.ToInt32(reader["VacationKind"]);
                    item.TichetKind = Convert.ToInt32(reader["TichetKind"]);
                    item.TicketCount = Convert.ToInt32(reader["TicketCount"]);
                    item.Days =Convert.ToInt32((DateTime.UtcNow- item.LastReturn.Value).TotalDays);
                    item.Service = Convert.ToInt32((DateTime.UtcNow -item.StartDate.Value ).TotalDays);
                }
                reader.Close();
            }
            return item;
        }
    }
}
