using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.Request
{
    public class Confirmation
    {
        public Guid Key { get; set; }
        public Guid Request { get; set; }   
        public Guid Person { get; set; }
        public string Comment { get; set; }
        public int Status { get; set; }
        public bool Final { get; set; }
    }
}
