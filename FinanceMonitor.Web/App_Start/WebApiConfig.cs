using System.Net.Http.Headers;
using System.Web.Http;

namespace FinanceMonitor.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{value}",
                defaults: new { value = RouteParameter.Optional }
            );
            config.Filters.Add(new ExceptionHandling());
        }
    }
}