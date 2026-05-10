using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.Request
{
    public class AnnualLeave
    {
        public Guid? Key { get; set; }
        public Guid? Employee { get; set; }
        public int Kind { get; set; }
        public DateTime LeaveDate { get; set; }
        public int LeaveDays { get; set; }
        public string Description { get; set; }
        public Boolean AttachmentStatus { get; set; }
        public string FileName { get; set; }
    }
}
