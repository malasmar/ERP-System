using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiAccounting.Cards
{
    public class ChartofAccounts
    {
        public Guid? Key { get; set; }
        public Guid? ParentKey { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Parent { get; set; }
        public int Kind { get; set; }
        public int Level { get; set; }
        public Boolean Financial { get; set; }
        public Boolean Source { get; set; }
        public int Category { get; set; }
        public Boolean Disable { get; set; }
        public Boolean isParent { get; set; }
        public int Transactions { get; set; }
        public string ParentName1 { get; set; }
        public string ParentName2 { get; set; }
        public int Status { get; set; }
        public bool DC { get; set; }
        public bool CloseSubAccounts { get; set; }
        public List<ChartofAccounts> GetList(string DB)
        {
            List<ChartofAccounts> items = new List<ChartofAccounts>();
            string selQuery = "select top 100 percent * from dbo.fnaccCards_AccountsList() order by [Code] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ChartofAccounts item = new ChartofAccounts();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.ParentKey = iCore.IsDbNullRtNull(reader["ParentKey"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Parent = Convert.ToString(reader["Parent"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Level = Convert.ToInt32(reader["Level"]);
                    item.Financial = Convert.ToBoolean(reader["Financial"]);
                    item.Source = Convert.ToBoolean(reader["Source"]);
                    item.Category = Convert.ToInt32(reader["Category"]);
                    item.Disable = Convert.ToBoolean(reader["Disable"]);
                    item.isParent = Convert.ToBoolean(reader["isParent"]);
                    item.Transactions = Convert.ToInt32(reader["Transactions"]);
                    item.ParentName1 = Convert.ToString(reader["ParentName1"]);
                    item.ParentName2 = Convert.ToString(reader["ParentName2"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.DC = Convert.ToBoolean(reader["DC"]);
                    item.CloseSubAccounts = Convert.ToBoolean(reader["CloseSubAccounts"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public ChartofAccounts GetItem(string DB,Guid? Key)
        {
            ChartofAccounts item = new ChartofAccounts();
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from dbo.fnaccCards_AccountDetails(@Key)   ";
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
                    
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Parent = Convert.ToString(reader["Parent"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Level = Convert.ToInt32(reader["Level"]);
                    item.Financial = Convert.ToBoolean(reader["Financial"]);
                    item.Source = Convert.ToBoolean(reader["Source"]);
                    item.Category = Convert.ToInt32(reader["Category"]);
                    item.Disable = Convert.ToBoolean(reader["Disable"]);
                    item.isParent = Convert.ToBoolean(reader["isParent"]);
                    item.Transactions = Convert.ToInt32(reader["Transactions"]);
                    item.ParentName1 = Convert.ToString(reader["ParentName1"]);
                    item.ParentName2 = Convert.ToString(reader["ParentName2"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.DC = Convert.ToBoolean(reader["DC"]);
                    item.CloseSubAccounts = Convert.ToBoolean(reader["CloseSubAccounts"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
