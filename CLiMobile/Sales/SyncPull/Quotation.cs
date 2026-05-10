using System;
using System.Collections.Generic;
using System.Text;
 
namespace CLiMobile.Sales.SyncPull
{
    public class Quotation
    {
        
        public Guid Key { get; set; }
        public int No { get; set; }
        public DateTime VoucherDate { get; set; }
        public DateTime Expiry { get; set; }
        public Guid? Client { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal Total { get; set; }
        public int CartItemCount { get; set; }
        public bool Sync { get; set; }
        public int Vehicles { get; set; }
    }
}
