using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Operation
{
    public class StocktakingDetails
    {
        public int RecNo { get; set; }
        public Guid? Key { get; set; }
        public int Index { get; set; }
        public Guid? Item { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Balance { get; set; }
    }
}
