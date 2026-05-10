using System;
using System.Collections.Generic;
using System.Text;

namespace CLiMobile.Sales.SyncPull
{
    public class Invoice
    {
        public Guid Key { get; set; }
        public int No { get; set; }
        public DateTime VoucherDate { get; set; }
        public int Branch { get; set; }
        public Guid Prefix { get; set; }
        public int SourceWarehouse { get; set; }
        public Guid? SalesPerson { get; set; }
        public DateTime Expiry { get; set; }
        public Guid? Client { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal Total { get; set; }
        public int CartItemCount { get; set; }
        public bool Sync { get; set; }
        public bool IsCredit { get; set; }
        public Guid Session { get; set; }
        public int CreateUser { get; set; }

    }
}
