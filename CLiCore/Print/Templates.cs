using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CLiCore.Print
{
    public class Templates
    {
        public int RecNo { get; set; }
        public int Dockind { get; set; }
        public int No { get; set; }
        public string Name { get; set; }
        public Boolean Default { get; set; }
        public string FileName { get; set; }
        public Boolean Disable { get; set; }

        public List<Templates> GetList(string DB,int DocKind)
        {
            List<Templates> items = new List<Templates>();
            string selQuery = "select top 100 percent * from [system_DocumentTemplate] where [sys_Dockind]=@DocKind order by [sys_No]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@DocKind", SqlDbType.Int).Value = DocKind;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Templates item = new Templates();
                    item.RecNo = Convert.ToInt32(reader["sys_RecNo"]);
                    item.Dockind = Convert.ToInt32(reader["sys_Dockind"]);
                    item.No = Convert.ToInt32(reader["sys_No"]);
                    item.Name = Convert.ToString(reader["sys_Name"]);
                    item.Default = Convert.ToBoolean(reader["sys_Default"]);
                    item.FileName = Convert.ToString(reader["sys_FileName"]);
                    item.Disable = Convert.ToBoolean(reader["sys_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
