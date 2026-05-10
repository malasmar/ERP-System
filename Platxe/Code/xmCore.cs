using RestSharp;
using SelectPdf;
using System.Net.Mail;
using System.Net;
using System.Drawing;

namespace Platxe
{
    public class AccountStatmentData
    {
        public List<CLiFinancial.Reports.AccountStatment.CurrentAccount> Statment { get; set; }
        public CLiCore.Print.CurrentAccountDetails Account { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        public string DB { get; set; }
        public string xLan { get; set; }
    }
    public class QuotationData
    {
        public CLiSales.Documents.Quotation Transaction { get; set; }
        public List<CLiSales.Documents.QuotationDetails> Details { get; set; }
        public CLiCore.Print.CurrentAccountDetails CurrentAccount { get; set; }
        public string DB { get; set; }
        public string xLan { get; set; }
      
    }
    public class InvoiceData
    {
        public CLiInventory.Documents.Transaction Transaction { get; set; }
        public List<CLiInventory.Documents.TransactionDetails> Details { get; set; }
        public CLiCore.Print.CurrentAccountDetails CurrentAccount { get; set; }
        public string DB { get; set; }
        public string xLan { get; set; }
        public string QR { get; set; }
    }
    public class xmCore
    {
        public async Task<bool> SendInvoice()
        {
            var url = "https://api.ultramsg.com/{INSTANCE_ID}/messages/document";
            var client = new RestClient(url);

            var request = new RestRequest(url, Method.Post);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("token", "{TOKEN}");
            request.AddParameter("to", "+966501488719");
            request.AddParameter("filename", "hello.pdf");
            request.AddParameter("document", "https://file-example.s3-accelerate.amazonaws.com/documents/cv.pdf");
            request.AddParameter("caption", "document caption");


            RestResponse response = await client.ExecuteAsync(request);
            return true;
        }
        public void SendDocumentEmail(string url, string FileName, Guid? Key)
        {
            if (Key == null)
                return;

            //CLiData.iSales.Quotation quotation = new CLiData.iSales.Quotation().GetItem(Key);
            CLiCore.Platx.PlatxeMails sender = new CLiCore.Platx.PlatxeMails().GetItem("no-reply");
            if (sender.Email == "")
            {
                return;
            }
            if (CLiCore.iCore.IsValidEmail("muhammad.shllash@gmail.com") == false)
            {
                return;
            }


            string result;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        result = content.ReadAsStringAsync().Result;
                    }
                }
            }

            Thread T1 = new Thread(delegate ()
            {


                using (var smtp = new SmtpClient())
                {
                    smtp.UseDefaultCredentials = sender.UseDefaultCredentials;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Port = sender.Port;
                    smtp.EnableSsl = sender.EnableSSL;

                    var message = new MailMessage();
                    message.To.Add(new MailAddress("muhammad.shllash@gmail.com"));
                    message.From = new MailAddress(sender.Email, "Tax Invoice");  // replace with valid value
                    message.Subject = FileName;
                    message.Body = result.ToString();
                    message.IsBodyHtml = sender.IsBodyHtml;
                    message.Attachments.Add(new Attachment(SavePDF(url), FileName + ".pdf", "application/pdf"));
                    message.SubjectEncoding = System.Text.Encoding.GetEncoding("utf-8");
                    message.Priority = MailPriority.High;
                    message.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                    var credential = new NetworkCredential
                    {
                        UserName = sender.UserName,  // replace with valid value
                        Password = sender.Password  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = sender.SMTP;

                    smtp.Send(message);
                }
            });
            T1.Start();
        }

        private Stream SavePDF(string url)
        {

            string pdf_page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                pdf_page_size, true);

            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation =
                (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation, true);

            int webPageWidth = 1024;
            int webPageHeight = 1450;

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.MarginBottom = 5;
            converter.Options.MarginLeft = 5;
            converter.Options.MarginRight = 5;
            converter.Options.MarginTop = 5;
            converter.Options.KeepImagesTogether = true;
            converter.Options.KeepTextsTogether = true;
            converter.Options.EmbedFonts = true;
            converter.Options.DrawBackground = true;
          
            //PdfTextSection sec = new PdfTextSection(0, 50, "Page: {page_number} of {total_pages}.", new System.Drawing.Font("") );
            //sec.ForeColor = System.Drawing.Color.Blue;
            //converter.Footer.Add(sec);

            //converter.Options.WebPageWidth = webPageWidth;
            //converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertUrl(url);

          

            MemoryStream ms = new MemoryStream();
            doc.Save(ms);
            ms.Position = 0;
            doc.Close();
            return ms;
        }

        private void SaveTempPDF(string url,string FileName)
        {

            string pdf_page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                pdf_page_size, true);

            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation =
                (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation, true);

            int webPageWidth = 1024;
            int webPageHeight = 1450;

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.MarginBottom = 5;
            converter.Options.MarginLeft = 5;
            converter.Options.MarginRight = 5;
            converter.Options.MarginTop = 5;
            converter.Options.KeepImagesTogether = true;
            converter.Options.KeepTextsTogether = true;
            converter.Options.EmbedFonts = true;
            converter.Options.DrawBackground = true;

            //converter.Options.WebPageWidth = webPageWidth;
            //converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertUrl(url);
           
            doc.Save(FileName);
           
            doc.Close();
        
        }



        public async Task<bool> SendWhatsapp(string DB, string to,string Message)
        {
            CLiCore.Platx.Whatsapp whatsapp = new CLiCore.Platx.Whatsapp().GetItem(DB);
            if (whatsapp == null || whatsapp.APIURL == "" || whatsapp.Status == false)
                return false;

            var url = "https://api.ultramsg.com/" + whatsapp.InstanceID + "/messages/chat";
            var client = new RestClient(url);

            var request = new RestRequest(url, Method.Post);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("token", whatsapp.Token);
            request.AddParameter("to", to);
            request.AddParameter("body", Message);


            RestResponse response = await client.ExecuteAsync(request);
            var output = response.Content;
            return true;
        }
        public async Task<bool> SendWhatsappFile(string DB, string to,string FileName,string DocumentURL,string Caption)
        {
            CLiCore.Platx.Whatsapp whatsapp = new CLiCore.Platx.Whatsapp().GetItem(DB);

            if (whatsapp == null || whatsapp.APIURL == "" || whatsapp.Status == false)
                return false;

            var url = "https://api.ultramsg.com/" + whatsapp.InstanceID + "/messages/document";
            var client = new RestClient(url);

            var request = new RestRequest(url, Method.Post);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("token", whatsapp.Token);
            request.AddParameter("to", to);
            request.AddParameter("filename", FileName);
            request.AddParameter("document", DocumentURL);
            request.AddParameter("caption", Caption);


            RestResponse response = await client.ExecuteAsync(request);
            var output = response.Content;
            return true;
        }



        public void SendWhatsappMesege(string Mobile, string Message)
        {
            Thread T1 = new Thread(async delegate ()
            {


                CLiCore.PL.Whatsapp whatsapp = new CLiCore.PL.Whatsapp().GetItem();
                if (whatsapp == null || whatsapp.APIURL == "" || whatsapp.Status == false)
                    return;

                try
                {
                    var phoneUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
                    var phoneNumber = phoneUtil.Parse(Mobile, "SA");
                    var internationalNumber = phoneUtil.Format(phoneNumber, PhoneNumbers.PhoneNumberFormat.INTERNATIONAL);
                    string mob = internationalNumber.Replace(" ", "");
                    var url = "https://api.ultramsg.com/" + whatsapp.InstanceID + "/messages/chat";
                    var client = new RestClient(url);

                    var request = new RestRequest(url, Method.Post);
                    request.AddHeader("content-type", "application/x-www-form-urlencoded");
                    request.AddParameter("token", whatsapp.Token);
                    request.AddParameter("to", mob);
                    request.AddParameter("body", Message);


                    RestResponse response = await client.ExecuteAsync(request);
                    var output = response.Content;
                    return;
                }
                catch (Exception ex)
                {
                    return;
                }


            });
            T1.Start();


        }
    }
}
