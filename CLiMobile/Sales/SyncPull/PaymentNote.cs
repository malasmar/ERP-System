using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiMobile.Sales.SyncPull
{
    public class PaymentNote
    {
        public Guid? Key { get; set; }
        public Guid? Client { get; set; }
        public Guid? Quotation { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int Kind { get; set; }
        public string Comment { get; set; }
        public static int Insert(string DB, PaymentNote item,Guid User)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO AppSales_PaymentNote ");
                str.Append("([pay_Key]");
                str.Append(",[pay_Client]");
                str.Append(",[pay_Quotation]");
                str.Append(",[pay_Date]");
                str.Append(",[pay_Amount]");
                str.Append(",[pay_Kind]");
                str.Append(",[pay_Comment]");
                str.Append(",[pay_Person])");
                str.Append(" VALUES ");
                str.Append("(@pay_Key");
                str.Append(",@pay_Client");
                str.Append(",@pay_Quotation");
                str.Append(",@pay_Date");
                str.Append(",@pay_Amount");
                str.Append(",@pay_Kind");
                str.Append(",@pay_Comment");
                str.Append(",@pay_Person)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@pay_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@pay_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Client);
                comm.Parameters.Add("@pay_Quotation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Quotation);
                comm.Parameters.Add("@pay_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.Date);
                comm.Parameters.Add("@pay_Amount", SqlDbType.Decimal).Value = item.Amount;
                comm.Parameters.Add("@pay_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@pay_Comment", SqlDbType.NVarChar, 500).Value = item.Comment ?? "";
                comm.Parameters.Add("@pay_Person", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(User);
                con.Open();
               res= comm.ExecuteNonQuery();
            }
            return res;
        }
    }
}
