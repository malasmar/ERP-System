using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Documents
{
    public class TransactionInfo
    {
        public Guid? Key { get; set; }
        public CLiCore.CardsInfo.User CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public CLiCore.CardsInfo.User LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public int Status { get; set; }
        public int DocumentKind { get; set; }
        public int VoucherNo { get; set; }
        public DateTime? VoucherDate { get; set; }
        public int MonthlyNo { get; set; }
        public CLiCore.CardsInfo.AccountDetails Account { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal Subtotal { get; set; }
        public decimal vatAmount { get; set; }
        public decimal Total { get; set; }
        public int Rows { get; set; }

        public TransactionInfo GetItem(string DB,string xLan,Guid? Key)
        {
            TransactionInfo item = new TransactionInfo();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from dbo.fnFinDocuments_TransactionInfo(@Key) ";
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
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["CreateDate"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["LastupDate"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.VoucherNo = Convert.ToInt32(reader["VoucherNo"]);
                    item.VoucherDate = iCore.IsDbNullRtNullDate(reader["VoucherDate"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.Description = Convert.ToString(reader["Description"])??"";
                    item.Currency = Convert.ToString(reader["Currency"])??"";
                    item.Subtotal = Convert.ToDecimal(reader["Subtotal"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.Rows = Convert.ToInt32(reader["Rows"]);
                    item.CreateUser = new CLiCore.CardsInfo.User().GetItem(xLan, Convert.ToInt32(reader["CreateUser"]));
                    item.LastupUser = new CLiCore.CardsInfo.User().GetItem(xLan, Convert.ToInt32(reader["LastupUser"]));
                    item.Account = new CLiCore.CardsInfo.AccountDetails().GetItem(DB, xLan, iCore.IsDbNullRtNull(reader["AccountKey"]));
                }
                reader.Close();
            }
            return item;
        }
    }
}
