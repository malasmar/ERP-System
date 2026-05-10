using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore
{
    public enum DocumentKind:int
    {
        acc_OpeningGL = 1,
        acc_GeneralLedger = 2,
        finCashPayment = 3,
        finCashCollection = 4,
        finBankPayment = 5,
        finBankCollection = 6,
        finCreditNote = 7,
        finDebitNote = 8,
        finJournalVoucher = 9,
        invOpeningBalance = 70,
        PurchaseInvoice = 71,
        PurchaseRequest = 72,
        PurchaseOrder = 73,
        PurchaseReceipt = 74,
        ReturnPurchase = 75,
        SendToWarehouse = 76,
        ConsumptionStock = 77,
        RetConsumptionStock = 78,
        ReceiptInWarehouse = 79,
        SalesInvoice = 80,
        Quotation = 81,
        SalesSendMaterial = 82,
        ReturnSalesInvoice = 83,
        SalesOrder = 86,
        SalesProformaInvoice = 101,
        InternalRequest = 102,
        DeliveryNote = 702,
        ExitCheques = 90,
        RecevedCheques = 91,
        SalesContract = 110,
    }
}
