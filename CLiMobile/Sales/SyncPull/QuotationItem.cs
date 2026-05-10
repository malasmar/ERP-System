using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.Sales.SyncPull
{
    public class QuotationItem
    {
        public Quotation Item { get; set; }
        public List<QuotationDetails> Details { get; set; }
    }
}
