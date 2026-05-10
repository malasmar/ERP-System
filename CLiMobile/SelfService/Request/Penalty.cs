using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.Request
{
    public class Penalty
    {
        public Guid? Key { get; set; }
        public List<Guid> Employee { get; set; }
        public Guid? PenaltyKey { get; set; }
        public Guid? Supervisor { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Kind { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }


    }

}
