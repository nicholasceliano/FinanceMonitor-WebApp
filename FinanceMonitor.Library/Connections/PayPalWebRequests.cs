using System.Net;
using FinanceMonitor.Config;
using FinanceMonitor.Library.Helpers;

namespace FinanceMonitor.Library.Connections
{
    public class PayPalWebRequests
    {
        public decimal GetPersonalPayPalBalance()
        {
            return GetPersonalBalance();
        }

        private HttpWebResponse LogInAndEstablishSession()
        {
            return WebRequestHelper.POST(AccountConfiguration.Current.PayPal.URL, new PaypalNVPMessage
            {
                METHOD = "GetBalance",
                VERSION = AccountConfiguration.Current.PayPal.VERSION,
                USER = AccountConfiguration.Current.PayPal.USER,
                PWD = AccountConfiguration.Current.PayPal.PWD,
                SIGNATURE = AccountConfiguration.Current.PayPal.SIGNATURE
            }, false);
        }

        private decimal GetPersonalBalance()
        {
            return decimal.Parse(WebUtility.UrlDecode(Helper.RetrieveResponseValueFromPartialText(LogInAndEstablishSession(), "L_AMT0=", "&")));
        }

        internal class PaypalNVPMessage
        {
            public string METHOD { get; set; }
            public string USER { get; set; }
            public string VERSION { get; set; }
            public string PWD { get; set; }
            public string SIGNATURE { get; set; }
        }
    }
}
