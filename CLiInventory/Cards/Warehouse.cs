using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiInventory.Cards
{
    public class Warehouse
    {
        public Guid? Key { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public int Kind { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal Height { get; set; }
        public Guid? accPurchase { get; set; }
        public Guid? accSales { get; set; }
        public Guid? accCost { get; set; }
        public Guid? accProduction { get; set; }
        public Guid? accFinishProduct { get; set; }
        public Guid? accOnRoad { get; set; }
        public Guid? Branch { get; set; }
        public Guid? Costcenter { get; set; }
        public Guid? Project { get; set; }
        public Boolean Disable { get; set; }

        public List<Warehouse> GetList(string DB)
        {
            List<Warehouse> items = new List<Warehouse>();
            string selQuery = "select top 100 percent * from InvCard_Warehouse order by [whs_No] ";
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
                    Warehouse item = new Warehouse();
                    item.Key = iCore.IsDbNullRtNull(reader["whs_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["whs_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["whs_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["whs_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["whs_LastupDate"]);
                    item.Kind = Convert.ToInt32(reader["whs_Kind"]);
                    item.No = Convert.ToInt32(reader["whs_No"]);
                    item.Name1 = Convert.ToString(reader["whs_Name1"]);
                    item.Name2 = Convert.ToString(reader["whs_Name2"]);
                    item.Phone = Convert.ToString(reader["whs_Phone"]);
                    item.Mobile = Convert.ToString(reader["whs_Mobile"]);
                    item.Fax = Convert.ToString(reader["whs_Fax"]);
                    item.Email = Convert.ToString(reader["whs_Email"]);
                    item.Address = Convert.ToString(reader["whs_Address"]);
                    item.Width = Convert.ToDecimal(reader["whs_Width"]);
                    item.Length = Convert.ToDecimal(reader["whs_Length"]);
                    item.Height = Convert.ToDecimal(reader["whs_Height"]);
                    item.accPurchase = iCore.IsDbNullRtNull(reader["whs_accPurchase"]);
                    item.accSales = iCore.IsDbNullRtNull(reader["whs_accSales"]);
                    item.accCost = iCore.IsDbNullRtNull(reader["whs_accCost"]);
                    item.accProduction = iCore.IsDbNullRtNull(reader["whs_accProduction"]);
                    item.accFinishProduct = iCore.IsDbNullRtNull(reader["whs_accFinishProduct"]);
                    item.accOnRoad = iCore.IsDbNullRtNull(reader["whs_accOnRoad"]);
                    item.Branch = iCore.IsDbNullRtNull(reader["whs_Branch"]);
                    item.Costcenter = iCore.IsDbNullRtNull(reader["whs_Costcenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["whs_Project"]);
                    item.Disable = Convert.ToBoolean(reader["whs_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public Warehouse GetItem(string DB,Guid? Key)
        {
            Warehouse item = new Warehouse();
            item.No = MaxOrder(DB);
            if (Key == null)
            {
                CLiCore.Configuration.WarehouseSettingsAccounts accounts = new CLiCore.Configuration.WarehouseSettingsAccounts ().GetItem(DB);
                item.accCost = accounts.wCost;
                item.accSales = accounts.wSales;

                return item;
            }

                

            string selQuery = "select top 100 percent * from InvCard_Warehouse where [whs_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["whs_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["whs_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["whs_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["whs_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["whs_LastupDate"]);
                    item.Kind = Convert.ToInt32(reader["whs_Kind"]);
                    item.No = Convert.ToInt32(reader["whs_No"]);
                    item.Name1 = Convert.ToString(reader["whs_Name1"]);
                    item.Name2 = Convert.ToString(reader["whs_Name2"]);
                    item.Phone = Convert.ToString(reader["whs_Phone"]);
                    item.Mobile = Convert.ToString(reader["whs_Mobile"]);
                    item.Fax = Convert.ToString(reader["whs_Fax"]);
                    item.Email = Convert.ToString(reader["whs_Email"]);
                    item.Address = Convert.ToString(reader["whs_Address"]);
                    item.Width = Convert.ToDecimal(reader["whs_Width"]);
                    item.Length = Convert.ToDecimal(reader["whs_Length"]);
                    item.Height = Convert.ToDecimal(reader["whs_Height"]);
                    item.accPurchase = iCore.IsDbNullRtNull(reader["whs_accPurchase"]);
                    item.accSales = iCore.IsDbNullRtNull(reader["whs_accSales"]);
                    item.accCost = iCore.IsDbNullRtNull(reader["whs_accCost"]);
                    item.accProduction = iCore.IsDbNullRtNull(reader["whs_accProduction"]);
                    item.accFinishProduct = iCore.IsDbNullRtNull(reader["whs_accFinishProduct"]);
                    item.accOnRoad = iCore.IsDbNullRtNull(reader["whs_accOnRoad"]);
                    item.Branch = iCore.IsDbNullRtNull(reader["whs_Branch"]);
                    item.Costcenter = iCore.IsDbNullRtNull(reader["whs_Costcenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["whs_Project"]);
                    item.Disable = Convert.ToBoolean(reader["whs_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, Warehouse item)
        {
            CLiCore.Configuration.WarehouseSettingsAccounts wacc = new CLiCore.Configuration.WarehouseSettingsAccounts().GetItem(DB);
            string code = xConfig.AccountCode(DB, wacc.wPurchase);
            Guid account = CLiCore.Platx.Account.Insert(DB, wacc.wPurchase, code, item.Name1, item.Name2);
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO InvCard_Warehouse");
                str.Append("([whs_CreateUser]");
                str.Append(",[whs_CreateDate]");
                str.Append(",[whs_LastupUser]");
                str.Append(",[whs_LastupDate]");
                str.Append(",[whs_Kind]");
                str.Append(",[whs_No]");
                str.Append(",[whs_Name1]");
                str.Append(",[whs_Name2]");
                str.Append(",[whs_Phone]");
                str.Append(",[whs_Mobile]");
                str.Append(",[whs_Fax]");
                str.Append(",[whs_Email]");
                str.Append(",[whs_Address]");
                str.Append(",[whs_Width]");
                str.Append(",[whs_Length]");
                str.Append(",[whs_Height]");
                str.Append(",[whs_accPurchase]");
                str.Append(",[whs_accSales]");
                str.Append(",[whs_accCost]");
                str.Append(",[whs_accProduction]");
                str.Append(",[whs_accFinishProduct]");
                str.Append(",[whs_accOnRoad]");
                str.Append(",[whs_Branch]");
                str.Append(",[whs_Costcenter]");
                str.Append(",[whs_Disable])");
                str.Append(" VALUES ");
                str.Append("(@whs_CreateUser");
                str.Append(",@whs_CreateDate");
                str.Append(",@whs_LastupUser");
                str.Append(",@whs_LastupDate");
                str.Append(",@whs_Kind");
                str.Append(",@whs_No");
                str.Append(",@whs_Name1");
                str.Append(",@whs_Name2");
                str.Append(",@whs_Phone");
                str.Append(",@whs_Mobile");
                str.Append(",@whs_Fax");
                str.Append(",@whs_Email");
                str.Append(",@whs_Address");
                str.Append(",@whs_Width");
                str.Append(",@whs_Length");
                str.Append(",@whs_Height");
                str.Append(",@whs_accPurchase");
                str.Append(",@whs_accSales");
                str.Append(",@whs_accCost");
                str.Append(",@whs_accProduction");
                str.Append(",@whs_accFinishProduct");
                str.Append(",@whs_accOnRoad");
                str.Append(",@whs_Branch");
                str.Append(",@whs_Costcenter");
                str.Append(",@whs_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@whs_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@whs_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@whs_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@whs_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@whs_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@whs_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@whs_Name1", SqlDbType.NVarChar, 100).Value = item.Name1 ?? "";
                comm.Parameters.Add("@whs_Name2", SqlDbType.NVarChar, 100).Value = item.Name2 ?? "";
                comm.Parameters.Add("@whs_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@whs_Mobile", SqlDbType.NVarChar, 15).Value = item.Mobile ?? "";
                comm.Parameters.Add("@whs_Fax", SqlDbType.NVarChar, 15).Value = item.Fax ?? "";
                comm.Parameters.Add("@whs_Email", SqlDbType.NVarChar, 50).Value = item.Email ?? "";
                comm.Parameters.Add("@whs_Address", SqlDbType.NVarChar, 255).Value = item.Address ?? "";
                comm.Parameters.Add("@whs_Width", SqlDbType.Decimal).Value = item.Width;
                comm.Parameters.Add("@whs_Length", SqlDbType.Decimal).Value = item.Length;
                comm.Parameters.Add("@whs_Height", SqlDbType.Decimal).Value = item.Height;
                comm.Parameters.Add("@whs_accPurchase", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(account);
                comm.Parameters.Add("@whs_accSales", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.accSales);
                comm.Parameters.Add("@whs_accCost", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.accCost);
                comm.Parameters.Add("@whs_accProduction", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.accProduction);
                comm.Parameters.Add("@whs_accFinishProduct", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.accFinishProduct);
                comm.Parameters.Add("@whs_accOnRoad", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.accOnRoad);
                comm.Parameters.Add("@whs_Branch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Branch);
                comm.Parameters.Add("@whs_Costcenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Costcenter);
                comm.Parameters.Add("@whs_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                comm.Parameters.Add("@whs_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, Warehouse item)
        {
            CLiCore.Platx.Account.Update(DB, item.accPurchase, item.Name1, item.Name2, item.Disable);
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update InvCard_Warehouse SET ");
                str.Append("[whs_CreateUser]=@whs_CreateUser");
                str.Append(",[whs_CreateDate]=@whs_CreateDate");
                str.Append(",[whs_LastupUser]=@whs_LastupUser");
                str.Append(",[whs_LastupDate]=@whs_LastupDate");
                str.Append(",[whs_Kind]=@whs_Kind");
                str.Append(",[whs_No]=@whs_No");
                str.Append(",[whs_Name1]=@whs_Name1");
                str.Append(",[whs_Name2]=@whs_Name2");
                str.Append(",[whs_Phone]=@whs_Phone");
                str.Append(",[whs_Mobile]=@whs_Mobile");
                str.Append(",[whs_Fax]=@whs_Fax");
                str.Append(",[whs_Email]=@whs_Email");
                str.Append(",[whs_Address]=@whs_Address");
                str.Append(",[whs_Width]=@whs_Width");
                str.Append(",[whs_Length]=@whs_Length");
                str.Append(",[whs_Height]=@whs_Height");
                str.Append(",[whs_accPurchase]=@whs_accPurchase");
                str.Append(",[whs_accSales]=@whs_accSales");
                str.Append(",[whs_accCost]=@whs_accCost");
                str.Append(",[whs_accProduction]=@whs_accProduction");
                str.Append(",[whs_accFinishProduct]=@whs_accFinishProduct");
                str.Append(",[whs_accOnRoad]=@whs_accOnRoad");
                str.Append(",[whs_Branch]=@whs_Branch");
                str.Append(",[whs_Costcenter]=@whs_Costcenter");
                str.Append(",[whs_Project]=@whs_Project");
                str.Append(",[whs_Disable]=@whs_Disable");
                str.Append(" WHERE whs_Key=@whs_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@whs_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@whs_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@whs_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@whs_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@whs_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@whs_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@whs_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@whs_Name1", SqlDbType.NVarChar, 100).Value = item.Name1 ?? "";
                comm.Parameters.Add("@whs_Name2", SqlDbType.NVarChar, 100).Value = item.Name2 ?? "";
                comm.Parameters.Add("@whs_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@whs_Mobile", SqlDbType.NVarChar, 15).Value = item.Mobile ?? "";
                comm.Parameters.Add("@whs_Fax", SqlDbType.NVarChar, 15).Value = item.Fax ?? "";
                comm.Parameters.Add("@whs_Email", SqlDbType.NVarChar, 50).Value = item.Email ?? "";
                comm.Parameters.Add("@whs_Address", SqlDbType.NVarChar, 255).Value = item.Address ?? "";
                comm.Parameters.Add("@whs_Width", SqlDbType.Decimal).Value = item.Width;
                comm.Parameters.Add("@whs_Length", SqlDbType.Decimal).Value = item.Length;
                comm.Parameters.Add("@whs_Height", SqlDbType.Decimal).Value = item.Height;
                comm.Parameters.Add("@whs_accPurchase", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.accPurchase);
                comm.Parameters.Add("@whs_accSales", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.accSales);
                comm.Parameters.Add("@whs_accCost", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.accCost);
                comm.Parameters.Add("@whs_accProduction", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.accProduction);
                comm.Parameters.Add("@whs_accFinishProduct", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.accFinishProduct);
                comm.Parameters.Add("@whs_accOnRoad", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.accOnRoad);
                comm.Parameters.Add("@whs_Branch", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Branch);
                comm.Parameters.Add("@whs_Costcenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Costcenter);
                comm.Parameters.Add("@whs_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                comm.Parameters.Add("@whs_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public int MaxOrder(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([whs_No])+1,1) from [InvCard_Warehouse]";
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
            int res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [InvCard_Warehouse] where [whs_Key]=@Key";
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
