using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiAccounting.Cards
{
    public class Project
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public DateTime? ContractDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Note { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public Boolean Disable { get; set; }
        public Guid? Client { get; set; }
        public decimal Price { get; set; }
        public Guid? Account { get; set; }
        public List<Project> GetList(string DB)
        {
            List<Project> items = new List<Project>();
            string selQuery = "select top 100 percent * from finCard_Project";
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
                    Project item = new Project();
                    item.Key = iCore.IsDbNullRtNull(reader["pjt_Key"]);
                    item.Code = Convert.ToString(reader["pjt_Code"]);
                    item.Name1 = Convert.ToString(reader["pjt_Name1"]);
                    item.Name2 = Convert.ToString(reader["pjt_Name2"]);
                    item.ContractDate = iCore.IsDbNullRtNullDate(reader["pjt_ContractDate"]);
                    item.StartDate = iCore.IsDbNullRtNullDate(reader["pjt_StartDate"]);
                    item.EndDate = iCore.IsDbNullRtNullDate(reader["pjt_EndDate"]);
                    item.Note = Convert.ToString(reader["pjt_Note"]);
                    item.Mobile = Convert.ToString(reader["pjt_Mobile"]);
                    item.Phone = Convert.ToString(reader["pjt_Phone"]);
                    item.ContactPerson = Convert.ToString(reader["pjt_ContactPerson"]);
                    item.Address = Convert.ToString(reader["pjt_Address"]);
                    item.Disable = Convert.ToBoolean(reader["pjt_Disable"]);
                    item.Client = iCore.IsDbNullRtNull(reader["pjt_Client"]);
                    item.Price = Convert.ToDecimal(reader["pjt_Price"]);
                    item.Account = iCore.IsDbNullRtNull(reader["pjt_Account"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public Project GetItem(string DB,Guid? Key)
        {
            Project item = new Project();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from finCard_Project where [pjt_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["pjt_Key"]);
                    item.Code = Convert.ToString(reader["pjt_Code"]);
                    item.Name1 = Convert.ToString(reader["pjt_Name1"]);
                    item.Name2 = Convert.ToString(reader["pjt_Name2"]);
                    item.ContractDate = iCore.IsDbNullRtNullDate(reader["pjt_ContractDate"]);
                    item.StartDate = iCore.IsDbNullRtNullDate(reader["pjt_StartDate"]);
                    item.EndDate = iCore.IsDbNullRtNullDate(reader["pjt_EndDate"]);
                    item.Note = Convert.ToString(reader["pjt_Note"]);
                    item.Mobile = Convert.ToString(reader["pjt_Mobile"]);
                    item.Phone = Convert.ToString(reader["pjt_Phone"]);
                    item.ContactPerson = Convert.ToString(reader["pjt_ContactPerson"]);
                    item.Address = Convert.ToString(reader["pjt_Address"]);
                    item.Disable = Convert.ToBoolean(reader["pjt_Disable"]);
                    item.Client = iCore.IsDbNullRtNull(reader["pjt_Client"]);
                    item.Price = Convert.ToDecimal(reader["pjt_Price"]);
                    item.Account = iCore.IsDbNullRtNull(reader["pjt_Account"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, Project item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO finCard_Project");
                str.Append("([pjt_Code]");
                str.Append(",[pjt_Name1]");
                str.Append(",[pjt_Name2]");
                str.Append(",[pjt_ContractDate]");
                str.Append(",[pjt_StartDate]");
                str.Append(",[pjt_EndDate]");
                str.Append(",[pjt_Note]");
                str.Append(",[pjt_Mobile]");
                str.Append(",[pjt_Phone]");
                str.Append(",[pjt_ContactPerson]");
                str.Append(",[pjt_Address]");
                str.Append(",[pjt_Disable]");
                str.Append(",[pjt_Client]");
                str.Append(",[pjt_Price]");
                str.Append(",[pjt_Account])");
                str.Append(" VALUES ");
                str.Append("(@pjt_Code");
                str.Append(",@pjt_Name1");
                str.Append(",@pjt_Name2");
                str.Append(",@pjt_ContractDate");
                str.Append(",@pjt_StartDate");
                str.Append(",@pjt_EndDate");
                str.Append(",@pjt_Note");
                str.Append(",@pjt_Mobile");
                str.Append(",@pjt_Phone");
                str.Append(",@pjt_ContactPerson");
                str.Append(",@pjt_Address");
                str.Append(",@pjt_Disable");
                str.Append(",@pjt_Client");
                str.Append(",@pjt_Price");
                str.Append(",@pjt_Account)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@pjt_Code", SqlDbType.NVarChar, 50).Value = item.Code ?? "";
                comm.Parameters.Add("@pjt_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@pjt_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@pjt_ContractDate", SqlDbType.Date).Value = item.ContractDate;
                comm.Parameters.Add("@pjt_StartDate", SqlDbType.Date).Value = item.StartDate;
                comm.Parameters.Add("@pjt_EndDate", SqlDbType.Date).Value = item.EndDate;
                comm.Parameters.Add("@pjt_Note", SqlDbType.NVarChar, 200).Value = item.Note ?? "";
                comm.Parameters.Add("@pjt_Mobile", SqlDbType.NVarChar, 15).Value = item.Mobile ?? "";
                comm.Parameters.Add("@pjt_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@pjt_ContactPerson", SqlDbType.NVarChar, 100).Value = item.ContactPerson ?? "";
                comm.Parameters.Add("@pjt_Address", SqlDbType.NVarChar, 500).Value = item.Address ?? "";
                comm.Parameters.Add("@pjt_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@pjt_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Client);
                comm.Parameters.Add("@pjt_Price", SqlDbType.Decimal).Value = item.Price;
                comm.Parameters.Add("@pjt_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Account);
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, Project item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update finCard_Project SET ");
                str.Append("[pjt_Code]=@pjt_Code");
                str.Append(",[pjt_Name1]=@pjt_Name1");
                str.Append(",[pjt_Name2]=@pjt_Name2");
                str.Append(",[pjt_ContractDate]=@pjt_ContractDate");
                str.Append(",[pjt_StartDate]=@pjt_StartDate");
                str.Append(",[pjt_EndDate]=@pjt_EndDate");
                str.Append(",[pjt_Note]=@pjt_Note");
                str.Append(",[pjt_Mobile]=@pjt_Mobile");
                str.Append(",[pjt_Phone]=@pjt_Phone");
                str.Append(",[pjt_ContactPerson]=@pjt_ContactPerson");
                str.Append(",[pjt_Address]=@pjt_Address");
                str.Append(",[pjt_Disable]=@pjt_Disable");
                str.Append(",[pjt_Client]=@pjt_Client");
                str.Append(",[pjt_Price]=@pjt_Price");
                str.Append(",[pjt_Account]=@pjt_Account");
                str.Append(" WHERE pjt_Key=@pjt_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@pjt_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@pjt_Code", SqlDbType.NVarChar, 50).Value = item.Code ?? "";
                comm.Parameters.Add("@pjt_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@pjt_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@pjt_ContractDate", SqlDbType.Date).Value = item.ContractDate;
                comm.Parameters.Add("@pjt_StartDate", SqlDbType.Date).Value = item.StartDate;
                comm.Parameters.Add("@pjt_EndDate", SqlDbType.Date).Value = item.EndDate;
                comm.Parameters.Add("@pjt_Note", SqlDbType.NVarChar, 200).Value = item.Note ?? "";
                comm.Parameters.Add("@pjt_Mobile", SqlDbType.NVarChar, 15).Value = item.Mobile ?? "";
                comm.Parameters.Add("@pjt_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@pjt_ContactPerson", SqlDbType.NVarChar, 100).Value = item.ContactPerson ?? "";
                comm.Parameters.Add("@pjt_Address", SqlDbType.NVarChar, 500).Value = item.Address ?? "";
                comm.Parameters.Add("@pjt_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@pjt_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Client);
                comm.Parameters.Add("@pjt_Price", SqlDbType.Decimal).Value = item.Price;
                comm.Parameters.Add("@pjt_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Account);
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [finCard_Project] where [pjt_Key]=@Key";
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
