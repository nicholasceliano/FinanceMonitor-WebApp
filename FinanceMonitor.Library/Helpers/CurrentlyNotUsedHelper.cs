using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace FinanceMonitor.Library.Helpers
{
    public class CurrentlyNotUsedHelper
    {
        //CURRENTLY NOT BEING USED --- KEEPING FOR POTENTIAL FUTURE USE//removed static from class
        //public object LoadIdenticalObjectsValues(object returnObj, object loadingObj)
        //{
        //    foreach (PropertyInfo l in loadingObj.GetType().GetProperties())
        //    {
        //        foreach (PropertyInfo r in returnObj.GetType().GetProperties())
        //        {
        //            if (l.Name.Equals(r.Name) && l.GetType().Equals(r.GetType()))
        //                r.SetValue(returnObj, l.GetValue(loadingObj, null));
        //        }
        //    }

        //    return returnObj;
        //}

        //public static Dictionary<string, string> Parse_NVP_Response(string response)
        //{
        //    return ConvertNVPResponseToDictionary(response);
        //}

        //public static string Parse_NVP_Response(string response, string returnValue)
        //{
        //    string outputValue;
        //    ConvertNVPResponseToDictionary(response).TryGetValue(returnValue, out outputValue);
        //    return outputValue;
        //}

        //#region Private Class Methods

        //private static Dictionary<string, string> ConvertNVPResponseToDictionary(string response)
        //{
        //    var responseDict = new Dictionary<string, string>();
        //    foreach (string i in WebUtility.UrlDecode(response).Split('&'))
        //        responseDict.Add(i.Split('=')[0], i.Split('=')[1]);

        //    return responseDict;
        //}

        //#endregion
    }
}
