using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiAccounting.Documents
{
    public class GeneralLedger
    {
        public Guid? OperationKey { get; set; }
        public DateTime? CreateDate { get; set; }
        public int DocumentKind { get; set; }
        public int No { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public GeneralLedger GetItem(string DB,Guid? Key)
        {
            GeneralLedger item = new GeneralLedger();
            item.No = core.MaxGeneralLedger(DB, DateTime.Now);

            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from dbo.fnaccDocuments_GeneralLedger(@Key) ";
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
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["CreateDate"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.No = Convert.ToInt32(reader["No"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                }
                reader.Close();
            }
            return item;
        }

    }
}
