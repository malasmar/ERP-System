using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/Dash/[Controller]/[Action]")]
    public class iDashboardController : Controller
    {
        private readonly string DB;
        private readonly string xLan;
        private readonly int Year;
        public iDashboardController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        }

        #region "Sales API"

        [HttpGet]
        public JsonResult WeaklyNetSales()
        {
            return Json(new CLiDashboard.Sales.WeaklyNetSales().GetList(DB, Year));
        }
        [HttpGet]
        public JsonResult WeaklyNetSalesPrev()
        {
            return Json(new CLiDashboard.Sales.WeaklyNetSales().GetList(DB, Year - 1));
        }
        [HttpGet]
        public JsonResult MonthlyNetSales()
        {
            return Json(new CLiDashboard.Sales.MonthlyNetSales().GetList(DB, Year));
        }
        [HttpGet]
        public JsonResult AccountsKind(string FirstDate, string LastDate)
        {
            DateTime first = new DateTime(Convert.ToInt32(FirstDate.Split(".")[2]), Convert.ToInt32(FirstDate.Split(".")[1]), Convert.ToInt32(FirstDate.Split(".")[0]));
            DateTime last = new DateTime(Convert.ToInt32(LastDate.Split(".")[2]), Convert.ToInt32(LastDate.Split(".")[1]), Convert.ToInt32(LastDate.Split(".")[0]));
            return Json(new CLiDashboard.Sales.AccountsKind().GetList(DB, first, last));
        }
        [HttpGet]
        public JsonResult ItemKindTotal(string FirstDate, string LastDate)
        {
            DateTime first = new DateTime(Convert.ToInt32(FirstDate.Split(".")[2]), Convert.ToInt32(FirstDate.Split(".")[1]), Convert.ToInt32(FirstDate.Split(".")[0]));
            DateTime last = new DateTime(Convert.ToInt32(LastDate.Split(".")[2]), Convert.ToInt32(LastDate.Split(".")[1]), Convert.ToInt32(LastDate.Split(".")[0]));
            return Json(new CLiDashboard.Sales.ItemKindTotal().GetList(DB, first, last));
        }
        [HttpGet]
        public JsonResult SalesTotal(string FirstDate, string LastDate)
        {
            DateTime first = new DateTime(Convert.ToInt32(FirstDate.Split(".")[2]), Convert.ToInt32(FirstDate.Split(".")[1]), Convert.ToInt32(FirstDate.Split(".")[0]));
            DateTime last = new DateTime(Convert.ToInt32(LastDate.Split(".")[2]), Convert.ToInt32(LastDate.Split(".")[1]), Convert.ToInt32(LastDate.Split(".")[0]));
            return Json(new CLiDashboard.Sales.SalesTotal().GetList(DB, first, last));
        }
        [HttpGet]
        public JsonResult Clients()
        {
            return Json(new CLiDashboard.Sales.Clients().GetList(DB));
        }
        [HttpGet]
        public JsonResult ClientsBalance()
        {
            return Json(new CLiDashboard.Sales.ClientsBalance().GetList(DB));
        }
        [HttpGet]
        public JsonResult Top5SalesPerson()
        {
            return Json(new CLiDashboard.Sales.SalesPerson().GetList(DB, xLan, Year));
        }
        [HttpGet]
        public JsonResult Top5ItemsSales()
        {
            return Json(new CLiDashboard.Sales.ItemsSales().GetList(DB, xLan, Year));
        }
        [HttpGet]
        public JsonResult Invoices()
        {
            return Json(new CLiDashboard.Sales.Invoices().GetList(DB, Year));
        }
        [HttpGet]
        public JsonResult Quotations()
        {
            return Json(new CLiDashboard.Sales.Quotations().GetList(DB, Year));
        }
        [HttpGet]
        public JsonResult Proforma()
        {
            return Json(new CLiDashboard.Sales.Quotations().Proforma(DB, Year));
        }
        [HttpGet]
        public JsonResult SalesPersonCount()
        {
            return Json(new CLiDashboard.Sales.SalesPersonCount().GetList(DB));
        }
        #endregion

        #region "Accounting API"
        [HttpGet]
        public JsonResult TrialBalance(string FirstDate, string LastDate)
        {
            DateTime first = new DateTime(Convert.ToInt32(FirstDate.Split(".")[2]), Convert.ToInt32(FirstDate.Split(".")[1]), Convert.ToInt32(FirstDate.Split(".")[0]));
            DateTime last = new DateTime(Convert.ToInt32(LastDate.Split(".")[2]), Convert.ToInt32(LastDate.Split(".")[1]), Convert.ToInt32(LastDate.Split(".")[0]));
            return Json(new CLiDashboard.Accounting.TrialBalance().GetList(DB, first, last));
        }
        [HttpGet]
        public JsonResult TrialBalanceOpening(string FirstDate)
        {
            DateTime first = new DateTime(Convert.ToInt32(FirstDate.Split(".")[2]), Convert.ToInt32(FirstDate.Split(".")[1]), Convert.ToInt32(FirstDate.Split(".")[0]));
            return Json(new CLiDashboard.Accounting.TrialBalance().GetOpening(DB, first));
        }
        [HttpGet]
        public JsonResult TrialBalanceKind(string FirstDate, string LastDate)
        {
            DateTime first = new DateTime(Convert.ToInt32(FirstDate.Split(".")[2]), Convert.ToInt32(FirstDate.Split(".")[1]), Convert.ToInt32(FirstDate.Split(".")[0]));
            DateTime last = new DateTime(Convert.ToInt32(LastDate.Split(".")[2]), Convert.ToInt32(LastDate.Split(".")[1]), Convert.ToInt32(LastDate.Split(".")[0]));
            return Json(new CLiDashboard.Accounting.TrialBalanceKind().GetList(DB, first, last));
        }
        [HttpGet]
        public JsonResult AccountsTotal()
        {
            return Json(new CLiDashboard.Accounting.AccountsTotal().GetList(DB));
        }
        [HttpGet]
        public JsonResult GLTotal()
        {
            return Json(new CLiDashboard.Accounting.GL().GetList(DB));
        }
        [HttpGet]
        public JsonResult IncomeStatmentMonthly()
        {
            return Json(new CLiDashboard.Accounting.IncomeStatmentMonthly().GetList(DB, Year));
        }
        [HttpGet]
        public JsonResult IncomeStatmentWeekly()
        {
            return Json(new CLiDashboard.Accounting.IncomeStatmentWeekly().GetList(DB, Year));
        }
        [HttpGet]
        public JsonResult IncomeStatmentDetails()
        {
            return Json(new CLiDashboard.Accounting.IncomeStatmentDetails().GetList(DB, xLan, Year));
        }
        #endregion

        #region "Purchasing Dashboard"
        [HttpGet]
        public JsonResult Suppliers()
        {
            return Json(new CLiDashboard.Purchase.Suppliers().GetList(DB));
        }
        [HttpGet]
        public JsonResult SuppliersBalance()
        {
            return Json(new CLiDashboard.Purchase.SuppliersBalance().GetList(DB));
        }
        [HttpGet]
        public JsonResult PurchaseAccountsKind(string FirstDate, string LastDate)
        {
            DateTime first = new DateTime(Convert.ToInt32(FirstDate.Split(".")[2]), Convert.ToInt32(FirstDate.Split(".")[1]), Convert.ToInt32(FirstDate.Split(".")[0]));
            DateTime last = new DateTime(Convert.ToInt32(LastDate.Split(".")[2]), Convert.ToInt32(LastDate.Split(".")[1]), Convert.ToInt32(LastDate.Split(".")[0]));
            return Json(new CLiDashboard.Purchase.AccountsKind().GetList(DB, first, last));
        }
        [HttpGet]
        public JsonResult PurchaseItemKindTotal(string FirstDate, string LastDate)
        {
            DateTime first = new DateTime(Convert.ToInt32(FirstDate.Split(".")[2]), Convert.ToInt32(FirstDate.Split(".")[1]), Convert.ToInt32(FirstDate.Split(".")[0]));
            DateTime last = new DateTime(Convert.ToInt32(LastDate.Split(".")[2]), Convert.ToInt32(LastDate.Split(".")[1]), Convert.ToInt32(LastDate.Split(".")[0]));
            return Json(new CLiDashboard.Purchase.ItemKindTotal().GetList(DB, first, last));
        }
        [HttpGet]
        public JsonResult PurchaseTotal(string FirstDate, string LastDate)
        {
            DateTime first = new DateTime(Convert.ToInt32(FirstDate.Split(".")[2]), Convert.ToInt32(FirstDate.Split(".")[1]), Convert.ToInt32(FirstDate.Split(".")[0]));
            DateTime last = new DateTime(Convert.ToInt32(LastDate.Split(".")[2]), Convert.ToInt32(LastDate.Split(".")[1]), Convert.ToInt32(LastDate.Split(".")[0]));
            return Json(new CLiDashboard.Purchase.PurchaseTotal().GetList(DB, first, last));
        }
        [HttpGet]
        public JsonResult MonthlyPurchase()
        {
            return Json(new CLiDashboard.Purchase.MonthlyPurchasing().GetList(DB, Year));
        }
        [HttpGet]
        public JsonResult Top10PurchaseItems()
        {
            return Json(new CLiDashboard.Purchase.ItemsPurchasing().GetList(DB, xLan, Year));
        }
        #endregion
        #region "Cost Center"
        [HttpGet]
        public JsonResult CostCenterIncomeBalance()
        {
            return Json(new CLiDashboard.CostCenter.CostCenterIncomeBalance().CostCenter(DB, xLan));
        }
        [HttpGet]
        public JsonResult ProjectIncomeBalance()
        {
            return Json(new CLiDashboard.CostCenter.CostCenterIncomeBalance().Project(DB, xLan));
        }

        [HttpGet]
        public JsonResult CostIncomeDetails(Guid? Key)
        {
            return Json(new CLiDashboard.CostCenter.IncomeStatmentDetails().CostCenter(DB, xLan, Key, Year));
        }

        [HttpGet]
        public JsonResult ProjectIncomeDetails(Guid? Key)
        {
            return Json(new CLiDashboard.CostCenter.IncomeStatmentDetails().Project(DB, xLan, Key, Year));
        }
        #endregion
        #region "Financial Dashboard"
        [HttpGet]
        public JsonResult Fixture()
        {
            return Json(new CLiDashboard.Financial.Fixture().GetItem(DB));
        }
        [HttpGet]
        public JsonResult CashOnHand()
        {
            return Json(new CLiDashboard.Financial.CashOnHand().GetList(DB));
        }
        [HttpGet]
        public JsonResult CurrentAccountBalance()
        {
            return Json(new CLiDashboard.Financial.CurrentAccountBalance().GetList(DB));
        }
        [HttpGet]
        public JsonResult EmployeesKindBalance()
        {
            return Json(new CLiDashboard.Financial.EmployeesKindBalance().GetList(DB));
        }
        [HttpGet]
        public JsonResult CurrentMonthVAT()
        {
            DateTime FirstDate = new DateTime(Year, DateTime.Now.Month, 1);
            DateTime LastDate = FirstDate.AddMonths(1);
            return Json(new CLiDashboard.Financial.vatDeclaration().GetList(DB, FirstDate, LastDate));
        }
        [HttpGet]
        public JsonResult PreviousMonthVAT()
        {
            DateTime td = new DateTime(Year, DateTime.Now.Month, 1);
            DateTime FirstDate = td.AddMonths(-1);
            DateTime LastDate = FirstDate.AddMonths(1);
            return Json(new CLiDashboard.Financial.vatDeclaration().GetList(DB, FirstDate, LastDate));
        }
        [HttpGet]
        public JsonResult CurrentQuarterVAT()
        {
            DateTime date = DateTime.Now;
            int quarterNumber = (date.Month - 1) / 3 + 1;
            DateTime FirstDate = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);
            DateTime LastDate = FirstDate.AddMonths(3).AddDays(-1);
            return Json(new CLiDashboard.Financial.vatDeclaration().GetList(DB, FirstDate, LastDate));
        }
        [HttpGet]
        public JsonResult PreviousQuarterVAT()
        {
            DateTime date = DateTime.Now;
            int quarterNumber = (date.Month - 1) / 3 + 1;
            DateTime FirstDate = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);
            DateTime LastDate = FirstDate.AddMonths(3).AddDays(-1);
            return Json(new CLiDashboard.Financial.vatDeclaration().GetList(DB, FirstDate.AddMonths(-3), LastDate.AddMonths(-3)));
        }
        [HttpGet]
        public JsonResult TotalVAT()
        {
            return Json(CLiDashboard.core.TotalVAT(DB));
        }
        [HttpGet]
        public JsonResult WeeklyAccountsBalance()
        {
            return Json(new CLiDashboard.Financial.WeeklyAccountsBalance().GetList(DB, DateTime.Now.AddYears(-1), DateTime.Now));
        }
        [HttpGet]
        public JsonResult FinancialIncomeStatmentDetails()
        {
            return Json(new CLiDashboard.Financial.IncomeStatmentDetails().GetList(DB, xLan, Year));
        }
        #endregion
    }
}
