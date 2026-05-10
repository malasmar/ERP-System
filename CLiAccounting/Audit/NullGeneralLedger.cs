using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiAccounting.Audit
{
    public class NullGeneralLedger
    {
        public Guid? OperationKey { get; set; }
        public int Kind { get; set; }
        public int No { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public int DocumentKind { get; set; }
        public int VoucherNo { get; set; }
        public int NullRows { get; set; }
        public List<NullGeneralLedger> GetList(string DB)
        {
            List<NullGeneralLedger> items = new List<NullGeneralLedger>();
            string selQuery = "select top 100 percent * from dbo.fnaccAudit_NullGeneralLedger() order by [Date] ";
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
                    NullGeneralLedger item = new NullGeneralLedger();
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.No = Convert.ToInt32(reader["No"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.VoucherNo = Convert.ToInt32(reader["VoucherNo"]);
                    item.NullRows = Convert.ToInt32(reader["NullRows"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

    }
}
