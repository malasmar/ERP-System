using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLiCore;
using System.Data;
using System.Data.SqlClient;

namespace CLiCore.Configuration
{
    public class Profile
    {
        public Guid? Key { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string RC { get; set; }
        public string vatRegNo { get; set; }
        public string Commercial { get; set; }
        public string Economic { get; set; }
        public string Detail1 { get; set; }
        public string Detail2 { get; set; }
        public string Detail3 { get; set; }
        public string Detail4 { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Websit { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string Address_en { get; set; }
        public Profile GetItem(string DB)
        {
            Profile item = new Profile();
            string selQuery = "select top(1) * from com_Profile";
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
                    item.Key = iCore.IsDbNullRtNull(reader["com_Key"]);
                    item.Name1 = (string)reader["com_Name1"];
                    item.Name2 = (string)reader["com_Name2"];
                    item.RC = (string)reader["com_RC"];
                    item.vatRegNo = (string)reader["com_vatRegNo"];
                    item.Commercial = (string)reader["com_Commercial"];
                    item.Economic = (string)reader["com_Economic"];
                    item.Detail1 = (string)reader["com_Detail1"];
                    item.Detail2 = (string)reader["com_Detail2"];
                    item.Detail3 = (string)reader["com_Detail3"];
                    item.Detail4 = (string)reader["com_Detail4"];
                    item.Phone = (string)reader["com_Phone"];
                    item.Mobile = (string)reader["com_Mobile"];
                    item.Fax = (string)reader["com_Fax"];
                    item.Email = (string)reader["com_Email"];
                    item.Websit = (string)reader["com_Websit"];
                    item.Address = (string)reader["com_Address"];
                    item.Image = (string)reader["com_Image"];
                }
                reader.Close();
            }
            return item;
        }
        public static void Insert(string DB,Profile SHLL)
        {
            if (CheckCompany(DB) > 0)
            {
                Update(DB, SHLL);
                return;
            }
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                ScStr.Clear();
                ScStr.Append("INSERT INTO com_Profile");
                ScStr.Append("([com_Name1]");
                ScStr.Append(",[com_Name2]");
                ScStr.Append(",[com_RC]");
                ScStr.Append(",[com_vatRegNo]");
                ScStr.Append(",[com_Commercial]");
                ScStr.Append(",[com_Economic]");
                ScStr.Append(",[com_Detail1]");
                ScStr.Append(",[com_Detail2]");
                ScStr.Append(",[com_Detail3]");
                ScStr.Append(",[com_Detail4]");
                ScStr.Append(",[com_Phone]");
                ScStr.Append(",[com_Mobile]");
                ScStr.Append(",[com_Fax]");
                ScStr.Append(",[com_Email]");
                ScStr.Append(",[com_Websit]");
                ScStr.Append(",[com_Address]");
                ScStr.Append(",[com_Image]");
                ScStr.Append(",[com_Address_en])");
                ScStr.Append(" VALUES ");
                ScStr.Append("(@com_Name1");
                ScStr.Append(",@com_Name2");
                ScStr.Append(",@com_RC");
                ScStr.Append(",@com_vatRegNo");
                ScStr.Append(",@com_Commercial");
                ScStr.Append(",@com_Economic");
                ScStr.Append(",@com_Detail1");
                ScStr.Append(",@com_Detail2");
                ScStr.Append(",@com_Detail3");
                ScStr.Append(",@com_Detail4");
                ScStr.Append(",@com_Phone");
                ScStr.Append(",@com_Mobile");
                ScStr.Append(",@com_Fax");
                ScStr.Append(",@com_Email");
                ScStr.Append(",@com_Websit");
                ScStr.Append(",@com_Address");
                ScStr.Append(",@com_Image");
                ScStr.Append(",@com_Address_en)");
                SqlCommand ScCom = new SqlCommand();
                ScCom.Connection = con;
                ScCom.CommandType = CommandType.Text;
                ScCom.CommandText = ScStr.ToString();
                ScCom.Parameters.Clear();
                ScCom.Parameters.Add("@com_Name1", SqlDbType.NVarChar, 100).Value = SHLL.Name1 ?? "";
                ScCom.Parameters.Add("@com_Name2", SqlDbType.NVarChar, 100).Value = SHLL.Name2 ?? "";
                ScCom.Parameters.Add("@com_RC", SqlDbType.NVarChar, 25).Value = SHLL.RC ?? "";
                ScCom.Parameters.Add("@com_vatRegNo", SqlDbType.NVarChar, 100).Value = SHLL.vatRegNo ?? "";
                ScCom.Parameters.Add("@com_Commercial", SqlDbType.NVarChar, 25).Value = SHLL.Commercial ?? "";
                ScCom.Parameters.Add("@com_Economic", SqlDbType.NVarChar, 25).Value = SHLL.Economic ?? "";
                ScCom.Parameters.Add("@com_Detail1", SqlDbType.NVarChar, 25).Value = SHLL.Detail1 ?? "";
                ScCom.Parameters.Add("@com_Detail2", SqlDbType.NVarChar, 25).Value = SHLL.Detail2 ?? "";
                ScCom.Parameters.Add("@com_Detail3", SqlDbType.NVarChar, 25).Value = SHLL.Detail3 ?? "";
                ScCom.Parameters.Add("@com_Detail4", SqlDbType.NVarChar, 255).Value = SHLL.Detail4 ?? "";
                ScCom.Parameters.Add("@com_Phone", SqlDbType.NVarChar, 15).Value = SHLL.Phone ?? "";
                ScCom.Parameters.Add("@com_Mobile", SqlDbType.NVarChar, 15).Value = SHLL.Mobile ?? "";
                ScCom.Parameters.Add("@com_Fax", SqlDbType.NVarChar, 15).Value = SHLL.Fax ?? "";
                ScCom.Parameters.Add("@com_Email", SqlDbType.NVarChar, 50).Value = SHLL.Email ?? "";
                ScCom.Parameters.Add("@com_Websit", SqlDbType.NVarChar, 50).Value = SHLL.Websit ?? "";
                ScCom.Parameters.Add("@com_Address", SqlDbType.NVarChar, 500).Value = SHLL.Address ?? "";
                ScCom.Parameters.Add("@com_Image", SqlDbType.NVarChar, 500).Value = SHLL.Image ?? "";
                ScCom.Parameters.Add("@com_Address_en", SqlDbType.NVarChar, 500).Value = SHLL.Address_en ?? "";
                con.Open();
                ScCom.ExecuteNonQuery();
            }
        }
        public static void Update(string DB,Profile SHLL)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                ScStr.Clear();
                ScStr.Append("UPDATE [com_Profile] SET ");
                ScStr.Append("[com_Name1] =@com_Name1");
                ScStr.Append(",[com_Name2] =@com_Name2");
                ScStr.Append(",[com_RC] =@com_RC");
                ScStr.Append(",[com_vatRegNo] =@com_vatRegNo");
                ScStr.Append(",[com_Commercial] =@com_Commercial");
                ScStr.Append(",[com_Economic] =@com_Economic");
                ScStr.Append(",[com_Detail1] =@com_Detail1");
                ScStr.Append(",[com_Detail2] =@com_Detail2");
                ScStr.Append(",[com_Detail3] =@com_Detail3");
                ScStr.Append(",[com_Detail4] =@com_Detail4");
                ScStr.Append(",[com_Phone] =@com_Phone");
                ScStr.Append(",[com_Mobile] =@com_Mobile");
                ScStr.Append(",[com_Fax] =@com_Fax");
                ScStr.Append(",[com_Email] =@com_Email");
                ScStr.Append(",[com_Websit] =@com_Websit");
                ScStr.Append(",[com_Address] =@com_Address");
                ScStr.Append(",[com_Address_en] =@com_Address_en");
                SqlCommand ScCom = new SqlCommand();
                ScCom = new SqlCommand();
                ScCom.Connection = con;
                ScCom.CommandType = CommandType.Text;
                ScCom.CommandText = ScStr.ToString();
                ScCom.Parameters.Clear();
                ScCom.Parameters.Add("@com_Name1", SqlDbType.NVarChar, 100).Value = SHLL.Name1 ?? "";
                ScCom.Parameters.Add("@com_Name2", SqlDbType.NVarChar, 100).Value = SHLL.Name2 ?? "";
                ScCom.Parameters.Add("@com_RC", SqlDbType.NVarChar, 25).Value = SHLL.RC ?? "";
                ScCom.Parameters.Add("@com_vatRegNo", SqlDbType.NVarChar, 100).Value = SHLL.vatRegNo ?? "";
                ScCom.Parameters.Add("@com_Commercial", SqlDbType.NVarChar, 25).Value = SHLL.Commercial ?? "";
                ScCom.Parameters.Add("@com_Economic", SqlDbType.NVarChar, 25).Value = SHLL.Economic ?? "";
                ScCom.Parameters.Add("@com_Detail1", SqlDbType.NVarChar, 25).Value = SHLL.Detail1 ?? "";
                ScCom.Parameters.Add("@com_Detail2", SqlDbType.NVarChar, 25).Value = SHLL.Detail2 ?? "";
                ScCom.Parameters.Add("@com_Detail3", SqlDbType.NVarChar, 25).Value = SHLL.Detail3 ?? "";
                ScCom.Parameters.Add("@com_Detail4", SqlDbType.NVarChar, 255).Value = SHLL.Detail4 ?? "";
                ScCom.Parameters.Add("@com_Phone", SqlDbType.NVarChar, 15).Value = SHLL.Phone ?? "";
                ScCom.Parameters.Add("@com_Mobile", SqlDbType.NVarChar, 15).Value = SHLL.Mobile ?? "";
                ScCom.Parameters.Add("@com_Fax", SqlDbType.NVarChar, 15).Value = SHLL.Fax ?? "";
                ScCom.Parameters.Add("@com_Email", SqlDbType.NVarChar, 50).Value = SHLL.Email ?? "";
                ScCom.Parameters.Add("@com_Websit", SqlDbType.NVarChar, 50).Value = SHLL.Websit ?? "";
                ScCom.Parameters.Add("@com_Address", SqlDbType.NVarChar, 255).Value = SHLL.Address ?? "";
                ScCom.Parameters.Add("@com_Address_en", SqlDbType.NVarChar, 500).Value = SHLL.Address_en ?? "";
                con.Open();
                ScCom.ExecuteNonQuery();
            }
        }
        public static void UpdateLogo(string DB, string ImageName, byte[] Iamge)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                ScStr.Clear();
                ScStr.Append("UPDATE [com_Profile] SET ");
                ScStr.Append(" [com_Logo] =@com_Logo");
                ScStr.Append(",[com_Image] =@com_Image");
                SqlCommand ScCom = new SqlCommand();
                ScCom = new SqlCommand();
                ScCom.Connection = con;
                ScCom.CommandType = CommandType.Text;
                ScCom.CommandText = ScStr.ToString();
                ScCom.Parameters.Clear();
                ScCom.Parameters.Add("@com_Logo", SqlDbType.Image).Value = Iamge;
                ScCom.Parameters.Add("@com_Image", SqlDbType.NVarChar, 500).Value = ImageName ?? "";
                con.Open();
                ScCom.ExecuteNonQuery();
            }
        }
        public static int CheckCompany(string DB)
        {
            int Res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "select count(*) from [dbo].com_Profile";
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
                    Res = (int)reader[0];
                }
                reader.Close();
            }
            return Res;
        }
    }
}
