
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using FinanceMonitor.Config;
namespace FinanceMonitor.Library.Helpers
{
    class Helper
    {
        public static decimal ConvertMoneyStringToDecimal(string strValue)
        {
            return decimal.Parse(strValue, System.Globalization.NumberStyles.Currency);
        }

        public static bool IsValidEmail(string strIn)
        {
            return Regex.IsMatch(strIn,
                    @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,24}))$", RegexOptions.IgnoreCase);
        }

        public static DateTime GetEasternTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(AppConfiguration.Current.Timezone));
        }

        public static string RetrieveElementValue(string responseText, string elementType, string elementID)
        {
            return SubstringValue(responseText, TextStartIndex(responseText, string.Format("<{0} id=\"{1}\">", elementType, elementID)), string.Format("</{0}>", elementType));
        }
        public static string RetrieveElementValue(HttpWebResponse response, string elementType, string elementID)
        {
            return ResponseValueFromHttpWebResponse(response, string.Format("<{0} id=\"{1}\">", elementType, elementID), string.Format("</{0}>", elementType));
        }

        public static string RetrieveResponseValueFromPartialText(string responseText, string partialText, string endText)
        {
            return SubstringValue(responseText, TextStartIndex(responseText, partialText), endText);
        }
        public static string RetrieveResponseValueFromPartialText(HttpWebResponse response, string partialText, string endText)
        {
            return ResponseValueFromHttpWebResponse(response, partialText, endText);
        }

        public static string Build_NVP_POST_Message(object cURLRequest)
        {
            StringBuilder req = new StringBuilder();
            foreach (PropertyInfo item in cURLRequest.GetType().GetProperties())
            {
                var gg = item.GetValue(cURLRequest, null);
                if (gg != null)
                {

                    if (item.PropertyType.MemberType == MemberTypes.NestedType)
                    {
                        var propertyObject = item.GetValue(cURLRequest, null);
                        foreach(PropertyInfo subItem in propertyObject.GetType().GetProperties())
                            req.Append(GetPropertyNVP(subItem, propertyObject));
                    }
                    else
                        req.Append(GetPropertyNVP(item, cURLRequest));
                }
            }

            return req.Remove(req.Length - 1, 1).ToString(); //remove trailing &
        }

        #region Private Methods

        private static string GetPropertyNVP(PropertyInfo property, object propertyObject)
        {//Replace("___", ".") is a business rule for model objects coming in
         //Replace("_replacementKey=", string.Empty) is a business rule for model objects from Ally Bank. Allows passing in property at runtime
            return string.Format("{0}={1}&", property.Name.Replace("___", "."), property.GetValue(propertyObject, null)).Replace("_replacementKey=", string.Empty);
        }

        private static int TextStartIndex(string response, string searchText)
        {
            return response.IndexOf(searchText, response.IndexOf(searchText)) + searchText.Length;
        }

        private static string SubstringValue(string response, int startIndex, string endText)
        {
            return response.Substring(startIndex, response.IndexOf(endText, startIndex) - startIndex).Trim();
        }

        private static string ResponseValueFromHttpWebResponse(HttpWebResponse response, string partialText, string endText)
        {
            using (var r = new StreamReader(response.GetResponseStream()))
            {
                while (!r.EndOfStream)
                {
                    var line = r.ReadLine();
                    if (line.Contains(partialText))
                        return SubstringValue(line, TextStartIndex(line, partialText), endText);
                }
            }
            return null;
        }

        #endregion
    }
}
