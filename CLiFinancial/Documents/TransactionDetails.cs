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
    public class TransactionDetails
    {
        public int RecNo { get; set; }
        public Guid? Key { get; set; }
        public Guid? OperationKey { get; set; }
        public int Index { get; set; }
        public int Status { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public string TransactionNo { get; set; }
        public int AccountKind { get; set; }
        public int AccountType { get; set; }
        public Guid? AccountKey { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public Guid? Client { get; set; }
        public Guid? Person { get; set; }
        public string Description { get; set; }
        public Guid? vatKey { get; set; }
        public decimal vatRate { get; set; }
        public decimal Amount { get; set; }
        public decimal vatAmount { get; set; }
        public decimal Total { get; set; }
        public string vatRegNo { get; set; }
        public string vatCurrent { get; set; }
        public Guid? Importation { get; set; }
        public int ExpensesKind { get; set; }
        public int PaymentKind { get; set; }
        public JsonData.AccountDetails jnAccount { get; set; }
        public JsonData.CostCenter jnCostCenter { get; set; }
        public JsonData.Project jnProject { get; set; }
        public JsonData.vatDetails jnvatDetails { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public bool isVAT { get; set; }
       public Guid? SalaryKey { get; set; } 
        public List<TransactionDetails> GetList(string DB, string xLan, Guid? Key)
        {
            List<TransactionDetails> items = new List<TransactionDetails>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fnfinDocuments_TransactionDetails(@Key) order by [Index] ";
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
                    TransactionDetails item = new TransactionDetails();
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.Index = Convert.ToInt32(reader["Index"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.ReferenceNo = Convert.ToString(reader["ReferenceNo"]);
                    item.ReferenceDate = iCore.IsDbNullRtNullDate(reader["ReferenceDate"]);
                    item.TransactionNo = Convert.ToString(reader["TransactionNo"]);
                    item.AccountKind = Convert.ToInt32(reader["AccountKind"]);
                    item.AccountType = Convert.ToInt32(reader["AccountType"]);
                    item.AccountKey = iCore.IsDbNullRtNull(reader["AccountKey"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Project"]);
                    item.Client = iCore.IsDbNullRtNull(reader["Client"]);
                    item.Person = iCore.IsDbNullRtNull(reader["Person"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["vatKey"]);
                    item.vatRate = Convert.ToDecimal(reader["vatRate"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.vatRegNo = Convert.ToString(reader["vatRegNo"]);
                    item.vatCurrent = Convert.ToString(reader["vatCurrent"]);
                    item.Importation = iCore.IsDbNullRtNull(reader["Importation"]);
                    item.ExpensesKind = Convert.ToInt32(reader["ExpensesKind"]);
                    item.PaymentKind = Convert.ToInt32(reader["PaymentKind"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.isVAT = Convert.ToBoolean(reader["isVAT"]);
                    item.jnAccount = new JsonData.AccountDetails().GetItem(DB, xLan, item.AccountKey);
                    item.jnCostCenter = new JsonData.CostCenter().GetItem(DB, xLan, item.CostCenter);
                    item.jnProject = new JsonData.Project().GetItem(DB, xLan, item.Project);
                    item.jnvatDetails = new JsonData.vatDetails().GetItem(DB, xLan, item.vatKey);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<TransactionDetails> GetListJV(string DB, string xLan, Guid? Key)
        {
            List<TransactionDetails> items = new List<TransactionDetails>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fnfinDocuments_TransactionDetails_JV(@Key) order by [Index] ";
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
                    TransactionDetails item = new TransactionDetails();
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.Index = Convert.ToInt32(reader["Index"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.ReferenceNo = Convert.ToString(reader["ReferenceNo"]);
                    item.ReferenceDate = iCore.IsDbNullRtNullDate(reader["ReferenceDate"]);
                    item.TransactionNo = Convert.ToString(reader["TransactionNo"]);
                    item.AccountKind = Convert.ToInt32(reader["AccountKind"]);
                    item.AccountType = Convert.ToInt32(reader["AccountType"]);
                    item.AccountKey = iCore.IsDbNullRtNull(reader["AccountKey"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Project"]);
                    item.Client = iCore.IsDbNullRtNull(reader["Client"]);
                    item.Person = iCore.IsDbNullRtNull(reader["Person"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["vatKey"]);
                    item.vatRate = Convert.ToDecimal(reader["vatRate"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.vatRegNo = Convert.ToString(reader["vatRegNo"]);
                    item.vatCurrent = Convert.ToString(reader["vatCurrent"]);
                    item.Importation = iCore.IsDbNullRtNull(reader["Importation"]);
                    item.ExpensesKind = Convert.ToInt32(reader["ExpensesKind"]);
                    item.PaymentKind = Convert.ToInt32(reader["PaymentKind"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.isVAT = Convert.ToBoolean(reader["isVAT"]);
                    item.jnAccount = new JsonData.AccountDetails().GetItem(DB, xLan, item.AccountKey);
                    item.jnCostCenter = new JsonData.CostCenter().GetItem(DB, xLan, item.CostCenter);
                    item.jnProject = new JsonData.Project().GetItem(DB, xLan, item.Project);
                    item.jnvatDetails = new JsonData.vatDetails().GetItem(DB, xLan, item.vatKey);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
