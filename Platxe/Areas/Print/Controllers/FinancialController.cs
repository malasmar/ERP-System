using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Security.Claims;
using FastReport.Web;
using System.IO.Pipelines;

namespace Platxe.Areas.Print.Controllers
{
    [Area("Print")]
    [Route("Print/[Controller]/[Action]")]
    [Authorize]
    public class FinancialController : Controller
    {
        private readonly IWebHostEnvironment root;
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        private readonly string xLan;
        public FinancialController(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            root = hostingEnvironment;
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
        }
        public IActionResult AccountStatment(Guid? Key, DateTime? FirstDate, DateTime? LastDate, bool Opening)
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();
         
            report.Report.Load(webRootPath + "/Print//Reports/" + "CurrentStatment.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.AccountDetails(DB,Key), "Account Details");
            report.Report.RegisterData(CLiCore.Print.Reports.AccountStatment(DB, Key,FirstDate,LastDate,Opening), "Report Details");
            report.Report.SetParameterValue("FirstDate", FirstDate.Value.ToString("dd.MM.yyyy"));
            report.Report.SetParameterValue("LastDate", LastDate.Value.ToString("dd.MM.yyyy"));
            report.Report.SetParameterValue("Opening", Opening);
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View();
        }
        public IActionResult ChartAccountStatment(Guid? Key, DateTime? FirstDate, DateTime? LastDate, bool Opening)
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();

            report.Report.Load(webRootPath + "/Print//Reports/" + "ChartAccountStatment.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.AccountDetails(DB, Key), "Account Details");
            report.Report.RegisterData(CLiCore.Print.Reports.ChartAccountStatment(DB, Key, FirstDate, LastDate, Opening), "Report Details");
            report.Report.SetParameterValue("FirstDate", FirstDate.Value.ToString("dd.MM.yyyy"));
            report.Report.SetParameterValue("LastDate", LastDate.Value.ToString("dd.MM.yyyy"));
            report.Report.SetParameterValue("Opening", Opening);
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View();
        }
        public IActionResult SummaryAccountStatment(Guid? Key, DateTime? FirstDate, DateTime? LastDate, bool Opening)
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();

            report.Report.Load(webRootPath + "/Print//Reports/" + "StatmentSummary.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.AccountDetails(DB, Key), "Account Details");
            report.Report.RegisterData(CLiCore.Print.Reports.SummaryAccountStatment(DB, Key, FirstDate, LastDate, Opening), "Report Details");
            report.Report.SetParameterValue("FirstDate", FirstDate.Value.ToString("dd.MM.yyyy"));
            report.Report.SetParameterValue("LastDate", LastDate.Value.ToString("dd.MM.yyyy"));
            report.Report.SetParameterValue("Opening", Opening);
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View();
        }


        public IActionResult TrialBalance(DateTime First, DateTime Last, int Level, bool ExpKind)
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();

            report.Report.Load(webRootPath + "/Print//Reports/" + "TrialBalance.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.TrialBalance(DB, First, Last, Level, ExpKind), "Report Details");
            report.Report.SetParameterValue("FirstDate", First.ToString("dd.MM.yyyyy"));
            report.Report.SetParameterValue("LastDate", Last.ToString("dd.MM.yyyyy"));
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View("Reports");
        }

        public IActionResult IncomeStatment(DateTime First, DateTime Last, bool ExpKind)
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();

            report.Report.Load(webRootPath + "/Print//Reports/" + "IncomeStatment.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.IncomeStatment(DB, First, Last, ExpKind), "Report Details");
            report.Report.SetParameterValue("FirstDate", First.ToString("dd.MM.yyyyy"));
            report.Report.SetParameterValue("LastDate", Last.ToString("dd.MM.yyyyy"));
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View("Reports");
        }

        //Accounts Balance
        public IActionResult CurrentAccountBalance(DateTime? FirstDate, DateTime? LastDate, string Parents, string Groups)
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();
            report.Report.Load(webRootPath + "/Print//Reports/" + "CurrentBalance.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.CurrentAccountBalance(DB,FirstDate,LastDate,Parents,Groups), "Report Details");
            report.Report.SetParameterValue("FirstDate", FirstDate.Value.ToString("dd.MM.yyyy"));
            report.Report.SetParameterValue("LastDate", LastDate.Value.ToString("dd.MM.yyyy"));
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View();
        }
        public IActionResult EmployeesBalance(DateTime? FirstDate, DateTime? LastDate, int Kind)
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();
            report.Report.Load(webRootPath + "/Print//Reports/" + "EmployeesBalance.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.EmployeeBalance(DB, FirstDate, LastDate, Kind), "Report Details");
            report.Report.SetParameterValue("FirstDate", FirstDate.Value.ToString("dd.MM.yyyy"));
            report.Report.SetParameterValue("LastDate", LastDate.Value.ToString("dd.MM.yyyy"));
            report.Report.SetParameterValue("Kind", Kind);
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View();
        }
        public IActionResult CashBalance(DateTime? FirstDate, DateTime? LastDate)
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();
            report.Report.Load(webRootPath + "/Print//Reports/" + "CashBalance.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.CashBalance(DB, FirstDate, LastDate), "Report Details");
            report.Report.SetParameterValue("FirstDate", FirstDate.Value.ToString("dd.MM.yyyy"));
            report.Report.SetParameterValue("LastDate", LastDate.Value.ToString("dd.MM.yyyy"));
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View();
        }
        public IActionResult BankBalance(DateTime? FirstDate, DateTime? LastDate)
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();
            report.Report.Load(webRootPath + "/Print//Reports/" + "BankBalance.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.BankBalance(DB, FirstDate, LastDate), "Report Details");
            report.Report.SetParameterValue("FirstDate", FirstDate.Value.ToString("dd.MM.yyyy"));
            report.Report.SetParameterValue("LastDate", LastDate.Value.ToString("dd.MM.yyyy"));
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View();
        }
        public IActionResult RevenueBalance(DateTime? FirstDate, DateTime? LastDate)
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();
            report.Report.Load(webRootPath + "/Print//Reports/" + "RevenuesBalance.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.RevenueBalance(DB, FirstDate, LastDate), "Report Details");
            report.Report.SetParameterValue("FirstDate", FirstDate.Value.ToString("dd.MM.yyyy"));
            report.Report.SetParameterValue("LastDate", LastDate.Value.ToString("dd.MM.yyyy"));
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View();
        }
        public IActionResult ExpensesBalance(DateTime? FirstDate, DateTime? LastDate)
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();
            report.Report.Load(webRootPath + "/Print//Reports/" + "ExpenseBalance.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.ExpenseBalance(DB, FirstDate, LastDate), "Report Details");
            report.Report.SetParameterValue("FirstDate", FirstDate.Value.ToString("dd.MM.yyyy"));
            report.Report.SetParameterValue("LastDate", LastDate.Value.ToString("dd.MM.yyyy"));
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View();
        }

        //Cost Center and Project
        public IActionResult CostCenterBalance()
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();
            report.Report.Load(webRootPath + "/Print//Reports/" + "CostCenterBalance.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.CostCenterBalance(DB), "Report Details");
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View();
        }
        public IActionResult CostCenterIncomeSummary(Guid? Key)
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();
            report.Report.Load(webRootPath + "/Print//Reports/" + "CostCenterIncomeSummary.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.CostCenterDetails(DB, Key), "Account Details");
            report.Report.RegisterData(CLiCore.Print.Reports.CostCenterIncomeSummary(DB,Key), "Report Details");
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View();
        }
        public IActionResult CostCenterStatment(Guid? Key, DateTime? FirstDate, DateTime? LastDate)
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();

            report.Report.Load(webRootPath + "/Print//Reports/" + "CostCenterStatment.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.CostCenterDetails(DB, Key), "Account Details");
            report.Report.RegisterData(CLiCore.Print.Reports.CostCenterStatment(DB, Key, FirstDate, LastDate), "Report Details");
            report.Report.SetParameterValue("FirstDate", FirstDate.Value.ToString("dd.MM.yyyy"));
            report.Report.SetParameterValue("LastDate", LastDate.Value.ToString("dd.MM.yyyy"));
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View();
        }

        public IActionResult ProjectBalance()
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();
            report.Report.Load(webRootPath + "/Print//Reports/" + "ProjectsBalance.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.ProjectBalance(DB), "Report Details");
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View();
        }
        public IActionResult ProjectIncomeSummary(Guid? Key)
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();
            report.Report.Load(webRootPath + "/Print//Reports/" + "ProjectIncomeDetails.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.ProjectDetails(DB, Key), "Account Details");
            report.Report.RegisterData(CLiCore.Print.Reports.ProjectIncomeSummary(DB, Key), "Report Details");
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View();
        }
        public IActionResult ProjectStatment(Guid? Key, DateTime? FirstDate, DateTime? LastDate)
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();

            report.Report.Load(webRootPath + "/Print//Reports/" + "ProjectStatment.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.ProjectDetails(DB, Key), "Account Details");
            report.Report.RegisterData(CLiCore.Print.Reports.ProjectStatment(DB, Key, FirstDate, LastDate), "Report Details");
            report.Report.SetParameterValue("FirstDate", FirstDate.Value.ToString("dd.MM.yyyy"));
            report.Report.SetParameterValue("LastDate", LastDate.Value.ToString("dd.MM.yyyy"));
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View();
        }
        public IActionResult WarehouseValue(int Key, DateTime? Date)
        {
            string webRootPath = root.WebRootPath;
            PxWebReport report = new PxWebReport();

            report.Report.Load(webRootPath + "/Print//Reports/" + "WarehouseValues.frx");
            report.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
            report.Report.RegisterData(CLiCore.Print.Reports.WarehouseDetails(DB, Key), "Warehouse Details");
            report.Report.RegisterData(CLiCore.Print.Reports.WarehouseValue(DB, Key, Date), "Report Details");
            report.Report.SetParameterValue("Date", Date.Value.ToString("dd.MM.yyyy"));
            report.EmbedPictures = true;
            ViewBag.WebReport = report;
            return View();
        }

        //public IActionResult AccountStatmentDes()
        //{
        //    WebReport WebReport = new WebReport(); // Create a Web Report Object
        //    WebReport.Width = "1000"; // Set the width of the report
        //    WebReport.Height = "1000"; // Set the height of the report
        //    string webRootPath = root.WebRootPath; // Get the path to wwwroot folder
        //                                                          //string contentRootPath = _hostingEnvironment.ContentRootPath;
        //    WebReport.Report.Load(webRootPath + "/Print//Reports/Financial/" + "CurrentStatment.frx"); // Load the report into a WebReport object
        //                                                                                               //WebReport.Report.RegisterData(DTPrint.CompanyProfile(), "Company Profile");
        //                                                                                               //WebReport.Report.RegisterData(DTPrint.FinanceHeader(null), "Transaction Header");
        //                                                                                               //WebReport.Report.RegisterData(DTPrint.FinanceHeader(null), "Transaction Details");
        //    WebReport.Report.RegisterData(CLiCore.Print.Reports.CompanyProfile(DB), "Company Profile");
        //    WebReport.Report.RegisterData(CLiCore.Print.Reports.AccountStatment(DB, Guid.NewGuid(), DateTime.Now, DateTime.Now, false), "Report");
        //    WebReport.Mode = WebReportMode.Designer; // Set the mode of the web report object - display of the designer
        //    WebReport.DesignerLocale = "en";
        //    WebReport.DesignerPath = "/WebReportDesigner/index.html"; // Specify the URL of the online designer
        //    WebReport.DesignerSaveCallBack = "/Reports/Reports/Financial/SaveDesignedReport"; // Set the view URL for the report save method
        //    WebReport.Debug = true;
        //    ViewBag.WebReport = WebReport; // pass report to View
        //    return View();
        //}
        //[HttpPost]
        //// call-back for save the designed report
        //public IActionResult SaveDesignedReport(string reportID, string reportUUID,string xxxx)
        //{
        //    string webRootPath = root.WebRootPath; // Get the path to wwwroot folder ViewBag.Message = String.Format("Confirmed {0} {1}", reportID, reportUUID); // Set a message for view

        //      //  Stream reportForSave = Request.Body; // Write the result of the Post query to the stream
        //    string pathToSave = webRootPath + "/Print/Reports/Financial/CurrentStatmentx115.frx"; // get the path to save the file
        //                                                                                       //using (FileStream file = new FileStream(pathToSave, FileMode.OpenOrCreate)) // Create a file stream {
        //                                                                                       //    reportForSave.CopyToAsync(file); // Save the result of the query to a file

        //    using (FileStream file = new FileStream(pathToSave, FileMode.OpenOrCreate)) // Create a file stream 
        //    {
        //        Request.Body.CopyToAsync(file).Wait(); // Save the result of the query to a file
        //    }
        //    return RedirectToAction("AccountStatmentDes");
        //}
    }
    public class PxWebReport : WebReport
    {
        public PxWebReport()
        {
           
            ShowCsvExport = true;
            ShowExcel2007Export = true;
            ShowOdsExport = false;
            ShowOdtExport = false;
            ShowPdfExport = false;
            ShowPowerPoint2007Export = false;
            ShowPreparedReport = false;
            ShowRtfExport = false;
            ShowTextExport = false;
            ShowWord2007Export = true;
            ShowXmlExcelExport = false;
            ShowXpsExport = false;
            this.EmbedPictures=true;
            
            
        }
    }
}
