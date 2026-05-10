using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.Selections
{
    public class StructureEmployee
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Image { get; set; }
        public List<StructureEmployee> GetList(string DB,Guid? Key)
        {
            List<StructureEmployee> items = new List<StructureEmployee>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_Selection_StructureEmployee(@Key) order by [Code]";
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
                    StructureEmployee item = new StructureEmployee();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"])??"";
                    item.Name1 = Convert.ToString(reader["Name1"]) ?? "";
                    item.Name2 = Convert.ToString(reader["Name2"]) ?? "";
                    item.Image = Convert.ToString(reader["Image"]) ?? "";
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

    }
}
