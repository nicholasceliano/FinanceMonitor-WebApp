using System.IO;
using System.Net;
using FinanceMonitor.Config;
using FinanceMonitor.Data;
using FinanceMonitor.Library.Helpers;

namespace FinanceMonitor.Library.Connections
{
    public class HessBenefitsWebRequests
    {
        #region Private Variables

        private const string returnValue_ElementType = "td";
        private const string returnValue_ElementID = "tableTextRight";
        private static string hbcBaseURI = AccountConfiguration.Current.HessBenefitsCenter.BaseURI;

        #endregion
        
        public decimal Get_401k_Total(DatabaseLoginCredentials cred)
        {
            return RetrieveValueFromHessBenefitResponse(new HessBenefitLoginCredentials() { userid = cred.userid, password = cred.password });
        }

        private HttpWebResponse LogInAndEstablishSession(HessBenefitLoginCredentials credentials) 
        {
            return WebRequestHelper.POST(hbcBaseURI + "login.jsp", new HessBenefitsCookieInfo()
                {
                    company = AccountConfiguration.Current.HessBenefitsCenter.CompanyCode,
                    PIN_FORM_REQUEST = "N",
                    PIN_FORM_AUTHORIZATION_REQUEST = "N",
                    AVM_IND = "N",
                    NAVID = string.Empty,
                    userid = credentials.userid,
                    password = credentials.password
                }, false);
        }

        private HttpWebResponse HomePageResponse(HessBenefitLoginCredentials credentials)
        {
            return WebRequestHelper.GET(hbcBaseURI + "nav/plansHome.ajax.jsp", false, LogInAndEstablishSession(credentials).Cookies);
        }

        private decimal RetrieveValueFromHessBenefitResponse(HessBenefitLoginCredentials credentials)
        {
            return Helper.ConvertMoneyStringToDecimal(Helper.RetrieveElementValue(HomePageResponse(credentials), returnValue_ElementType, returnValue_ElementID)); ;
        }

        #region Internally Used Classes

        private class HessBenefitsCookieInfo
        {
            public int company { get; set; }
            public string PIN_FORM_REQUEST { get; set; }
            public string PIN_FORM_AUTHORIZATION_REQUEST { get; set; }
            public string AVM_IND { get; set; }
            public string NAVID { get; set; }
            public string userid { get; set; }
            public string password { get; set; }
        }

        private class HessBenefitLoginCredentials : LoginCredentials
        {
            public string userid { get; set; }
            public string password { get; set; }
        }

        #endregion
    }
}
