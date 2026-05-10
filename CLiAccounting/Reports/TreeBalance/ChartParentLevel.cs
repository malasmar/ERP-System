using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiAccounting.Reports.TreeBalance
{
    public class ChartParentLevel
    {
        public Guid? Key { get; set; }
        public string id { get; set; }
        //public string parent { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public menustate state { get; set; }

        public List<ChartParentLevel> children { get; set; }
        public List<ChartParentLevel> GetList(string DB, string Code, string xLan)
        {
            List<ChartParentLevel> items = new List<ChartParentLevel>();
            string selQuery = "SELECT TOP 100 PERCENT  * from dbo.fnaccChart_TreeParentLevel(@Code) order by [Code]";
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
                    ChartParentLevel item = new ChartParentLevel();
                    item.id = reader["Code"].ToString();
                    // item.parent = reader["file_Parent"].ToString();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    switch (xLan)
                    {
                        case "en":
                            item.text = ((string)reader["Name2"] == "" ? (string)reader["Name1"] : (string)reader["Name2"])  ;
                            break;
                        case "ar":
                            item.text = (string)reader["Name1"];
                            break;
                        default:
                            item.text = (string)reader["Name1"];
                            break;
                    }

                    item.state = new menustate() { selected = false, opened = false };
                    item.children = new ChartParentLevel().GetList(DB, item.id, xLan);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
