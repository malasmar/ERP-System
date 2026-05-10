using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiCore.Configuration
{
    public class Settings
    {
        public Guid? Key { get; set; }
        public Guid? vatKey { get; set; }
        public Guid? CreditDiscount { get; set; }
        public Guid? DebitDiscount { get; set; }
        public Guid? CloseYear { get; set; }
        public Guid? ExpiryExpensess { get; set; }
        public Guid? ImportationAccount { get; set; }
        public Guid? PaymentCheck { get; set; }
        public Guid? CollectionCheck { get; set; }
        public Guid? UnderCollection { get; set; }
        public Guid? wPurchase { get; set; }
        public Guid? wSales { get; set; }
        public Guid? wCost { get; set; }
        public Guid? wOnRoad { get; set; }
        public Guid? wProduction { get; set; }
        public Guid? wFinishGood { get; set; }
        public Boolean wProductionDetailed { get; set; }
        public Boolean wFinishGoodDetailed { get; set; }
        public Boolean ClosePurchaseUnit { get; set; }
        public Boolean AutoReceiptShipping { get; set; }
        public Boolean ShowExpiry { get; set; }
        public Boolean ShowBatch { get; set; }
        public Guid? RetentionLess { get; set; }
        public Guid? PaymentDiscount { get; set; }
        public bool ShowMonthlyNo { get; set; }
        public Settings GetItem(string DB)
        {
            Settings item = new Settings();
            string selQuery = "select top(1) * from com_Settings";
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
                    item.Key = iCore.IsDbNullRtNull(reader["com_Key"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["com_vatKey"]);
                    item.CreditDiscount = iCore.IsDbNullRtNull(reader["com_CreditDiscount"]);
                    item.DebitDiscount = iCore.IsDbNullRtNull(reader["com_DebitDiscount"]);
                    item.CloseYear = iCore.IsDbNullRtNull(reader["com_CloseYear"]);
                    item.ExpiryExpensess = iCore.IsDbNullRtNull(reader["com_ExpiryExpensess"]);
                    item.ImportationAccount = iCore.IsDbNullRtNull(reader["com_ImportationAccount"]);
                    item.PaymentCheck = iCore.IsDbNullRtNull(reader["com_PaymentCheck"]);
                    item.CollectionCheck = iCore.IsDbNullRtNull(reader["com_CollectionCheck"]);
                    item.UnderCollection = iCore.IsDbNullRtNull(reader["com_UnderCollection"]);
                    item.wPurchase = iCore.IsDbNullRtNull(reader["com_wPurchase"]);
                    item.wSales = iCore.IsDbNullRtNull(reader["com_wSales"]);
                    item.wCost = iCore.IsDbNullRtNull(reader["com_wCost"]);
                    item.wOnRoad = iCore.IsDbNullRtNull(reader["com_wOnRoad"]);
                    item.wProduction = iCore.IsDbNullRtNull(reader["com_wProduction"]);
                    item.wFinishGood = iCore.IsDbNullRtNull(reader["com_wFinishGood"]);
                    item.wProductionDetailed = Convert.ToBoolean(reader["com_wProductionDetailed"]);
                    item.wFinishGoodDetailed = Convert.ToBoolean(reader["com_wFinishGoodDetailed"]);
                    item.ClosePurchaseUnit = Convert.ToBoolean(reader["com_ClosePurchaseUnit"]);
                    item.AutoReceiptShipping = Convert.ToBoolean(reader["com_AutoReceiptShipping"]);
                    item.ShowExpiry = Convert.ToBoolean(reader["com_ShowExpiry"]);
                    item.ShowBatch = Convert.ToBoolean(reader["com_ShowBatch"]);
                    item.RetentionLess = iCore.IsDbNullRtNull(reader["com_RetentionLess"]);
                    item.PaymentDiscount = iCore.IsDbNullRtNull(reader["com_PaymentDiscount"]);
                    item.ShowMonthlyNo = Convert.ToBoolean(reader["com_ShowMonthlyNo"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, Settings item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("delete from com_Settings ");
                str.Append("INSERT INTO com_Settings ");
                str.Append("([com_vatKey]");
                str.Append(",[com_CreditDiscount]");
                str.Append(",[com_DebitDiscount]");
                str.Append(",[com_CloseYear]");
                str.Append(",[com_ExpiryExpensess]");
                str.Append(",[com_ImportationAccount]");
                str.Append(",[com_PaymentCheck]");
                str.Append(",[com_CollectionCheck]");
                str.Append(",[com_UnderCollection]");
                str.Append(",[com_wPurchase]");
                str.Append(",[com_wSales]");
                str.Append(",[com_wCost]");
                str.Append(",[com_wOnRoad]");
                str.Append(",[com_wProduction]");
                str.Append(",[com_wFinishGood]");
                str.Append(",[com_wProductionDetailed]");
                str.Append(",[com_wFinishGoodDetailed]");
                str.Append(",[com_ClosePurchaseUnit]");
                str.Append(",[com_AutoReceiptShipping]");
                str.Append(",[com_ShowExpiry]");
                str.Append(",[com_ShowBatch]");
                str.Append(",[com_RetentionLess]");
                str.Append(",[com_PaymentDiscount]");
                str.Append(",[com_ShowMonthlyNo])");
                str.Append(" VALUES ");
                str.Append("(@com_vatKey");
                str.Append(",@com_CreditDiscount");
                str.Append(",@com_DebitDiscount");
                str.Append(",@com_CloseYear");
                str.Append(",@com_ExpiryExpensess");
                str.Append(",@com_ImportationAccount");
                str.Append(",@com_PaymentCheck");
                str.Append(",@com_CollectionCheck");
                str.Append(",@com_UnderCollection");
                str.Append(",@com_wPurchase");
                str.Append(",@com_wSales");
                str.Append(",@com_wCost");
                str.Append(",@com_wOnRoad");
                str.Append(",@com_wProduction");
                str.Append(",@com_wFinishGood");
                str.Append(",@com_wProductionDetailed");
                str.Append(",@com_wFinishGoodDetailed");
                str.Append(",@com_ClosePurchaseUnit");
                str.Append(",@com_AutoReceiptShipping");
                str.Append(",@com_ShowExpiry");
                str.Append(",@com_ShowBatch");
                str.Append(",@com_RetentionLess");
                str.Append(",@com_PaymentDiscount");
                str.Append(",@com_ShowMonthlyNo)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@com_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                comm.Parameters.Add("@com_CreditDiscount", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CreditDiscount);
                comm.Parameters.Add("@com_DebitDiscount", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.DebitDiscount);
                comm.Parameters.Add("@com_CloseYear", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CloseYear);
                comm.Parameters.Add("@com_ExpiryExpensess", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ExpiryExpensess);
                comm.Parameters.Add("@com_ImportationAccount", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ImportationAccount);
                comm.Parameters.Add("@com_PaymentCheck", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.PaymentCheck);
                comm.Parameters.Add("@com_CollectionCheck", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.CollectionCheck);
                comm.Parameters.Add("@com_UnderCollection", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.UnderCollection);
                comm.Parameters.Add("@com_wPurchase", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.wPurchase);
                comm.Parameters.Add("@com_wSales", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.wSales);
                comm.Parameters.Add("@com_wCost", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.wCost);
                comm.Parameters.Add("@com_wOnRoad", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.wOnRoad);
                comm.Parameters.Add("@com_wProduction", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.wProduction);
                comm.Parameters.Add("@com_wFinishGood", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.wFinishGood);
                comm.Parameters.Add("@com_wProductionDetailed", SqlDbType.Bit).Value = item.wProductionDetailed;
                comm.Parameters.Add("@com_wFinishGoodDetailed", SqlDbType.Bit).Value = item.wFinishGoodDetailed;
                comm.Parameters.Add("@com_ClosePurchaseUnit", SqlDbType.Bit).Value = item.ClosePurchaseUnit;
                comm.Parameters.Add("@com_AutoReceiptShipping", SqlDbType.Bit).Value = item.AutoReceiptShipping;
                comm.Parameters.Add("@com_ShowExpiry", SqlDbType.Bit).Value = item.ShowExpiry;
                comm.Parameters.Add("@com_ShowBatch", SqlDbType.Bit).Value = item.ShowBatch;
                comm.Parameters.Add("@com_RetentionLess", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.RetentionLess);
                comm.Parameters.Add("@com_PaymentDiscount", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.PaymentDiscount);
                comm.Parameters.Add("@com_ShowMonthlyNo", SqlDbType.Bit).Value = item.ShowMonthlyNo;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
    }
}
