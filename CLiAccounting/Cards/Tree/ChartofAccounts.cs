using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
namespace CLiAccounting.Cards.Tree
{
    public class ChartofAccounts
    {
        public Guid? Key { get; set; }
        public string id { get; set; }
        //public string parent { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public menustate state { get; set; }

        public List<ChartofAccounts> children { get; set; }
        public List<ChartofAccounts> GetList(string DB, string Code, string xLan)
        {
            List<ChartofAccounts> SHLL = new List<ChartofAccounts>();
            int i = 0;
            bool opened = false;
            string selQuery = "SELECT TOP 100 PERCENT  * from dbo.fnaccCards_ChartOfAccount(@Code) order by [Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Code", SqlDbType.NVarChar, 15).Value = Code ?? "";
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    if (i == 0)
                        opened = true;

                    ChartofAccounts item = new ChartofAccounts();
                    item.id = reader["Code"].ToString();
                    // item.parent = reader["file_Parent"].ToString();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    switch (xLan)
                    {
                        case "en":
                            item.text = ((string)reader["Name2"] == "" ? (string)reader["Name1"] : (string)reader["Name2"]) + " (" + Convert.ToString(reader["Childe"]) + " Childes)";
                            break;
                        case "ar":
                            item.text = (string)reader["Name1"] + " (" + Convert.ToString(reader["Childe"]) + " Childes)";
                            break;
                        default:
                            item.text = (string)reader["Name1"] + " (" + Convert.ToString(reader["Childe"]) + " Childes)";
                            break;
                    }

                    item.state = new menustate() { selected = false, opened = opened };
                    item.children = new ChartofAccounts().GetList(DB, item.id, xLan);
                    opened = false;
                    ++i;
                    SHLL.Add(item);
                }
                reader.Close();
            }
            return SHLL;
        }
    }
}
