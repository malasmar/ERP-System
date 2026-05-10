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
    public class Transaction
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public int Status { get; set; }
        public int Branch { get; set; }
        public Guid? Prefix { get; set; }
        public Guid? Category { get; set; }
        public int DocumentKind { get; set; }
        public int VoucherNo { get; set; }
        public DateTime? VoucherDate { get; set; }
        public int MonthlyNo { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int AccountKind { get; set; }
        public Guid? AccountKey { get; set; }
        public string Description { get; set; }
        public string TransactionNo { get; set; }
        public string Currency { get; set; }
        public decimal Subtotal { get; set; }
        public decimal vatAmount { get; set; }
        public decimal Total { get; set; }
        public int Rows { get; set; }
        public Boolean IncloudExp { get; set; }

        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public string RecipientName { get; set; }

        public CLiCore.CardsInfo.User CreateUserInfo { get; set; }
        public CLiCore.CardsInfo.User UpdateUserInfo { get; set; }
        public Transaction GetItem(string DB,string xLan,Guid UserKey, Guid? Key, int DocKind,int Year)
        {
            Transaction item = new Transaction();
            item.VoucherNo = VoucherOperation.GetMaxVouchers(DB, DocKind, Year);
            item.MonthlyNo = VoucherOperation.GetMaxMonthlyVouchers(DB, DateTime.Now.Year, DateTime.Now.Month, DocKind);
            item.DocumentKind = DocKind;
            item.VoucherDate = DateTime.Now;
            item.Currency = xConfig.DefaultCurrency(DB);
            item.CreateDate= DateTime.Now;
            item.CreateUserInfo = new CLiCore.CardsInfo.User().GetItem(xLan, UserKey);
            item.UpdateUserInfo = new CLiCore.CardsInfo.User().GetItem(xLan, UserKey);
            item.CreateUser = item.CreateUserInfo.No;
            CLiCore.Shared.UserDefaultData defaultData = CLiCore.Shared.UserDefaultData.GetItem(UserKey);
         
            item.Prefix = defaultData.Prefix;
            item.Branch = defaultData.Branch;
            item.Project = defaultData.Project;
            item.CostCenter = defaultData.CostCenter;
            switch (DocKind)
            {
                case (int)CLiCore.DocumentKind.finCashCollection:
                case (int)CLiCore.DocumentKind.finCashPayment:
                    item.AccountKind = (int)PLenums.TransactionAccount.CashBox;
                    break;
                case (int)CLiCore.DocumentKind.finBankCollection:
                case (int)CLiCore.DocumentKind.finBankPayment:
                    item.AccountKind = (int)PLenums.TransactionAccount.Bank;
                    break;
                case (int)CLiCore.DocumentKind.finDebitNote:
                case (int)CLiCore.DocumentKind.finCreditNote:
                    item.AccountKind = (int)PLenums.TransactionAccount.CurrentAccount;
                    break;
                default:
                    item.AccountKind = 0;
                    break;
            }
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from dbo.fnfinDocuments_Transaction(@Key)";
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
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["LastupDate"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Branch = Convert.ToInt32(reader["Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["Prefix"]);
                    item.Category = iCore.IsDbNullRtNull(reader["Category"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.VoucherNo = Convert.ToInt32(reader["VoucherNo"]);
                    item.VoucherDate = iCore.IsDbNullRtNullDate(reader["VoucherDate"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.ReferenceNo = Convert.ToString(reader["ReferenceNo"]);
                    item.ReferenceDate = iCore.IsDbNullRtNullDate(reader["ReferenceDate"]);
                    item.DueDate = iCore.IsDbNullRtNullDate(reader["DueDate"]);
                    item.AccountKind = Convert.ToInt32(reader["AccountKind"]);
                    item.AccountKey = iCore.IsDbNullRtNull(reader["AccountKey"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.TransactionNo = Convert.ToString(reader["TransactionNo"]);
                    item.Currency = Convert.ToString(reader["Currency"]);
                    item.Subtotal = Convert.ToDecimal(reader["Subtotal"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.Rows = Convert.ToInt32(reader["Rows"]);
                    item.IncloudExp = Convert.ToBoolean(reader["IncloudExp"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Project"]);
                    item.RecipientName = Convert.ToString(reader["RecipientName"]);
                    item.CreateUserInfo = new CLiCore.CardsInfo.User().GetItem(xLan, item.CreateUser);
                    item.UpdateUserInfo = new CLiCore.CardsInfo.User().GetItem(xLan, item.LastupUser);
                }
                reader.Close();
            }
            return item;
        }
        public Transaction GetItem(string DB,Guid? Key)
        {
            Transaction item = new Transaction();
           
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from dbo.fnfinDocuments_Transaction(@Key)";
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
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["LastupDate"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Branch = Convert.ToInt32(reader["Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["Prefix"]);
                    item.Category = iCore.IsDbNullRtNull(reader["Category"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.VoucherNo = Convert.ToInt32(reader["VoucherNo"]);
                    item.VoucherDate = iCore.IsDbNullRtNullDate(reader["VoucherDate"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.ReferenceNo = Convert.ToString(reader["ReferenceNo"]);
                    item.ReferenceDate = iCore.IsDbNullRtNullDate(reader["ReferenceDate"]);
                    item.DueDate = iCore.IsDbNullRtNullDate(reader["DueDate"]);
                    item.AccountKind = Convert.ToInt32(reader["AccountKind"]);
                    item.AccountKey = iCore.IsDbNullRtNull(reader["AccountKey"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.TransactionNo = Convert.ToString(reader["TransactionNo"]);
                    item.Currency = Convert.ToString(reader["Currency"]);
                    item.Subtotal = Convert.ToDecimal(reader["Subtotal"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.Rows = Convert.ToInt32(reader["Rows"]);
                    item.IncloudExp = Convert.ToBoolean(reader["IncloudExp"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Project"]);
                    item.RecipientName = Convert.ToString(reader["RecipientName"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
