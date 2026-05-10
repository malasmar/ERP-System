using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CLiCore;
using CLiDashboard.Sales;
using CLiDashboard.Purchase;
using System.Data;

namespace CLiDashboard.CostCenter
{
    public class CostCenterIncomeBalance
    {
        public Guid? key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string Display { get; set; }
        public List<CostCenterIncomeBalance> CostCenter(string DB,string xLan)
        {
            List<CostCenterIncomeBalance> items = new List<CostCenterIncomeBalance>();
            string selQuery = "select top 100 percent * from dbo.fnDashCostCenter_IncomeBalance() order by [Code]";
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
                    CostCenterIncomeBalance item = new CostCenterIncomeBalance();
                    item.key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.Balance = Convert.ToDecimal(reader["Balance"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = item.Name2 == "" ? item.Name1 : item.Name2;
                            break;
                        case "ar":
                            item.Display = item.Name1 == "" ? item.Name2 : item.Name1;
                            break;
                        default:
                            item.Display = item.Name1 == "" ? item.Name2 : item.Name1;
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }

            return items;
        }
        public List<CostCenterIncomeBalance> Project(string DB, string xLan)
        {
            List<CostCenterIncomeBalance> items = new List<CostCenterIncomeBalance>();
            string selQuery = "select top 100 percent * from dbo.fnDashProject_IncomeBalance() order by [Code]";
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
                    CostCenterIncomeBalance item = new CostCenterIncomeBalance();
                    item.key = iCore.IsDbNullRtNull(reader["key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.Balance = Convert.ToDecimal(reader["Balance"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = item.Name2 == "" ? item.Name1 : item.Name2;
                            break;
                        case "ar":
                            item.Display = item.Name1 == "" ? item.Name2 : item.Name1;
                            break;
                        default:
                            item.Display = item.Name1 == "" ? item.Name2 : item.Name1;
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }

            return items;
        }
    }
}
