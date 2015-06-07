using FinanceMonitor.Data;
using FinanceMonitor.Library.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FinanceMonitor.Library.Connections
{
    public class AllyBankWebRequests
    {
        public decimal GetAllyBankSavingsBalance(DatabaseLoginCredentials creds)
        {
            //return SavingsBalance(new AllyBankLoginCredentials() { userid = creds.userid, password = creds.password });
            return 0;
        }

        private decimal SavingsBalance(AllyBankLoginCredentials creds)
        {

            //Start to do calls
            HttpWebResponse resp = WebRequestHelper.GET("https://secure.ally.com/allyWebClient/login.do", true);

            string responseText = new StreamReader(resp.GetResponseStream()).ReadToEnd();
            string key = Helper.RetrieveResponseValueFromPartialText(responseText, "<form name=\"actionForm\" method=\"post\" action=\"/allyWebClient/login.do\" id=\"noautocomplete\" autocomplete=\"off\"><input type=\"hidden\" name=\"", "\"");
            string value = Helper.RetrieveResponseValueFromPartialText(responseText, key + "\" value=\"", "\"");
            string flowExecutionKey = Helper.RetrieveResponseValueFromPartialText(responseText, "_flowExecutionKey\" value=\"", "\">");
            string _pm_fp = "mozilla%2F5.0+%28windows+nt+6.1%3B+wow64%29+applewebkit%2F537.36+%28khtml%2C+like+gecko%29+chrome%2F34.0.1847.137+safari%2F537.36%7C5.0+%28Windows+NT+6.1%3B+WOW64%29+AppleWebKit%2F537.36+%28KHTML%2C+like+Gecko%29+Chrome%2F34.0.1847.137+Safari%2F537.36%7CWin3224%7C1280%7C1024%7C984widevinecdmadapter.dll%7Cpepflashplayer.dll%7Cinternal-remoting-viewer%7CppGoogleNaClPluginChrome.dll%7Cpdf.dll%7CnpdeployJava1.dll%7CNPAUTHZ.DLL%7CNPSPWRAP.DLL%7CnpGoogleUpdate3.dll%7Cnpjp2.dll%7CNPMcFFPlg.dll%7Cnpmeetingjoinpluginoc.dll%7Cnpappdetector.dll%7Cnp32dsw.dll%7CnpWatWeb.dll%7Cnpctrl.dll";

            string replacementKey = key + "=" + value;
            
            HttpWebResponse resp2 = WebRequestHelper.POST("https://secure.ally.com/allyWebClient/login.do", new EnterUsername()
            {
                _replacementKey = replacementKey,
                _flowExecutionKey = flowExecutionKey,
                fp_browser = string.Empty,
                fp_screen = string.Empty,
                _eventId = "submit",
                fp_software = string.Empty,
                pm_fp = _pm_fp,
                enrollValue = string.Empty,
                userNamePvtEncrypt = creds.userid,
                pageName = "AllyBank%3ALogin%3AMy+Account%3ACustomer+Username"
            }, false, resp.Cookies);

            HttpWebResponse resp3 = WebRequestHelper.POST("https://secure.ally.com/allyWebClient/login.do", new EnterPassword()
                {
                    _replacementKey = replacementKey,
                    _flowExecutionKey = flowExecutionKey,
                    _eventId_submit = string.Empty,
                    userPrefs = "TF1%3B015%3B%3B%3B%3B%3B%3B%3B%3B%3B%3B%3B%3B%3B%3B%3B%3B%3B%3B%3B%3B%3B%3BMozilla%3BNetscape%3B5.0%2520%2528Windows%2520NT%25206.1%253B%2520WOW64%2529%2520AppleWebKit%2F537.36%2520%2528KHTML%252C%2520like%2520Gecko%2529%2520Chrome%2F34.0.1847.137%2520Safari%2F537.36%3B20030107%3Bundefined%3Btrue%3B%3Btrue%3BWin32%3Bundefined%3BMozilla%2F5.0%2520%2528Windows%2520NT%25206.1%253B%2520WOW64%2529%2520AppleWebKit%2F537.36%2520%2528KHTML%252C%2520like%2520Gecko%2529%2520Chrome%2F34.0.1847.137%2520Safari%2F537.36%3Ben-US%3BISO-8859-1%3Bsecure.ally.com%3Bundefined%3Bundefined%3Bundefined%3Bundefined%3Btrue%3Btrue%3B1400606124441%3B-5%3B6%2F7%2F2005%25209%253A33%253A44%2520PM%3B1280%3B1024%3B%3B13.0%3B%3B1.6.0_29%3B11.5.9.620%3B2013%3B43%3B300%3B240%3B5%2F20%2F2014%25201%253A15%253A24%2520PM%3B24%3B1280%3B984%3B0%3B0%3B%3B%3B%3B%3BShockwave%2520for%2520Director%257CAdobe%2520Shockwave%2520for%2520Director%2520Netscape%2520plug-in%252C%2520version%252011.5.9.620%3BShockwave%2520Flash%257CShockwave%2520Flash%252013.0%2520r0%3B%3B%3B%3B%3B%3B%3B%3B%3BSilverlight%2520Plug-In%257C5.1.20513.0%3B%3B%3B%3B12%3B",
                    pm_fp = _pm_fp,
                    passwordPvtBlock = creds.password,
                    pageName = "AllyBank%3ALogin%3AMy+Account%3ACustomer+Password"

                }, true, resp.Cookies, resp2.Cookies);

            string responseText2 = new StreamReader(resp3.GetResponseStream()).ReadToEnd();



            return 0;
        }

        private class EnterUsername
        {
            public string _replacementKey { get; set; }
            public string _flowExecutionKey { get; set; }
            public string fp_browser { get; set; }
            public string fp_screen { get; set; }
            public string _eventId { get; set; }
            public string fp_software { get; set; }
            public string pm_fp { get; set; }
            public string enrollValue { get; set; }
            public string userNamePvtEncrypt { get; set; }
            public string pageName { get; set; }
        }

        private class EnterPassword
        {
            public string _replacementKey { get; set; }
            public string _flowExecutionKey { get; set; }
            public string _eventId_submit { get; set; }
            public string userPrefs { get; set; }
            public string pm_fp { get; set; }
            public string passwordPvtBlock { get; set; }
            public string pageName { get; set; }
        }

        #region Internal Classes

        internal class AllyBankLoginCredentials : LoginCredentials
        {
            public string userid { get; set; }
            public string password { get; set; }
        }

        #endregion
    }
}
