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
    public class GeneralLedgerDetails
    {
        public Guid? Key { get; set; }
        public Guid? OperationKey { get; set; }
        public int Index { get; set; }
        public Guid? Account { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public JsonData.Account jnAccount {get;set;}
        public JsonData.CostCenter jnCostCenter { get;set;}
        public JsonData.Project jnProject { get; set; }


        public List<GeneralLedgerDetails> GetList(string DB,string xLan,Guid? Key)
        {
            List<GeneralLedgerDetails> items = new List<GeneralLedgerDetails>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fnaccDocuments_GeneralLedgerDetails(@Key) order by [Index] ";
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
                    GeneralLedgerDetails item = new GeneralLedgerDetails();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.Index = Convert.ToInt32(reader["Index"]);
                    item.Account = iCore.IsDbNullRtNull(reader["Account"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Project"]);
                    item.jnAccount = new JsonData.Account().GetItem(DB, xLan, item.Account);
                    item.jnCostCenter = new JsonData.CostCenter().GetItem(DB, xLan, item.CostCenter);
                    item.jnProject = new JsonData.Project().GetItem(DB, xLan, item.Project);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
