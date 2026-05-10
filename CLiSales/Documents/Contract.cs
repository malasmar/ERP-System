using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiSales.Documents
{
    public class Contract
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
        public int InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string ReferenceNo { get; set; }
        public Guid? Client { get; set; }
        public Guid? SalesPerson { get; set; }
        public int SalesHand { get; set; }
        public string Description { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal vatAmount { get; set; }
        public decimal Total { get; set; }
        public decimal Quantity { get; set; }
        public Boolean Invoiced { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }

        public CLiCore.CardsInfo.User CreateUserInfo { get; set; }
        public CLiCore.CardsInfo.User UpdateUserInfo { get; set; }
        public Contract GetItem(string DB,string xLan, Guid UserKey, Guid? Key, int DocKind, int Year)
        {
            Contract item = new Contract();
            item.InvoiceNo = 1;// xcore.MaxTransaction(DB, DocKind, Year);
            item.InvoiceDate = DateTime.Now;
            item.CreateDate = DateTime.Now;
            item.CreateUserInfo = new CLiCore.CardsInfo.User().GetItem(xLan, UserKey);
            item.UpdateUserInfo = new CLiCore.CardsInfo.User().GetItem(xLan, UserKey);
            item.CreateUser = item.CreateUserInfo.No;
            CLiCore.Shared.UserDefaultData defaultData = CLiCore.Shared.UserDefaultData.GetItem(UserKey);
       
            item.Prefix = defaultData.Prefix;
            item.Branch = defaultData.Branch;
            item.Project = defaultData.Project;
            item.CostCenter = defaultData.CostCenter;
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from [SalesDocument_Contract] where [sal_OperationKey]=@Key ";
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
                    item.RecNo = Convert.ToInt32(reader["sal_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["sal_OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["sal_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["sal_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["sal_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["sal_LastupDate"]);
                    item.Status = Convert.ToInt32(reader["sal_Status"]);
                    item.Branch = Convert.ToInt32(reader["sal_Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["sal_Prefix"]);
                    item.InvoiceNo = Convert.ToInt32(reader["sal_InvoiceNo"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["sal_InvoiceDate"]);
                    item.ReferenceNo = Convert.ToString(reader["sal_ReferenceNo"]);
                    item.Client = iCore.IsDbNullRtNull(reader["sal_Client"]);
                    item.SalesPerson = iCore.IsDbNullRtNull(reader["sal_SalesPerson"]);
                    item.SalesHand = Convert.ToInt32(reader["sal_SalesHand"]);
                    item.Description = Convert.ToString(reader["sal_Description"]);
                    item.SubTotal = Convert.ToDecimal(reader["sal_SubTotal"]);
                    item.Discount = Convert.ToDecimal(reader["sal_Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["sal_vatAmount"]);
                    item.Total = Convert.ToDecimal(reader["sal_Total"]);
                    item.Quantity = Convert.ToDecimal(reader["sal_Quantity"]);
                    item.Invoiced = Convert.ToBoolean(reader["sal_Invoiced"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["sal_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["sal_Project"]);
                    item.CreateUserInfo = new CLiCore.CardsInfo.User().GetItem(xLan, item.CreateUser);
                    item.UpdateUserInfo = new CLiCore.CardsInfo.User().GetItem(xLan, item.LastupUser);
                }
                reader.Close();
            }
            return item;
        }
        public List<Contract> GetList(string DB, int Year)
        {
            DateTime First = new DateTime(Year, 1, 1);
            DateTime Last = new DateTime(Year, 12, 31);

            List<Contract> items = new List<Contract>();
            string selQuery = "select top 100 percent * from SalesDocument_Contract where [sal_InvoiceDate]>=@FirstDate and [sal_InvoiceDate]<=@LastDate order by [sal_InvoiceDate] desc,sal_InvoiceNo";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = First;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = Last;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Contract item = new Contract();
                    item.RecNo = Convert.ToInt32(reader["sal_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["sal_OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["sal_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["sal_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["sal_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["sal_LastupDate"]);
                    item.Status = Convert.ToInt32(reader["sal_Status"]);
                    item.Branch = Convert.ToInt32(reader["sal_Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["sal_Prefix"]);
                    item.InvoiceNo = Convert.ToInt32(reader["sal_InvoiceNo"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["sal_InvoiceDate"]);
                    item.ReferenceNo = Convert.ToString(reader["sal_ReferenceNo"]);
                    item.Client = iCore.IsDbNullRtNull(reader["sal_Client"]);
                    item.SalesPerson = iCore.IsDbNullRtNull(reader["sal_SalesPerson"]);
                    item.SalesHand = Convert.ToInt32(reader["sal_SalesHand"]);
                    item.Description = Convert.ToString(reader["sal_Description"]);
                    item.SubTotal = Convert.ToDecimal(reader["sal_SubTotal"]);
                    item.Discount = Convert.ToDecimal(reader["sal_Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["sal_vatAmount"]);
                    item.Total = Convert.ToDecimal(reader["sal_Total"]);
                    item.Quantity = Convert.ToDecimal(reader["sal_Quantity"]);
                    item.Invoiced = Convert.ToBoolean(reader["sal_Invoiced"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["sal_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["sal_Project"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
