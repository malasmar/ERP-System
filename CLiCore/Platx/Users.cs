using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiCore.Platx
{
    public class Users
    {
        public Guid? Key { get; set; }
        public Guid? Subscribe { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Kind { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Passwoard { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Guid? DataBase { get; set; }
        public Guid? Employee { get; set; }
        public Boolean AppSalesPerson { get; set; }
        public Boolean AppSales { get; set; }
        public Boolean AppWarehouse { get; set; }
        public Boolean CanDelete { get; set; }
        public Boolean CanPost { get; set; }
        public Boolean ConfirmFinance { get; set; }
        public Boolean ConfirmInventory { get; set; }
        public Boolean ConfirmPurchase { get; set; }
        public Boolean ConfirmSales { get; set; }
        public Boolean CloseSalesPrice { get; set; }
        public Boolean CloseSalesDiscount { get; set; }
        public int Branch { get; set; }
        public Guid? Prefix { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public Guid? Cash { get; set; }
        public int WarehouseNo { get; set; }
        public Boolean Disable { get; set; }
        public bool ActiveFilter { get; set; }
        public string? WhatsappNumber { get; set; }

        public List<Users> GetList(Guid Key)
        {
            List<Users> items = new List<Users>();
            string selQuery = "select top 100 percent * from px_Users where  [user_Subscribe]=@Key order by [user_No] ";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
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
                    Users item = new Users();
                    item.Key = iCore.IsDbNullRtNull(reader["user_Key"]);
                    item.Subscribe = iCore.IsDbNullRtNull(reader["user_Subscribe"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["user_CreateDate"]);
                    item.UpdateDate = iCore.IsDbNullRtNullDate(reader["user_UpdateDate"]);
                    item.Kind = Convert.ToInt32(reader["user_Kind"]);
                    item.No = Convert.ToInt32(reader["user_No"]);
                    item.Name1 = Convert.ToString(reader["user_Name1"]);
                    item.Name2 = Convert.ToString(reader["user_Name2"]);
                    item.Passwoard = Convert.ToString(reader["user_Passwoard"]);
                    item.Email = Convert.ToString(reader["user_Email"]);
                    item.Phone = Convert.ToString(reader["user_Phone"]);
                    item.WhatsappNumber = Convert.ToString(reader["user_WhatsappNumber"]);
                    item.DataBase = iCore.IsDbNullRtNull(reader["user_DataBase"]);
                    item.Employee = iCore.IsDbNullRtNull(reader["user_Employee"]);
                    item.AppSalesPerson = Convert.ToBoolean(reader["user_AppSalesPerson"]);
                    item.AppSales = Convert.ToBoolean(reader["user_AppSales"]);
                    item.AppWarehouse = Convert.ToBoolean(reader["user_AppWarehouse"]);
                    item.CanDelete = Convert.ToBoolean(reader["user_CanDelete"]);
                    item.CanPost = Convert.ToBoolean(reader["user_CanPost"]);
                    item.ConfirmFinance = Convert.ToBoolean(reader["user_ConfirmFinance"]);
                    item.ConfirmInventory = Convert.ToBoolean(reader["user_ConfirmInventory"]);
                    item.ConfirmPurchase = Convert.ToBoolean(reader["user_ConfirmPurchase"]);
                    item.ConfirmSales = Convert.ToBoolean(reader["user_ConfirmSales"]);
                    item.CloseSalesPrice = Convert.ToBoolean(reader["user_CloseSalesPrice"]);
                    item.CloseSalesDiscount = Convert.ToBoolean(reader["user_CloseSalesDiscount"]);
                    item.Branch = Convert.ToInt32(reader["user_Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["user_Prefix"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["user_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["user_Project"]);
                    item.Cash = iCore.IsDbNullRtNull(reader["user_Cash"]);
                    item.WarehouseNo = Convert.ToInt32(reader["user_WarehouseNo"]);
                    item.Disable = Convert.ToBoolean(reader["user_Disable"]);
                    item.ActiveFilter = Convert.ToBoolean(reader["user_ActiveFilter"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public Users GetItem(Guid? Key)
        {
            Users item = new Users();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from px_Users where [user_Key]=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
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
                    item.Key = iCore.IsDbNullRtNull(reader["user_Key"]);
                    item.Subscribe = iCore.IsDbNullRtNull(reader["user_Subscribe"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["user_CreateDate"]);
                    item.UpdateDate = iCore.IsDbNullRtNullDate(reader["user_UpdateDate"]);
                    item.Kind = Convert.ToInt32(reader["user_Kind"]);
                    item.No = Convert.ToInt32(reader["user_No"]);
                    item.Name1 = Convert.ToString(reader["user_Name1"]);
                    item.Name2 = Convert.ToString(reader["user_Name2"]);
                    item.Passwoard = Convert.ToString(reader["user_Passwoard"]);
                    item.Email = Convert.ToString(reader["user_Email"]);
                    item.Phone = Convert.ToString(reader["user_Phone"]);
                    item.WhatsappNumber = Convert.ToString(reader["user_WhatsappNumber"]);
                    item.DataBase = iCore.IsDbNullRtNull(reader["user_DataBase"]);
                    item.Employee = iCore.IsDbNullRtNull(reader["user_Employee"]);
                    item.AppSalesPerson = Convert.ToBoolean(reader["user_AppSalesPerson"]);
                    item.AppSales = Convert.ToBoolean(reader["user_AppSales"]);
                    item.AppWarehouse = Convert.ToBoolean(reader["user_AppWarehouse"]);
                    item.CanDelete = Convert.ToBoolean(reader["user_CanDelete"]);
                    item.CanPost = Convert.ToBoolean(reader["user_CanPost"]);
                    item.ConfirmFinance = Convert.ToBoolean(reader["user_ConfirmFinance"]);
                    item.ConfirmInventory = Convert.ToBoolean(reader["user_ConfirmInventory"]);
                    item.ConfirmPurchase = Convert.ToBoolean(reader["user_ConfirmPurchase"]);
                    item.ConfirmSales = Convert.ToBoolean(reader["user_ConfirmSales"]);
                    item.CloseSalesPrice = Convert.ToBoolean(reader["user_CloseSalesPrice"]);
                    item.CloseSalesDiscount = Convert.ToBoolean(reader["user_CloseSalesDiscount"]);
                    item.Branch = Convert.ToInt32(reader["user_Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["user_Prefix"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["user_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["user_Project"]);
                    item.Cash = iCore.IsDbNullRtNull(reader["user_Cash"]);
                    item.WarehouseNo = Convert.ToInt32(reader["user_WarehouseNo"]);
                    item.Disable = Convert.ToBoolean(reader["user_Disable"]);
                    item.ActiveFilter = Convert.ToBoolean(reader["user_ActiveFilter"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(Users item)
        {
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO px_Users ");
                str.Append("([user_Key]");
                str.Append(",[user_Subscribe]");
                str.Append(",[user_CreateDate]");
                str.Append(",[user_UpdateDate]");
                str.Append(",[user_Kind]");
                str.Append(",[user_No]");
                str.Append(",[user_Name1]");
                str.Append(",[user_Name2]");
                str.Append(",[user_Passwoard]");
                str.Append(",[user_Email]");
                str.Append(",[user_Phone]");
                str.Append(",[user_WhatsappNumber]");
                str.Append(",[user_DataBase]");
                str.Append(",[user_Employee]");
                str.Append(",[user_AppSalesPerson]");
                str.Append(",[user_AppSales]");
                str.Append(",[user_AppWarehouse]");
                str.Append(",[user_CanDelete]");
                str.Append(",[user_CanPost]");
                str.Append(",[user_ConfirmFinance]");
                str.Append(",[user_ConfirmInventory]");
                str.Append(",[user_ConfirmPurchase]");
                str.Append(",[user_ConfirmSales]");
                str.Append(",[user_CloseSalesPrice]");
                str.Append(",[user_CloseSalesDiscount]");
                str.Append(",[user_Branch]");
                str.Append(",[user_Prefix]");
                str.Append(",[user_CostCenter]");
                str.Append(",[user_Project]");
                str.Append(",[user_Cash]");
                str.Append(",[user_WarehouseNo]");
                str.Append(",[user_ActiveFilter]");
                str.Append(",[user_Disable])");
                str.Append(" VALUES ");
                str.Append("(@user_Key");
                str.Append(",@user_Subscribe");
                str.Append(",@user_CreateDate");
                str.Append(",@user_UpdateDate");
                str.Append(",@user_Kind");
                str.Append(",(select top 100 percent isnull(max(x.user_no)+1,1) from px_Users x) ");
                str.Append(",@user_Name1");
                str.Append(",@user_Name2");
                str.Append(",@user_Passwoard");
                str.Append(",@user_Email");
                str.Append(",@user_Phone");
                str.Append(",@user_WhatsappNumber");
                str.Append(",@user_DataBase");
                str.Append(",@user_Employee");
                str.Append(",@user_AppSalesPerson");
                str.Append(",@user_AppSales");
                str.Append(",@user_AppWarehouse");
                str.Append(",@user_CanDelete");
                str.Append(",@user_CanPost");
                str.Append(",@user_ConfirmFinance");
                str.Append(",@user_ConfirmInventory");
                str.Append(",@user_ConfirmPurchase");
                str.Append(",@user_ConfirmSales");
                str.Append(",@user_CloseSalesPrice");
                str.Append(",@user_CloseSalesDiscount");
                str.Append(",@user_Branch");
                str.Append(",@user_Prefix");
                str.Append(",@user_CostCenter");
                str.Append(",@user_Project");
                str.Append(",@user_Cash");
                str.Append(",@user_WarehouseNo");
                str.Append(",@user_ActiveFilter");
                str.Append(",@user_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@user_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@user_Subscribe", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Subscribe);
                comm.Parameters.Add("@user_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@user_UpdateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.UpdateDate);
                comm.Parameters.Add("@user_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@user_Name1", SqlDbType.NVarChar, 100).Value = item.Name1 ?? "";
                comm.Parameters.Add("@user_Name2", SqlDbType.NVarChar, 100).Value = item.Name2 ?? "";
                comm.Parameters.Add("@user_Passwoard", SqlDbType.NVarChar, 200).Value = item.Passwoard ?? "";
                comm.Parameters.Add("@user_Email", SqlDbType.NVarChar, 500).Value = item.Email ?? "";
                comm.Parameters.Add("@user_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@user_WhatsappNumber", SqlDbType.NVarChar, 15).Value = item.WhatsappNumber ?? "";
                comm.Parameters.Add("@user_DataBase", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.DataBase);
                comm.Parameters.Add("@user_Employee", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Employee);
                comm.Parameters.Add("@user_AppSalesPerson", SqlDbType.Bit).Value = item.AppSalesPerson;
                comm.Parameters.Add("@user_AppSales", SqlDbType.Bit).Value = item.AppSales;
                comm.Parameters.Add("@user_AppWarehouse", SqlDbType.Bit).Value = item.AppWarehouse;
                comm.Parameters.Add("@user_CanDelete", SqlDbType.Bit).Value = item.CanDelete;
                comm.Parameters.Add("@user_CanPost", SqlDbType.Bit).Value = item.CanPost;
                comm.Parameters.Add("@user_ConfirmFinance", SqlDbType.Bit).Value = item.ConfirmFinance;
                comm.Parameters.Add("@user_ConfirmInventory", SqlDbType.Bit).Value = item.ConfirmInventory;
                comm.Parameters.Add("@user_ConfirmPurchase", SqlDbType.Bit).Value = item.ConfirmPurchase;
                comm.Parameters.Add("@user_ConfirmSales", SqlDbType.Bit).Value = item.ConfirmSales;
                comm.Parameters.Add("@user_CloseSalesPrice", SqlDbType.Bit).Value = item.CloseSalesPrice;
                comm.Parameters.Add("@user_CloseSalesDiscount", SqlDbType.Bit).Value = item.CloseSalesDiscount;
                comm.Parameters.Add("@user_Branch", SqlDbType.Int).Value = item.Branch;
                comm.Parameters.Add("@user_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Prefix);
                comm.Parameters.Add("@user_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                comm.Parameters.Add("@user_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                comm.Parameters.Add("@user_Cash", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Cash);
                comm.Parameters.Add("@user_WarehouseNo", SqlDbType.Int).Value = item.WarehouseNo;
                comm.Parameters.Add("@user_ActiveFilter", SqlDbType.Bit).Value = item.ActiveFilter;
                comm.Parameters.Add("@user_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(Users item)
        {
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update px_Users SET ");
                str.Append(" [user_UpdateDate]=@user_UpdateDate");
                str.Append(",[user_Name1]=@user_Name1");
                str.Append(",[user_Name2]=@user_Name2");
                str.Append(",[user_Email]=@user_Email");
                str.Append(",[user_Phone]=@user_Phone");
                str.Append(",[user_WhatsappNumber]=@user_WhatsappNumber");
                str.Append(",[user_DataBase]=@user_DataBase");
                str.Append(",[user_Employee]=@user_Employee");
                str.Append(",[user_AppSalesPerson]=@user_AppSalesPerson");
                str.Append(",[user_AppSales]=@user_AppSales");
                str.Append(",[user_AppWarehouse]=@user_AppWarehouse");
                str.Append(",[user_CanDelete]=@user_CanDelete");
                str.Append(",[user_CanPost]=@user_CanPost");
                str.Append(",[user_ConfirmFinance]=@user_ConfirmFinance");
                str.Append(",[user_ConfirmInventory]=@user_ConfirmInventory");
                str.Append(",[user_ConfirmPurchase]=@user_ConfirmPurchase");
                str.Append(",[user_ConfirmSales]=@user_ConfirmSales");
                str.Append(",[user_CloseSalesPrice]=@user_CloseSalesPrice");
                str.Append(",[user_CloseSalesDiscount]=@user_CloseSalesDiscount");
                str.Append(",[user_Branch]=@user_Branch");
                str.Append(",[user_Prefix]=@user_Prefix");
                str.Append(",[user_CostCenter]=@user_CostCenter");
                str.Append(",[user_Project]=@user_Project");
                str.Append(",[user_Cash]=@user_Cash");
                str.Append(",[user_WarehouseNo]=@user_WarehouseNo");
                str.Append(",[user_Disable]=@user_Disable");
                str.Append(",[user_ActiveFilter]=@user_ActiveFilter");
                str.Append(" WHERE user_Key=@user_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@user_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@user_UpdateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.UpdateDate);
                comm.Parameters.Add("@user_Name1", SqlDbType.NVarChar, 100).Value = item.Name1 ?? "";
                comm.Parameters.Add("@user_Name2", SqlDbType.NVarChar, 100).Value = item.Name2 ?? "";
                comm.Parameters.Add("@user_Passwoard", SqlDbType.NVarChar, 200).Value = item.Passwoard ?? "";
                comm.Parameters.Add("@user_Email", SqlDbType.NVarChar, 500).Value = item.Email ?? "";
                comm.Parameters.Add("@user_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@user_WhatsappNumber", SqlDbType.NVarChar, 15).Value = item.WhatsappNumber ?? "";
                comm.Parameters.Add("@user_DataBase", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.DataBase);
                comm.Parameters.Add("@user_Employee", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Employee);
                comm.Parameters.Add("@user_AppSalesPerson", SqlDbType.Bit).Value = item.AppSalesPerson;
                comm.Parameters.Add("@user_AppSales", SqlDbType.Bit).Value = item.AppSales;
                comm.Parameters.Add("@user_AppWarehouse", SqlDbType.Bit).Value = item.AppWarehouse;
                comm.Parameters.Add("@user_CanDelete", SqlDbType.Bit).Value = item.CanDelete;
                comm.Parameters.Add("@user_CanPost", SqlDbType.Bit).Value = item.CanPost;
                comm.Parameters.Add("@user_ConfirmFinance", SqlDbType.Bit).Value = item.ConfirmFinance;
                comm.Parameters.Add("@user_ConfirmInventory", SqlDbType.Bit).Value = item.ConfirmInventory;
                comm.Parameters.Add("@user_ConfirmPurchase", SqlDbType.Bit).Value = item.ConfirmPurchase;
                comm.Parameters.Add("@user_ConfirmSales", SqlDbType.Bit).Value = item.ConfirmSales;
                comm.Parameters.Add("@user_CloseSalesPrice", SqlDbType.Bit).Value = item.CloseSalesPrice;
                comm.Parameters.Add("@user_CloseSalesDiscount", SqlDbType.Bit).Value = item.CloseSalesDiscount;
                comm.Parameters.Add("@user_Branch", SqlDbType.Int).Value = item.Branch;
                comm.Parameters.Add("@user_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Prefix);
                comm.Parameters.Add("@user_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CostCenter);
                comm.Parameters.Add("@user_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Project);
                comm.Parameters.Add("@user_Cash", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Cash);
                comm.Parameters.Add("@user_WarehouseNo", SqlDbType.Int).Value = item.WarehouseNo;
                comm.Parameters.Add("@user_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@user_ActiveFilter", SqlDbType.Bit).Value = item.ActiveFilter;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static int Delete(Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                string delQuery = " update   [px_Users] set [user_Disable]=1 where [user_Key]=@Key";
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
        public static int CheckUserEmail(string Email)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                con.Open();
                string delQuery = "select count(*) from [px_Users] where [user_Email]=@Email  ";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value = Email;
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    res = (int)reader[0];
                }
                reader.Close();
            }
            return res;
        }
        public static int CheckUserUsername(string Username)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                con.Open();
                string delQuery = "select count(*) from [px_Users] where [user_Phone]=@Username";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Username", SqlDbType.NVarChar, 255).Value = Username ?? "";
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    res = (int)reader[0];
                }
                reader.Close();
            }
            return res;
        }

        public static int UpdatePassword(Guid? Key, string Pass)
        {
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update px_Users SET ");
                str.Append(" [user_Passwoard]=@user_Passwoard");
                str.Append(" WHERE user_Key=@user_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@user_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                comm.Parameters.Add("@user_Passwoard", SqlDbType.NVarChar, 100).Value = Pass ?? "";

                con.Open();
                return comm.ExecuteNonQuery();
            }
        }
    }
}
