using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Cards
{
    public class CurrentAccount
    {
        public Guid? Key { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public Guid? Parent { get; set; }
        public int Kind { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string? ConnectPerson { get; set; }
        public string? vat { get; set; }
        public string? CR { get; set; }
        public Guid? Account { get; set; }
        public Guid? Group { get; set; }
        public Guid? Activity { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? Websit { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public decimal LimitDebit { get; set; }
        public decimal LimitCredit { get; set; }
        public Boolean Disable { get; set; }
        public int DueDays { get; set; }
        public Boolean OpenSales { get; set; }
        public Boolean OpenPurchasing { get; set; }
        public Guid? SalesPerson { get; set; }
        public Guid? Prefix { get; set; }
        public int Source { get; set; }
        public int Status { get; set; }
        public List<CurrentAccount> GetList(string DB)
        {
            List<CurrentAccount> items = new List<CurrentAccount>();
            string selQuery = "select top 100 percent * from finCard_CurrentAccount order by [ca_Kind],[ca_Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    CurrentAccount item = new CurrentAccount();
                    item.Key = iCore.IsDbNullRtNull(reader["ca_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["ca_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["ca_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["ca_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["ca_LastupDate"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["ca_Parent"]);
                    item.Kind = Convert.ToInt32(reader["ca_Kind"]);
                    item.Code = Convert.ToString(reader["ca_Code"]);
                    item.Name1 = Convert.ToString(reader["ca_Name1"]);
                    item.Name2 = Convert.ToString(reader["ca_Name2"]);
                    item.ConnectPerson = Convert.ToString(reader["ca_ConnectPerson"]);
                    item.vat = Convert.ToString(reader["ca_vat"]);
                    item.CR = Convert.ToString(reader["ca_CR"]);
                    item.Account = iCore.IsDbNullRtNull(reader["ca_Account"]);
                    item.Group = iCore.IsDbNullRtNull(reader["ca_Group"]);
                    item.Activity = iCore.IsDbNullRtNull(reader["ca_Activity"]);
                    item.Phone = Convert.ToString(reader["ca_Phone"]);
                    item.Mobile = Convert.ToString(reader["ca_Mobile"]);
                    item.Fax = Convert.ToString(reader["ca_Fax"]);
                    item.Email = Convert.ToString(reader["ca_Email"]);
                    item.Websit = Convert.ToString(reader["ca_Websit"]);
                    item.Address = Convert.ToString(reader["ca_Address"]);
                    item.City = Convert.ToString(reader["ca_City"]);
                    item.LimitDebit = Convert.ToDecimal(reader["ca_LimitDebit"]);
                    item.LimitCredit = Convert.ToDecimal(reader["ca_LimitCredit"]);
                    item.Disable = Convert.ToBoolean(reader["ca_Disable"]);
                    item.DueDays = Convert.ToInt32(reader["ca_DueDays"]);
                    item.OpenSales = Convert.ToBoolean(reader["ca_OpenSales"]);
                    item.OpenPurchasing = Convert.ToBoolean(reader["ca_OpenPurchasing"]);
                    item.SalesPerson = iCore.IsDbNullRtNull(reader["ca_SalesPerson"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["ca_Prefix"]);
                    item.Source = Convert.ToInt32(reader["ca_Source"]);
                    item.Status = Convert.ToInt32(reader["ca_Status"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public CurrentAccount GetItem(string DB,Guid? Key)
        {
            CurrentAccount item = new CurrentAccount();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from finCard_CurrentAccount where [ca_Key]=@Key";
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
                    item.Key = iCore.IsDbNullRtNull(reader["ca_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["ca_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["ca_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["ca_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["ca_LastupDate"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["ca_Parent"]);
                    item.Kind = Convert.ToInt32(reader["ca_Kind"]);
                    item.Code = Convert.ToString(reader["ca_Code"]);
                    item.Name1 = Convert.ToString(reader["ca_Name1"]);
                    item.Name2 = Convert.ToString(reader["ca_Name2"]);
                    item.ConnectPerson = Convert.ToString(reader["ca_ConnectPerson"]);
                    item.vat = Convert.ToString(reader["ca_vat"]);
                    item.CR = Convert.ToString(reader["ca_CR"]);
                    item.Account = iCore.IsDbNullRtNull(reader["ca_Account"]);
                    item.Group = iCore.IsDbNullRtNull(reader["ca_Group"]);
                    item.Activity = iCore.IsDbNullRtNull(reader["ca_Activity"]);
                    item.Phone = Convert.ToString(reader["ca_Phone"]);
                    item.Mobile = Convert.ToString(reader["ca_Mobile"]);
                    item.Fax = Convert.ToString(reader["ca_Fax"]);
                    item.Email = Convert.ToString(reader["ca_Email"]);
                    item.Websit = Convert.ToString(reader["ca_Websit"]);
                    item.Address = Convert.ToString(reader["ca_Address"]);
                    item.City = Convert.ToString(reader["ca_City"]);
                    item.LimitDebit = Convert.ToDecimal(reader["ca_LimitDebit"]);
                    item.LimitCredit = Convert.ToDecimal(reader["ca_LimitCredit"]);
                    item.Disable = Convert.ToBoolean(reader["ca_Disable"]);
                    item.DueDays = Convert.ToInt32(reader["ca_DueDays"]);
                    item.OpenSales = Convert.ToBoolean(reader["ca_OpenSales"]);
                    item.OpenPurchasing = Convert.ToBoolean(reader["ca_OpenPurchasing"]);
                    item.SalesPerson = iCore.IsDbNullRtNull(reader["ca_SalesPerson"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["ca_Prefix"]);
                    item.Source = Convert.ToInt32(reader["ca_Source"]);
                    item.Status = Convert.ToInt32(reader["ca_Status"]);
                }
                reader.Close();
            }
            return item;
        }

        public static Guid? Insert(string DB, CurrentAccount item)
        {
            CLiCore.Platx.Account parentdetails = new CLiCore.Platx.Account().GetItem(DB, item.Parent);
            Guid? account;
            if (parentdetails.CloseSubAccounts == true)
            {
                account = parentdetails.Key;
            }
            else
            {
                string code = xConfig.AccountCode(DB, item.Parent);
                account = CLiCore.Platx.Account.Insert(DB, item.Parent, code, item.Name1, item.Name2);
            }

           
            Guid? key = Guid.NewGuid();
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO finCard_CurrentAccount");
                str.Append("([ca_Key]");
                str.Append(",[ca_CreateUser]");
                str.Append(",[ca_CreateDate]");
                str.Append(",[ca_LastupUser]");
                str.Append(",[ca_LastupDate]");
                str.Append(",[ca_Parent]");
                str.Append(",[ca_Kind]");
                str.Append(",[ca_Code]");
                str.Append(",[ca_Name1]");
                str.Append(",[ca_Name2]");
                str.Append(",[ca_ConnectPerson]");
                str.Append(",[ca_vat]");
                str.Append(",[ca_CR]");
                str.Append(",[ca_Account]");
                str.Append(",[ca_Group]");
                str.Append(",[ca_Activity]");
                str.Append(",[ca_Phone]");
                str.Append(",[ca_Mobile]");
                str.Append(",[ca_Fax]");
                str.Append(",[ca_Email]");
                str.Append(",[ca_Websit]");
                str.Append(",[ca_Address]");
                str.Append(",[ca_City]");
                str.Append(",[ca_LimitDebit]");
                str.Append(",[ca_LimitCredit]");
                str.Append(",[ca_Disable]");
                str.Append(",[ca_DueDays]");
                str.Append(",[ca_OpenSales]");
                str.Append(",[ca_OpenPurchasing]");
                str.Append(",[ca_Prefix]");
                str.Append(",[ca_Source]");
                str.Append(",[ca_Status])");
                str.Append(" VALUES ");
                str.Append("(@ca_Key");
                str.Append(",@ca_CreateUser");
                str.Append(",@ca_CreateDate");
                str.Append(",@ca_LastupUser");
                str.Append(",@ca_LastupDate");
                str.Append(",@ca_Parent");
                str.Append(",@ca_Kind");
                str.Append(",@ca_Code");
                str.Append(",@ca_Name1");
                str.Append(",@ca_Name2");
                str.Append(",@ca_ConnectPerson");
                str.Append(",@ca_vat");
                str.Append(",@ca_CR");
                str.Append(",@ca_Account");
                str.Append(",@ca_Group");
                str.Append(",@ca_Activity");
                str.Append(",@ca_Phone");
                str.Append(",@ca_Mobile");
                str.Append(",@ca_Fax");
                str.Append(",@ca_Email");
                str.Append(",@ca_Websit");
                str.Append(",@ca_Address");
                str.Append(",@ca_City");
                str.Append(",@ca_LimitDebit");
                str.Append(",@ca_LimitCredit");
                str.Append(",@ca_Disable");
                str.Append(",@ca_DueDays");
                str.Append(",@ca_OpenSales");
                str.Append(",@ca_OpenPurchasing");
                str.Append(",@ca_Prefix");
                str.Append(",@ca_Source");
                str.Append(",@ca_Status)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@ca_Key", SqlDbType.UniqueIdentifier).Value = key;
                comm.Parameters.Add("@ca_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@ca_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@ca_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@ca_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@ca_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Parent);
                comm.Parameters.Add("@ca_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@ca_Code", SqlDbType.NVarChar, 100).Value = item.Code ?? "";
                comm.Parameters.Add("@ca_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@ca_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@ca_ConnectPerson", SqlDbType.NVarChar, 200).Value = item.ConnectPerson ?? "";
                comm.Parameters.Add("@ca_vat", SqlDbType.NVarChar, 15).Value = item.vat ?? "";
                comm.Parameters.Add("@ca_CR", SqlDbType.NVarChar, 15).Value = item.CR ?? "";
                comm.Parameters.Add("@ca_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(account);
                comm.Parameters.Add("@ca_Group", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Group);
                comm.Parameters.Add("@ca_Activity", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Activity);
                comm.Parameters.Add("@ca_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@ca_Mobile", SqlDbType.NVarChar, 15).Value = item.Mobile ?? "";
                comm.Parameters.Add("@ca_Fax", SqlDbType.NVarChar, 15).Value = item.Fax ?? "";
                comm.Parameters.Add("@ca_Email", SqlDbType.NVarChar, 200).Value = item.Email ?? "";
                comm.Parameters.Add("@ca_Websit", SqlDbType.NVarChar, 100).Value = item.Websit ?? "";
                comm.Parameters.Add("@ca_Address", SqlDbType.NVarChar, 255).Value = item.Address ?? "";
                comm.Parameters.Add("@ca_City", SqlDbType.NVarChar, 15).Value = item.City ?? "";
                comm.Parameters.Add("@ca_LimitDebit", SqlDbType.Decimal).Value = item.LimitDebit;
                comm.Parameters.Add("@ca_LimitCredit", SqlDbType.Decimal).Value = item.LimitCredit;
                comm.Parameters.Add("@ca_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@ca_DueDays", SqlDbType.Int).Value = item.DueDays;
                comm.Parameters.Add("@ca_OpenSales", SqlDbType.Bit).Value = item.OpenSales;
                comm.Parameters.Add("@ca_OpenPurchasing", SqlDbType.Bit).Value = item.OpenPurchasing;
                comm.Parameters.Add("@ca_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.SalesPerson);
                comm.Parameters.Add("@ca_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Prefix);
                comm.Parameters.Add("@ca_Source", SqlDbType.Int).Value = 0;
                comm.Parameters.Add("@ca_Status", SqlDbType.Int).Value = 0;
                con.Open();
                comm.ExecuteNonQuery();
            }
            return key;
        }

        public static Guid? Update(string DB, CurrentAccount item)
        {
            CLiCore.Platx.Account parentdetails = new CLiCore.Platx.Account().GetItem(DB, item.Parent);
            if (parentdetails.CloseSubAccounts == false)
            {
                CLiCore.Platx.Account.Update(DB, item.Account, item.Name1, item.Name2, item.Disable);
            }
           
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update finCard_CurrentAccount SET ");
                str.Append("[ca_CreateUser]=@ca_CreateUser");
                str.Append(",[ca_CreateDate]=@ca_CreateDate");
                str.Append(",[ca_LastupUser]=@ca_LastupUser");
                str.Append(",[ca_LastupDate]=@ca_LastupDate");
                str.Append(",[ca_Parent]=@ca_Parent");
                str.Append(",[ca_Kind]=@ca_Kind");
                str.Append(",[ca_Code]=@ca_Code");
                str.Append(",[ca_Name1]=@ca_Name1");
                str.Append(",[ca_Name2]=@ca_Name2");
                str.Append(",[ca_ConnectPerson]=@ca_ConnectPerson");
                str.Append(",[ca_vat]=@ca_vat");
                str.Append(",[ca_CR]=@ca_CR");
                str.Append(",[ca_Account]=@ca_Account");
                str.Append(",[ca_Group]=@ca_Group");
                str.Append(",[ca_Activity]=@ca_Activity");
                str.Append(",[ca_Phone]=@ca_Phone");
                str.Append(",[ca_Mobile]=@ca_Mobile");
                str.Append(",[ca_Fax]=@ca_Fax");
                str.Append(",[ca_Email]=@ca_Email");
                str.Append(",[ca_Websit]=@ca_Websit");
                str.Append(",[ca_Address]=@ca_Address");
                str.Append(",[ca_City]=@ca_City");
                str.Append(",[ca_LimitDebit]=@ca_LimitDebit");
                str.Append(",[ca_LimitCredit]=@ca_LimitCredit");
                str.Append(",[ca_Disable]=@ca_Disable");
                str.Append(",[ca_DueDays]=@ca_DueDays");
                str.Append(",[ca_OpenSales]=@ca_OpenSales");
                str.Append(",[ca_OpenPurchasing]=@ca_OpenPurchasing");
                str.Append(",[ca_SalesPerson]=@ca_SalesPerson");
                str.Append(",[ca_Prefix]=@ca_Prefix");
                str.Append(" WHERE ca_Key=@ca_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@ca_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@ca_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@ca_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@ca_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@ca_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@ca_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Parent);
                comm.Parameters.Add("@ca_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@ca_Code", SqlDbType.NVarChar, 100).Value = item.Code ?? "";
                comm.Parameters.Add("@ca_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@ca_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@ca_ConnectPerson", SqlDbType.NVarChar, 200).Value = item.ConnectPerson ?? "";
                comm.Parameters.Add("@ca_vat", SqlDbType.NVarChar, 15).Value = item.vat ?? "";
                comm.Parameters.Add("@ca_CR", SqlDbType.NVarChar, 15).Value = item.CR ?? "";
                comm.Parameters.Add("@ca_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Account);
                comm.Parameters.Add("@ca_Group", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Group);
                comm.Parameters.Add("@ca_Activity", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Activity);
                comm.Parameters.Add("@ca_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@ca_Mobile", SqlDbType.NVarChar, 15).Value = item.Mobile ?? "";
                comm.Parameters.Add("@ca_Fax", SqlDbType.NVarChar, 15).Value = item.Fax ?? "";
                comm.Parameters.Add("@ca_Email", SqlDbType.NVarChar, 200).Value = item.Email ?? "";
                comm.Parameters.Add("@ca_Websit", SqlDbType.NVarChar, 100).Value = item.Websit ?? "";
                comm.Parameters.Add("@ca_Address", SqlDbType.NVarChar, 255).Value = item.Address ?? "";
                comm.Parameters.Add("@ca_City", SqlDbType.NVarChar, 15).Value = item.City ?? "";
                comm.Parameters.Add("@ca_LimitDebit", SqlDbType.Decimal).Value = item.LimitDebit;
                comm.Parameters.Add("@ca_LimitCredit", SqlDbType.Decimal).Value = item.LimitCredit;
                comm.Parameters.Add("@ca_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@ca_DueDays", SqlDbType.Int).Value = item.DueDays;
                comm.Parameters.Add("@ca_OpenSales", SqlDbType.Bit).Value = item.OpenSales;
                comm.Parameters.Add("@ca_OpenPurchasing", SqlDbType.Bit).Value = item.OpenPurchasing;
                comm.Parameters.Add("@ca_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.SalesPerson);
                comm.Parameters.Add("@ca_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Prefix);
                con.Open();
                comm.ExecuteNonQuery();
            }
            return item.Key;
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "EXEC dbo.spfinDelete_CurrentAccount @Key";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                res = comm.ExecuteNonQuery();
            }
            return res;
        }

    }
}
