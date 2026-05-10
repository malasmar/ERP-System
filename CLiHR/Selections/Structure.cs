using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiHR.Selections
{
    public class Structure
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }

        public static List<Structure> GetList(string DB, string xLan)
        {
            List<Structure> items = new List<Structure>();
            string selQuery = "select top 100 percent [str_Key],[str_No],[str_Name1],[str_Name2] from [HRStructure_Organizational] order by [str_Level],[str_Order] ";
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
                    Structure item = new Structure();
                    item.Key = iCore.IsDbNullRtNull(reader["str_Key"]);
                    item.No = Convert.ToInt32(reader["str_No"]);
                    item.Name1 = Convert.ToString(reader["str_Name1"]);
                    item.Name2 = Convert.ToString(reader["str_Name2"]);
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
    }
    public class Structurex
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public int Kind { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Guid? Parent { get; set; }
        public string ManagerName1 { get; set; }
        public string ManagerName2 { get; set; }
        public int Level { get; set; }
        public int Order { get; set; }
        public string? Display { get; set; }
        public string? Manager { get; set; }
        public   List<Structurex> GetList(string DB, string xLan)
        {
            List<Structurex> items = new List<Structurex>();
            string selQuery = "select top 100 percent * from dbo.fnhrSelections_Structure() order by [Level],[Order] desc ";
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
                    Structurex item = new Structurex();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.No = Convert.ToInt32(reader["No"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["Parent"]);
                    item.ManagerName1 = Convert.ToString(reader["ManagerName1"]);
                    item.ManagerName2 = Convert.ToString(reader["ManagerName2"]);
                    item.Level = Convert.ToInt32(reader["Level"]);
                    item.Order = Convert.ToInt32(reader["Order"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2);
                            item.Manager = (item.ManagerName2 == "" ? item.ManagerName1 : item.ManagerName2);
                            break;
                        case "ar":
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1);
                            item.Manager = (item.ManagerName1 == "" ? item.ManagerName2 : item.ManagerName1);
                            break;
                        default:
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1);
                            item.Manager = (item.ManagerName1 == "" ? item.ManagerName2 : item.ManagerName1);
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
