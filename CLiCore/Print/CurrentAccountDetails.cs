using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore.Print
{
    public class CurrentAccountDetails
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string ConnectPerson { get; set; }
        public string vat { get; set; }
        public string CR { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int DueDays { get; set; }

        public CurrentAccountDetails GetItem(string DB,Guid? Key)
        {
            CurrentAccountDetails item = new CurrentAccountDetails();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from dbo.Print_CurrentDetails(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key",SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.ConnectPerson = Convert.ToString(reader["ConnectPerson"]);
                    item.vat = Convert.ToString(reader["vat"]);
                    item.CR = Convert.ToString(reader["CR"]);
                    item.Phone = Convert.ToString(reader["Phone"]);
                    item.Mobile = Convert.ToString(reader["Mobile"]);
                    item.Fax = Convert.ToString(reader["Fax"]);
                    item.Email = Convert.ToString(reader["Email"]);
                    item.Website = Convert.ToString(reader["Website"]);
                    item.Address = Convert.ToString(reader["Address"]);
                    item.City = Convert.ToString(reader["City"]);
                    item.DueDays = Convert.ToInt32(reader["DueDays"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
