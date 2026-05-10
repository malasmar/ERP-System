using System;
using System.Collections.Generic;
using System.Text;
 
namespace CLiMobile.Sales.SyncPull
{
    public class InvoicePaymentMethod
    {
        public Guid Key { get; set; }
        public Guid InvoiceKey { get; set; }
        public Guid MethodKey { get; set; }
        public Guid Session { get; set; }
        public decimal Amount { get; set; }
    }
}
