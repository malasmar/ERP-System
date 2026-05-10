using System;
using System.Collections.Generic;
using System.Text;

namespace CLiMobile.Sales.SyncPull
{
    public class InvoiceItem
    {
        public Invoice Item { get; set; }
        public List<InvoiceDetails> Details { get; set; }
        public List<InvoicePaymentMethod> Methods { get; set; }
    }
}
