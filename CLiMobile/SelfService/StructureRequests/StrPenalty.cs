using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.StructureRequests
{
    public class StrPenalty
    {
        public Guid? Key { get; set; }
        public Guid? ConfirmationKey { get; set; }
        public bool FinalApproval { get; set; }
        public DateTime? Create { get; set; }
        public string PenaltyName1 { get; set; }
        public string PenaltyName2 { get; set; }
        public string SupervisorCode { get; set; }
        public string SupervisorName1 { get; set; }
        public string SupervisorName2 { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Kind { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Structure { get; set; }
        public int Persons { get; set; }
        public List<StrPenalty> GetList(string DB, Guid? Key)
        {
            List<StrPenalty> items = new List<StrPenalty>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_SturcturePenalty(@Key) order by [Create] ";
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
                    StrPenalty item = new StrPenalty();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.ConfirmationKey = iCore.IsDbNullRtNull(reader["ConfirmationKey"]);
                    item.FinalApproval = Convert.ToBoolean(reader["FinalApproval"]);
                    item.Create = iCore.IsDbNullRtNullDate(reader["Create"]);
                    item.PenaltyName1 = Convert.ToString(reader["PenaltyName1"]);
                    item.PenaltyName2 = Convert.ToString(reader["PenaltyName2"]);
                    item.SupervisorCode = Convert.ToString(reader["SupervisorCode"]);
                    item.SupervisorName1 = Convert.ToString(reader["SupervisorName1"]);
                    item.SupervisorName2 = Convert.ToString(reader["SupervisorName2"]);
                    item.Year = Convert.ToInt32(reader["Year"]);
                    item.Month = Convert.ToInt32(reader["Month"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Value = Convert.ToDecimal(reader["Value"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Structure = Convert.ToString(reader["Structure"]);
                    item.Persons = Convert.ToInt32(reader["Persons"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
