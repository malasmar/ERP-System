using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiAccounting.Reports
{
    public class AccountStatment
    {
        private static readonly HashSet<string> _updatedDatabases = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private static readonly DateTime SqlMinDate = new DateTime(1753, 1, 1);

        public Guid? Key { get; set; }
        public Guid? OperationKey { get; set; }
        public Guid? AccountKey { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public DateTime? Date { get; set; }
        public int No { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal DebitBalance { get; set; }
        public decimal CreditBalance { get; set; }
        public int DocumentKind { get; set; }
        public int VoucherNo { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public int RowType { get; set; } // 0 = Opening Balance, 1 = Transactions

        private static void UpdateSqlFunction(string db)
        {
            try
            {
                using var con = new SqlConnection(iCore.GetCon(db));
                con.Open();
                const string ddlQuery = @"
CREATE OR ALTER FUNCTION [dbo].[fnaccReport_AccountStatment]
(
    @Key UNIQUEIDENTIFIER,
    @FirstDate DATE,
    @LastDate DATE,
    @Opening BIT = 0
)
RETURNS TABLE
AS
RETURN
(
    WITH CombinedRows AS (
        -- 1. Opening Balance Row (RowType = 0)
        SELECT 
            CAST('00000000-0000-0000-0000-000000000000' AS UNIQUEIDENTIFIER) as [Key],
            CAST(NULL as UNIQUEIDENTIFIER) as OperationKey,
            CAST(NULL as UNIQUEIDENTIFIER) as AccountKey,
            N'' as Code,
            N'' as Name1,
            N'' as Name2,
            @FirstDate as [Date],
            0 as [No],
            N'Opening Balance / رصيد افتتاحي' as [Description],
            0.00 as Debit,
            0.00 as Credit,
            NetAmount,
            0 as DocumentKind,
            0 as VoucherNo,
            0 as RowType,
            CAST(NULL as UNIQUEIDENTIFIER) as CostCenter,
            CAST(NULL as UNIQUEIDENTIFIER) as Project
        FROM (
            SELECT ISNULL(SUM(d.gl_Debit - d.gl_Credit), 0.00) as NetAmount
            FROM accCard_Accounts ac
            INNER JOIN accDocument_GeneralLedgerDetails d ON ac.acc_Key = d.gl_Account
            INNER JOIN accDocument_GeneralLedger h ON d.gl_OperationKey = h.gl_OperationKey
            WHERE ac.acc_Key = @Key
              AND h.gl_Date < @FirstDate
        ) t
        WHERE @Opening = 0

        UNION ALL

        -- 2. Transaction Rows (RowType = 1)
        SELECT 
            CAST(HASHBYTES('MD5', CAST(d.gl_OperationKey AS VARCHAR(36)) + CAST(d.gl_Index AS VARCHAR(10))) AS UNIQUEIDENTIFIER) as [Key],
            h.gl_OperationKey as OperationKey,
            ac.acc_Key as AccountKey,
            ac.acc_Code as Code,
            ac.acc_Name1 as Name1,
            ac.acc_Name2 as Name2,
            h.gl_Date as [Date],
            h.gl_No as [No],
            d.gl_Description as [Description],
            ISNULL(d.gl_Debit, 0.00) as Debit,
            ISNULL(d.gl_Credit, 0.00) as Credit,
            ISNULL(d.gl_Debit - d.gl_Credit, 0.00) as NetAmount,
            h.gl_DocumentKind as DocumentKind,
            h.gl_No as VoucherNo,
            1 as RowType,
            d.gl_CostCenter as CostCenter,
            d.gl_Project as Project
        FROM accCard_Accounts ac
        INNER JOIN accDocument_GeneralLedgerDetails d ON ac.acc_Key = d.gl_Account
        INNER JOIN accDocument_GeneralLedger h ON d.gl_OperationKey = h.gl_OperationKey
        WHERE ac.acc_Key = @Key 
          AND h.gl_Date >= @FirstDate 
          AND h.gl_Date < DATEADD(day, 1, @LastDate)
    ),
    RunningBalance AS (
        SELECT 
            [Key],
            OperationKey,
            AccountKey,
            Code,
            Name1,
            Name2,
            [Date],
            [No],
            [Description],
            Debit,
            Credit,
            SUM(NetAmount) OVER (
                ORDER BY RowType, [Date], [No], DocumentKind, VoucherNo, [Key]
                ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
            ) as CumulativeBalance,
            DocumentKind,
            VoucherNo,
            RowType,
            CostCenter,
            Project
        FROM CombinedRows
    )
    SELECT 
        [Key],
        OperationKey,
        AccountKey,
        Code,
        Name1,
        Name2,
        [Date],
        [No],
        [Description],
        Debit,
        Credit,
        CASE WHEN CumulativeBalance > 0 THEN CumulativeBalance ELSE 0.00 END as DebitBalance,
        CASE WHEN CumulativeBalance < 0 THEN ABS(CumulativeBalance) ELSE 0.00 END as CreditBalance,
        DocumentKind,
        VoucherNo,
        RowType,
        CostCenter,
        Project
    FROM RunningBalance
);
";
                using var cmd = new SqlCommand(ddlQuery, con);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                // Suppressed to prevent app crash if SQL permissions are restricted
            }
        }

        public List<AccountStatment> GetList(string db, Guid? key, DateTime firstDate, DateTime lastDate, bool opening)
        {
            var items = new List<AccountStatment>();

            if (key == null)
                return items;

            if (firstDate < SqlMinDate) firstDate = SqlMinDate;
            if (lastDate < SqlMinDate) lastDate = SqlMinDate;

            lock (_updatedDatabases)
            {
                if (!_updatedDatabases.Contains(db))
                {
                    UpdateSqlFunction(db);
                    _updatedDatabases.Add(db);
                }
            }

            const string selQuery = @"
        SELECT *
        FROM dbo.fnaccReport_AccountStatment(@Key,@FirstDate,@LastDate,@Opening)
        ORDER BY [RowType],[Date],[No],[DocumentKind],[VoucherNo]";

            using var con = new SqlConnection(iCore.GetCon(db));
            using var com = new SqlCommand(selQuery, con);

            com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = key;
            com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = firstDate;
            com.Parameters.Add("@LastDate", SqlDbType.Date).Value = lastDate;
            com.Parameters.Add("@Opening", SqlDbType.Bit).Value = opening;

            con.Open();

            using var reader = com.ExecuteReader();
            
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
            int colCostCenter = reader.GetOrdinal("CostCenter");
            int colProject = reader.GetOrdinal("Project");
            int colRowType = reader.GetOrdinal("RowType");

            while (reader.Read())
            {
                items.Add(new AccountStatment
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
                    CostCenter = reader.IsDBNull(colCostCenter) ? (Guid?)null : (Guid)reader.GetValue(colCostCenter),
                    Project = reader.IsDBNull(colProject) ? (Guid?)null : (Guid)reader.GetValue(colProject),
                    RowType = reader.IsDBNull(colRowType) ? 1 : Convert.ToInt32(reader.GetValue(colRowType))
                });
            }

            return items;
        }
    }
}
