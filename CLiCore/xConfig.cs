using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiCore
{
    public class xConfig
    {
        public static Guid? vatKey(string DB)
        {
            Guid? result=null;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top(1) [com_vatKey] from [com_Settings]  ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result =iCore.IsDbNullRtNull(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static decimal vatRate(string DB,Guid? Key)
        {
            decimal result = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top 100 percent [vat_Rate] from [com_vatRates] where [vat_Key]=@Key  ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = Convert.ToDecimal(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static string DefaultCurrency(string DB)
        {
            string Res = "";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "select top(1) [com_Currency] as [Code] from [com_DefaultSettings]";
                SqlCommand command = new SqlCommand();
                command = new SqlCommand();
                command.Connection = con;
                command.CommandType = CommandType.Text;
                command.CommandText = delQuery;
                command.Parameters.Clear();
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Res = (string)reader[0];
                }
                reader.Close();
            }
            return Res;
        }

        public static string AccountCode(string DB, string Parent)
        {
            string Res = "";
            if (Parent == "")
                return Res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "select top 100 PERCENT dbo.sfnaccCard_GetCode(@Parent) ";
                SqlCommand command = new SqlCommand();
                command = new SqlCommand();
                command.Connection = con;
                command.CommandType = CommandType.Text;
                command.CommandText = delQuery;
                command.Parameters.Clear();
                command.Parameters.Add("@Parent", SqlDbType.NVarChar).Value = Parent ?? "";
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Res = (string)reader[0];
                }
                reader.Close();
            }
            return Res;
        }
        public static string AccountCode(string DB, Guid? Parent)
        {
            string Res = "";
            if (Parent == null)
                return Res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "select top 100 PERCENT dbo.SFnAcc_CodeByParentKey(@Parent) ";
                SqlCommand command = new SqlCommand();
                command = new SqlCommand();
                command.Connection = con;
                command.CommandType = CommandType.Text;
                command.CommandText = delQuery;
                command.Parameters.Clear();
                command.Parameters.Add("@Parent", SqlDbType.UniqueIdentifier).Value = Parent;
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Res = (string)reader[0];
                }
                reader.Close();
            }
            return Res;
        }

        public static Guid? UserPrefix(Guid? Key)
        {
            Guid? result = null;
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                string selQuery = "select top(1) [user_Prefix] from [px_Users] where [user_Key]=@Key  ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = iCore.IsDbNullRtNull(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static bool UserPrefixFilter(Guid? Key)
        {
            bool result = false;
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                string selQuery = "select top(1) [User_ActiveFilter] from [px_Users] where [user_Key]=@Key";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = Convert.ToBoolean(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        //Settings
        public static Guid? SalesDiscountAccount(string DB)
        {
            Guid? result = null;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top(1) [com_DebitDiscount] from [com_Settings]  ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = iCore.IsDbNullRtNull(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static Guid? RetentionAccount(string DB)
        {
            Guid? result = null;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top(1) [com_RetentionLess] from [com_Settings]  ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = iCore.IsDbNullRtNull(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static Guid? PaymentDiscount(string DB)
        {
            Guid? result = null;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top(1) [com_PaymentDiscount] from [com_Settings]  ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = iCore.IsDbNullRtNull(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static Guid? wOnRoad(string DB)
        {
            Guid? result = null;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top(1) [com_wOnRoad] from [com_Settings]  ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = iCore.IsDbNullRtNull(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static bool ExpiryDateStatus(string DB)
        {
            bool result = false;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top(1) [com_ShowExpiry] from [com_Settings]  ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = Convert.ToBoolean(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static bool AutoReceiptShipping(string DB)
        {
            bool result = false;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top(1) [com_AutoReceiptShipping] from [com_Settings]  ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = Convert.ToBoolean(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static bool ShowMonthlyNo(string DB)
        {
            bool result = false;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top(1) [com_ShowMonthlyNo] from [com_Settings]  ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = Convert.ToBoolean(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static Guid? CloseFinancialYear(string DB)
        {
            Guid? result = null;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top(1) [com_CloseYear] from [com_Settings]  ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = iCore.IsDbNullRtNull(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
    }
}
