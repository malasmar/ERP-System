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
    public class Bank
    {
        public Guid? Key { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public Guid? Parent { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Branch { get; set; }
        public string SwiftCode { get; set; }
        public string IBAN { get; set; }
        public string AccountNo { get; set; }
        public string CustomerNo { get; set; }
        public Guid? Account { get; set; }
        public string ContactPerson1 { get; set; }
        public string ContactPerson2 { get; set; }
        public string Phone1 { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Websit { get; set; }
        public string Address { get; set; }
        public Boolean Disable { get; set; }
        public List<Bank> GetList(string DB)
        {
            List<Bank> items = new List<Bank>();
            string selQuery = "select top 100 percent * from finCard_Bank order by [bank_Code]";
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
                    Bank item = new Bank();
                    item.Key = iCore.IsDbNullRtNull(reader["bank_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["bank_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["bank_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["bank_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["bank_LastupDate"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["bank_Parent"]);
                    item.Code = Convert.ToString(reader["bank_Code"]);
                    item.Name1 = Convert.ToString(reader["bank_Name1"]);
                    item.Name2 = Convert.ToString(reader["bank_Name2"]);
                    item.Branch = Convert.ToString(reader["bank_Branch"]);
                    item.SwiftCode = Convert.ToString(reader["bank_SwiftCode"]);
                    item.IBAN = Convert.ToString(reader["bank_IBAN"]);
                    item.AccountNo = Convert.ToString(reader["bank_AccountNo"]);
                    item.CustomerNo = Convert.ToString(reader["bank_CustomerNo"]);
                    item.Account = iCore.IsDbNullRtNull(reader["bank_Account"]);
                    item.ContactPerson1 = Convert.ToString(reader["bank_ContactPerson1"]);
                    item.ContactPerson2 = Convert.ToString(reader["bank_ContactPerson2"]);
                    item.Phone1 = Convert.ToString(reader["bank_Phone1"]);
                    item.Mobile = Convert.ToString(reader["bank_Mobile"]);
                    item.Fax = Convert.ToString(reader["bank_Fax"]);
                    item.Email = Convert.ToString(reader["bank_Email"]);
                    item.Websit = Convert.ToString(reader["bank_Websit"]);
                    item.Address = Convert.ToString(reader["bank_Address"]);
                    item.Disable = Convert.ToBoolean(reader["bank_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public Bank GetItem(string DB, Guid? Key)
        {
            Bank item = new Bank();
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from finCard_Bank where bank_Key=@Key";
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
                    item.Key = iCore.IsDbNullRtNull(reader["bank_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["bank_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["bank_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["bank_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["bank_LastupDate"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["bank_Parent"]);
                    item.Code = Convert.ToString(reader["bank_Code"]);
                    item.Name1 = Convert.ToString(reader["bank_Name1"]);
                    item.Name2 = Convert.ToString(reader["bank_Name2"]);
                    item.Branch = Convert.ToString(reader["bank_Branch"]);
                    item.SwiftCode = Convert.ToString(reader["bank_SwiftCode"]);
                    item.IBAN = Convert.ToString(reader["bank_IBAN"]);
                    item.AccountNo = Convert.ToString(reader["bank_AccountNo"]);
                    item.CustomerNo = Convert.ToString(reader["bank_CustomerNo"]);
                    item.Account = iCore.IsDbNullRtNull(reader["bank_Account"]);
                    item.ContactPerson1 = Convert.ToString(reader["bank_ContactPerson1"]);
                    item.ContactPerson2 = Convert.ToString(reader["bank_ContactPerson2"]);
                    item.Phone1 = Convert.ToString(reader["bank_Phone1"]);
                    item.Mobile = Convert.ToString(reader["bank_Mobile"]);
                    item.Fax = Convert.ToString(reader["bank_Fax"]);
                    item.Email = Convert.ToString(reader["bank_Email"]);
                    item.Websit = Convert.ToString(reader["bank_Websit"]);
                    item.Address = Convert.ToString(reader["bank_Address"]);
                    item.Disable = Convert.ToBoolean(reader["bank_Disable"]);
                }
                reader.Close();
            }
            return item;
        }
        public static Guid Insert(string DB, Bank item)
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
        

            Guid key = Guid.NewGuid();
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO finCard_Bank");
                str.Append("([bank_Key]");
                str.Append(",[bank_CreateUser]");
                str.Append(",[bank_CreateDate]");
                str.Append(",[bank_LastupUser]");
                str.Append(",[bank_LastupDate]");
                str.Append(",[bank_Parent]");
                str.Append(",[bank_Code]");
                str.Append(",[bank_Name1]");
                str.Append(",[bank_Name2]");
                str.Append(",[bank_Branch]");
                str.Append(",[bank_SwiftCode]");
                str.Append(",[bank_IBAN]");
                str.Append(",[bank_AccountNo]");
                str.Append(",[bank_CustomerNo]");
                str.Append(",[bank_Account]");
                str.Append(",[bank_ContactPerson1]");
                str.Append(",[bank_ContactPerson2]");
                str.Append(",[bank_Phone1]");
                str.Append(",[bank_Mobile]");
                str.Append(",[bank_Fax]");
                str.Append(",[bank_Email]");
                str.Append(",[bank_Websit]");
                str.Append(",[bank_Disable])");
                str.Append(" VALUES ");
                str.Append("(@bank_Key");
                str.Append(",@bank_CreateUser");
                str.Append(",@bank_CreateDate");
                str.Append(",@bank_LastupUser");
                str.Append(",@bank_LastupDate");
                str.Append(",@bank_Parent");
                str.Append(",@bank_Code");
                str.Append(",@bank_Name1");
                str.Append(",@bank_Name2");
                str.Append(",@bank_Branch");
                str.Append(",@bank_SwiftCode");
                str.Append(",@bank_IBAN");
                str.Append(",@bank_AccountNo");
                str.Append(",@bank_CustomerNo");
                str.Append(",@bank_Account");
                str.Append(",@bank_ContactPerson1");
                str.Append(",@bank_ContactPerson2");
                str.Append(",@bank_Phone1");
                str.Append(",@bank_Mobile");
                str.Append(",@bank_Fax");
                str.Append(",@bank_Email");
                str.Append(",@bank_Websit");
                str.Append(",@bank_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@bank_Key", SqlDbType.UniqueIdentifier).Value = key;
                comm.Parameters.Add("@bank_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@bank_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@bank_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@bank_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@bank_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Parent);
                comm.Parameters.Add("@bank_Code", SqlDbType.NVarChar, 25).Value = item.Code ?? "";
                comm.Parameters.Add("@bank_Name1", SqlDbType.NVarChar, 100).Value = item.Name1 ?? "";
                comm.Parameters.Add("@bank_Name2", SqlDbType.NVarChar, 100).Value = item.Name2 ?? "";
                comm.Parameters.Add("@bank_Branch", SqlDbType.NVarChar, 100).Value = item.Branch ?? "";
                comm.Parameters.Add("@bank_SwiftCode", SqlDbType.NVarChar, 50).Value = item.SwiftCode ?? "";
                comm.Parameters.Add("@bank_IBAN", SqlDbType.NVarChar, 50).Value = item.IBAN ?? "";
                comm.Parameters.Add("@bank_AccountNo", SqlDbType.NVarChar, 50).Value = item.AccountNo ?? "";
                comm.Parameters.Add("@bank_CustomerNo", SqlDbType.NVarChar, 50).Value = item.CustomerNo ?? "";
                comm.Parameters.Add("@bank_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(account);
                comm.Parameters.Add("@bank_ContactPerson1", SqlDbType.NVarChar, 50).Value = item.ContactPerson1 ?? "";
                comm.Parameters.Add("@bank_ContactPerson2", SqlDbType.NVarChar, 50).Value = item.ContactPerson2 ?? "";
                comm.Parameters.Add("@bank_Phone1", SqlDbType.NVarChar, 15).Value = item.Phone1 ?? "";
                comm.Parameters.Add("@bank_Mobile", SqlDbType.NVarChar, 15).Value = item.Mobile ?? "";
                comm.Parameters.Add("@bank_Fax", SqlDbType.NVarChar, 15).Value = item.Fax ?? "";
                comm.Parameters.Add("@bank_Email", SqlDbType.NVarChar, 50).Value = item.Email ?? "";
                comm.Parameters.Add("@bank_Websit", SqlDbType.NVarChar, 50).Value = item.Websit ?? "";
                comm.Parameters.Add("@bank_Address", SqlDbType.NVarChar, 255).Value = item.Address ?? "";
                comm.Parameters.Add("@bank_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
            return key;
        }
        public static Guid? Update(string DB, Bank item)
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
                str.Append("Update finCard_Bank SET ");
                str.Append("[bank_CreateUser]=@bank_CreateUser");
                str.Append(",[bank_CreateDate]=@bank_CreateDate");
                str.Append(",[bank_LastupUser]=@bank_LastupUser");
                str.Append(",[bank_LastupDate]=@bank_LastupDate");
                str.Append(",[bank_Parent]=@bank_Parent");
                str.Append(",[bank_Code]=@bank_Code");
                str.Append(",[bank_Name1]=@bank_Name1");
                str.Append(",[bank_Name2]=@bank_Name2");
                str.Append(",[bank_Branch]=@bank_Branch");
                str.Append(",[bank_SwiftCode]=@bank_SwiftCode");
                str.Append(",[bank_IBAN]=@bank_IBAN");
                str.Append(",[bank_AccountNo]=@bank_AccountNo");
                str.Append(",[bank_CustomerNo]=@bank_CustomerNo");
                str.Append(",[bank_Account]=@bank_Account");
                str.Append(",[bank_ContactPerson1]=@bank_ContactPerson1");
                str.Append(",[bank_ContactPerson2]=@bank_ContactPerson2");
                str.Append(",[bank_Phone1]=@bank_Phone1");
                str.Append(",[bank_Mobile]=@bank_Mobile");
                str.Append(",[bank_Fax]=@bank_Fax");
                str.Append(",[bank_Email]=@bank_Email");
                str.Append(",[bank_Websit]=@bank_Websit");
                str.Append(",[bank_Address]=@bank_Address");
                str.Append(",[bank_Disable]=@bank_Disable");
                str.Append(" WHERE bank_Key=@bank_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@bank_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@bank_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@bank_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@bank_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@bank_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@bank_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Parent);
                comm.Parameters.Add("@bank_Code", SqlDbType.NVarChar, 25).Value = item.Code ?? "";
                comm.Parameters.Add("@bank_Name1", SqlDbType.NVarChar, 100).Value = item.Name1 ?? "";
                comm.Parameters.Add("@bank_Name2", SqlDbType.NVarChar, 100).Value = item.Name2 ?? "";
                comm.Parameters.Add("@bank_Branch", SqlDbType.NVarChar, 100).Value = item.Branch ?? "";
                comm.Parameters.Add("@bank_SwiftCode", SqlDbType.NVarChar, 50).Value = item.SwiftCode ?? "";
                comm.Parameters.Add("@bank_IBAN", SqlDbType.NVarChar, 50).Value = item.IBAN ?? "";
                comm.Parameters.Add("@bank_AccountNo", SqlDbType.NVarChar, 50).Value = item.AccountNo ?? "";
                comm.Parameters.Add("@bank_CustomerNo", SqlDbType.NVarChar, 50).Value = item.CustomerNo ?? "";
                comm.Parameters.Add("@bank_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Account);
                comm.Parameters.Add("@bank_ContactPerson1", SqlDbType.NVarChar, 50).Value = item.ContactPerson1 ?? "";
                comm.Parameters.Add("@bank_ContactPerson2", SqlDbType.NVarChar, 50).Value = item.ContactPerson2 ?? "";
                comm.Parameters.Add("@bank_Phone1", SqlDbType.NVarChar, 15).Value = item.Phone1 ?? "";
                comm.Parameters.Add("@bank_Mobile", SqlDbType.NVarChar, 15).Value = item.Mobile ?? "";
                comm.Parameters.Add("@bank_Fax", SqlDbType.NVarChar, 15).Value = item.Fax ?? "";
                comm.Parameters.Add("@bank_Email", SqlDbType.NVarChar, 50).Value = item.Email ?? "";
                comm.Parameters.Add("@bank_Websit", SqlDbType.NVarChar, 50).Value = item.Websit ?? "";
                comm.Parameters.Add("@bank_Address", SqlDbType.NVarChar, 255).Value = item.Address ?? "";
                comm.Parameters.Add("@bank_Disable", SqlDbType.Bit).Value = item.Disable;
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
                string delQuery = "EXEC dbo.spfinDelete_BankCard @Key";
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
