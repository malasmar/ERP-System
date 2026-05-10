using CLiCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Controllers
{
    [Authorize]
    public class CardsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public CardsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }
        public IActionResult Account(Guid? Key, int Kind)
        {
            switch (Kind)
            {
                case (int)PLenums.TransactionAccount.CurrentAccount:
                    return PartialView("_CurrentAccount", new CLiFinancial.Cards.CurrentAccount().GetItem(DB, Key));
                case (int)PLenums.TransactionAccount.Bank:
                    return PartialView("_Bank", new CLiFinancial.Cards.Bank().GetItem(DB, Key));
                case (int)PLenums.TransactionAccount.CashBox:
                    return PartialView("_CashBox", new CLiFinancial.Cards.CashBox().GetItem(DB, Key));
                case (int)PLenums.TransactionAccount.Expenses:
                    return PartialView("_Expenses", new CLiFinancial.Cards.Expenses().GetItem(DB, Key));
                case (int)PLenums.TransactionAccount.Revenue:
                    return PartialView("_Revenue", new CLiFinancial.Cards.Revenue().GetItem(DB, Key));
                case (int)PLenums.TransactionAccount.Stock:
                    return PartialView("_StockItem", new CLiInventory.Cards.StockItem().GetItem(DB, Key));
                default:
                    return PartialView("_CurrentAccount", new CLiFinancial.Cards.CurrentAccount().GetItem(DB, Key));
            }
        }

    }
}
