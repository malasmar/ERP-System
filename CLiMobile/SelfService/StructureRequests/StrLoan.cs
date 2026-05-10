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
    public class StrLoan
    {
        public Guid? Key { get; set; }
        public Guid? ConfirmationKey { get; set; }
        public bool FinalApproval { get; set; }
        public DateTime? Create { get; set; }
        public string Image { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public Boolean Payment { get; set; }
        public string Structure { get; set; }
        public List<StrLoan> GetList(string DB,Guid? Key)
        {
            List<StrLoan> items = new List<StrLoan>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_SturctureLoan(@Key) order by [Create] ";
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
                    StrLoan item = new StrLoan();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.ConfirmationKey = iCore.IsDbNullRtNull(reader["ConfirmationKey"]);
                    item.FinalApproval = Convert.ToBoolean(reader["FinalApproval"]);
                    item.Create = iCore.IsDbNullRtNullDate(reader["Create"]);
                    item.Image = Convert.ToString(reader["Image"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Year = Convert.ToInt32(reader["Year"]);
                    item.Month = Convert.ToInt32(reader["Month"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Payment = Convert.ToBoolean(reader["Payment"]);
                    item.Structure = Convert.ToString(reader["Structure"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
