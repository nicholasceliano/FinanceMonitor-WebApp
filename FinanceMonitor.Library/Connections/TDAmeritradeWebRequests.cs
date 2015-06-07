using System;
using System.IO;
using System.Net;
using FinanceMonitor.Config;
using FinanceMonitor.Data;
using FinanceMonitor.Library.Helpers;

namespace FinanceMonitor.Library.Connections
{
    public class TDAmeritradeWebRequests
    {
        #region Private Variables
        
        private static string baseURI = AccountConfiguration.Current.TDAmeritrade.BaseURI;

        #endregion

        public decimal GetPersonalTDAmeritradeBalance(DatabaseLoginCredentials cred)
        {
            return GetBalance(new TDAmeritradeLoginCredentials() { userid = cred.userid, password = cred.password });
        }

        private HttpWebResponse LoginCallResp(TDAmeritradeLoginCredentials creds)
        {
            return WebRequestHelper.POST(baseURI + "grid/p/login", new LoginCall
            {
                DV_DATA = string.Empty,
                fp_browser = string.Empty,
                fp_screen = string.Empty,
                fp_software = string.Empty,
                fp_timezone = string.Empty,
                fp_language = string.Empty,
                fp_java = 1,
                fp_cookie = 1,
                flashVersion = "13.0.0",
                AgentID = string.Empty,
                mAction = "submit",
                tbUsername = creds.userid,
                tbPassword = creds.password,
                ldl = "main%3Ahome"

            }, false);
        }

        private HttpWebResponse SecurityQuestionCallResp(HttpWebResponse loginCallResponse)
        {
            string respText = new StreamReader(loginCallResponse.GetResponseStream()).ReadToEnd();

            return WebRequestHelper.POST(baseURI + "grid/m/securityChallenge", new SecurityQuestionCall
            {
                challengeAnswer = AnswerSecurityQuestion(Helper.RetrieveResponseValueFromPartialText(respText, "Question:</span>", "<")),
                mAction = "submit",
                moduleId = "securityChallenge-1",
                transactionToken = Helper.RetrieveResponseValueFromPartialText(respText, "transactionToken:'", "'"),
                xhrToken = true,
                dojo___preventCache = string.Empty
            }, false, loginCallResponse.Cookies);
        }

        private HttpWebResponse BalanceAndPositionsCallResp(TDAmeritradeLoginCredentials creds)
        {
            return WebRequestHelper.GET(baseURI + "cgi-bin/apps/u/BalancesAndPositions", false, LogInAndEstablishSession(creds));
        }

        private CookieCollection LogInAndEstablishSession(TDAmeritradeLoginCredentials creds)
        {
            var loginResponse = LoginCallResp(creds);
            var cc = new CookieCollection();
            cc.Add(loginResponse.Cookies);
            cc.Add(SecurityQuestionCallResp(loginResponse).Cookies);
            return cc;
        }

        private string AnswerSecurityQuestion(string questionText)
        {
            if (questionText == AccountConfiguration.Current.TDAmeritrade.Question1)
                return AccountConfiguration.Current.TDAmeritrade.Answer1;
            else if (questionText == AccountConfiguration.Current.TDAmeritrade.Question2)
                return AccountConfiguration.Current.TDAmeritrade.Answer2;
            else if (questionText == AccountConfiguration.Current.TDAmeritrade.Question3)
                return AccountConfiguration.Current.TDAmeritrade.Answer3;
            else if (questionText == AccountConfiguration.Current.TDAmeritrade.Question4)
                return AccountConfiguration.Current.TDAmeritrade.Answer4;
            else 
                return null;
        }

        private decimal GetBalance(TDAmeritradeLoginCredentials credentials)
        {
            return decimal.Parse(Helper.RetrieveResponseValueFromPartialText(BalanceAndPositionsCallResp(credentials), "<td><b>$", "<"));
        }

        #region Internal Classes

        internal class TDAmeritradeLoginCredentials : LoginCredentials
        {
            public string userid { get; set; }
            public string password { get; set; }
        }

        internal class SecurityQuestionCall
        {
            public string challengeAnswer { get; set; }
            public string rememberDevice { get; set; }
            public string mAction { get; set; }
            public string moduleId { get; set; }
            public string transactionToken { get; set; }
            public bool xhrToken { get; set; }
            public string dojo___preventCache { get; set; }//'___' replaces '.' for property name
        }                                                 //Gets converted back in HTTPHelper.Build_NVP_POST_Message

        internal class LoginCall
        {
            public string DV_DATA { get; set; }
            public string fp_browser { get; set; }
            public string fp_screen { get; set; }
            public string fp_software { get; set; }
            public string fp_timezone { get; set; }
            public string fp_language { get; set; }
            public int fp_java { get; set; }
            public int fp_cookie { get; set; }
            public string flashVersion { get; set; }
            public string AgentID { get; set; }
            public string mAction { get; set; }
            public string tbUsername { get; set; }
            public string tbPassword { get; set; }
            public string ldl { get; set; }
        }

        #endregion
    }
}
