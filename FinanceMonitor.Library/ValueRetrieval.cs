using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;
using FinanceMonitor.Data;
using FinanceMonitor.Library.Helpers;
using Newtonsoft.Json;

namespace FinanceMonitor.Library
{
    public static class ValueRetrieval
    {
        private static StoredProcedures sp = new StoredProcedures();

        #region Private Enums
        
        private enum ConnectionName
        {
            HessBenefitsCenter = 1,
            PayPal = 2,
            TDAmeritrade = 3,
            BankOfAmerica = 4,
            AllyBank = 5
        }

        private enum AccountType
        {
            Retirement = 1,
            Investment = 2,
            Checking = 3,
            Savings = 4,
            Credit = 5
        }

        private sealed class Timeframe
        {
            internal const string WEEK = "Week";
            internal const string MONTH = "Month";
            internal const string YEAR = "Year";
        }

        #endregion

        public static void Test()
        {   
            new Connections.AllyBankWebRequests().GetAllyBankSavingsBalance(sp.GetConnectionCredentials(23));
        }

        public static object GetLatestAmountByConnNameAccType(int accID)
        {
            AccountNameInformation ani = sp.GetConnNameAccTypeByAccID(accID);
            decimal accAmount = 0;

            switch ((ConnectionName)ani.ConnectionNameID)
	        {
                case ConnectionName.HessBenefitsCenter:
                    switch ((AccountType)ani.AccountTypeID)
	                {
                        case AccountType.Retirement:
                            accAmount = new Connections.HessBenefitsWebRequests().Get_401k_Total(sp.GetConnectionCredentials(accID));
                            break;
	                }
                    break;
                case ConnectionName.PayPal:
                    switch ((AccountType)ani.AccountTypeID)
	                {
                        case AccountType.Checking:
                            accAmount = new Connections.PayPalWebRequests().GetPersonalPayPalBalance();
                            break;
	                }
                    break;
                case ConnectionName.TDAmeritrade:
                    switch ((AccountType)ani.AccountTypeID)
	                {
                        case AccountType.Investment: 
                            accAmount = new Connections.TDAmeritradeWebRequests().GetPersonalTDAmeritradeBalance(sp.GetConnectionCredentials(accID));
                            break;
	                }
                    break;
                case ConnectionName.BankOfAmerica:
                    switch ((AccountType)ani.AccountTypeID)
	                {
                        case AccountType.Checking:
                            accAmount = new Connections.BankOfAmericaWebRequests().GetBankOfAmericaCheckingBalance(sp.GetConnectionCredentials(accID));
                            break;
                        case AccountType.Credit:
                            accAmount = new Connections.BankOfAmericaWebRequests().GetBankOfAmericaCreditCardBalance(sp.GetConnectionCredentials(accID));
                            break;
                        case AccountType.Savings:
                            accAmount = new Connections.BankOfAmericaWebRequests().GetBankOfAmericaSavingsBalance(sp.GetConnectionCredentials(accID));
                            break;
	                }
                    break;
                case ConnectionName.AllyBank:
                    switch ((AccountType)ani.AccountTypeID)
                    {
                        case AccountType.Savings:
                            accAmount = new Connections.AllyBankWebRequests().GetAllyBankSavingsBalance(sp.GetConnectionCredentials(accID));
                            break;
                    }
                    break;
            }

            sp.InsertAccountAmount(accID, accAmount);

            return new AccountAmountInformation
            {
                Amount = accAmount,
                RequestDate = Helper.GetEasternTime()
            };
        }

        public static void GetAllLatestAmountsByUser(int userID)
        {
            var sb = new StringBuilder("<table class='dailyAccountValueTable'>");
            decimal totalAccountValue = 0;

            foreach (var acc in sp.GetAllAccountsByUser(userID))
            {
                decimal amount = ((AccountAmountInformation)GetLatestAmountByConnNameAccType(Convert.ToInt32(acc.ID))).Amount;
                totalAccountValue += amount;
                sb.Append(string.Format("<tr><td style=\"text-overflow:ellipsis;overflow:hidden;white-space:nowrap;width:70%;\">{0}</td><td {1} style=\"text-align:right;\">{2}</td></tr>", acc.AccNickName, SetEmailTextColorByValue(amount), amount.ToString("C")));
            }
            
            sb.Append(string.Format("<tr><td style=\"text-align:right\">Total Amount:</td><td {0} style=\"border-top:1px solid black;text-align:right;\">{1}</td></tr>", SetEmailTextColorByValue(totalAccountValue), totalAccountValue.ToString("C")));

            if(Helper.GetEasternTime().DayOfWeek == DayOfWeek.Sunday)
            {
                sb.Append(GetTotalAccountValuePercentChangeByTimeframe(userID, Timeframe.WEEK, totalAccountValue));
                sb.Append(GetTotalAccountValuePercentChangeByTimeframe(userID, Timeframe.MONTH, totalAccountValue));
                sb.Append(GetTotalAccountValuePercentChangeByTimeframe(userID, Timeframe.YEAR, totalAccountValue));
            }
            sb.Append("</table>");

            new Email(sp.GetEmailByUserID(userID), Email.EmailType.AccountValueStatusDailyEmail, "Daily Account Value Status", sb.ToString()).SendEmail();
        }

        public static object GetAllAccountsByUser(int userID)
        {
            return sp.GetAllAccountsByUser(userID);
        }

        public static object GetAllPossibleAccounts()
        {
            return sp.GetAllPossibleAccounts();
        }

        #region Account CRUD Calls
        
        public static void UpdateAccountConnectionCredentials(object credentialsByAccount)
        {
            sp.UpdateAccountConnectionCredentials(JsonConvert.DeserializeObject<AccountConnectionLoginCredentials>(credentialsByAccount.ToString()));
        }

        public static int CreateAccount(object newAccountInfo)
        {
            return sp.CreateAccount(JsonConvert.DeserializeObject<CreateAccountInfo>(newAccountInfo.ToString()));
        }

        public static void DeleteAccount(int accID)
        {
            sp.DeleteAccount(accID);
        }

        #endregion

        #region User Calls

        public static int? ValidateUserCredentials(object credentials)
        {
            return sp.ValidateUserCredentials(JsonConvert.DeserializeObject<DatabaseLoginCredentials>(credentials.ToString()));
        }

        public static void UpdateUserPassword(object info)
        {
            sp.UpdateUserPassword(JsonConvert.DeserializeObject<UpdatePasswordInfo>(info.ToString()));
        }

        public static int CreateUser(object info)
        {
            CreateUserInfo userInfo = JsonConvert.DeserializeObject<CreateUserInfo>(info.ToString());
            return Helper.IsValidEmail(userInfo.userid) ? sp.CreateUser(userInfo) : -1;
        }

        public static void DeleteUser(int userID)
        {
            sp.DeleteUser(userID);
        }

        #endregion

        #region Private Methods

        private static string GetTotalAccountValuePercentChangeByTimeframe(int userID, string timeframe, decimal totalAccountValue)
        {
            decimal previousValue = 0;
            foreach (var item in sp.GetAccountValuesByUserAndTimeframe(userID, timeframe))
                previousValue += item.Amount;

            return PrecentChangeRow(totalAccountValue, previousValue, timeframe);
        }

        private static string PrecentChangeRow(decimal currentValue, decimal previousValue, string changeTimeframe)
        {
            decimal percentChange = (previousValue == 0 ? 0 : ((currentValue - previousValue) / previousValue));
            return "<tr><td style=\"text-align:right\">" + changeTimeframe + "ly Change:</td><td " + SetEmailTextColorByValue(percentChange) + ">" + (previousValue == 0 ? "N/A" : percentChange.ToString("P2")) + "</td></tr>";
        }

        private static string SetEmailTextColorByValue(decimal value)
        {
            return (value == 0 ? string.Empty : (value > 0 ? "class='numPositive'" : "class='numNegative'"));
        }

        #endregion
    }
}
