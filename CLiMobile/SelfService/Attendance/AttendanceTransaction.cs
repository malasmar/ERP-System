using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.Attendance
{
    public class AttendanceTransaction
    {
        public Guid? Key { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public DateTime? LoginDate { get; set; }
        public DateTime? LogoutDate { get; set; }
        public Guid? Location { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
    }
}
