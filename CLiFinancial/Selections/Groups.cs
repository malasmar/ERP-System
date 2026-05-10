using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Selections
{
    public class Groups
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }

        public static List<Groups> CurrentAccount(string DB, string xLan)
        {
            List<Groups> items = new List<Groups>();
            string selQuery = "select top 100 percent * from [finCard_Group_CurrentAccount] order by [grp_No] ";
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
                    Groups item = new Groups();
                    item.Key = iCore.IsDbNullRtNull(reader["grp_Key"]);
                    item.No = Convert.ToInt32(reader["grp_No"]);
                    item.Name1 = Convert.ToString(reader["grp_Name1"]);
                    item.Name2 = Convert.ToString(reader["grp_Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.No.ToString() + ")";
                            break;
                        case "ar":
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
                            break;
                        default:
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public static List<Groups> CashBox(string DB, string xLan)
        {
            List<Groups> items = new List<Groups>();
            string selQuery = "select top 100 percent * from [finCard_Group_CashBox] order by [grp_No] ";
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
                    Groups item = new Groups();
                    item.Key = iCore.IsDbNullRtNull(reader["grp_Key"]);
                    item.No = Convert.ToInt32(reader["grp_No"]);
                    item.Name1 = Convert.ToString(reader["grp_Name1"]);
                    item.Name2 = Convert.ToString(reader["grp_Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.No.ToString() + ")";
                            break;
                        case "ar":
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
                            break;
                        default:
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public static List<Groups> Expenses(string DB, string xLan)
        {
            List<Groups> items = new List<Groups>();
            string selQuery = "select top 100 percent * from [finCard_Group_Expenses] order by [grp_No] ";
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
                    Groups item = new Groups();
                    item.Key = iCore.IsDbNullRtNull(reader["grp_Key"]);
                    item.No = Convert.ToInt32(reader["grp_No"]);
                    item.Name1 = Convert.ToString(reader["grp_Name1"]);
                    item.Name2 = Convert.ToString(reader["grp_Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.No.ToString() + ")";
                            break;
                        case "ar":
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
                            break;
                        default:
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public static List<Groups> Revenue(string DB, string xLan)
        {
            List<Groups> items = new List<Groups>();
            string selQuery = "select top 100 percent * from [finCard_Group_Revenue] order by [grp_No] ";
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
                    Groups item = new Groups();
                    item.Key = iCore.IsDbNullRtNull(reader["grp_Key"]);
                    item.No = Convert.ToInt32(reader["grp_No"]);
                    item.Name1 = Convert.ToString(reader["grp_Name1"]);
                    item.Name2 = Convert.ToString(reader["grp_Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.No.ToString() + ")";
                            break;
                        case "ar":
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
                            break;
                        default:
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public static List<Groups> JVCategories(string DB, string xLan)
        {
            List<Groups> items = new List<Groups>();
            string selQuery = "select top 100 percent * from [finCard_CategoriesJournalVoucher] order by [cat_No] ";
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
                    Groups item = new Groups();
                    item.Key = iCore.IsDbNullRtNull(reader["cat_Key"]);
                    item.No = Convert.ToInt32(reader["cat_No"]);
                    item.Name1 = Convert.ToString(reader["cat_Name1"]);
                    item.Name2 = Convert.ToString(reader["cat_Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.No.ToString() + ")";
                            break;
                        case "ar":
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
                            break;
                        default:
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public static List<Groups> TransactionCategories(string DB, string xLan)
        {
            List<Groups> items = new List<Groups>();
            string selQuery = "select top 100 percent * from [finCard_CategoriesTransaction] order by [cat_No] ";
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
                    Groups item = new Groups();
                    item.Key = iCore.IsDbNullRtNull(reader["cat_Key"]);
                    item.No = Convert.ToInt32(reader["cat_No"]);
                    item.Name1 = Convert.ToString(reader["cat_Name1"]);
                    item.Name2 = Convert.ToString(reader["cat_Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.No.ToString() + ")";
                            break;
                        case "ar":
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
                            break;
                        default:
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public static List<Groups> SalariesIntegration(string DB, string xLan)
        {
            List<Groups> items = new List<Groups>();
            string selQuery = "SELECT TOP 100 PERCENT si.SI_Key AS [Key],si.SI_Code AS [Code],si.SI_Name1 AS [Name1],si.SI_Name2 AS [Name2] FROM finCard_SalariesIntegration si order by [SI_Code] ";
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
                    Groups item = new Groups();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    //item.No = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
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
