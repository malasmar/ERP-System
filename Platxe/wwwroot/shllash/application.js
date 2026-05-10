var PLxSettings;
$(document).ready(function ()
{
    var r = document.cookie.replace(/(?:(?:^|.*;\s*)PlatxeLan\s*\=\s*([^;]*).*$)|^.*$/, "$1");
    if (r == "ar")
    {
        DevExpress.config({
            rtlEnabled: true
        });
        // DevExpress.localization.loadMessages('ar');
    }
    $('#dropdown-year').on('click', '.btn-year', function ()
    {
        var xYear = $('#dropdown-year').data('key');
        if (xYear == $(this).data('key'))
            return
        ConfirmChangeYear($(this).data('key'));
    });
    $('#dropdown-database').on('click', '.btn-db', function ()
    {
        var xDatabase = $('#dropdown-database').data('key');
        if (xDatabase == $(this).data('key'))
            return;
        ConfirmChangeFile($(this).data('key'), $(this).data('name1'), $(this).data('name2'));
    });
    setNavigation();
    GetCompanyFiles();
    GetFinancialYear();
});
function PushStatus (x)
{
    // alert('Hello');
    if (typeof (history.pushState) != "undefined")
    {
        var obj = {
            Title: 'ERP',
            Url: $(x).attr("href")
        };
        history.pushState(obj, obj.Title, obj.Url);
        var t = '';
        t = $(x).find('.menu-text').text();
        $(document).attr('title','Platx | ' +  t);
    }
    setNavigation();
}
function setNavigation ()
{
    $('#sidebar .menu-item').removeClass('active');
    var path = window.location;
    var p = path.toString().replace('http://', '').replace('https://', '').replace(path.host, '');
    $(".menu-submenu a").each(function ()
    {
        var href = $(this).attr('href').toString();
        if (p === href)
        {
            $(this).closest('.menu-item').addClass('active');
            $(this).parents().addClass('active');
        }
    });
    $(".menu-item a").each(function ()
    {
        var href = $(this).attr('href').toString();
        if (p === href)
        {
            $(this).closest('.menu-item').addClass('active');
            $(this).parents().addClass('active');
            $(document).attr('title', 'Platx | ' + $(this).find('.menu-text').text());
        }
    });
}
function ConfirmLogout ()
{
    $.confirm({
        title: 'Confirm',
        content: 'Are you want to logout?',
        buttons: {
            confirm: {
                text: 'Logout',
                action: function ()
                {
                    window.location.replace('/Account/Logout');
                }
            },
            cancel: {
                text: 'Cancel',
                action: function () { return; }
            }
        }
    });
}
function ChangeLanguage (value)
{
    $.ajax({
        type: "Post",
        url: "/api/apiaccount/ChangeLanguage",
        data: { Lan: value },
        dataType: "json",
        success: function (response)
        {
            if (response == true)
            {
                document.location.reload();
            }
        },
        failure: function (response)
        {
            alert(response);
        },
        error: function (response)
        {
            alert(response.responseText);
        }
    });
}
function GetCompanyFiles ()
{
    $.ajax({
        type: "Get",
        url: "/api/icore/Databases",
        dataType: "json",
        success: function (response)
        {
            $('#dropdown-database').empty();
            for (const item of response)
            {
                var str = '';
                str = '<a href="javascript:;" class="dropdown-item btn-db" data-key=' + item.Key + ' data-name1=' + item.Name1 + ' data-name2=' + item.Name2 + '>' + item.Display + '</a>'
                $('#dropdown-database').append(str);
            }
        },
        failure: function (response)
        {
            alert(response)
        },
        error: function (response)
        {
            alert(response.responseText)
        }
    });
}
function ConfirmChangeFile (Target, name1, name2)
{
    var r = document.cookie.replace(/(?:(?:^|.*;\s*)PlatxeLan\s*\=\s*([^;]*).*$)|^.*$/, "$1");
    var display = '';
    display = name2;
    if (r == 'ar')
    {
        display = name1;
    }
    $.confirm({
        title: '<small class="f-s-11 f-w-600 text-dark">' + 'Change Company File' + '</small>',
        content: 'Are you want to change file to: ' + '<b class="text-red">' + display + '</b> ',
        buttons: {
            confirm: {
                text: 'Confirm',
                action: function ()
                {
                    $.ajax({
                        type: "Post",
                        url: "/api/apiaccount/ChangeFile",
                        dataType: "json",
                        data: { Target: Target },
                        success: function (response)
                        {
                            window.location.reload();
                        },
                        failure: function (response)
                        {
                            alert(response)
                        },
                        error: function (response)
                        {
                            alert(response.responseText)
                        }
                    });
                }
            },
            cancel: {
                text: 'Cancel',
                action: function () { return; }
            }
        }
    });
}
function ConfirmChangeYear (Target)
{
    $.confirm({
        title: '<small class="f-s-11 f-w-600 text-dark">' + 'Change Financial Year' + '</small>',
        content: 'Are you want to change year to: ' + '<b class="text-red">' + Target + '</b> ',
        buttons: {
            confirm: {
                text: 'Confirm',
                action: function ()
                {
                    $.ajax({
                        type: "Post",
                        url: "/api/apiaccount/ChangeYear",
                        dataType: "json",
                        data: { Year: Target },
                        success: function (response)
                        {
                            window.location.reload();
                        },
                        failure: function (response)
                        {
                            alert(response)
                        },
                        error: function (response)
                        {
                            alert(response.responseText)
                        }
                    });
                }
            },
            cancel: {
                text: 'Cancel',
                action: function () { return; }
            }
        }
    });
}
function GetFinancialYear ()
{
    $.ajax({
        type: "Get",
        url: "/api/icore/Years",
        dataType: "json",
        success: function (response)
        {
            $('#dropdown-year').empty();
            for (const item of response)
            {
                var str = '';
                str = '<a href="javascript:;" class="dropdown-item btn-year" data-key=' + item.No + ' data-status="' + item.Closed + '">' + item.No + ClosedYearTxt(item.Closed) + '</a>';
                $('#dropdown-year').append(str);
                if (item.No == $('#dropdown-year').data('key'))
                {
                    if (item.Closed == true)
                    {
                        $('#txt-YearStatus').prop('hidden', false);
                    }
                }
            }
        },
        failure: function (response)
        {
            alert(response)
        },
        error: function (response)
        {
            alert(response.responseText)
        }
    });
}
function ClosedYearTxt (Status)
{
    if (Status == true)
        return '<small class="text-danger mr-2 ml-2"> Closed</small>';
    else
        return '';
}
function comma (n)
{
    var x;
    if (n % 1 == 0)
    {
        x = Number(n).toFixed(0);
    } else
    {
        x = Number(n).toFixed(2);
    }
    return x.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
function comma4 (n)
{
    var n = Number(n).toFixed(4);
    return n.replace(/\B(?=(\d{3})+(?!\d))/g, ",");;
}
function comma3 (n)
{
    var n = Number(n).toFixed(3);
    return n.replace(/\B(?=(\d{3})+(?!\d))/g, ",");;
}
function exDocumentError (data)
{
    $.alert({
        title: 'Error',
        icon: 'fa fa-warning',
        type: 'red',
        content: '<b>There is some thing <span class="text-red">error</span>!</b> <p class="m-1 p-1"><small>' + data.Message + '<small></p>',
        buttons: {
            ok: {}
        }
    });
}
function exErrorMessage (Message)
{
    $.alert({
        title: 'Error',
        icon: 'fa fa-warning',
        type: 'red',
        content: '<b>There is some thing <span class="text-red">error</span>!</b> <p class="m-1 p-1"><small>' + Message + '<small></p>',
        buttons: {
            ok: {}
        }
    });
}
function GeneratGuid ()
{
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c)
    {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
function ViewFinancialTransaction (Key)
{
    $('#con-FinTransaction').empty();
    $("#con-FinTransaction").load('/Documents/FinancialTransaction?Key=' + Key);
    $("#mod-FinTransaction").modal({ backdrop: 'static' }).modal('toggle');
}
function ViewJournalVoucher (Key)
{
    $('#con-FinTransaction').empty();
    $("#con-FinTransaction").load('/Documents/JournalVoucher?Key=' + Key);
    $("#mod-FinTransaction").modal({ backdrop: 'static' }).modal('toggle');
}
function ViewQuotation (Key)
{
    $('#con-Transaction').empty();
    $("#con-Transaction").load('/Documents/Quotation?Key=' + Key);
    $("#mod-Transaction").modal({ backdrop: 'static' }).modal('toggle');
}
function ViewSalesInvoices (Key)
{
    $('#con-Invoice').empty();
    $("#con-Invoice").load('/Documents/SalesInvoices?Key=' + Key);
    $("#mod-Invoice").modal({ backdrop: 'static' }).modal('toggle');
}
function ViewPurchaseInvoice (Key)
{
    $('#con-Invoice').empty();
    $("#con-Invoice").load('/Documents/PurchaseInvoice?Key=' + Key);
    $("#mod-Invoice").modal({ backdrop: 'static' }).modal('toggle');
}
function ViewDocumentNote (Key)
{
    $('#con-DocumentNote').empty();
    $("#con-DocumentNote").load('/Documents/DocumentNote?Key=' + Key);
    $("#mod-DocumentNote").modal({ backdrop: 'static' }).modal('toggle');
}
function ViewGeneralLedger (Key)
{
    if (Key == '' || Key == null)
        return;

    $('#con-GL').empty();
    $("#con-GL").load('/Documents/GeneralLedger?Key=' + Key);
    $("#mod-GL").modal({ backdrop: 'static' }).modal('toggle');
}
function UpdateFinancialTransaction (Key, Status)
{
    $.ajax({
        type: "post",
        url: "/api/icore/UpdateTransactionStatus",
        data: { Key: Key, Status: Status },
        dataType: "json",
        success: function (response)
        {
            switch (Status)
            {
                case 0:
                    $('#td-Status').empty();
                    $('#td-Status').append('<span class="badge bg-yellow text-dark badg-plx">Pending</span>');
                    break;
                case 3:
                    $('#td-Status').empty();
                    $('#td-Status').append('<span class="badge bg-yellow text-dark badg-plx">Reaudit</span>');
                    break;
                case 2:
                    $('#td-Status').empty();
                    $('#td-Status').append('<span class="badge bg-yellow text-dark badg-plx">Confirmed</span>');
                    break;
            }
        },
        failure: function (response)
        {
            alert(response)
        },
        error: function (response)
        {
            alert(response.responseText)
        }
    });
}
function UpdateQuotationStatus (Key, Status)
{
    $.ajax({
        type: "post",
        url: "/api/icore/UpdateQuotationStatus",
        data: { Key: Key, Status: Status },
        dataType: "json",
        success: function (response)
        {
            switch (Status)
            {
                case 0:
                    $('#td-Status').empty();
                    $('#td-Status').append('<span class="badge bg-yellow text-dark badg-plx">Pending</span>');
                    break;
                case 1:
                    $('#td-Status').empty();
                    $('#td-Status').append('<span class="badge bg-yellow text-dark badg-plx">Confirmed</span>');
                    break;
                case 2:
                    $('#td-Status').empty();
                    $('#td-Status').append('<span class="badge bg-yellow text-dark badg-plx">Without Advance</span>');
                    break;
                case 3:
                    $('#td-Status').empty();
                    $('#td-Status').append('<span class="badge bg-yellow text-dark badg-plx">Re-Audit</span>');
                    break;
            }
        },
        failure: function (response)
        {
            alert(response)
        },
        error: function (response)
        {
            alert(response.responseText)
        }
    });
}

function FinancialPrintTemplates (DocKind, Key)
{
    $('#con-PrintTemplates').empty();
    $("#con-PrintTemplates").load('/Documents/PrintTemplates?DocKind=' + DocKind + '&Key=' + Key);
    $("#mod-PrintTemplates").modal({ backdrop: 'static' }).modal('toggle');
}
function SalesInvoicePrintTemplates (DocKind, Key)
{
    $('#con-PrintTemplates').empty();
    $("#con-PrintTemplates").load('/Documents/PrintTemplateSales?DocKind=' + DocKind + '&Key=' + Key);
    $("#mod-PrintTemplates").modal({ backdrop: 'static' }).modal('toggle');
}
function ProformaInvoicePrintTemplates (DocKind, Key)
{
    $('#con-PrintTemplates').empty();
    $("#con-PrintTemplates").load('/Documents/PrintTemplateProforma?DocKind=' + DocKind + '&Key=' + Key);
    $("#mod-PrintTemplates").modal({ backdrop: 'static' }).modal('toggle');
}
function ConfirmLogout ()
{
    $.confirm({
        title: 'Confirm',
        content: 'Are you sure to signout?',
        type: 'red',
        buttons: {
            Logout: {
                text: 'Logout',
                btnClass: 'btn-red',
                action: function ()
                {
                    window.location.replace('/Account/Logout');
                }
            },
            Cancel: {
                text: 'Cancel',
                action: function ()
                {
                    return;
                }
            }
        }
    });
}
function DeleteFinancialTransaction (Key)
{
    $.confirm({
        title: 'Confirm',
        content: 'Are you sure to delete?',
        type: 'red',
        buttons: {
            Delete: {
                text: 'Delete',
                btnClass: 'btn-red',
                action: function ()
                {
                    $.ajax({
                        type: "Post",
                        url: "/api/icore/DeleteFinancialTransaction",
                        data: { Key: Key },
                        dataType: "json",
                        success: function (response)
                        {
                            $("#mod-FinTransaction").modal({ backdrop: 'static' }).modal('toggle');
                        },
                        failure: function (response)
                        {
                            alert(response);
                        },
                        error: function (response)
                        {
                            alert(response.responseText);
                        }
                    });
                }
            },
            Cancel: {
                text: 'Cancel',
                action: function ()
                {
                    return;
                }
            }
        }
    });
}
function DeleteInventoryTransaction (Key)
{
    $.confirm({
        title: 'Confirm',
        content: 'Are you sure to delete?',
        type: 'red',
        buttons: {
            Delete: {
                text: 'Delete',
                btnClass: 'btn-red',
                action: function ()
                {
                    $.ajax({
                        type: "Post",
                        url: "/api/icore/DeleteInventoryTransaction",
                        data: { Key: Key },
                        dataType: "json",
                        success: function (response)
                        {
                            $("#mod-Invoice").modal({ backdrop: 'static' }).modal('toggle');
                        },
                        failure: function (response)
                        {
                            alert(response);
                        },
                        error: function (response)
                        {
                            alert(response.responseText);
                        }
                    });
                }
            },
            Cancel: {
                text: 'Cancel',
                action: function ()
                {
                    return;
                }
            }
        }
    });
}

function AccountBalance (id,Key, Date, Kind, Type)
{
    $.ajax({
        type: "get",
        url: "/api/icore/GetAccountBalance",
        data: { Key: Key, Date: Date, Kind: Kind, Type: Type },
        dataType: "json",
        success: function (response)
        {
            $('#' + id).text(comma(response));
        },
        failure: function (response)
        {
            alert(response);
        },
        error: function (response)
        {
            alert(response.responseText);
        }
    });
}
 

//Devextrem Template
function ctBold (element, info)
{
    element.append('<div>' + info.text + '</div>')
        .css('font-weight', '600');
}
function ctBold2x (element, info)
{
    element.append('<div>' + info.text + '</div>')
        .css('font-weight', '500');
}
function ctRedBold (element, info)
{
    element.append('<div>' + info.text + '</div>')
        .css('color', '#F6331A')
        .css('font-weight', '600');
}
function ctRed (element, info)
{
    element.append('<div>' + info.text + '</div>')
        .css('color', '#F6331A');
}
function ctCreditRed (element, info)
{
    if (info.value < 0)
    {
        element.append('<div>' + info.text + '</div>')
            .css('color', '#F6331A');
    }
}
function ctWordWrap (container, options)
{
    var str = '';
    str = '<p class="m-0 p-0 f-s-10" style="word-break:break-all;white-space:normal;">' + options.text + '</p>';
    $(str).appendTo(container)
}
