using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiFinancial.Documents
{
    public class vatTransaction
    {
        public Guid? Key { get; set; }
        public Guid? OperationKey { get; set; }
        public Guid? vatKey { get; set; }
        public decimal vatRate { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
