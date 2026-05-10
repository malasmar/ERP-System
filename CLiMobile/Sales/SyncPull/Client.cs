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
    public class Client
    {
         public Guid Key { get; set; }
        public Guid? Category { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Person { get; set; }
        public string vat { get; set; }
        public string CR { get; set; }
        public Guid? Activity { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Guid? City { get; set; }
        public string Address { get; set; }
        public static int Insert(string DB, Client item,Guid User)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO AppSales_Client ");
                str.Append("([ca_Key]");
                str.Append(",[ca_Category]");
                str.Append(",[ca_CreateUser]");
                str.Append(",[ca_CreateDate]");
                str.Append(",[ca_Name1]");
                str.Append(",[ca_Name2]");
                str.Append(",[ca_Person]");
                str.Append(",[ca_vat]");
                str.Append(",[ca_CR]");
                str.Append(",[ca_Activity]");
                str.Append(",[ca_Phone]");
                str.Append(",[ca_Mobile]");
                str.Append(",[ca_Email]");
                str.Append(",[ca_Address]");
                str.Append(",[ca_City]");
                str.Append(",[ca_User])");
                str.Append(" VALUES ");
                str.Append("(@ca_Key");
                str.Append(",@ca_Category");
                str.Append(",@ca_CreateUser");
                str.Append(",@ca_CreateDate");
                str.Append(",@ca_Name1");
                str.Append(",@ca_Name2");
                str.Append(",@ca_Person");
                str.Append(",@ca_vat");
                str.Append(",@ca_CR");
                str.Append(",@ca_Activity");
                str.Append(",@ca_Phone");
                str.Append(",@ca_Mobile");
                str.Append(",@ca_Email");
                str.Append(",@ca_Address");
                str.Append(",@ca_City");
                str.Append(",@ca_User)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@ca_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@ca_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Category);
                comm.Parameters.Add("@ca_CreateUser", SqlDbType.Int).Value = 0;
                comm.Parameters.Add("@ca_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                comm.Parameters.Add("@ca_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@ca_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@ca_Person", SqlDbType.NVarChar, 200).Value = item.Person ?? "";
                comm.Parameters.Add("@ca_vat", SqlDbType.NVarChar, 15).Value = item.vat ?? "";
                comm.Parameters.Add("@ca_CR", SqlDbType.NVarChar, 15).Value = item.CR ?? "";
                comm.Parameters.Add("@ca_Activity", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Activity);
                comm.Parameters.Add("@ca_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@ca_Mobile", SqlDbType.NVarChar, 15).Value = item.Mobile ?? "";
                comm.Parameters.Add("@ca_Email", SqlDbType.NVarChar, 200).Value = item.Email ?? "";
                comm.Parameters.Add("@ca_Address", SqlDbType.NVarChar, 255).Value = item.Address ?? "";
                comm.Parameters.Add("@ca_City", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.City);
                comm.Parameters.Add("@ca_User", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(User);
                con.Open();
               res= comm.ExecuteNonQuery();


                Guid? Parent = Platx.core.ClientCategoryParent(DB,item.Category);
                string code = xConfig.AccountCode(DB, Parent);
                Guid account = CLiCore.Platx.Account.Insert(DB, Parent, code, item.Name1, item.Name2);
               

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
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@ca_Key", SqlDbType.UniqueIdentifier).Value = item.Key;
                comm.Parameters.Add("@ca_CreateUser", SqlDbType.Int).Value = 0;
                comm.Parameters.Add("@ca_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                comm.Parameters.Add("@ca_LastupUser", SqlDbType.Int).Value = 0;
                comm.Parameters.Add("@ca_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(DateTime.Now);
                comm.Parameters.Add("@ca_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Parent);
                comm.Parameters.Add("@ca_Kind", SqlDbType.Int).Value = 0;
                comm.Parameters.Add("@ca_Code", SqlDbType.NVarChar, 100).Value = code ?? "";
                comm.Parameters.Add("@ca_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@ca_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@ca_ConnectPerson", SqlDbType.NVarChar, 200).Value = item.Person ?? "";
                comm.Parameters.Add("@ca_vat", SqlDbType.NVarChar, 15).Value = item.vat ?? "";
                comm.Parameters.Add("@ca_CR", SqlDbType.NVarChar, 15).Value = item.CR ?? "";
                comm.Parameters.Add("@ca_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(account);
                comm.Parameters.Add("@ca_Group", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                comm.Parameters.Add("@ca_Activity", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Activity);
                comm.Parameters.Add("@ca_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@ca_Mobile", SqlDbType.NVarChar, 15).Value = item.Mobile ?? "";
                comm.Parameters.Add("@ca_Fax", SqlDbType.NVarChar, 15).Value = "";
                comm.Parameters.Add("@ca_Email", SqlDbType.NVarChar, 200).Value = item.Email ?? "";
                comm.Parameters.Add("@ca_Websit", SqlDbType.NVarChar, 100).Value = "";
                comm.Parameters.Add("@ca_Address", SqlDbType.NVarChar, 255).Value = item.Address ?? "";
                comm.Parameters.Add("@ca_City", SqlDbType.NVarChar, 15).Value = "";
                comm.Parameters.Add("@ca_LimitDebit", SqlDbType.Decimal).Value = 0;
                comm.Parameters.Add("@ca_LimitCredit", SqlDbType.Decimal).Value = 0;
                comm.Parameters.Add("@ca_Disable", SqlDbType.Bit).Value = false;
                comm.Parameters.Add("@ca_DueDays", SqlDbType.Int).Value = 30;
                comm.Parameters.Add("@ca_OpenSales", SqlDbType.Bit).Value = true;
                comm.Parameters.Add("@ca_OpenPurchasing", SqlDbType.Bit).Value = false;
                comm.Parameters.Add("@ca_SalesPerson", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(User);
                comm.Parameters.Add("@ca_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                comm.Parameters.Add("@ca_Source", SqlDbType.Int).Value = 1;
                comm.Parameters.Add("@ca_Status", SqlDbType.Int).Value = 0;
                comm.ExecuteNonQuery();
            }

           
           


            return res;
        }

    }
}
