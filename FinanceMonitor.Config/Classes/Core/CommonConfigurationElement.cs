using System;
using System.Configuration;

namespace FinanceMonitor.Config
{
    public enum SystemType
    {
        FinanceMonitor
    }

    public class CommonConfigurationElements : ConfigurationElementCollectionBase<CommonConfigurationElement> { }

    public class CommonConfigurationElement : ConfigurationElementBase
    {
        #region Configuration Properties

        [ConfigurationProperty("_configurationName", IsRequired = true)]
        public string ConfigurationName
        {
            get { return Convert.ToString(this["_configurationName"]); }
        }

        [ConfigurationProperty("systemType", DefaultValue = SystemType.FinanceMonitor, IsRequired = true)]
        public SystemType SystemType
        {
            get
            {
                if (this["systemType"] == null) return SystemType.FinanceMonitor;
                return (SystemType)this["systemType"];
            }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return Convert.ToString(this["value"]); }
        }

        #endregion

        #region Method Overrides

        public override object GetKeyValue()
        {
            return Enum.GetName(typeof(SystemType), this.SystemType) + this.ConfigurationName;
        }

        #endregion
    }
}