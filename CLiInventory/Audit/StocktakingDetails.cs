using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Audit
{
    public class StocktakingDetails
    {
        public int RecNo { get; set; }
        public Guid? Key { get; set; }
        public int Index { get; set; }
        public Guid? Item { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Balance { get; set; }
        public decimal Difference { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Cost { get; set; }
        public List<StocktakingDetails> GetList(string DB,Guid? Key)
        {
            List<StocktakingDetails> items = new List<StocktakingDetails>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fninvOperation_StocktakingOffset(@Key) order by [index]";
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
                    StocktakingDetails item = new StocktakingDetails();
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Index = Convert.ToInt32(reader["Index"]);
                    item.Item = iCore.IsDbNullRtNull(reader["Item"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.Balance = Convert.ToDecimal(reader["Balance"]);
                    item.Difference = Convert.ToDecimal(reader["Difference"]);
                    item.UnitCost = Convert.ToDecimal(reader["UnitCost"]);
                    item.Cost = item.Difference * item.UnitCost;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
