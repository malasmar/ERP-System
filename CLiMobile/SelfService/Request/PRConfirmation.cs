using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.Request
{
    public class PRConfirmation
    {
        public Guid Key { get; set; }
        public Guid Request { get; set; }
        public Guid Person { get; set; }
        public string Comment { get; set; }
        public int Status { get; set; }
        public bool Final { get; set; }
        public bool Editable { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Kind { get; set; }
        public decimal Value { get; set; }
    }
}
