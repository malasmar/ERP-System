using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Security.Claims;
using System.Text;

namespace Platxe.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Route("Inventory/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Inventory")]
	[Authorize(Roles = "PL-Inventory")]
	public class OperationController : Controller
    {
        private readonly IWebHostEnvironment root;
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        public OperationController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            root = hostingEnvironment;
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        }

        public IActionResult Stocktaking()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Stocktaking(CLiInventory.Operation.Stocktaking Header, IFormFile StockFiles)
        {
            string filename = DateTime.Now.ToString("dd-MM-yyyy") + "_" + Header.Warehouse.ToString() + ".csv";
            if (StockFiles != null)
            {
                using (var streamReader = new MemoryStream())
                {
                    StockFiles.CopyTo(streamReader);
                    System.IO.File.WriteAllBytes(Path.Combine(root.WebRootPath, "aiwa-data", DB, @"Files\Stocktaking\" + filename), streamReader.ToArray());
                }
                Header.FileName = filename;
                Header.CreateDate = DateTime.Now;
                Header.CreateUser= UserID;
                List<CLiInventory.Operation.StocktakingDetails> Details = new List<CLiInventory.Operation.StocktakingDetails>();
                using (var reader = new System.IO.StreamReader(StockFiles.OpenReadStream()))
                {
                    int i = 0;
                    while (reader.Peek() >= 0)
                    {
                        if (i == 0)
                        {
                            ++i;
                            continue;
                        }
                           

                        string[] dt = reader.ReadLine().Split(',');
                        Guid? Key=CLiInventory.core.GetItemKeyFromCode(DB,dt[1].Trim());
                        if (Key != null)
                        {
                            CLiInventory.Operation.StocktakingDetails item = new CLiInventory.Operation.StocktakingDetails();
                            item.Item = Key;
                            item.Unit = dt[4];
                            decimal qty = 0;
                            decimal.TryParse(dt[5], out qty);
                            item.Quantity = qty;
                            item.Balance = CLiInventory.core.ItemBalanceMainUnit(DB, Key, Header.Warehouse, Header.Date);
                            Details.Add(item);
                        }
                        ++i;
                    }
                }
                CLiInventory.Operation.Stocktaking.UpdateReceiptInWarehouse(DB, Header, Details);
            }
            return RedirectToAction("Succeed");
        }
        public IActionResult Succeed()
        {
            return View();
        }
        public IActionResult DownloadStockItems()
        {

            List<CLiInventory.Selections.DownloadItems> items = new CLiInventory.Selections.DownloadItems().GetList(DB);


            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);

            sw.WriteLine("Category,SKU,Name,English Name,Unit,Qty", Encoding.UTF8);
            foreach (CLiInventory.Selections.DownloadItems item in items)
            {
                sw.WriteLine(item.Category + "," + item.Code.ToString() + "," + item.Name1.Replace(",", "") + "," + item.Name2.Replace(",", "") + "," + item.Unit.Replace(",", "") + "," + "0", Encoding.UTF8);
            }
            sw.Flush();
            ms.Position = 0;
            //sw.WriteLine(str.ToString(), UnicodeEncoding.UTF8);
            return File(ms.GetBuffer(), "text/csv", "Stock.csv");
        }
    }
}
