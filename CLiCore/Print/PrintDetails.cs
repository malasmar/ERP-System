using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore.Print
{
    public class PrintDetails
    {
        public Guid? OperationKey { get; set; }
        public Guid? Bank { get; set; }
        public string arSignature { get; set; }
        public string enSignature { get; set; }
        public string SigPostion { get; set; }
        public string arApproved { get; set; }
        public string enApproved { get; set; }
        public string AppPostion { get; set; }

        public string Template { get; set; }
        public PrintDetails GetList(string DB, Guid? Key)
        {
            PrintDetails item = new PrintDetails();
            string selQuery = "select top 100 percent * from SalesDocument_PrintDetails where [spd_OperationKey]=@Key ";
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

                    item.OperationKey = iCore.IsDbNullRtNull(reader["spd_OperationKey"]);
                    item.Bank = iCore.IsDbNullRtNull(reader["spd_Bank"]);
                    item.arSignature = (string)reader["spd_arSignature"];
                    item.enSignature = (string)reader["spd_enSignature"];
                    item.SigPostion = (string)reader["spd_SigPostion"];
                    item.arApproved = (string)reader["spd_arApproved"];
                    item.enApproved = (string)reader["spd_enApproved"];
                    item.AppPostion = (string)reader["spd_AppPostion"];

                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, PrintDetails item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                ScStr.Clear();
                ScStr.Append("delete from SalesDocument_PrintDetails where [spd_OperationKey]=@spd_OperationKey ");
                ScStr.Append("INSERT INTO SalesDocument_PrintDetails ");
                ScStr.Append("([spd_OperationKey]");
                ScStr.Append(",[spd_Bank]");
                ScStr.Append(",[spd_arSignature]");
                ScStr.Append(",[spd_enSignature]");
                ScStr.Append(",[spd_SigPostion]");
                ScStr.Append(",[spd_arApproved]");
                ScStr.Append(",[spd_enApproved]");
                ScStr.Append(",[spd_AppPostion])");
                ScStr.Append(" VALUES ");
                ScStr.Append("(@spd_OperationKey");
                ScStr.Append(",@spd_Bank");
                ScStr.Append(",@spd_arSignature");
                ScStr.Append(",@spd_enSignature");
                ScStr.Append(",@spd_SigPostion");
                ScStr.Append(",@spd_arApproved");
                ScStr.Append(",@spd_enApproved");
                ScStr.Append(",@spd_AppPostion)");
                SqlCommand ScCom = new SqlCommand();
                ScCom = new SqlCommand();
                ScCom.Connection = con;
                ScCom.CommandType = CommandType.Text;
                ScCom.CommandText = ScStr.ToString();
                ScCom.Parameters.Clear();
                ScCom.Parameters.Add("@spd_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.OperationKey);
                ScCom.Parameters.Add("@spd_Bank", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Bank);
                ScCom.Parameters.Add("@spd_arSignature", SqlDbType.NVarChar, 200).Value = item.arSignature ?? "";
                ScCom.Parameters.Add("@spd_enSignature", SqlDbType.NVarChar, 200).Value = item.enSignature ?? "";
                ScCom.Parameters.Add("@spd_SigPostion", SqlDbType.NVarChar, 200).Value = item.SigPostion ?? "";
                ScCom.Parameters.Add("@spd_arApproved", SqlDbType.NVarChar, 200).Value = item.arApproved ?? "";
                ScCom.Parameters.Add("@spd_enApproved", SqlDbType.NVarChar, 200).Value = item.enApproved ?? "";
                ScCom.Parameters.Add("@spd_AppPostion", SqlDbType.NVarChar, 200).Value = item.AppPostion ?? "";
                con.Open();
                ScCom.ExecuteNonQuery();
            }
        }
    }
}
