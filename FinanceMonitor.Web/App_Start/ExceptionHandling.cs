using System.Web.Http.Filters;
using FinanceMonitor.Config;
using FinanceMonitor.Library;

namespace FinanceMonitor.Web
{
    public class ExceptionHandling : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            //Log Critical errors
            //TODO: Log Errors in database

            //SendEmailToAdmin(context);
        }

        private void SendEmailToAdmin(HttpActionExecutedContext context)
        {
            string message = "Exception: " + context.Exception.Message + "\\n Request URI: " + context.Request.RequestUri;
            new Email(AppConfiguration.Current.AdminEmail, Email.EmailType.NoHTML, "Application Error", message).SendEmail();
        }
    }
}