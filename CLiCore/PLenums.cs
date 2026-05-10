using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore
{
    public static class PLenums
    {
        public enum AccountingCategory : int
        {
            CurrentAccount = 1,
            CashBox = 2,
            Bank = 3,
            Revenue = 4,
            Expenses = 5,

        }
        public enum TransactionAccount : int
        {
            CurrentAccount = 0,
            Employee = 1,
            CashBox = 2,
            Bank = 3,
            Fixture = 4,
            Revenue = 6,
            Expenses = 5,
            ChartofAccount = 7,
            Stock = 8

        }
        public enum CurrentAccountKind : int
        {
            General = 0,
            Supplier = 1,
            Client = 2,
            Employee = 3,
            Owner = 4,
            SisterCompany = 5,
            Adjustment = 6,
            All=-1
        }
        public enum PaymentKind : int
        {
            General=0,
            CloseInvoice=1,
            Advance=2
        }
        public enum vatKind : int
        {
            Sales=0,
            Purchase=1,
        }
    }
}
