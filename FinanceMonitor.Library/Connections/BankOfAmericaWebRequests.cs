using FinanceMonitor.Config;
using FinanceMonitor.Data;
using FinanceMonitor.Library.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FinanceMonitor.Library.Connections
{
    public class BankOfAmericaWebRequests
    {
        private static string CLIENT_ID = AccountConfiguration.Current.Plaid.CLIENTID;
        private static string SECRET = AccountConfiguration.Current.Plaid.SECRET;

        public decimal GetBankOfAmericaCheckingBalance(DatabaseLoginCredentials creds)
        {
            return CheckingBalance(new BankOfAmericaLoginCredentials() { userid = creds.userid, password = creds.password });
        }

        public decimal GetBankOfAmericaSavingsBalance(DatabaseLoginCredentials creds)
        {
            return SavingsBalance(new BankOfAmericaLoginCredentials() { userid = creds.userid, password = creds.password });
        }

        public decimal GetBankOfAmericaCreditCardBalance(DatabaseLoginCredentials creds)
        {
            return CreditCardBalance(new BankOfAmericaLoginCredentials() { userid = creds.userid, password = creds.password });
        }

        #region Get Account Balances

        private decimal CheckingBalance(BankOfAmericaLoginCredentials creds)
        {
            return GetAccountBalance(from a in ConnectToAccount(creds).accounts
                                     where a.type == "depository" & a.meta.name.Contains("Checking")
                                     select a);
        }

        private decimal SavingsBalance(BankOfAmericaLoginCredentials creds)
        {
            return GetAccountBalance(from a in ConnectToAccount(creds).accounts
                                     where a.type == "depository" & a.meta.name.Contains("Savings")
                                     select a);
        }

        private decimal CreditCardBalance(BankOfAmericaLoginCredentials creds)
        {
            return GetAccountBalance(from a in ConnectToAccount(creds).accounts
                                      where a.type == "credit"
                                      select a);
        }

        private decimal GetAccountBalance(IEnumerable<Accounts> accounts)
        {
            decimal totalAccountBalance = 0;
            foreach (Accounts a in accounts)
                totalAccountBalance += a.balance.current;
            return totalAccountBalance;
        }

        #endregion

        #region Connections to Plaid API

        private PlaidAPIAccounts ConnectToAccount(BankOfAmericaLoginCredentials creds)
        {
            if (creds.password.Length > 0)
            {
                StringBuilder sb = new StringBuilder(AccountConfiguration.Current.Plaid.BaseURI + "connect");
                sb.Append("?client_id=" + CLIENT_ID);
                sb.Append("&secret=" + SECRET);
                sb.Append("&access_token=" + creds.password);

                return JsonConvert.DeserializeObject<PlaidAPIAccounts>(new StreamReader((WebRequestHelper.GET(sb.ToString(), false)).GetResponseStream()).ReadToEnd());
            }
            else
            {
                //TODO: Finish Setting up account Creation
                PlaidAPIResponse resp = SetUpPlaidAccountConnection("", "", "", "");
                return SubmitPlaidAccountMFA("", resp.access_token);
            }
        }

        private PlaidAPIResponse SetUpPlaidAccountConnection(string username, string password, string accountType, string email)
        {
            var response = WebRequestHelper.POST(AccountConfiguration.Current.Plaid.BaseURI + "connect", new Connect
                {
                    client_id = CLIENT_ID,
                    secret = SECRET,
                    credentials = new Connect_Credentials
                    {
                        username = username,
                        password = password
                    },
                    type = accountType,
                    email = email,
                    options = new Connect_Options
                    {
                        login = "true",
                        webhook = "https://financemonitor.azurewebsites.net/api/tests/WebHookTest/"
                    }
                }, false);

            return JsonConvert.DeserializeObject<PlaidAPIResponse>(new StreamReader(response.GetResponseStream()).ReadToEnd());
        }

        private PlaidAPIAccounts SubmitPlaidAccountMFA(string userMFA, string accessToken)
        {
            return JsonConvert.DeserializeObject<PlaidAPIAccounts>(new StreamReader(WebRequestHelper.POST(AccountConfiguration.Current.Plaid.BaseURI + "connect/step", new MFA_Authentication
            {
                client_id = CLIENT_ID,
                secret = SECRET,
                mfa = userMFA,
                access_token = accessToken
            }, false).GetResponseStream()).ReadToEnd());
        }

        #endregion

        #region Plaid Objects

        private class MFA_Authentication
        {
            public string client_id { get; set; }
            public string secret { get; set; }
            public string mfa { get; set; }
            public string access_token { get; set; }
        }

        private class PlaidAPIResponse
        {
            public string type { get; set; }
            public string device { get; set; }
            public string access_token { get; set; }
        }

        private class Connect
        {
            public string client_id { get; set; }
            public string secret { get; set; }
            public Connect_Credentials credentials { get; set; }
            public string type { get; set; }
            public string email { get; set; }
            public Connect_Options options { get; set; }
            public string access_token { get; set; }
        }

        private class Connect_Options
        {
            public string login { get; set; }
            public string webhook { get; set; }
        }

        private class Connect_Credentials
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        #region Plaid API Account Objects

        private class PlaidAPIAccounts
        {
            public List<Accounts> accounts { get; set; }
        }

        private class Accounts
        {
            public string _id { get; set; }
            public string _item { get; set; }
            public string _user { get; set; }
            public AccountBalance balance { get; set; }
            public AccountMeta meta { get; set; }
            public string institution_type { get; set; }
            public string type { get; set; }
        }

        private class AccountBalance
        {
            public decimal current { get; set; }
            public decimal available { get; set; }
        }

        private class AccountMeta
        {
            public string number { get; set; }
            public string name { get; set; }
            public string limit { get; set; }
        }

        #endregion

        #endregion

        #region Internal Classes

        internal class BankOfAmericaLoginCredentials : LoginCredentials
        {
            public string userid { get; set; }
            public string password { get; set; }
        }

        #endregion
    }
}
