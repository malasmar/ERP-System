using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.Request
{
    public class Loan
    {
        public Guid? Key { get; set; }
        public Guid? Employee { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
        public decimal MonthlyAmount { get; set; }
        public string Description { get; set; }
    }
    public class LoanSchedule
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
        public List<LoanSchedule> SplitAdvanceByAmount(decimal Amount, decimal Value, int Year, int Month)
        {
            var items = new List<LoanSchedule>();
            var C = Math.Floor(Amount / Value);
            int Y = Year;
            int M = Month;
            int mc = 0;
            int mi = M;
            decimal SlitAmount = Value;
            decimal totamount = 0;
            for (int i = 0; i <= C; i++)
            {
                if (i == C)
                {
                    SlitAmount = Amount - totamount;
                }
                //lstSpliter.AddItem([i + 1, Y, M + mc, SlitAmount]);
                items.Add(new LoanSchedule()
                {
                    Year = Y,
                    Month = M + mc,
                    Amount = SlitAmount,
                });
                mc = mc + 1;
                if (mi == 12)
                {
                    M = 1;
                    mi = 0;
                    mc = 0;
                    Y = Y + 1;
                }
                mi = mi + 1;
                totamount = totamount + SlitAmount;
            }
            return items;
        }
        public List<LoanSchedule> SplitAdvanceByMonth(decimal Amount, decimal Value, int Year, int Month)
        {
            var items = new List<LoanSchedule>();
            var Y = Year;
            var M = Month;
            var mc = 0;
            var mi = M;
            var SlitAmount = Math.Ceiling(Amount / Value);
            for (int i = 0; i <= Value - 1; i++)
            {
                if (i == Value - 1)
                {
                    SlitAmount = Amount - Math.Ceiling(Amount / Value) * i;
                }
                //lstSpliter.AddItem([i + 1, Y, M + mc, SlitAmount]);
                items.Add(new LoanSchedule()
                {
                    Year = Y,
                    Month = M + mc,
                    Amount = SlitAmount,
                });
                mc = mc + 1;
                if (mi == 12)
                {
                    M = 1;
                    mi = 0;
                    mc = 0;
                    Y = Y + 1;
                }
                mi = mi + 1;
            }
            return items;
        }
    }
}
