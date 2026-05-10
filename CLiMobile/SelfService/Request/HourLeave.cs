using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.Request
{
    public class HourLeave
    {
        public Guid? Key { get; set; }
        public Guid? Employee { get; set; }
        public int Kind { get; set; }
        public DateTime LeaveDate { get; set; }
        public TimeSpan LeaveHour { get; set; }
        public TimeSpan ReturnHour { get; set; }
        public Boolean EndDay { get; set; }
        public string Description { get; set; }
    }
}
