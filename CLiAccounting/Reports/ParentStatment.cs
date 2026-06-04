using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiAccounting.Reports
{
    public class ParentStatment
    {
        private static readonly DateTime SqlMinDate = new DateTime(1753, 1, 1);

        public Guid? Key { get; set; }
        public Guid? OperationKey { get; set; }
        public Guid? AccountKey { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public int No { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal DebitBalance { get; set; }
        public decimal CreditBalance { get; set; }
        public int DocumentKind { get; set; }
        public int VoucherNo { get; set; }
        public int RowType { get; set; }

        // =========================
        // CORE QUERY METHOD
        // =========================
        private List<ParentStatment> Load(string db, string key, DateTime firstDate, DateTime lastDate, bool opening)
        {
            var items = new List<ParentStatment>();

            if (firstDate < SqlMinDate) firstDate = SqlMinDate;
            if (lastDate < SqlMinDate) lastDate = SqlMinDate;

            const string sql = @"
SELECT *
FROM dbo.fnaccReport_ParentStatment(@Key, @FirstDate, @LastDate, @Opening)
ORDER BY RowType, [Date], [No], DocumentKind, VoucherNo;
";

            using var con = new SqlConnection(iCore.GetCon(db));
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.Add("@Key", SqlDbType.NVarChar, 50).Value = (object)key ?? DBNull.Value;
            cmd.Parameters.Add("@FirstDate", SqlDbType.Date).Value = firstDate;
            cmd.Parameters.Add("@LastDate", SqlDbType.Date).Value = lastDate;
            cmd.Parameters.Add("@Opening", SqlDbType.Bit).Value = opening;

            con.Open();

            using var reader = cmd.ExecuteReader();
            
            // Optimization: Fetch ordinals outside the loop to improve performance
            int colKey = reader.GetOrdinal("Key");
            int colOperationKey = reader.GetOrdinal("OperationKey");
            int colAccountKey = reader.GetOrdinal("AccountKey");
            int colCode = reader.GetOrdinal("Code");
            int colName1 = reader.GetOrdinal("Name1");
            int colName2 = reader.GetOrdinal("Name2");
            int colDate = reader.GetOrdinal("Date");
            int colNo = reader.GetOrdinal("No");
            int colDescription = reader.GetOrdinal("Description");
            int colDebit = reader.GetOrdinal("Debit");
            int colCredit = reader.GetOrdinal("Credit");
            int colDebitBalance = reader.GetOrdinal("DebitBalance");
            int colCreditBalance = reader.GetOrdinal("CreditBalance");
            int colDocumentKind = reader.GetOrdinal("DocumentKind");
            int colVoucherNo = reader.GetOrdinal("VoucherNo");
            int colRowType = reader.GetOrdinal("RowType");

            while (reader.Read())
            {
                items.Add(new ParentStatment
                {
                    Key = reader.IsDBNull(colKey) ? (Guid?)null : (Guid)reader.GetValue(colKey),
                    OperationKey = reader.IsDBNull(colOperationKey) ? (Guid?)null : (Guid)reader.GetValue(colOperationKey),
                    AccountKey = reader.IsDBNull(colAccountKey) ? (Guid?)null : (Guid)reader.GetValue(colAccountKey),
                    Code = reader.IsDBNull(colCode) ? null : Convert.ToString(reader.GetValue(colCode)),
                    Name1 = reader.IsDBNull(colName1) ? null : Convert.ToString(reader.GetValue(colName1)),
                    Name2 = reader.IsDBNull(colName2) ? null : Convert.ToString(reader.GetValue(colName2)),
                    Date = reader.IsDBNull(colDate) ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(colDate)),
                    No = reader.IsDBNull(colNo) ? 0 : Convert.ToInt32(reader.GetValue(colNo)),
                    Description = reader.IsDBNull(colDescription) ? null : Convert.ToString(reader.GetValue(colDescription)),
                    Debit = reader.IsDBNull(colDebit) ? 0m : Convert.ToDecimal(reader.GetValue(colDebit)),
                    Credit = reader.IsDBNull(colCredit) ? 0m : Convert.ToDecimal(reader.GetValue(colCredit)),
                    DebitBalance = reader.IsDBNull(colDebitBalance) ? 0m : Convert.ToDecimal(reader.GetValue(colDebitBalance)),
                    CreditBalance = reader.IsDBNull(colCreditBalance) ? 0m : Convert.ToDecimal(reader.GetValue(colCreditBalance)),
                    DocumentKind = reader.IsDBNull(colDocumentKind) ? 0 : Convert.ToInt32(reader.GetValue(colDocumentKind)),
                    VoucherNo = reader.IsDBNull(colVoucherNo) ? 0 : Convert.ToInt32(reader.GetValue(colVoucherNo)),
                    RowType = reader.IsDBNull(colRowType) ? 1 : Convert.ToInt32(reader.GetValue(colRowType))
                });
            }

            return items;
        }

        // =========================
        // PUBLIC METHODS
        // =========================
        public List<ParentStatment> GetList(string db, string key, DateTime firstDate, DateTime lastDate, bool opening = false)
        {
            if (string.IsNullOrWhiteSpace(key))
                return new List<ParentStatment>();

            return Load(db, key, firstDate, lastDate, opening);
        }

        public List<ParentStatment> YearlyStatment(string db, DateTime firstDate, DateTime lastDate, bool opening = false)
        {
            return Load(db, string.Empty, firstDate, lastDate, opening);
        }
    }
}