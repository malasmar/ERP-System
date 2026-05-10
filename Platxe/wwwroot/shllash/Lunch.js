$(document).ready(function ()
{
    var r = document.cookie.replace(/(?:(?:^|.*;\s*)PlatxeLan\s*\=\s*([^;]*).*$)|^.*$/, "$1");
    if (r == "ar")
    {
        $("#xhtml").attr("dir", "rtl");
        $("html").attr("dir", "rtl");
        $("body").addClass("rtl-mode");
        $("body").toggleClass("dx-rtl", true);
        DevExpress.config({
            rtlEnabled: true
        });
        // DevExpress.localization.loadMessages('ar');
        LoadArabicPlatx();
    } else
    {
        LoadEnglishPlatx();
    }
});