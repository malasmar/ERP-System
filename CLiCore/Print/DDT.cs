using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore.Print
{
    public class DDT
    {
        public static DataTable CompanyProfile(string DB)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top(1) * from dbo.Print_CompanyProfile() ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable InvoiceHeader(string DB,Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select TOP 100 percent * from dbo.Print_Invoice(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable InvoiceDetails(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select TOP 100 percent  * from dbo.Print_InvoiceDetails(@Key) order by [Index]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable CurrentDetails(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select TOP 100 percent * from dbo.Print_CurrentDetails(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable BankDetails(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select TOP 100 PERCENT * from dbo.Print_BankDetails(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value =iCore.IsNullRtDbNull(Key);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable SignaturDetails(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select TOP 100 PERCENT * from dbo.Print_SignaturDetails(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable TransactionQR(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.Print_TransactionQR(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable InvoiceQR(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.Print_InvoiceQR(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable UserDetails(int Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.Print_UserDetails(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.Int).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable BranchDetails(string DB, int Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.Print_Branch(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.Int).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable Transaction(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.Print_Transaction(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable TransactionDetails(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.Print_TransactionDetails(@Key) order by [Index]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable TransactionDetailsJV(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.Print_TransactionDetailsJV(@Key) order by [Index] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }

        public static DataTable GL(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.Print_GeneralLedger(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable GLDetails(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select top 100 percent * from dbo.Print_GeneralLedgerDetails(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable Proforma(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select TOP 100 percent * from dbo.Print_ProformaInvoice(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable ProformaDetails(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select TOP 100 percent  * from dbo.Print_ProformaInvoiceDetails(@Key) order by [Index]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }

        public static DataTable Quotation(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select TOP 100 percent * from dbo.Print_Quotation(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
        public static DataTable QuotationDetails(string DB, Guid? Key)
        {
            DataTable Res = new DataTable();
            string selQuery = "select TOP 100 percent  * from dbo.Print_QuotationDetails(@Key) order by [Index]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(Res);
                return Res;
            }
        }
    }
}
