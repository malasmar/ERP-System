using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore.Const
{
    public class Tables
    {

        public static string FinDocument_TransactionDetails
        {
            get
            {
                return GetFinancialDetails();
            }
        }
        public static string FinDocument_Transaction
        {
            get
            {
                return GetFinDocument_Transaction();
            }
        }
        public static string InvDocument_Transaction
        {
            get
            {
                return GetInvDocument_Transaction();
            }
        }
        public static string InvDocument_TransactionDetails
        {
            get
            {
                return GetInvDocument_TransactionDetails();
            }
        }
        static string GetFinancialDetails()
        {
            StringBuilder str = new StringBuilder();
            str.Clear();
            str.Append("INSERT INTO finDocument_TransactionDetails");
            str.Append("([Fin_Key]");
            str.Append(",[Fin_OperationKey]");
            str.Append(",[Fin_Index]");
            str.Append(",[Fin_Status]");
            str.Append(",[Fin_ReferenceNo]");
            str.Append(",[Fin_ReferenceDate]");
            str.Append(",[Fin_TransactionNo]");
            str.Append(",[Fin_Account1Kind]");
            str.Append(",[Fin_Account1Type]");
            str.Append(",[Fin_Account1Key]");
            str.Append(",[Fin_Account2Kind]");
            str.Append(",[Fin_Account2Type]");
            str.Append(",[Fin_Account2Key]");
            str.Append(",[Fin_CostCenter]");
            str.Append(",[Fin_Project]");
            str.Append(",[Fin_Client]");
            str.Append(",[Fin_Person]");
            str.Append(",[Fin_Description]");
            str.Append(",[Fin_DC]");
            str.Append(",[Fin_Amount]");
            str.Append(",[Fin_vatKey]");
            str.Append(",[Fin_vatRate]");
            str.Append(",[Fin_vatAmount]");
            str.Append(",[Fin_Total]");
            str.Append(",[Fin_cxAmount]");
            str.Append(",[Fin_cxvatAmount]");
            str.Append(",[Fin_cxTotal]");
            str.Append(",[Fin_IsVAT]");
            str.Append(",[Fin_Hidden]");
            str.Append(",[Fin_vatRegNo]");
            str.Append(",[Fin_vatCurrent]");
            str.Append(",[Fin_Importation]");
            str.Append(",[Fin_ExpensesKind]");
            str.Append(",[Fin_PaymentKind]");
            str.Append(",[Fin_IsSalesCost])");
            str.Append(" VALUES ");
            str.Append("(@Fin_Key");
            str.Append(",@Fin_OperationKey");
            str.Append(",@Fin_Index");
            str.Append(",@Fin_Status");
            str.Append(",@Fin_ReferenceNo");
            str.Append(",@Fin_ReferenceDate");
            str.Append(",@Fin_TransactionNo");
            str.Append(",@Fin_Account1Kind");
            str.Append(",@Fin_Account1Type");
            str.Append(",@Fin_Account1Key");
            str.Append(",@Fin_Account2Kind");
            str.Append(",@Fin_Account2Type");
            str.Append(",@Fin_Account2Key");
            str.Append(",@Fin_CostCenter");
            str.Append(",@Fin_Project");
            str.Append(",@Fin_Client");
            str.Append(",@Fin_Person");
            str.Append(",@Fin_Description");
            str.Append(",@Fin_DC");
            str.Append(",@Fin_Amount");
            str.Append(",@Fin_vatKey");
            str.Append(",@Fin_vatRate");
            str.Append(",@Fin_vatAmount");
            str.Append(",@Fin_Total");
            str.Append(",@Fin_cxAmount");
            str.Append(",@Fin_cxvatAmount");
            str.Append(",@Fin_cxTotal");
            str.Append(",@Fin_IsVAT");
            str.Append(",@Fin_Hidden");
            str.Append(",@Fin_vatRegNo");
            str.Append(",@Fin_vatCurrent");
            str.Append(",@Fin_Importation");
            str.Append(",@Fin_ExpensesKind");
            str.Append(",@Fin_PaymentKind");
            str.Append(",@Fin_IsSalesCost)");
            return str.ToString();
        }
        static string GetFinDocument_Transaction()
        {
            StringBuilder str = new StringBuilder();
            str.Clear();
            str.Append("INSERT INTO finDocument_Transaction");
            str.Append("([Fin_OperationKey]");
            str.Append(",[Fin_CreateUser]");
            str.Append(",[Fin_CreateDate]");
            str.Append(",[Fin_LastupUser]");
            str.Append(",[Fin_LastupDate]");
            str.Append(",[Fin_Status]");
            str.Append(",[Fin_Branch]");
            str.Append(",[Fin_Prefix]");
            str.Append(",[Fin_Category]");
            str.Append(",[Fin_DocumentKind]");
            str.Append(",[Fin_VoucherNo]");
            str.Append(",[Fin_VoucherDate]");
            str.Append(",[Fin_MonthlyNo]");
            str.Append(",[Fin_ReferenceNo]");
            str.Append(",[Fin_ReferenceDate]");
            str.Append(",[Fin_DueDate]");
            str.Append(",[Fin_AccountKind]");
            str.Append(",[Fin_AccountKey]");
            str.Append(",[Fin_Description]");
            str.Append(",[Fin_TransactionNo]");
            str.Append(",[Fin_Currency]");
            str.Append(",[Fin_Subtotal]");
            str.Append(",[Fin_vatAmount]");
            str.Append(",[Fin_Total]");
            str.Append(",[Fin_Rows]");
            str.Append(",[Fin_IcloudExp]");
            str.Append(",[Fin_CostCenter]");
            str.Append(",[Fin_Project]");
            str.Append(",[Fin_RecipientName])");
            str.Append(" VALUES ");
            str.Append("(@Fin_OperationKey");
            str.Append(",@Fin_CreateUser");
            str.Append(",@Fin_CreateDate");
            str.Append(",@Fin_LastupUser");
            str.Append(",@Fin_LastupDate");
            str.Append(",@Fin_Status");
            str.Append(",@Fin_Branch");
            str.Append(",@Fin_Prefix");
            str.Append(",@Fin_Category");
            str.Append(",@Fin_DocumentKind");
            str.Append(",@Fin_VoucherNo");
            str.Append(",@Fin_VoucherDate");
            str.Append(",@Fin_MonthlyNo");
            str.Append(",@Fin_ReferenceNo");
            str.Append(",@Fin_ReferenceDate");
            str.Append(",@Fin_DueDate");
            str.Append(",@Fin_AccountKind");
            str.Append(",@Fin_AccountKey");
            str.Append(",@Fin_Description");
            str.Append(",@Fin_TransactionNo");
            str.Append(",@Fin_Currency");
            str.Append(",@Fin_Subtotal");
            str.Append(",@Fin_vatAmount");
            str.Append(",@Fin_Total");
            str.Append(",@Fin_Rows");
            str.Append(",@Fin_IcloudExp");
            str.Append(",@Fin_CostCenter");
            str.Append(",@Fin_Project");
            str.Append(",@Fin_RecipientName)");
            return str.ToString();
        }
        static string GetInvDocument_Transaction()
        {
            StringBuilder str = new StringBuilder();
            str.Clear();
            str.Append("INSERT INTO InvDocument_Transaction ");
            str.Append("([inv_OperationKey]");
            str.Append(",[inv_Session]");
            str.Append(",[inv_CreateUser]");
            str.Append(",[inv_CreateDate]");
            str.Append(",[inv_LastupUser]");
            str.Append(",[inv_LastupDate]");
            str.Append(",[inv_Status]");
            str.Append(",[inv_Branch]");
            str.Append(",[inv_Prefix]");
            str.Append(",[inv_SourceWarehouse]");
            str.Append(",[inv_TargetWarehouse]");
            str.Append(",[inv_DocumentKind]");
            str.Append(",[inv_InvoiceKind]");
            str.Append(",[inv_InvoiceNo]");
            str.Append(",[inv_InvoiceDate]");
            str.Append(",[inv_InvoiceDatetime]");
            str.Append(",[inv_MonthlyNo]");
            str.Append(",[inv_ReferenceNo]");
            str.Append(",[inv_ReferenceDate]");
            str.Append(",[inv_DueDate]");
            str.Append(",[inv_AccountKind]");
            str.Append(",[inv_AccountKey]");
            str.Append(",[inv_CurrentKey]");
            str.Append(",[inv_SalesPerson]");
            str.Append(",[inv_SalesHand]");
            str.Append(",[inv_Description]");
            str.Append(",[inv_Currency]");
            str.Append(",[inv_SubTotal]");
            str.Append(",[inv_Discount]");
            str.Append(",[inv_vatAmount]");
            str.Append(",[inv_BonusAmount]");
            str.Append(",[inv_Total]");
            str.Append(",[inv_PaymentDiscount]");
            str.Append(",[inv_RetentionLess]");
            str.Append(",[inv_InvoiceCost]");
            str.Append(",[inv_Quantity]");
            str.Append(",[inv_BonusQuantity]");
            str.Append(",[inv_DeliveryKind]");
            str.Append(",[inv_IncludeFxd]");
            str.Append(",[inv_IncludeExp]");
            str.Append(",[inv_ImportationKey]");
            str.Append(",[inv_Returned]");
            str.Append(",[inv_OriginalInvoice]");
            str.Append(",[inv_Source]");
            str.Append(",[inv_IsCredit]");
            str.Append(",[inv_CostCenter]");
            str.Append(",[inv_Project])");
            str.Append(" VALUES ");
            str.Append("(@inv_OperationKey");
            str.Append(",@inv_Session");
            str.Append(",@inv_CreateUser");
            str.Append(",@inv_CreateDate");
            str.Append(",@inv_LastupUser");
            str.Append(",@inv_LastupDate");
            str.Append(",@inv_Status");
            str.Append(",@inv_Branch");
            str.Append(",@inv_Prefix");
            str.Append(",@inv_SourceWarehouse");
            str.Append(",@inv_TargetWarehouse");
            str.Append(",@inv_DocumentKind");
            str.Append(",@inv_InvoiceKind");
            str.Append(",@inv_InvoiceNo");
            str.Append(",@inv_InvoiceDate");
            str.Append(",@inv_InvoiceDatetime");
            str.Append(",@inv_MonthlyNo");
            str.Append(",@inv_ReferenceNo");
            str.Append(",@inv_ReferenceDate");
            str.Append(",@inv_DueDate");
            str.Append(",@inv_AccountKind");
            str.Append(",@inv_AccountKey");
            str.Append(",@inv_CurrentKey");
            str.Append(",@inv_SalesPerson");
            str.Append(",@inv_SalesHand");
            str.Append(",@inv_Description");
            str.Append(",@inv_Currency");
            str.Append(",@inv_SubTotal");
            str.Append(",@inv_Discount");
            str.Append(",@inv_vatAmount");
            str.Append(",@inv_BonusAmount");
            str.Append(",@inv_Total");
            str.Append(",@inv_PaymentDiscount");
            str.Append(",@inv_RetentionLess");
            str.Append(",@inv_InvoiceCost");
            str.Append(",@inv_Quantity");
            str.Append(",@inv_BonusQuantity");
            str.Append(",@inv_DeliveryKind");
            str.Append(",@inv_IncludeFxd");
            str.Append(",@inv_IncludeExp");
            str.Append(",@inv_ImportationKey");
            str.Append(",@inv_Returned");
            str.Append(",@inv_OriginalInvoice");
            str.Append(",@inv_Source");
            str.Append(",@inv_IsCredit");
            str.Append(",@inv_CostCenter");
            str.Append(",@inv_Project)");
            return str.ToString();
        }
        static string GetInvDocument_TransactionDetails()
        {
            StringBuilder str = new StringBuilder();
            str.Clear();
            str.Append("INSERT INTO InvDocument_TransactionDetails");
            str.Append("([inv_OperationKey]");
            str.Append(",[inv_Key]");
            str.Append(",[inv_Index]");
            str.Append(",[inv_IO]");
            str.Append(",[inv_SourceWarehouse]");
            str.Append(",[inv_TargetWarehouse]");
            str.Append(",[inv_ItemKind]");
            str.Append(",[inv_ItemKey]");
            str.Append(",[inv_ProDate]");
            str.Append(",[inv_ExpDate]");
            str.Append(",[inv_Color]");
            str.Append(",[inv_Size]");
            str.Append(",[inv_Unit]");
            str.Append(",[inv_UnitPrice]");
            str.Append(",[inv_SalesPrice]");
            str.Append(",[inv_Quantity]");
            str.Append(",[inv_Bonus]");
            str.Append(",[inv_Amount]");
            str.Append(",[inv_Discount]");
            str.Append(",[inv_DiscountText]");
            str.Append(",[inv_vatAmount]");
            str.Append(",[inv_UnitCost]");
            str.Append(",[inv_vatKey]");
            str.Append(",[inv_vatRate]");
            str.Append(",[inv_Total]");
            str.Append(",[inv_Batch]");
            str.Append(",[inv_ConsumptionKind]");
            str.Append(",[inv_Hidden]");
            str.Append(",[inv_Printable]");
            str.Append(",[inv_Status]");
            str.Append(",[inv_Description]");
            str.Append(",[inv_CostCenter]");
            str.Append(",[inv_Project])");
            str.Append(" VALUES ");
            str.Append("(@inv_OperationKey");
            str.Append(",@inv_Key");
            str.Append(",@inv_Index");
            str.Append(",@inv_IO");
            str.Append(",@inv_SourceWarehouse");
            str.Append(",@inv_TargetWarehouse");
            str.Append(",@inv_ItemKind");
            str.Append(",@inv_ItemKey");
            str.Append(",@inv_ProDate");
            str.Append(",@inv_ExpDate");
            str.Append(",@inv_Color");
            str.Append(",@inv_Size");
            str.Append(",@inv_Unit");
            str.Append(",@inv_UnitPrice");
            str.Append(",@inv_SalesPrice");
            str.Append(",@inv_Quantity");
            str.Append(",@inv_Bonus");
            str.Append(",@inv_Amount");
            str.Append(",@inv_Discount");
            str.Append(",@inv_DiscountText");
            str.Append(",@inv_vatAmount");
            str.Append(",@inv_UnitCost");
            str.Append(",@inv_vatKey");
            str.Append(",@inv_vatRate");
            str.Append(",@inv_Total");
            str.Append(",@inv_Batch");
            str.Append(",@inv_ConsumptionKind");
            str.Append(",@inv_Hidden");
            str.Append(",@inv_Printable");
            str.Append(",@inv_Status");
            str.Append(",@inv_Description");
            str.Append(",@inv_CostCenter");
            str.Append(",@inv_Project)");
            return str.ToString();
        }
    }
}
