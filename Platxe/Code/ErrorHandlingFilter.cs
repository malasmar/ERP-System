using Microsoft.AspNetCore.Mvc.Filters;
namespace Platxe.Code
{
    public class ErrorHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            //log your exception here
            string xm = context.Exception.Message;
            // iNCore.xCore.SendEmail(exception);
            context.ExceptionHandled = true; //optional 
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            str.AppendLine(exception.Source);
            str.AppendLine(exception.Message);
            str.AppendLine();
            str.AppendLine(" ****************************** ");
            str.AppendLine(exception.StackTrace);
            System.IO.File.WriteAllText($"D:\\Platx Log\\" + System.DateTime.UtcNow.ToString("dd-MM-yyyy HH-mm") + " " + exception.Source + ".txt", str.ToString());
        }
    }
}
