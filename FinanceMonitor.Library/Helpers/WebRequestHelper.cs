using System.IO;
using System.Net;

namespace FinanceMonitor.Library.Helpers
{
    public class WebRequestHelper
    {
        private static object PostMessage { get; set; }

        public static HttpWebResponse GET(string url, bool redirect)
        {
            return BuildGenericRequest(url, HTTPMethod.GET, redirect);
        }

        public static HttpWebResponse GET(string url, bool redirect, params CookieCollection[] cookies)
        {
            return BuildGenericRequest(url, HTTPMethod.GET, redirect, PopulateCookieContainer(cookies));
        }

        public static HttpWebResponse POST(string url, object postMessage, bool redirect)
        {
            PostMessage = postMessage;
            return BuildGenericRequest(url, HTTPMethod.POST, redirect);
        }

        public static HttpWebResponse POST(string url, object postMessage, bool redirect, params CookieCollection[] cookies)
        {
            PostMessage = postMessage;
            return BuildGenericRequest(url, HTTPMethod.POST, redirect, PopulateCookieContainer(cookies));
        }

        #region Private Methods

        private static HttpWebResponse BuildGenericRequest(string url, string httpMethod, bool redirect, CookieContainer cc = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = httpMethod;
            request.ContentType = MIMETypes.FormEncoded;
            request.AllowAutoRedirect = redirect;
            request = BuildRequestHeader(request);
            request.CookieContainer = cc == null ? new CookieContainer() : cc;

            if (request.Method == HTTPMethod.POST)
            {
                using (var r =new StreamWriter(request.GetRequestStream()))
                    r.Write(Helper.Build_NVP_POST_Message(PostMessage));
            }

            return (HttpWebResponse)request.GetResponse();
        }

        private static HttpWebRequest BuildRequestHeader(HttpWebRequest request)
        {
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.116 Safari/537.36";
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
            return request;
        }

        private static CookieContainer PopulateCookieContainer(params CookieCollection[] cookies)
        {
            CookieContainer cc = new CookieContainer();
            foreach (var cookie in cookies)
                cc.Add(cookie);

            return cc;
        }

        #endregion

        #region Private Sealed Classes
        
        private sealed class HTTPMethod
        {
            internal const string GET = "GET";
            internal const string POST = "POST";
        }

        private sealed class MIMETypes
        {
            internal const string FormEncoded = "application/x-www-form-urlencoded";
            internal const string utf8TextXML = "text/xml; charset=utf-8";
        }

        #endregion
    }
}
