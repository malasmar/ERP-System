using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.FixedAssets.Operation
{
    public class BookValuesDetails
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public Guid? Key { get; set; }
        public int Index { get; set; }
        public Guid? Fixture { get; set; }
        public DateTime? PurDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public string Description { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public JsonData.AccountDetails jnFixture { get; set; }
        public JsonData.CostCenter jnCostCenter { get; set; }
        public JsonData.Project jnProject { get; set; }
        public string strPurDate { get; set; }
        public List<BookValuesDetails> GetList(string DB,string xLan,Guid? Key)
        {
            List<BookValuesDetails> items = new List<BookValuesDetails>();
            string selQuery = "select top 100 percent * from finFixedAssets_BookValuesDetails where [fxd_OperationKey]=@Key order by [fxd_Index] ";
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
                    BookValuesDetails item = new BookValuesDetails();
                    item.RecNo = Convert.ToInt32(reader["fxd_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["fxd_OperationKey"]);
                    item.Key = iCore.IsDbNullRtNull(reader["fxd_Key"]);
                    item.Index = Convert.ToInt32(reader["fxd_Index"]);
                    item.Fixture = iCore.IsDbNullRtNull(reader["fxd_Fixture"]);
                    item.PurDate = iCore.IsDbNullRtNullDate(reader["fxd_PurDate"]);
                    item.Quantity = Convert.ToDecimal(reader["fxd_Quantity"]);
                    item.UnitPrice = Convert.ToDecimal(reader["fxd_UnitPrice"]);
                    item.Total = Convert.ToDecimal(reader["fxd_Total"]);
                    item.Description = Convert.ToString(reader["fxd_Description"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["fxd_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["fxd_Project"]);
                    item.jnFixture = new JsonData.AccountDetails().GetItem(DB, xLan, item.Fixture);
                    item.jnCostCenter = new JsonData.CostCenter().GetItem(DB, xLan, item.CostCenter);
                    item.jnProject = new JsonData.Project().GetItem(DB, xLan, item.Project);
                    item.strPurDate = item.PurDate.Value.ToString("dd.MM.yyyy");
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
