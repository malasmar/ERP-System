using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading;
namespace Platxe.Areas.Inventory.Controllers.api
{

    [Area("Inventory")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/inv/[Controller]/[Action]")]
    public class icardsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        public icardsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        }
        [HttpGet]
        public JsonResult Warehouse(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Cards.Warehouse().GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateWarehouse(CLiInventory.Cards.Warehouse data)
        {
            if (data.Key == null)
            {
                CLiInventory.Cards.Warehouse.Insert(DB, data);
            }
            else
            {
                CLiInventory.Cards.Warehouse.Update(DB, data);
            }
            return Json(true);
        }
        [HttpDelete]
        public JsonResult DeleteWarehouse(Guid? Key)
        {
            return Json(CLiInventory.Cards.Warehouse.Delete(DB, Key));
        }


        [HttpGet]
        public JsonResult Unit(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Cards.Unit().GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateUnit(CLiInventory.Cards.Unit data)
        {
            if (data.Key == null)
            {
                CLiInventory.Cards.Unit.Insert(DB, data);
            }
            else
            {
                CLiInventory.Cards.Unit.Update(DB, data);
            }
            return Json(true);
        }
        [HttpDelete]
        public JsonResult DeleteUnit(Guid? Key)
        {
            return Json(CLiInventory.Cards.Unit.Delete(DB, Key));
        }


        [HttpGet]
        public JsonResult Category(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Cards.Category().GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateCategory(CLiInventory.Cards.Category data)
        {
            if (data.Key == null)
            {
                CLiInventory.Cards.Category.Insert(DB, data);
            }
            else
            {
                CLiInventory.Cards.Category.Update(DB, data);
            }
            return Json(true);
        }
        [HttpDelete]
        public JsonResult DeleteCategory(Guid? Key)
        {
            return Json(CLiInventory.Cards.Category.Delete(DB, Key));
        }

        [HttpGet]
        public JsonResult StockItem(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Cards.StockItem().GetList(DB), loadOptions));
        }
        
              [HttpGet]
        public JsonResult PackageItems(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Cards.StockItem().GetPackages(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateStockItem(CLiInventory.Cards.StockItem data)
        {
            Guid? key;
            if (data.Key == null)
            {
                key = CLiInventory.Cards.StockItem.Insert(DB, data);
            }
            else
            {
                key = CLiInventory.Cards.StockItem.Update(DB, data);
            }
            return Json(key);
        }
        [HttpDelete]
        public JsonResult DeleteStockItem(Guid? Key)
        {
            return Json(CLiInventory.Cards.StockItem.Delete(DB, Key));
        }


        //Operation
        [HttpGet]
        public JsonResult ItemCode(Guid? Key, string Prefix)
        {
            return Json(CLiCore.VoucherOperation.InventoryAccountCode(DB, Key, Prefix));
        }
        [HttpGet]
        public JsonResult StockUnit(Guid? Key, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Cards.Stock.StockUnit().GetList(DB, Key), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateStockUnit(CLiInventory.Cards.Stock.StockUnit data)
        {
            if (data.Key == null)
            {
                CLiInventory.Cards.Stock.StockUnit.Insert(DB, data);
            }
            else
            {
                CLiInventory.Cards.Stock.StockUnit.Update(DB, data);
            }
            return Json(true);
        }

        #region "Stock Tag Cards"
        [HttpGet]
        public JsonResult Color(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Cards.Tag.Color().GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateColor(CLiInventory.Cards.Tag.Color data)
        {
            if (data.Key == null)
            {
                CLiInventory.Cards.Tag.Color.Insert(DB, data);
            }
            else
            {
                CLiInventory.Cards.Tag.Color.Update(DB, data);
            }
            return Json(true);
        }
        [HttpDelete]
        public JsonResult DeleteColor(Guid? Key)
        {
            return Json(CLiInventory.Cards.Tag.Color.Delete(DB, Key));
        }

        [HttpGet]
        public JsonResult Size(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Cards.Tag.Size().GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateSize(CLiInventory.Cards.Tag.Size data)
        {
            if (data.Key == null)
            {
                CLiInventory.Cards.Tag.Size.Insert(DB, data);
            }
            else
            {
                CLiInventory.Cards.Tag.Size.Update(DB, data);
            }
            return Json(true);
        }
        [HttpDelete]
        public JsonResult DeleteSize(Guid? Key)
        {
            return Json(CLiInventory.Cards.Tag.Size.Delete(DB, Key));
        }

        [HttpGet]
        public JsonResult Price(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Cards.Tag.Price().GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdatePrice(CLiInventory.Cards.Tag.Price data)
        {
            if (data.Key == null)
            {
                CLiInventory.Cards.Tag.Price.Insert(DB, data);
            }
            else
            {
                CLiInventory.Cards.Tag.Price.Update(DB, data);
            }
            return Json(true);
        }
        [HttpDelete]
        public JsonResult DeletePrice(Guid? Key)
        {
            return Json(CLiInventory.Cards.Tag.Price.Delete(DB, Key));
        }
        #endregion

        //Batch
        [HttpGet]
        public JsonResult ItemBatches(Guid? Key, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Cards.Stock.ItemBatch().GetList(DB, Key), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateItemBatch(CLiInventory.Cards.Stock.ItemBatch data)
        {
            if (data.Key == null)
            {
                CLiInventory.Cards.Stock.ItemBatch.Insert(DB, data);
            }
            else
            {
                CLiInventory.Cards.Stock.ItemBatch.Update(DB, data);
            }
            return Json(true);
        }


        [HttpPut]
        public JsonResult UpdateStockCodes(Guid? key, string values, Guid? xKey)
        {
            CLiInventory.Cards.Stock.StockCodes item = new CLiInventory.Cards.Stock.StockCodes();
            JsonConvert.PopulateObject(values, item);
            item.Item = xKey;
            CLiInventory.Cards.Stock.StockCodes.Update(DB, item);
            return Json(Ok());
        }
        [HttpPost]
        public JsonResult InsertStockCodes(Guid? key, string values,Guid? xKey)
        {
            CLiInventory.Cards.Stock.StockCodes item = new CLiInventory.Cards.Stock.StockCodes();
            JsonConvert.PopulateObject(values, item);
            item.Item = xKey;
            CLiInventory.Cards.Stock.StockCodes.Insert(DB, item);
            return Json(Ok());
        }
        [HttpDelete]
        public void DeleteStockCodes(Guid? key)
        {
            CLiInventory.Cards.Stock.StockCodes.Delete(DB, key);
        }
        [HttpGet]
        public JsonResult StockCodes(Guid? Key, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Cards.Stock.StockCodes().GetList(DB, Key), loadOptions));
        }
    }
}
