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
    public class BalanceItems
    {
        public Guid? Key { get; set; }
        public string id { get; set; }
        //public string parent { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public menustate state { get; set; }
     
        public List<BalanceItems> children { get; set; }
        public   List<BalanceItems> GetList(string DB, string Parent,string xLan)
        {
            List<BalanceItems> SHLL = new List<BalanceItems>();
            int i = 0;
            bool opened = false;
            string selQuery = "SELECT TOP 100 PERCENT  * from [accCard_BalanceSheetItems] where [bsi_Parent]=@Parent ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Parent", SqlDbType.NVarChar, 15).Value = Parent ?? "";
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    if (i == 0)
                        opened = true;

                    BalanceItems item = new BalanceItems();
                    item.id = reader["bsi_Code"].ToString();
                    // item.parent = reader["file_Parent"].ToString();
                    item.Key = iCore.IsDbNullRtNull(reader["bsi_Key"]);
                    switch (xLan)
                    {
                        case "en":
                            item.text = (string)reader["bsi_Name2"]==""? (string)reader["bsi_Name1"]: (string)reader["bsi_Name2"];
                            break;
                        case "ar":
                            item.text = (string)reader["bsi_Name1"];
                            break;
                        default:
                            item.text = (string)reader["bsi_Name1"];
                            break;
                    }
                
                    item.state = new menustate() { selected = false, opened = opened };
                    item.children = new BalanceItems().GetList(DB, item.id,xLan);
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
