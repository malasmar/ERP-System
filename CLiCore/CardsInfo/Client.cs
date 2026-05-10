using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiCore.CardsInfo
{
    public class Client
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
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Display { get; set; }

        public Client GetItem(string DB,string xLan, Guid? Key)
        {
            Client item = new Client();
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
                    item.Code = Convert.ToString(reader["ca_Code"]);
                    item.Name1 = Convert.ToString(reader["ca_Name1"]);
                    item.Name2 = Convert.ToString(reader["ca_Name2"]);
                    item.ConnectPerson = Convert.ToString(reader["ca_ConnectPerson"]);
                    item.vat = Convert.ToString(reader["ca_vat"]);
                    item.CR = Convert.ToString(reader["ca_CR"]);
                    item.Phone = Convert.ToString(reader["ca_Phone"]);
                    item.Mobile = Convert.ToString(reader["ca_Mobile"]);
                    item.Email = Convert.ToString(reader["ca_Email"]);
                    item.Address = Convert.ToString(reader["ca_Address"]);
                    item.City = Convert.ToString(reader["ca_City"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2);
                            break;
                        case "ar":
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1);
                            break;
                        default:
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1);
                            break;
                    }
                }
                reader.Close();
            }
            return item;
        }
    }
}
