using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CLiCore.Print
{
    public class FinancialTransaction
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
        public Guid? AccountKey { get; set; }
        public Guid? OriginalInvoice { get; set; }
        public FinancialTransaction GetItem(string DB, Guid? Key)
        {
            FinancialTransaction item = new FinancialTransaction();

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
                    item.AccountKey = iCore.IsDbNullRtNull(reader["AccountKey"]);
                    item.OriginalInvoice = iCore.IsDbNullRtNull(reader["OriginalInvoice"]);
                }
                reader.Close();
            }
            return item;
        }
        public FinancialTransaction GetProforma(string DB, Guid? Key)
        {
            FinancialTransaction item = new FinancialTransaction();

            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from [SalesDocument_Proforma] where [sal_OperationKey]=@Key ";
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
                    item.RecNo = Convert.ToInt32(reader["sal_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["sal_OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["sal_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["sal_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["sal_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["sal_LastupDate"]);
                    item.Status = Convert.ToInt32(reader["sal_Status"]);
                    item.Branch = Convert.ToInt32(reader["sal_Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["sal_Prefix"]);
               
                    item.AccountKey = iCore.IsDbNullRtNull(reader["sal_Client"]);
                   
                }
                reader.Close();
            }
            return item;
        }
        public FinancialTransaction GetQuotation(string DB, Guid? Key)
        {
            FinancialTransaction item = new FinancialTransaction();

            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from [SalesDocument_Quotation] where [sal_OperationKey]=@Key ";
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
                    item.RecNo = Convert.ToInt32(reader["sal_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["sal_OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["sal_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["sal_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["sal_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["sal_LastupDate"]);
                    item.Status = Convert.ToInt32(reader["sal_Status"]);
                    item.Branch = Convert.ToInt32(reader["sal_Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["sal_Prefix"]);

                    item.AccountKey = iCore.IsDbNullRtNull(reader["sal_Client"]);

                }
                reader.Close();
            }
            return item;
        }
    }
}
