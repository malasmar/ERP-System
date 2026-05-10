using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiFinancial.Reports.Project
{
    public class ProjectStatment
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public int VoucherNo { get; set; }
        public DateTime? VoucherDate { get; set; }
        public int DocumentKind { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public int AccountKind { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal DebitBalance { get; set; }
        public decimal CreditBalance { get; set; }
        public string Description { get; set; }
        public List<ProjectStatment> GetList(string DB,Guid? Key,DateTime? FirstDate,DateTime? LastDate)
        {
            decimal Balance = 0;
            List<ProjectStatment> items = new List<ProjectStatment>();
            string selQuery = "select top 100 percent * from dbo.ReportFin_ProjectStatment(@Key,@FirstDate,@LastDate) order by [VoucherDate],[VoucherNo],[DocumentKind] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(FirstDate);
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(LastDate);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ProjectStatment item = new ProjectStatment();
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.VoucherNo = Convert.ToInt32(reader["VoucherNo"]);
                    item.VoucherDate = iCore.IsDbNullRtNullDate(reader["VoucherDate"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.AccountKind = Convert.ToInt32(reader["AccountKind"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
              
                    item.Description = Convert.ToString(reader["Description"]);
                    Balance += item.Debit - item.Credit;
                    if (Balance > 0)
                    {
                        item.DebitBalance = Balance;
                    }
                    else
                    {
                        item.CreditBalance = Math.Abs(Balance);
                    }
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
