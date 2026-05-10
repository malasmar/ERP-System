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
    public class StockItem
    {
        public Guid? Key { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public Guid? Category { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public Guid? vatKey { get; set; }
        public decimal Total { get; set; }
        public string Barcode { get; set; }
        public string Unit { get; set; }
        public Boolean EnablePart { get; set; }
        public decimal PartRate { get; set; }
        public string PartName { get; set; }
        public decimal PartPrice { get; set; }
        public Boolean Favorite { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Target { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public decimal Thickness { get; set; }
        public Guid? Group { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Location { get; set; }
        public Boolean Stocktaking { get; set; }
        public Boolean EnableBatch { get; set; }
        public Boolean EnableSerial { get; set; }
        public Boolean EnableColor { get; set; }
        public Boolean EnableSize { get; set; }
        public Boolean EnablePkg { get; set; }
        public Boolean EnableProduction { get; set; }
        public Guid? WorkCenter { get; set; }
        public Guid? GenericName { get; set; }
        public string Comment { get; set; }
        public Guid? Supplier { get; set; }
        public string Image { get; set; }
        public Boolean Disable { get; set; }
        public bool EnableApplication { get; set; }
        public List<StockItem> GetList(string DB)
        {
            List<StockItem> items = new List<StockItem>();
            string selQuery = "select top 100 percent * from invCard_StockItem order by [item_Category],[item_Code]";
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
                    StockItem item = new StockItem();
                    item.Key = iCore.IsDbNullRtNull(reader["item_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["item_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["item_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["item_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["item_LastupDate"]);
                    item.Category = iCore.IsDbNullRtNull(reader["item_Category"]);
                    item.Code = Convert.ToString(reader["item_Code"]);
                    item.Name1 = Convert.ToString(reader["item_Name1"]);
                    item.Name2 = Convert.ToString(reader["item_Name2"]);
                    item.Cost = Convert.ToDecimal(reader["item_Cost"]);
                    item.Price = Convert.ToDecimal(reader["item_Price"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["item_vatKey"]);
                    item.Total = Convert.ToDecimal(reader["item_Total"]);
                    item.Barcode = Convert.ToString(reader["item_Barcode"]);
                    item.Unit = Convert.ToString(reader["item_Unit"]);
                    item.EnablePart = Convert.ToBoolean(reader["item_EnablePart"]);
                    item.PartRate = Convert.ToDecimal(reader["item_PartRate"]);
                    item.PartName = Convert.ToString(reader["item_PartName"]);
                    item.PartPrice = Convert.ToDecimal(reader["item_PartPrice"]);
                    item.Favorite = Convert.ToBoolean(reader["item_Favorite"]);
                    item.Min = Convert.ToDecimal(reader["item_Min"]);
                    item.Max = Convert.ToDecimal(reader["item_Max"]);
                    item.Target = Convert.ToDecimal(reader["item_Target"]);
                    item.Length = Convert.ToDecimal(reader["item_Length"]);
                    item.Width = Convert.ToDecimal(reader["item_Width"]);
                    item.Height = Convert.ToDecimal(reader["item_Height"]);
                    item.Weight = Convert.ToDecimal(reader["item_Weight"]);
                    item.Thickness = Convert.ToDecimal(reader["item_Thickness"]);
                    item.Group = iCore.IsDbNullRtNull(reader["item_Group"]);
                    item.Model = Convert.ToString(reader["item_Model"]);
                    item.Brand = Convert.ToString(reader["item_Brand"]);
                    item.Location = Convert.ToString(reader["item_Location"]);
                    item.Stocktaking = Convert.ToBoolean(reader["item_Stocktaking"]);
                    item.EnableBatch = Convert.ToBoolean(reader["item_EnableBatch"]);
                    item.EnableSerial = Convert.ToBoolean(reader["item_EnableSerial"]);
                    item.EnableColor = Convert.ToBoolean(reader["item_EnableColor"]);
                    item.EnableSize = Convert.ToBoolean(reader["item_EnableSize"]);
                    item.EnablePkg = Convert.ToBoolean(reader["item_EnablePkg"]);
                    item.EnableProduction = Convert.ToBoolean(reader["item_EnableProduction"]);
                    item.WorkCenter = iCore.IsDbNullRtNull(reader["item_WorkCenter"]);
                    item.GenericName = iCore.IsDbNullRtNull(reader["item_GenericName"]);
                    item.Comment = Convert.ToString(reader["item_Comment"]);
                    item.Supplier = iCore.IsDbNullRtNull(reader["item_Supplier"]);
                    item.Image = Convert.ToString(reader["item_Image"]);
                    item.Disable = Convert.ToBoolean(reader["item_Disable"]);
                    item.EnableApplication = Convert.ToBoolean(reader["item_EnableApplication"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<StockItem> GetPackages(string DB)
        {
            List<StockItem> items = new List<StockItem>();
            string selQuery = "select top 100 percent * from invCard_StockItem where [item_EnablePkg]=1 order by [item_Category],[item_Code]";
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
                    StockItem item = new StockItem();
                    item.Key = iCore.IsDbNullRtNull(reader["item_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["item_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["item_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["item_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["item_LastupDate"]);
                    item.Category = iCore.IsDbNullRtNull(reader["item_Category"]);
                    item.Code = Convert.ToString(reader["item_Code"]);
                    item.Name1 = Convert.ToString(reader["item_Name1"]);
                    item.Name2 = Convert.ToString(reader["item_Name2"]);
                    item.Cost = Convert.ToDecimal(reader["item_Cost"]);
                    item.Price = Convert.ToDecimal(reader["item_Price"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["item_vatKey"]);
                    item.Total = Convert.ToDecimal(reader["item_Total"]);
                    item.Barcode = Convert.ToString(reader["item_Barcode"]);
                    item.Unit = Convert.ToString(reader["item_Unit"]);
                    item.EnablePart = Convert.ToBoolean(reader["item_EnablePart"]);
                    item.PartRate = Convert.ToDecimal(reader["item_PartRate"]);
                    item.PartName = Convert.ToString(reader["item_PartName"]);
                    item.PartPrice = Convert.ToDecimal(reader["item_PartPrice"]);
                    item.Favorite = Convert.ToBoolean(reader["item_Favorite"]);
                    item.Min = Convert.ToDecimal(reader["item_Min"]);
                    item.Max = Convert.ToDecimal(reader["item_Max"]);
                    item.Target = Convert.ToDecimal(reader["item_Target"]);
                    item.Length = Convert.ToDecimal(reader["item_Length"]);
                    item.Width = Convert.ToDecimal(reader["item_Width"]);
                    item.Height = Convert.ToDecimal(reader["item_Height"]);
                    item.Weight = Convert.ToDecimal(reader["item_Weight"]);
                    item.Thickness = Convert.ToDecimal(reader["item_Thickness"]);
                    item.Group = iCore.IsDbNullRtNull(reader["item_Group"]);
                    item.Model = Convert.ToString(reader["item_Model"]);
                    item.Brand = Convert.ToString(reader["item_Brand"]);
                    item.Location = Convert.ToString(reader["item_Location"]);
                    item.Stocktaking = Convert.ToBoolean(reader["item_Stocktaking"]);
                    item.EnableBatch = Convert.ToBoolean(reader["item_EnableBatch"]);
                    item.EnableSerial = Convert.ToBoolean(reader["item_EnableSerial"]);
                    item.EnableColor = Convert.ToBoolean(reader["item_EnableColor"]);
                    item.EnableSize = Convert.ToBoolean(reader["item_EnableSize"]);
                    item.EnablePkg = Convert.ToBoolean(reader["item_EnablePkg"]);
                    item.EnableProduction = Convert.ToBoolean(reader["item_EnableProduction"]);
                    item.WorkCenter = iCore.IsDbNullRtNull(reader["item_WorkCenter"]);
                    item.GenericName = iCore.IsDbNullRtNull(reader["item_GenericName"]);
                    item.Comment = Convert.ToString(reader["item_Comment"]);
                    item.Supplier = iCore.IsDbNullRtNull(reader["item_Supplier"]);
                    item.Image = Convert.ToString(reader["item_Image"]);
                    item.Disable = Convert.ToBoolean(reader["item_Disable"]);
                    item.EnableApplication = Convert.ToBoolean(reader["item_EnableApplication"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public StockItem GetItem(string DB, Guid? Key)
        {
            StockItem item = new StockItem();
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from invCard_StockItem where [item_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["item_Key"]);
                    item.CreateUser = Convert.ToInt32(reader["item_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["item_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["item_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["item_LastupDate"]);
                    item.Category = iCore.IsDbNullRtNull(reader["item_Category"]);
                    item.Code = Convert.ToString(reader["item_Code"]);
                    item.Name1 = Convert.ToString(reader["item_Name1"]);
                    item.Name2 = Convert.ToString(reader["item_Name2"]);
                    item.Cost = Convert.ToDecimal(reader["item_Cost"]);
                    item.Price = Convert.ToDecimal(reader["item_Price"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["item_vatKey"]);
                    item.Total = Convert.ToDecimal(reader["item_Total"]);
                    item.Barcode = Convert.ToString(reader["item_Barcode"]);
                    item.Unit = Convert.ToString(reader["item_Unit"]);
                    item.EnablePart = Convert.ToBoolean(reader["item_EnablePart"]);
                    item.PartRate = Convert.ToDecimal(reader["item_PartRate"]);
                    item.PartName = Convert.ToString(reader["item_PartName"]);
                    item.PartPrice = Convert.ToDecimal(reader["item_PartPrice"]);
                    item.Favorite = Convert.ToBoolean(reader["item_Favorite"]);
                    item.Min = Convert.ToDecimal(reader["item_Min"]);
                    item.Max = Convert.ToDecimal(reader["item_Max"]);
                    item.Target = Convert.ToDecimal(reader["item_Target"]);
                    item.Length = Convert.ToDecimal(reader["item_Length"]);
                    item.Width = Convert.ToDecimal(reader["item_Width"]);
                    item.Height = Convert.ToDecimal(reader["item_Height"]);
                    item.Weight = Convert.ToDecimal(reader["item_Weight"]);
                    item.Thickness = Convert.ToDecimal(reader["item_Thickness"]);
                    item.Group = iCore.IsDbNullRtNull(reader["item_Group"]);
                    item.Model = Convert.ToString(reader["item_Model"]);
                    item.Brand = Convert.ToString(reader["item_Brand"]);
                    item.Location = Convert.ToString(reader["item_Location"]);
                    item.Stocktaking = Convert.ToBoolean(reader["item_Stocktaking"]);
                    item.EnableBatch = Convert.ToBoolean(reader["item_EnableBatch"]);
                    item.EnableSerial = Convert.ToBoolean(reader["item_EnableSerial"]);
                    item.EnableColor = Convert.ToBoolean(reader["item_EnableColor"]);
                    item.EnableSize = Convert.ToBoolean(reader["item_EnableSize"]);
                    item.EnablePkg = Convert.ToBoolean(reader["item_EnablePkg"]);
                    item.EnableProduction = Convert.ToBoolean(reader["item_EnableProduction"]);
                    item.WorkCenter = iCore.IsDbNullRtNull(reader["item_WorkCenter"]);
                    item.GenericName = iCore.IsDbNullRtNull(reader["item_GenericName"]);
                    item.Comment = Convert.ToString(reader["item_Comment"]);
                    item.Supplier = iCore.IsDbNullRtNull(reader["item_Supplier"]);
                    item.Image = Convert.ToString(reader["item_Image"]);
                    item.Disable = Convert.ToBoolean(reader["item_Disable"]);
                    item.EnableApplication = Convert.ToBoolean(reader["item_EnableApplication"]);
                }
                reader.Close();
            }
            return item;
        }

        public static Guid? Insert(string DB, StockItem item)
        {
            Guid? key = Guid.NewGuid();
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO invCard_StockItem");
                str.Append("([item_Key]");
                str.Append(",[item_CreateUser]");
                str.Append(",[item_CreateDate]");
                str.Append(",[item_LastupUser]");
                str.Append(",[item_LastupDate]");
                str.Append(",[item_Category]");
                str.Append(",[item_Code]");
                str.Append(",[item_Name1]");
                str.Append(",[item_Name2]");
                str.Append(",[item_Cost]");
                str.Append(",[item_Price]");
                str.Append(",[item_vatKey]");
                str.Append(",[item_Total]");
                str.Append(",[item_Barcode]");
                str.Append(",[item_Unit]");
                str.Append(",[item_EnablePart]");
                str.Append(",[item_PartRate]");
                str.Append(",[item_PartName]");
                str.Append(",[item_PartPrice]");
                str.Append(",[item_Favorite]");
                str.Append(",[item_Min]");
                str.Append(",[item_Max]");
                str.Append(",[item_Target]");
                str.Append(",[item_Length]");
                str.Append(",[item_Width]");
                str.Append(",[item_Height]");
                str.Append(",[item_Weight]");
                str.Append(",[item_Thickness]");
                str.Append(",[item_Group]");
                str.Append(",[item_Model]");
                str.Append(",[item_Brand]");
                str.Append(",[item_Location]");
                str.Append(",[item_Stocktaking]");
                str.Append(",[item_EnableBatch]");
                str.Append(",[item_EnableSerial]");
                str.Append(",[item_EnableColor]");
                str.Append(",[item_EnableSize]");
                str.Append(",[item_EnablePkg]");
                str.Append(",[item_EnableProduction]");
                str.Append(",[item_WorkCenter]");
                str.Append(",[item_GenericName]");
                str.Append(",[item_Comment]");
                str.Append(",[item_Supplier]");
                str.Append(",[item_Image]");
                str.Append(",[item_Disable]");
                str.Append(",[item_EnableApplication])");
                str.Append(" VALUES ");
                str.Append("(@item_Key");
                str.Append(",@item_CreateUser");
                str.Append(",@item_CreateDate");
                str.Append(",@item_LastupUser");
                str.Append(",@item_LastupDate");
                str.Append(",@item_Category");
                str.Append(",@item_Code");
                str.Append(",@item_Name1");
                str.Append(",@item_Name2");
                str.Append(",@item_Cost");
                str.Append(",@item_Price");
                str.Append(",@item_vatKey");
                str.Append(",@item_Total");
                str.Append(",@item_Barcode");
                str.Append(",@item_Unit");
                str.Append(",@item_EnablePart");
                str.Append(",@item_PartRate");
                str.Append(",@item_PartName");
                str.Append(",@item_PartPrice");
                str.Append(",@item_Favorite");
                str.Append(",@item_Min");
                str.Append(",@item_Max");
                str.Append(",@item_Target");
                str.Append(",@item_Length");
                str.Append(",@item_Width");
                str.Append(",@item_Height");
                str.Append(",@item_Weight");
                str.Append(",@item_Thickness");
                str.Append(",@item_Group");
                str.Append(",@item_Model");
                str.Append(",@item_Brand");
                str.Append(",@item_Location");
                str.Append(",@item_Stocktaking");
                str.Append(",@item_EnableBatch");
                str.Append(",@item_EnableSerial");
                str.Append(",@item_EnableColor");
                str.Append(",@item_EnableSize");
                str.Append(",@item_EnablePkg");
                str.Append(",@item_EnableProduction");
                str.Append(",@item_WorkCenter");
                str.Append(",@item_GenericName");
                str.Append(",@item_Comment");
                str.Append(",@item_Supplier");
                str.Append(",@item_Image");
                str.Append(",@item_Disable");
                str.Append(",@item_EnableApplication)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@item_Key", SqlDbType.UniqueIdentifier).Value = key;
                comm.Parameters.Add("@item_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@item_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@item_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@item_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@item_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Category);
                comm.Parameters.Add("@item_Code", SqlDbType.NVarChar, 100).Value = item.Code ?? "";
                comm.Parameters.Add("@item_Name1", SqlDbType.NVarChar, 1000).Value = item.Name1 ?? "";
                comm.Parameters.Add("@item_Name2", SqlDbType.NVarChar, 1000).Value = item.Name2 ?? "";
                comm.Parameters.Add("@item_Cost", SqlDbType.Decimal).Value = item.Cost;
                comm.Parameters.Add("@item_Price", SqlDbType.Decimal).Value = item.Price;
                comm.Parameters.Add("@item_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                comm.Parameters.Add("@item_Total", SqlDbType.Decimal).Value = item.Total;
                comm.Parameters.Add("@item_Barcode", SqlDbType.NVarChar, 50).Value = item.Barcode ?? "";
                comm.Parameters.Add("@item_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                comm.Parameters.Add("@item_EnablePart", SqlDbType.Bit).Value = item.EnablePart;
                comm.Parameters.Add("@item_PartRate", SqlDbType.Decimal).Value = item.PartRate;
                comm.Parameters.Add("@item_PartName", SqlDbType.NVarChar, 5).Value = item.PartName ?? "";
                comm.Parameters.Add("@item_PartPrice", SqlDbType.Decimal).Value = item.PartPrice;
                comm.Parameters.Add("@item_Favorite", SqlDbType.Bit).Value = item.Favorite;
                comm.Parameters.Add("@item_Min", SqlDbType.Decimal).Value = item.Min;
                comm.Parameters.Add("@item_Max", SqlDbType.Decimal).Value = item.Max;
                comm.Parameters.Add("@item_Target", SqlDbType.Decimal).Value = item.Target;
                comm.Parameters.Add("@item_Length", SqlDbType.Decimal).Value = item.Length;
                comm.Parameters.Add("@item_Width", SqlDbType.Decimal).Value = item.Width;
                comm.Parameters.Add("@item_Height", SqlDbType.Decimal).Value = item.Height;
                comm.Parameters.Add("@item_Weight", SqlDbType.Decimal).Value = item.Weight;
                comm.Parameters.Add("@item_Thickness", SqlDbType.Decimal).Value = item.Thickness;
                comm.Parameters.Add("@item_Group", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Group);
                comm.Parameters.Add("@item_Model", SqlDbType.NVarChar, 200).Value = item.Model ?? "";
                comm.Parameters.Add("@item_Brand", SqlDbType.NVarChar, 200).Value = item.Brand ?? "";
                comm.Parameters.Add("@item_Location", SqlDbType.NVarChar, 200).Value = item.Location ?? "";
                comm.Parameters.Add("@item_Stocktaking", SqlDbType.Bit).Value = item.Stocktaking;
                comm.Parameters.Add("@item_EnableBatch", SqlDbType.Bit).Value = item.EnableBatch;
                comm.Parameters.Add("@item_EnableSerial", SqlDbType.Bit).Value = item.EnableSerial;
                comm.Parameters.Add("@item_EnableColor", SqlDbType.Bit).Value = item.EnableColor;
                comm.Parameters.Add("@item_EnableSize", SqlDbType.Bit).Value = item.EnableSize;
                comm.Parameters.Add("@item_EnablePkg", SqlDbType.Bit).Value = item.EnablePkg;
                comm.Parameters.Add("@item_EnableProduction", SqlDbType.Bit).Value = item.EnableProduction;
                comm.Parameters.Add("@item_WorkCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.WorkCenter);
                comm.Parameters.Add("@item_GenericName", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.GenericName);
                comm.Parameters.Add("@item_Comment", SqlDbType.NVarChar, 1000).Value = item.Comment ?? "";
                comm.Parameters.Add("@item_Supplier", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Supplier);
                comm.Parameters.Add("@item_Image", SqlDbType.NVarChar, 1000).Value = item.Image ?? "";
                comm.Parameters.Add("@item_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@item_EnableApplication", SqlDbType.Bit).Value = item.EnableApplication;
                con.Open();
                comm.ExecuteNonQuery();
            }
            return key;
        }

        public static Guid? Update(string DB, StockItem item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update invCard_StockItem SET ");
                str.Append("[item_CreateUser]=@item_CreateUser");
                str.Append(",[item_CreateDate]=@item_CreateDate");
                str.Append(",[item_LastupUser]=@item_LastupUser");
                str.Append(",[item_LastupDate]=@item_LastupDate");
                str.Append(",[item_Category]=@item_Category");
                str.Append(",[item_Code]=@item_Code");
                str.Append(",[item_Name1]=@item_Name1");
                str.Append(",[item_Name2]=@item_Name2");
                str.Append(",[item_Cost]=@item_Cost");
                str.Append(",[item_Price]=@item_Price");
                str.Append(",[item_vatKey]=@item_vatKey");
                str.Append(",[item_Total]=@item_Total");
                str.Append(",[item_Barcode]=@item_Barcode");
                str.Append(",[item_Unit]=@item_Unit");
                str.Append(",[item_EnablePart]=@item_EnablePart");
                str.Append(",[item_PartRate]=@item_PartRate");
                str.Append(",[item_PartName]=@item_PartName");
                str.Append(",[item_PartPrice]=@item_PartPrice");
                str.Append(",[item_Favorite]=@item_Favorite");
                str.Append(",[item_Min]=@item_Min");
                str.Append(",[item_Max]=@item_Max");
                str.Append(",[item_Target]=@item_Target");
                str.Append(",[item_Length]=@item_Length");
                str.Append(",[item_Width]=@item_Width");
                str.Append(",[item_Height]=@item_Height");
                str.Append(",[item_Weight]=@item_Weight");
                str.Append(",[item_Thickness]=@item_Thickness");
                str.Append(",[item_Group]=@item_Group");
                str.Append(",[item_Model]=@item_Model");
                str.Append(",[item_Brand]=@item_Brand");
                str.Append(",[item_Location]=@item_Location");
                str.Append(",[item_Stocktaking]=@item_Stocktaking");
                str.Append(",[item_EnableBatch]=@item_EnableBatch");
                str.Append(",[item_EnableSerial]=@item_EnableSerial");
                str.Append(",[item_EnableColor]=@item_EnableColor");
                str.Append(",[item_EnableSize]=@item_EnableSize");
                str.Append(",[item_EnablePkg]=@item_EnablePkg");
                str.Append(",[item_EnableProduction]=@item_EnableProduction");
                str.Append(",[item_WorkCenter]=@item_WorkCenter");
                str.Append(",[item_GenericName]=@item_GenericName");
                str.Append(",[item_Comment]=@item_Comment");
                str.Append(",[item_Supplier]=@item_Supplier");
                str.Append(",[item_Image]=@item_Image");
                str.Append(",[item_Disable]=@item_Disable");
                str.Append(",[item_EnableApplication]=@item_EnableApplication");
                str.Append(" WHERE item_Key=@item_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@item_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@item_CreateUser", SqlDbType.Int).Value = item.CreateUser;
                comm.Parameters.Add("@item_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@item_LastupUser", SqlDbType.Int).Value = item.LastupUser;
                comm.Parameters.Add("@item_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.LastupDate);
                comm.Parameters.Add("@item_Category", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Category);
                comm.Parameters.Add("@item_Code", SqlDbType.NVarChar, 100).Value = item.Code ?? "";
                comm.Parameters.Add("@item_Name1", SqlDbType.NVarChar, 1000).Value = item.Name1 ?? "";
                comm.Parameters.Add("@item_Name2", SqlDbType.NVarChar, 1000).Value = item.Name2 ?? "";
                comm.Parameters.Add("@item_Cost", SqlDbType.Decimal).Value = item.Cost;
                comm.Parameters.Add("@item_Price", SqlDbType.Decimal).Value = item.Price;
                comm.Parameters.Add("@item_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                comm.Parameters.Add("@item_Total", SqlDbType.Decimal).Value = item.Total;
                comm.Parameters.Add("@item_Barcode", SqlDbType.NVarChar, 50).Value = item.Barcode ?? "";
                comm.Parameters.Add("@item_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                comm.Parameters.Add("@item_EnablePart", SqlDbType.Bit).Value = item.EnablePart;
                comm.Parameters.Add("@item_PartRate", SqlDbType.Decimal).Value = item.PartRate;
                comm.Parameters.Add("@item_PartName", SqlDbType.NVarChar, 5).Value = item.PartName ?? "";
                comm.Parameters.Add("@item_PartPrice", SqlDbType.Decimal).Value = item.PartPrice;
                comm.Parameters.Add("@item_Favorite", SqlDbType.Bit).Value = item.Favorite;
                comm.Parameters.Add("@item_Min", SqlDbType.Decimal).Value = item.Min;
                comm.Parameters.Add("@item_Max", SqlDbType.Decimal).Value = item.Max;
                comm.Parameters.Add("@item_Target", SqlDbType.Decimal).Value = item.Target;
                comm.Parameters.Add("@item_Length", SqlDbType.Decimal).Value = item.Length;
                comm.Parameters.Add("@item_Width", SqlDbType.Decimal).Value = item.Width;
                comm.Parameters.Add("@item_Height", SqlDbType.Decimal).Value = item.Height;
                comm.Parameters.Add("@item_Weight", SqlDbType.Decimal).Value = item.Weight;
                comm.Parameters.Add("@item_Thickness", SqlDbType.Decimal).Value = item.Thickness;
                comm.Parameters.Add("@item_Group", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Group);
                comm.Parameters.Add("@item_Model", SqlDbType.NVarChar, 200).Value = item.Model ?? "";
                comm.Parameters.Add("@item_Brand", SqlDbType.NVarChar, 200).Value = item.Brand ?? "";
                comm.Parameters.Add("@item_Location", SqlDbType.NVarChar, 200).Value = item.Location ?? "";
                comm.Parameters.Add("@item_Stocktaking", SqlDbType.Bit).Value = item.Stocktaking;
                comm.Parameters.Add("@item_EnableBatch", SqlDbType.Bit).Value = item.EnableBatch;
                comm.Parameters.Add("@item_EnableSerial", SqlDbType.Bit).Value = item.EnableSerial;
                comm.Parameters.Add("@item_EnableColor", SqlDbType.Bit).Value = item.EnableColor;
                comm.Parameters.Add("@item_EnableSize", SqlDbType.Bit).Value = item.EnableSize;
                comm.Parameters.Add("@item_EnablePkg", SqlDbType.Bit).Value = item.EnablePkg;
                comm.Parameters.Add("@item_EnableProduction", SqlDbType.Bit).Value = item.EnableProduction;
                comm.Parameters.Add("@item_WorkCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.WorkCenter);
                comm.Parameters.Add("@item_GenericName", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.GenericName);
                comm.Parameters.Add("@item_Comment", SqlDbType.NVarChar, 1000).Value = item.Comment ?? "";
                comm.Parameters.Add("@item_Supplier", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Supplier);
                comm.Parameters.Add("@item_Image", SqlDbType.NVarChar, 1000).Value = item.Image ?? "";
                comm.Parameters.Add("@item_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@item_EnableApplication", SqlDbType.Bit).Value = item.EnableApplication;
                con.Open();
                comm.ExecuteNonQuery();
            }
            return item.Key;
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [invCard_StockItem] where [item_Key]=@Key";
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
