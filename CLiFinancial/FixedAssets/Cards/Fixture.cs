using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.FixedAssets.Cards
{
    public class Fixture
    {
        public Guid? Key { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public int Kind { get; set; }
        public Boolean Tangible { get; set; }
        public Boolean BatchEnable { get; set; }
        public int Percent { get; set; }
        public Guid? Category { get; set; }
        public Guid? Branch { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public Guid? Group { get; set; }
        public Guid? Sector { get; set; }
        public string Barcode { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string Details1 { get; set; }
        public string Details2 { get; set; }
        public string Details3 { get; set; }
        public string Image { get; set; }
        public Boolean Disable { get; set; }

        public List<Fixture> GetList(string DB)
        {

            List<Fixture> items = new List<Fixture>();
            string selQuery = "select top 100 percent * from finFixedAssets_Fixture order by [fxd_Code] ";
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
                    Fixture item = new Fixture();
                    item.Key = iCore.IsDbNullRtNull(reader["fxd_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["fxd_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["fxd_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["fxd_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["fxd_LastupDate"]);
                    item.Code = Convert.ToString(reader["fxd_Code"]);
                    item.Name1 = Convert.ToString(reader["fxd_Name1"]);
                    item.Name2 = Convert.ToString(reader["fxd_Name2"]);
                    item.Kind = Convert.ToInt32(reader["fxd_Kind"]);
                    item.Tangible = Convert.ToBoolean(reader["fxd_Tangible"]);
                    item.BatchEnable = Convert.ToBoolean(reader["fxd_BatchEnable"]);
                    item.Percent = Convert.ToInt32(reader["fxd_Percent"]);
                    item.Category = iCore.IsDbNullRtNull(reader["fxd_Category"]);
                    item.Branch = iCore.IsDbNullRtNull(reader["fxd_Branch"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["fxd_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["fxd_Project"]);
                    item.Group = iCore.IsDbNullRtNull(reader["fxd_Group"]);
                    item.Sector = iCore.IsDbNullRtNull(reader["fxd_Sector"]);
                    item.Barcode = Convert.ToString(reader["fxd_Barcode"]);
                    item.Model = Convert.ToString(reader["fxd_Model"]);
                    item.Brand = Convert.ToString(reader["fxd_Brand"]);
                    item.Color = Convert.ToString(reader["fxd_Color"]);
                    item.Details1 = Convert.ToString(reader["fxd_Details1"]);
                    item.Details2 = Convert.ToString(reader["fxd_Details2"]);
                    item.Details3 = Convert.ToString(reader["fxd_Details3"]);
                    item.Image = Convert.ToString(reader["fxd_Image"]);
                    item.Disable = Convert.ToBoolean(reader["fxd_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public Fixture GetItem(string DB, Guid? Key)
        {
            Fixture item = new Fixture();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from finFixedAssets_Fixture where [fxd_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["fxd_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["fxd_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["fxd_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["fxd_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["fxd_LastupDate"]);
                    item.Code = Convert.ToString(reader["fxd_Code"]);
                    item.Name1 = Convert.ToString(reader["fxd_Name1"]);
                    item.Name2 = Convert.ToString(reader["fxd_Name2"]);
                    item.Kind = Convert.ToInt32(reader["fxd_Kind"]);
                    item.Tangible = Convert.ToBoolean(reader["fxd_Tangible"]);
                    item.BatchEnable = Convert.ToBoolean(reader["fxd_BatchEnable"]);
                    item.Percent = Convert.ToInt32(reader["fxd_Percent"]);
                    item.Category = iCore.IsDbNullRtNull(reader["fxd_Category"]);
                    item.Branch = iCore.IsDbNullRtNull(reader["fxd_Branch"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["fxd_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["fxd_Project"]);
                    item.Group = iCore.IsDbNullRtNull(reader["fxd_Group"]);
                    item.Sector = iCore.IsDbNullRtNull(reader["fxd_Sector"]);
                    item.Barcode = Convert.ToString(reader["fxd_Barcode"]);
                    item.Model = Convert.ToString(reader["fxd_Model"]);
                    item.Brand = Convert.ToString(reader["fxd_Brand"]);
                    item.Color = Convert.ToString(reader["fxd_Color"]);
                    item.Details1 = Convert.ToString(reader["fxd_Details1"]);
                    item.Details2 = Convert.ToString(reader["fxd_Details2"]);
                    item.Details3 = Convert.ToString(reader["fxd_Details3"]);
                    item.Image = Convert.ToString(reader["fxd_Image"]);
                    item.Disable = Convert.ToBoolean(reader["fxd_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, Fixture item)
        {

            Guid? key = Guid.NewGuid();
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO finFixedAssets_Fixture");
                str.Append("([fxd_CreateUser]");
                str.Append(",[fxd_CreateDate]");
                str.Append(",[fxd_LastupUser]");
                str.Append(",[fxd_LastupDate]");
                str.Append(",[fxd_Code]");
                str.Append(",[fxd_Name1]");
                str.Append(",[fxd_Name2]");
                str.Append(",[fxd_Kind]");
                str.Append(",[fxd_Tangible]");
                str.Append(",[fxd_BatchEnable]");
                str.Append(",[fxd_Percent]");
                str.Append(",[fxd_Category]");
                str.Append(",[fxd_Branch]");
                str.Append(",[fxd_CostCenter]");
                str.Append(",[fxd_Project]");
                str.Append(",[fxd_Group]");
                str.Append(",[fxd_Sector]");
                str.Append(",[fxd_Barcode]");
                str.Append(",[fxd_Model]");
                str.Append(",[fxd_Brand]");
                str.Append(",[fxd_Color]");
                str.Append(",[fxd_Details1]");
                str.Append(",[fxd_Details2]");
                str.Append(",[fxd_Details3]");
                str.Append(",[fxd_Disable])");
                str.Append(" VALUES ");
                str.Append("(@fxd_CreateUser");
                str.Append(",@fxd_CreateDate");
                str.Append(",@fxd_LastupUser");
                str.Append(",@fxd_LastupDate");
                str.Append(",@fxd_Code");
                str.Append(",@fxd_Name1");
                str.Append(",@fxd_Name2");
                str.Append(",@fxd_Kind");
                str.Append(",@fxd_Tangible");
                str.Append(",@fxd_BatchEnable");
                str.Append(",@fxd_Percent");
                str.Append(",@fxd_Category");
                str.Append(",@fxd_Branch");
                str.Append(",@fxd_CostCenter");
                str.Append(",@fxd_Project");
                str.Append(",@fxd_Group");
                str.Append(",@fxd_Sector");
                str.Append(",@fxd_Barcode");
                str.Append(",@fxd_Model");
                str.Append(",@fxd_Brand");
                str.Append(",@fxd_Color");
                str.Append(",@fxd_Details1");
                str.Append(",@fxd_Details2");
                str.Append(",@fxd_Details3");
                str.Append(",@fxd_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@fxd_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@fxd_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@fxd_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@fxd_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@fxd_Code", SqlDbType.NVarChar, 25).Value = item.Code ?? "";
                comm.Parameters.Add("@fxd_Name1", SqlDbType.NVarChar, 100).Value = item.Name1 ?? "";
                comm.Parameters.Add("@fxd_Name2", SqlDbType.NVarChar, 100).Value = item.Name2 ?? "";
                comm.Parameters.Add("@fxd_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@fxd_Tangible", SqlDbType.Bit).Value = item.Tangible;
                comm.Parameters.Add("@fxd_BatchEnable", SqlDbType.Bit).Value = item.BatchEnable;
                comm.Parameters.Add("@fxd_Percent", SqlDbType.Int).Value = item.Percent;
                comm.Parameters.Add("@fxd_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Category);
                comm.Parameters.Add("@fxd_Branch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Branch);
                comm.Parameters.Add("@fxd_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                comm.Parameters.Add("@fxd_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                comm.Parameters.Add("@fxd_Group", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Group);
                comm.Parameters.Add("@fxd_Sector", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Sector);
                comm.Parameters.Add("@fxd_Barcode", SqlDbType.NVarChar, 25).Value = item.Barcode ?? "";
                comm.Parameters.Add("@fxd_Model", SqlDbType.NVarChar, 100).Value = item.Model ?? "";
                comm.Parameters.Add("@fxd_Brand", SqlDbType.NVarChar, 100).Value = item.Brand ?? "";
                comm.Parameters.Add("@fxd_Color", SqlDbType.NVarChar, 100).Value = item.Color ?? "";
                comm.Parameters.Add("@fxd_Details1", SqlDbType.NVarChar, 500).Value = item.Details1 ?? "";
                comm.Parameters.Add("@fxd_Details2", SqlDbType.NVarChar, 500).Value = item.Details2 ?? "";
                comm.Parameters.Add("@fxd_Details3", SqlDbType.NVarChar, 500).Value = item.Details3 ?? "";
                comm.Parameters.Add("@fxd_Image", SqlDbType.NVarChar, 500).Value = item.Image ?? "";
                comm.Parameters.Add("@fxd_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, Fixture item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update finFixedAssets_Fixture SET ");
                str.Append("[fxd_CreateUser]=@fxd_CreateUser");
                str.Append(",[fxd_CreateDate]=@fxd_CreateDate");
                str.Append(",[fxd_LastupUser]=@fxd_LastupUser");
                str.Append(",[fxd_LastupDate]=@fxd_LastupDate");
                str.Append(",[fxd_Code]=@fxd_Code");
                str.Append(",[fxd_Name1]=@fxd_Name1");
                str.Append(",[fxd_Name2]=@fxd_Name2");
                str.Append(",[fxd_Kind]=@fxd_Kind");
                str.Append(",[fxd_Tangible]=@fxd_Tangible");
                str.Append(",[fxd_BatchEnable]=@fxd_BatchEnable");
                str.Append(",[fxd_Percent]=@fxd_Percent");
                str.Append(",[fxd_Category]=@fxd_Category");
                str.Append(",[fxd_Branch]=@fxd_Branch");
                str.Append(",[fxd_CostCenter]=@fxd_CostCenter");
                str.Append(",[fxd_Project]=@fxd_Project");
                str.Append(",[fxd_Group]=@fxd_Group");
                str.Append(",[fxd_Sector]=@fxd_Sector");
                str.Append(",[fxd_Barcode]=@fxd_Barcode");
                str.Append(",[fxd_Model]=@fxd_Model");
                str.Append(",[fxd_Brand]=@fxd_Brand");
                str.Append(",[fxd_Color]=@fxd_Color");
                str.Append(",[fxd_Details1]=@fxd_Details1");
                str.Append(",[fxd_Details2]=@fxd_Details2");
                str.Append(",[fxd_Details3]=@fxd_Details3");
                str.Append(",[fxd_Image]=@fxd_Image");
                str.Append(",[fxd_Disable]=@fxd_Disable");
                str.Append(" WHERE fxd_Key=@fxd_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@fxd_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@fxd_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@fxd_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@fxd_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@fxd_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@fxd_Code", SqlDbType.NVarChar, 25).Value = item.Code ?? "";
                comm.Parameters.Add("@fxd_Name1", SqlDbType.NVarChar, 100).Value = item.Name1 ?? "";
                comm.Parameters.Add("@fxd_Name2", SqlDbType.NVarChar, 100).Value = item.Name2 ?? "";
                comm.Parameters.Add("@fxd_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@fxd_Tangible", SqlDbType.Bit).Value = item.Tangible;
                comm.Parameters.Add("@fxd_BatchEnable", SqlDbType.Bit).Value = item.BatchEnable;
                comm.Parameters.Add("@fxd_Percent", SqlDbType.Int).Value = item.Percent;
                comm.Parameters.Add("@fxd_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Category);
                comm.Parameters.Add("@fxd_Branch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Branch);
                comm.Parameters.Add("@fxd_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                comm.Parameters.Add("@fxd_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                comm.Parameters.Add("@fxd_Group", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Group);
                comm.Parameters.Add("@fxd_Sector", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Sector);
                comm.Parameters.Add("@fxd_Barcode", SqlDbType.NVarChar, 25).Value = item.Barcode ?? "";
                comm.Parameters.Add("@fxd_Model", SqlDbType.NVarChar, 100).Value = item.Model ?? "";
                comm.Parameters.Add("@fxd_Brand", SqlDbType.NVarChar, 100).Value = item.Brand ?? "";
                comm.Parameters.Add("@fxd_Color", SqlDbType.NVarChar, 100).Value = item.Color ?? "";
                comm.Parameters.Add("@fxd_Details1", SqlDbType.NVarChar, 500).Value = item.Details1 ?? "";
                comm.Parameters.Add("@fxd_Details2", SqlDbType.NVarChar, 500).Value = item.Details2 ?? "";
                comm.Parameters.Add("@fxd_Details3", SqlDbType.NVarChar, 500).Value = item.Details3 ?? "";
                comm.Parameters.Add("@fxd_Image", SqlDbType.NVarChar, 500).Value = item.Image ?? "";
                comm.Parameters.Add("@fxd_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "EXEC dbo.spfinDelete_Fixture @Key";
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
