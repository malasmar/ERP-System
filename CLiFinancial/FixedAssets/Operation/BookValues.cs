using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.FixedAssets.Operation
{
    public class BookValues
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public int Status { get; set; }
        public int Branch { get; set; }
        public Guid? Prefix { get; set; }
        public int VoucherNo { get; set; }
        public DateTime? VoucherDate { get; set; }
        public string ReferenceNo { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal Total { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public List<BookValues> GetList(string DB)
        {
            List<BookValues> items = new List<BookValues>();
            string selQuery = "select top 100 percent * from finFixedAssets_BookValues order by [fxd_VoucherDate] desc,[fxd_VoucherNo] desc";
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
                    BookValues item = new BookValues();
                    item.RecNo = Convert.ToInt32(reader["fxd_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["fxd_OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["fxd_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["fxd_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["fxd_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["fxd_LastupDate"]);
                    item.Status = Convert.ToInt32(reader["fxd_Status"]);
                    item.Branch = Convert.ToInt32(reader["fxd_Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["fxd_Prefix"]);
                    item.VoucherNo = Convert.ToInt32(reader["fxd_VoucherNo"]);
                    item.VoucherDate = iCore.IsDbNullRtNullDate(reader["fxd_VoucherDate"]);
                    item.ReferenceNo = Convert.ToString(reader["fxd_ReferenceNo"]);
                    item.Description = Convert.ToString(reader["fxd_Description"]);
                    item.Quantity = Convert.ToDecimal(reader["fxd_Quantity"]);
                    item.Total = Convert.ToDecimal(reader["fxd_Total"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["fxd_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["fxd_Project"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public BookValues GetItem(string DB,int UserID, Guid? Key)
        {
            BookValues item = new BookValues();
            item.VoucherNo = 1;// xcore.MaxTransaction(DB, DocKind, Year);
            item.VoucherDate = DateTime.Now;
            item.CreateDate = DateTime.Now;
            item.CreateUser = UserID;
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from finFixedAssets_BookValues where [fxd_OperationKey]=@Key ";
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
                  
                    item.RecNo = Convert.ToInt32(reader["fxd_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["fxd_OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["fxd_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["fxd_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["fxd_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["fxd_LastupDate"]);
                    item.Status = Convert.ToInt32(reader["fxd_Status"]);
                    item.Branch = Convert.ToInt32(reader["fxd_Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["fxd_Prefix"]);
                    item.VoucherNo = Convert.ToInt32(reader["fxd_VoucherNo"]);
                    item.VoucherDate = iCore.IsDbNullRtNullDate(reader["fxd_VoucherDate"]);
                    item.ReferenceNo = Convert.ToString(reader["fxd_ReferenceNo"]);
                    item.Description = Convert.ToString(reader["fxd_Description"]);
                    item.Quantity = Convert.ToDecimal(reader["fxd_Quantity"]);
                    item.Total = Convert.ToDecimal(reader["fxd_Total"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["fxd_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["fxd_Project"]);
                 
                }
                reader.Close();
            }
            return item;
        }
    }
}
