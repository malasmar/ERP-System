using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.RequestDetails
{
    public class DetailsAdvance
    {
        public Guid? Key { get; set; }
        public DateTime? Create { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public Boolean Payment { get; set; }
        public decimal AdvanceHistory { get; set; }
        public decimal AdvanceClosed { get; set; }
        public decimal AdvanceReminig { get; set; }
        public Data.PersonDetails PersonDetails { get; set; }
        public PreviousApproval PreviousApproval { get; set; }

        public DetailsAdvance GetItem(string DB,Guid? Key)
        {
            DetailsAdvance item = new DetailsAdvance();
            if(Key==null)
                return item;

            string selQuery = "select top 100 percent * from hrRequest_Advance where [req_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["req_Key"]);
                    item.Create = iCore.IsDbNullRtNullDate(reader["req_Create"]);
                    item.Year = Convert.ToInt32(reader["req_Year"]);
                    item.Month = Convert.ToInt32(reader["req_Month"]);
                    item.Amount = Convert.ToDecimal(reader["req_Amount"]);
                    item.Description = Convert.ToString(reader["req_Description"]);
                    item.Status = Convert.ToInt32(reader["req_Status"]);
                    item.Payment = Convert.ToBoolean(reader["req_Payment"]);
                    item.PersonDetails = new Data.PersonDetails().GetItem(DB, iCore.IsDbNullRtNull(reader["req_Employee"]));
                    item.PreviousApproval = new PreviousApproval().GetItem(DB, Key);
                    item.AdvanceHistory = core.GetEmployeeAdvance(DB, iCore.IsDbNullRtNull(reader["req_Employee"]));
                    item.AdvanceClosed = core.GetEmployeeClosedAdvance(DB, iCore.IsDbNullRtNull(reader["req_Employee"]));
                    item.AdvanceReminig = item.AdvanceHistory - item.AdvanceClosed;
                }
                reader.Close();
            }
            return item;
        }


    }
}
