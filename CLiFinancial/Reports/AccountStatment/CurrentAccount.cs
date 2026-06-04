using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Reports.AccountStatment
{
    public class CurrentAccount
    {
        private static readonly HashSet<string> _updatedDatabases = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public int DocumentKind { get; set; }
        public int VoucherNo { get; set; }
        public int MonthlyNo { get; set; }
        public DateTime? VoucherDate { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public int CounterKind { get; set; }
        public Guid? CounterKey { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal DebitBalance { get; set; }
        public decimal CreditBalance { get; set; }
        public Guid? vatKey { get; set; }
        public decimal vatRate { get; set; }
        public int T { get; set; }
        public int AccountType { get; set; }
        public string strMonthlyNo { get; set; }

        private static void UpdateSqlFunctions(string DB)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                string ddlQuery1 = @"
CREATE OR ALTER FUNCTION [dbo].[ReportFin_AccountStatment]
(
    @Key UNIQUEIDENTIFIER,
    @FirstDate DATE,
    @LastDate DATE,
    @Opening BIT
)
RETURNS TABLE
AS
RETURN
(
    WITH CombinedRows AS (
        -- 1. Opening Balance Row (T = 0)
        SELECT 
            0 as RecNo,
            CAST('00000000-0000-0000-0000-000000000000' AS UNIQUEIDENTIFIER) as OperationKey,
            0 as DocumentKind,
            0 as VoucherNo,
            0 as MonthlyNo,
            @FirstDate as VoucherDate,
            N'' as ReferenceNo,
            CAST(NULL as DATE) as ReferenceDate,
            0 as CounterKind,
            CAST(NULL as UNIQUEIDENTIFIER) as CounterKey,
            CAST(NULL as UNIQUEIDENTIFIER) as CostCenter,
            CAST(NULL as UNIQUEIDENTIFIER) as Project,
            N'Opening Balance / رصيد افتتاحي' as [Description],
            0.00 as Debit,
            0.00 as Credit,
            ISNULL(SUM(d.gl_Debit - d.gl_Credit), 0.00) as NetAmount,
            CAST(NULL as UNIQUEIDENTIFIER) as vatKey,
            0.00 as vatRate,
            0 as T
        FROM accCard_Accounts ac
        LEFT JOIN accDocument_GeneralLedgerDetails d ON ac.acc_Key = d.gl_Account
        LEFT JOIN accDocument_GeneralLedger h ON d.gl_OperationKey = h.gl_OperationKey AND h.gl_Date < @FirstDate
        WHERE ac.acc_Key = @Key AND @Opening = 0
        GROUP BY ac.acc_Key

        UNION ALL

        -- 2. Transaction Rows (T = 1)
        SELECT 
            ROW_NUMBER() OVER (ORDER BY h.gl_Date, h.gl_No, h.gl_DocumentKind, d.gl_Index) as RecNo,
            h.gl_OperationKey as OperationKey,
            h.gl_DocumentKind as DocumentKind,
            h.gl_No as VoucherNo,
            0 as MonthlyNo,
            h.gl_Date as VoucherDate,
            N'' as ReferenceNo,
            CAST(NULL as DATE) as ReferenceDate,
            0 as CounterKind,
            CAST(NULL as UNIQUEIDENTIFIER) as CounterKey,
            d.gl_CostCenter as CostCenter,
            d.gl_Project as Project,
            d.gl_Description as [Description],
            ISNULL(d.gl_Debit, 0.00) as Debit,
            ISNULL(d.gl_Credit, 0.00) as Credit,
            ISNULL(d.gl_Debit - d.gl_Credit, 0.00) as NetAmount,
            CAST(NULL as UNIQUEIDENTIFIER) as vatKey,
            0.00 as vatRate,
            1 as T
        FROM accCard_Accounts ac
        INNER JOIN accDocument_GeneralLedgerDetails d ON ac.acc_Key = d.gl_Account
        INNER JOIN accDocument_GeneralLedger h ON d.gl_OperationKey = h.gl_OperationKey
        WHERE ac.acc_Key = @Key 
          AND h.gl_Date >= @FirstDate 
          AND h.gl_Date <= @LastDate
    ),
    RunningBalance AS (
        SELECT 
            RecNo,
            OperationKey,
            DocumentKind,
            VoucherNo,
            MonthlyNo,
            VoucherDate,
            ReferenceNo,
            ReferenceDate,
            CounterKind,
            CounterKey,
            CostCenter,
            Project,
            [Description],
            Debit,
            Credit,
            SUM(NetAmount) OVER (
                ORDER BY T, VoucherDate, VoucherNo, DocumentKind, RecNo, OperationKey
                ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
            ) as CumulativeBalance,
            vatKey,
            vatRate,
            T
        FROM CombinedRows
    )
    SELECT 
        RecNo,
        OperationKey,
        DocumentKind,
        VoucherNo,
        MonthlyNo,
        VoucherDate,
        ReferenceNo,
        ReferenceDate,
        CounterKind,
        CounterKey,
        CostCenter,
        Project,
        [Description],
        Debit,
        Credit,
        CASE WHEN CumulativeBalance > 0 THEN CumulativeBalance ELSE 0.00 END as DebitBalance,
        CASE WHEN CumulativeBalance < 0 THEN ABS(CumulativeBalance) ELSE 0.00 END as CreditBalance,
        vatKey,
        vatRate,
        T
    FROM RunningBalance
);
";
                string ddlQuery2 = @"
CREATE OR ALTER FUNCTION [dbo].[ReportFin_EmployeeStatment]
(
    @Key UNIQUEIDENTIFIER,
    @FirstDate DATE,
    @LastDate DATE,
    @Opening BIT,
    @Kind NVARCHAR(25)
)
RETURNS TABLE
AS
RETURN
(
    WITH CombinedRows AS (
        -- 1. Opening Balance Row (T = 0)
        SELECT 
            0 as RecNo,
            CAST('00000000-0000-0000-0000-000000000000' AS UNIQUEIDENTIFIER) as OperationKey,
            0 as DocumentKind,
            0 as VoucherNo,
            0 as MonthlyNo,
            @FirstDate as VoucherDate,
            N'' as ReferenceNo,
            CAST(NULL as DATE) as ReferenceDate,
            0 as CounterKind,
            CAST(NULL as UNIQUEIDENTIFIER) as CounterKey,
            CAST(NULL as UNIQUEIDENTIFIER) as CostCenter,
            CAST(NULL as UNIQUEIDENTIFIER) as Project,
            N'Opening Balance / رصيد افتتاحي' as [Description],
            0.00 as Debit,
            0.00 as Credit,
            ISNULL(SUM(d.gl_Debit - d.gl_Credit), 0.00) as NetAmount,
            CAST(NULL as UNIQUEIDENTIFIER) as vatKey,
            0.00 as vatRate,
            0 as T,
            0 as AccountType
        FROM accCard_Accounts ac
        LEFT JOIN accDocument_GeneralLedgerDetails d ON ac.acc_Key = d.gl_Account
        LEFT JOIN accDocument_GeneralLedger h ON d.gl_OperationKey = h.gl_OperationKey AND h.gl_Date < @FirstDate
        WHERE ac.acc_Key = @Key AND @Opening = 0
        GROUP BY ac.acc_Key

        UNION ALL

        -- 2. Transaction Rows (T = 1)
        SELECT 
            ROW_NUMBER() OVER (ORDER BY h.gl_Date, h.gl_No, h.gl_DocumentKind, d.gl_Index) as RecNo,
            h.gl_OperationKey as OperationKey,
            h.gl_DocumentKind as DocumentKind,
            h.gl_No as VoucherNo,
            0 as MonthlyNo,
            h.gl_Date as VoucherDate,
            N'' as ReferenceNo,
            CAST(NULL as DATE) as ReferenceDate,
            0 as CounterKind,
            CAST(NULL as UNIQUEIDENTIFIER) as CounterKey,
            d.gl_CostCenter as CostCenter,
            d.gl_Project as Project,
            d.gl_Description as [Description],
            ISNULL(d.gl_Debit, 0.00) as Debit,
            ISNULL(d.gl_Credit, 0.00) as Credit,
            ISNULL(d.gl_Debit - d.gl_Credit, 0.00) as NetAmount,
            CAST(NULL as UNIQUEIDENTIFIER) as vatKey,
            0.00 as vatRate,
            1 as T,
            0 as AccountType
        FROM accCard_Accounts ac
        INNER JOIN accDocument_GeneralLedgerDetails d ON ac.acc_Key = d.gl_Account
        INNER JOIN accDocument_GeneralLedger h ON d.gl_OperationKey = h.gl_OperationKey
        WHERE ac.acc_Key = @Key 
          AND h.gl_Date >= @FirstDate 
          AND h.gl_Date <= @LastDate
    ),
    RunningBalance AS (
        SELECT 
            RecNo,
            OperationKey,
            DocumentKind,
            VoucherNo,
            MonthlyNo,
            VoucherDate,
            ReferenceNo,
            ReferenceDate,
            CounterKind,
            CounterKey,
            CostCenter,
            Project,
            [Description],
            Debit,
            Credit,
            SUM(NetAmount) OVER (
                ORDER BY T, VoucherDate, VoucherNo, DocumentKind, RecNo, OperationKey
                ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
            ) as CumulativeBalance,
            vatKey,
            vatRate,
            T,
            AccountType
        FROM CombinedRows
    )
    SELECT 
        RecNo,
        OperationKey,
        DocumentKind,
        VoucherNo,
        MonthlyNo,
        VoucherDate,
        ReferenceNo,
        ReferenceDate,
        CounterKind,
        CounterKey,
        CostCenter,
        Project,
        [Description],
        Debit,
        Credit,
        CASE WHEN CumulativeBalance > 0 THEN CumulativeBalance ELSE 0.00 END as DebitBalance,
        CASE WHEN CumulativeBalance < 0 THEN ABS(CumulativeBalance) ELSE 0.00 END as CreditBalance,
        vatKey,
        vatRate,
        T,
        AccountType
    FROM RunningBalance
);
";
                using (SqlCommand cmd1 = new SqlCommand(ddlQuery1, con))
                {
                    cmd1.ExecuteNonQuery();
                }
                using (SqlCommand cmd2 = new SqlCommand(ddlQuery2, con))
                {
                    cmd2.ExecuteNonQuery();
                }
            }
        }

        public List<CurrentAccount> GetList(string DB, Guid? Key, DateTime? FirstDate, DateTime? LastDate, bool Opening)
        {
            List<CurrentAccount> items = new List<CurrentAccount>();
            if (Key == null)
                return items;

            DateTime queryFirst = FirstDate ?? new DateTime(1753, 1, 1);
            DateTime queryLast = LastDate ?? new DateTime(1753, 1, 1);
            if (queryFirst < new DateTime(1753, 1, 1)) queryFirst = new DateTime(1753, 1, 1);
            if (queryLast < new DateTime(1753, 1, 1)) queryLast = new DateTime(1753, 1, 1);

            lock (_updatedDatabases)
            {
                if (!_updatedDatabases.Contains(DB))
                {
                    try
                    {
                        UpdateSqlFunctions(DB);
                        _updatedDatabases.Add(DB);
                    }
                    catch (Exception)
                    {
                        // Fail silently to prevent app crash if SQL permissions are restricted
                    }
                }
            }

            string selQuery = "select * from dbo.ReportFin_AccountStatment(@Key,@FirstDate,@LastDate,@Opening) order by [T],[VoucherDate],[VoucherNo],[DocumentKind],[RecNo]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand(selQuery, con))
                {
                    com.CommandType = CommandType.Text;
                    com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = queryFirst;
                    com.Parameters.Add("@LastDate", SqlDbType.Date).Value = queryLast;
                    com.Parameters.Add("@Opening", SqlDbType.Bit).Value = Opening;

                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CurrentAccount item = new CurrentAccount();
                            item.RecNo = reader["RecNo"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RecNo"]);
                            item.OperationKey = reader["OperationKey"] == DBNull.Value ? (Guid?)null : (Guid)reader["OperationKey"];
                            item.DocumentKind = reader["DocumentKind"] == DBNull.Value ? 0 : Convert.ToInt32(reader["DocumentKind"]);
                            item.VoucherNo = reader["VoucherNo"] == DBNull.Value ? 0 : Convert.ToInt32(reader["VoucherNo"]);
                            item.MonthlyNo = reader["MonthlyNo"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MonthlyNo"]);
                            item.VoucherDate = reader["VoucherDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["VoucherDate"]);
                            item.ReferenceNo = reader["ReferenceNo"] == DBNull.Value ? "" : Convert.ToString(reader["ReferenceNo"]);
                            item.ReferenceDate = reader["ReferenceDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ReferenceDate"]);
                            item.CounterKind = reader["CounterKind"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CounterKind"]);
                            item.CounterKey = reader["CounterKey"] == DBNull.Value ? (Guid?)null : (Guid)reader["CounterKey"];
                            item.CostCenter = reader["CostCenter"] == DBNull.Value ? (Guid?)null : (Guid)reader["CostCenter"];
                            item.Project = reader["Project"] == DBNull.Value ? (Guid?)null : (Guid)reader["Project"];
                            item.Description = reader["Description"] == DBNull.Value ? "" : Convert.ToString(reader["Description"]);
                            item.Debit = reader["Debit"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Debit"]);
                            item.Credit = reader["Credit"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Credit"]);
                            item.DebitBalance = reader["DebitBalance"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["DebitBalance"]);
                            item.CreditBalance = reader["CreditBalance"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["CreditBalance"]);
                            item.vatKey = reader["vatKey"] == DBNull.Value ? (Guid?)null : (Guid)reader["vatKey"];
                            item.vatRate = reader["vatRate"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["vatRate"]);
                            item.T = reader["T"] == DBNull.Value ? 1 : Convert.ToInt32(reader["T"]);

                            if (item.VoucherDate.HasValue)
                            {
                                item.strMonthlyNo = item.VoucherDate.Value.ToString("MM") + "-" + item.MonthlyNo.ToString("0000");
                            }
                            else
                            {
                                item.strMonthlyNo = "";
                            }
                            
                            items.Add(item);
                        }
                    }
                }
            }
            return items;
        }

        public List<CurrentAccount> GetList(string DB, Guid? Key, DateTime? FirstDate, DateTime? LastDate, bool Opening, string Kind)
        {
            List<CurrentAccount> items = new List<CurrentAccount>();
            if (Key == null)
                return items;

            DateTime queryFirst = FirstDate ?? new DateTime(1753, 1, 1);
            DateTime queryLast = LastDate ?? new DateTime(1753, 1, 1);
            if (queryFirst < new DateTime(1753, 1, 1)) queryFirst = new DateTime(1753, 1, 1);
            if (queryLast < new DateTime(1753, 1, 1)) queryLast = new DateTime(1753, 1, 1);

            lock (_updatedDatabases)
            {
                if (!_updatedDatabases.Contains(DB))
                {
                    try
                    {
                        UpdateSqlFunctions(DB);
                        _updatedDatabases.Add(DB);
                    }
                    catch (Exception)
                    {
                        // Fail silently to prevent app crash if SQL permissions are restricted
                    }
                }
            }

            string selQuery = "select * from dbo.ReportFin_EmployeeStatment(@Key,@FirstDate,@LastDate,@Opening,@Kind) order by [T],[VoucherDate],[VoucherNo],[DocumentKind],[RecNo]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand(selQuery, con))
                {
                    com.CommandType = CommandType.Text;
                    com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                    com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = queryFirst;
                    com.Parameters.Add("@LastDate", SqlDbType.Date).Value = queryLast;
                    com.Parameters.Add("@Opening", SqlDbType.Bit).Value = Opening;
                    com.Parameters.Add("@Kind", SqlDbType.NVarChar, 25).Value = Kind ?? "";

                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CurrentAccount item = new CurrentAccount();
                            item.RecNo = reader["RecNo"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RecNo"]);
                            item.OperationKey = reader["OperationKey"] == DBNull.Value ? (Guid?)null : (Guid)reader["OperationKey"];
                            item.DocumentKind = reader["DocumentKind"] == DBNull.Value ? 0 : Convert.ToInt32(reader["DocumentKind"]);
                            item.VoucherNo = reader["VoucherNo"] == DBNull.Value ? 0 : Convert.ToInt32(reader["VoucherNo"]);
                            item.MonthlyNo = reader["MonthlyNo"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MonthlyNo"]);
                            item.VoucherDate = reader["VoucherDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["VoucherDate"]);
                            item.ReferenceNo = reader["ReferenceNo"] == DBNull.Value ? "" : Convert.ToString(reader["ReferenceNo"]);
                            item.ReferenceDate = reader["ReferenceDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ReferenceDate"]);
                            item.CounterKind = reader["CounterKind"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CounterKind"]);
                            item.CounterKey = reader["CounterKey"] == DBNull.Value ? (Guid?)null : (Guid)reader["CounterKey"];
                            item.CostCenter = reader["CostCenter"] == DBNull.Value ? (Guid?)null : (Guid)reader["CostCenter"];
                            item.Project = reader["Project"] == DBNull.Value ? (Guid?)null : (Guid)reader["Project"];
                            item.Description = reader["Description"] == DBNull.Value ? "" : Convert.ToString(reader["Description"]);
                            item.Debit = reader["Debit"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Debit"]);
                            item.Credit = reader["Credit"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Credit"]);
                            item.DebitBalance = reader["DebitBalance"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["DebitBalance"]);
                            item.CreditBalance = reader["CreditBalance"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["CreditBalance"]);
                            item.vatKey = reader["vatKey"] == DBNull.Value ? (Guid?)null : (Guid)reader["vatKey"];
                            item.vatRate = reader["vatRate"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["vatRate"]);
                            item.T = reader["T"] == DBNull.Value ? 1 : Convert.ToInt32(reader["T"]);
                            item.AccountType = reader["AccountType"] == DBNull.Value ? 0 : Convert.ToInt32(reader["AccountType"]);

                            if (item.VoucherDate.HasValue)
                            {
                                item.strMonthlyNo = item.VoucherDate.Value.ToString("MM") + "-" + item.MonthlyNo.ToString("0000");
                            }
                            else
                            {
                                item.strMonthlyNo = "";
                            }

                            items.Add(item);
                        }
                    }
                }
            }
            return items;
        }
    }
}
