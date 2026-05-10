using System;
using System.Collections.Generic;
using System.Text;
 
namespace CLiMobile.Sales.SyncPull
{
    public class QuotationDetails
    {
       
        public Guid Key { get; set; }
        public Guid QuotationKey { get; set; }
        public Guid Product { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Bonus { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal vatAmount { get; set; }
        public Guid? vatKey { get; set; }
        public decimal vatRate { get; set; }
        public decimal Total { get; set; }
    }
}
