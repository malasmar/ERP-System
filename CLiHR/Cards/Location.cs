using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiHR.Cards
{
    public class Location
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Boolean Disable { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public List<Location> GetList(string DB)
        {
            List<Location> items = new List<Location>();
            string selQuery = "select top 100 percent * from hrCard_Location order by [loc_No] ";
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
                    Location item = new Location();
                    item.Key = iCore.IsDbNullRtNull(reader["loc_Key"]);
                    item.No = Convert.ToInt32(reader["Loc_No"]);
                    item.Name1 = Convert.ToString(reader["Loc_Name1"]);
                    item.Name2 = Convert.ToString(reader["Loc_Name2"]);
                    item.Latitude = Convert.ToDouble(reader["Loc_Latitude"]);
                    item.Longitude = Convert.ToDouble(reader["Loc_Longitude"]);
                    item.Disable = Convert.ToBoolean(reader["Loc_Disable"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["Loc_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Loc_Project"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public Location GetItem(string DB, Guid? Key)
        {
            Location item = new Location();
            item.No = MaxNo(DB);
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from hrCard_Location where [loc_Key]=@Key";
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
                    item.Key = iCore.IsDbNullRtNull(reader["loc_Key"]);
                    item.No = Convert.ToInt32(reader["Loc_No"]);
                    item.Name1 = Convert.ToString(reader["Loc_Name1"]);
                    item.Name2 = Convert.ToString(reader["Loc_Name2"]);
                    item.Latitude = Convert.ToDouble(reader["Loc_Latitude"]);
                    item.Longitude = Convert.ToDouble(reader["Loc_Longitude"]);
                    item.Disable = Convert.ToBoolean(reader["Loc_Disable"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["Loc_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Loc_Project"]);
                }
                reader.Close();
            }
            return item;
        }
        public static void Insert(string DB, Location item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO hrCard_Location");
                str.Append("([Loc_No]");
                str.Append(",[Loc_Name1]");
                str.Append(",[Loc_Name2]");
                str.Append(",[Loc_Latitude]");
                str.Append(",[Loc_Longitude]");
                str.Append(",[Loc_Disable]");
                str.Append(",[Loc_CostCenter]");
                str.Append(",[Loc_Project])");
                str.Append(" VALUES ");
                str.Append("(@Loc_No");
                str.Append(",@Loc_Name1");
                str.Append(",@Loc_Name2");
                str.Append(",@Loc_Latitude");
                str.Append(",@Loc_Longitude");
                str.Append(",@Loc_Disable");
                str.Append(",@Loc_CostCenter");
                str.Append(",@Loc_Project)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Loc_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@Loc_Name1", SqlDbType.NVarChar, 150).Value = item.Name1 ?? "";
                comm.Parameters.Add("@Loc_Name2", SqlDbType.NVarChar, 150).Value = item.Name2 ?? "";
                comm.Parameters.Add("@Loc_Latitude", SqlDbType.Float).Value = item.Latitude;
                comm.Parameters.Add("@Loc_Longitude", SqlDbType.Float).Value = item.Longitude;
                comm.Parameters.Add("@Loc_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@Loc_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                comm.Parameters.Add("@Loc_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public static void Update(string DB, Location item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update hrCard_Location SET ");
                str.Append("[Loc_No]=@Loc_No");
                str.Append(",[Loc_Name1]=@Loc_Name1");
                str.Append(",[Loc_Name2]=@Loc_Name2");
                str.Append(",[Loc_Latitude]=@Loc_Latitude");
                str.Append(",[Loc_Longitude]=@Loc_Longitude");
                str.Append(",[Loc_Disable]=@Loc_Disable");
                str.Append(",[Loc_CostCenter]=@Loc_CostCenter");
                str.Append(",[Loc_Project]=@Loc_Project");
                str.Append(" WHERE loc_Key=@loc_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@loc_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@Loc_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@Loc_Name1", SqlDbType.NVarChar, 150).Value = item.Name1 ?? "";
                comm.Parameters.Add("@Loc_Name2", SqlDbType.NVarChar, 150).Value = item.Name2 ?? "";
                comm.Parameters.Add("@Loc_Latitude", SqlDbType.Float).Value = item.Latitude;
                comm.Parameters.Add("@Loc_Longitude", SqlDbType.Float).Value = item.Longitude;
                comm.Parameters.Add("@Loc_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@Loc_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                comm.Parameters.Add("@Loc_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public int MaxNo(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([loc_No])+1,1) from [hrCard_Location]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand comm = new SqlCommand();
                comm.CommandText = selQuery;
                comm.CommandType = CommandType.Text;
                comm.Connection = con;
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    res = (int)reader[0];
                }
                reader.Close();
            }
            return res;
        }
        public static int Delete(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [hrCard_Location] where [loc_Key]=@Key";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                res = comm.ExecuteNonQuery();
            }
            return res;
        }
    }
}
