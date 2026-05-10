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
    public class Session
    {
        public Guid? Key { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public decimal Sales { get; set; }
        public decimal vatAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetSales { get; set; }
        public decimal Credit { get; set; }
        public Boolean Closed { get; set; }
        public static int Insert(string DB, Session item,Guid? User)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO AppSales_Session ");
                str.Append("([ses_Key]");
                str.Append(",[ses_User]");
                str.Append(",[ses_OpenDate]");
                str.Append(",[ses_CloseDate]");
                str.Append(",[ses_Sales]");
                str.Append(",[ses_vatAmount]");
                str.Append(",[ses_Discount]");
                str.Append(",[ses_NetSales]");
                str.Append(",[ses_Credit]");
                str.Append(",[ses_Closed])");
                str.Append(" VALUES ");
                str.Append("(@ses_Key");
                str.Append(",@ses_User");
                str.Append(",@ses_OpenDate");
                str.Append(",@ses_CloseDate");
                str.Append(",@ses_Sales");
                str.Append(",@ses_vatAmount");
                str.Append(",@ses_Discount");
                str.Append(",@ses_NetSales");
                str.Append(",@ses_Credit");
                str.Append(",@ses_Closed)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@ses_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@ses_User", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(User);
                comm.Parameters.Add("@ses_OpenDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.OpenDate);
                comm.Parameters.Add("@ses_CloseDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CloseDate);
                comm.Parameters.Add("@ses_Sales", SqlDbType.Decimal).Value = item.Sales;
                comm.Parameters.Add("@ses_vatAmount", SqlDbType.Decimal).Value = item.vatAmount;
                comm.Parameters.Add("@ses_Discount", SqlDbType.Decimal).Value = item.Discount;
                comm.Parameters.Add("@ses_NetSales", SqlDbType.Decimal).Value = item.NetSales;
                comm.Parameters.Add("@ses_Credit", SqlDbType.Decimal).Value = item.Credit;
                comm.Parameters.Add("@ses_Closed", SqlDbType.Bit).Value = item.Closed;
                con.Open();
                res = comm.ExecuteNonQuery();
            }
            return res;
        }
    }
}
