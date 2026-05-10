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
    public class ParentBalance
    {
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string parent { get; set; }
        public int Level { get; set; }
        public int Kind { get; set; }
        public decimal Opening { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public Boolean DC { get; set; }
        public int ExpensesKind { get; set; }
        public List<ParentBalance> GetList(string DB,string Account, DateTime First, DateTime Last, bool ExpKind)
        {
            List<ParentBalance> items = new List<ParentBalance>();
            string selQuery = "select top 100 percent * from dbo.fnaccReport_ParentBalance(@Account,@First,@Last,@ExpKind) order by ExpensesKind,Code";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Account", SqlDbType.NVarChar).Value = Account??"";
                com.Parameters.Add("@First", SqlDbType.Date).Value = First;
                com.Parameters.Add("@Last", SqlDbType.Date).Value = Last;
                com.Parameters.Add("@ExpKind", SqlDbType.Bit).Value = ExpKind;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ParentBalance item = new ParentBalance();
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.parent = Convert.ToString(reader["parent"]);
                    item.Level = Convert.ToInt32(reader["Level"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Opening = Convert.ToDecimal(reader["Opening"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.Balance = Convert.ToDecimal(reader["Balance"]);
                    item.DC = Convert.ToBoolean(reader["DC"]);
                    item.ExpensesKind = Convert.ToInt32(reader["ExpensesKind"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
