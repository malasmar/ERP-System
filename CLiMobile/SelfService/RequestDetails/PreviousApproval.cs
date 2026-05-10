using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.RequestDetails
{
    public class PreviousApproval
    {
        public string StructureName1 { get; set; }
        public string StructureName2 { get; set; }
        public string ManagerName1 { get; set; }
        public string ManagerName2 { get; set; }
        public string Comment { get; set; }
        public int Status { get; set; }

        public PreviousApproval GetItem(string DB,Guid? Key)
        {
            PreviousApproval item = new PreviousApproval();
            if(Key == null)
                return item;
            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_PreviousApproval(@Key) ";
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
                    item.StructureName1 = Convert.ToString(reader["StructureName1"]);
                    item.StructureName2 = Convert.ToString(reader["StructureName2"]);
                    item.ManagerName1 = Convert.ToString(reader["ManagerName1"]);
                    item.ManagerName2 = Convert.ToString(reader["ManagerName2"]);
                    item.Comment = Convert.ToString(reader["Comment"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
