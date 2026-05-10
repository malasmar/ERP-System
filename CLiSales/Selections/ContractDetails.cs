using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiSales.Selections
{
    public class ContractDetails
    {
        public int RecNo { get; set; }
        public Guid? Key { get; set; }
        public int Index { get; set; }
        public int ItemKind { get; set; }
        public Guid? ItemKey { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal vatAmount { get; set; }
        public Guid? vatKey { get; set; }
        public decimal vatRate { get; set; }
        public decimal Total { get; set; }
        public decimal ReturnedAmount { get; set; }
        public List<ContractDetails> GetList(string DB,Guid? Key)
        {
            List<ContractDetails> items = new List<ContractDetails>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fnSalesSelection_ContractDetails(@Key) order by [Index] ";
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
                    ContractDetails item = new ContractDetails();
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Index = Convert.ToInt32(reader["Index"]);
                    item.ItemKind = Convert.ToInt32(reader["ItemKind"]);
                    item.ItemKey = iCore.IsDbNullRtNull(reader["ItemKey"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
                    item.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.Discount = Convert.ToDecimal(reader["Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["vatKey"]);
                    item.vatRate = Convert.ToDecimal(reader["vatRate"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.ReturnedAmount = Convert.ToDecimal(reader["ReturnedAmount"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
