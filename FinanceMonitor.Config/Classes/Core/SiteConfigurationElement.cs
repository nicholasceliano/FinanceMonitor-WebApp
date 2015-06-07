using System;
using System.Configuration;

namespace FinanceMonitor.Config
{
    public class SiteConfigurationElements : ConfigurationElementCollectionBase<SiteConfigurationElement> { }

    public class SiteConfigurationElement : CommonConfigurationElement
    {
        #region Configuration Properties

       [ConfigurationProperty("value")]
        public string Value
        {
            get { return Convert.ToString(this["value"]); }
        }

       [ConfigurationProperty("systemType")]
       public object SystemType
       {
           get { return ""; }
       }

        [ConfigurationProperty("_configurationName")]
        public string ConfigurationName
        {
            get { return Convert.ToString(this["_configurationName"]); }
        }


        #endregion
    }
}