using System;
using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace FinanceMonitor.Config
{
    public class AppConfiguration : ConfigurationSection
    {
        #region Static Members

        protected static AppConfiguration _Current;
        protected global::System.Configuration.Configuration _Configuration;

        public static AppConfiguration Current
        {
            get
            {
                if (AppConfiguration._Current == null)
                {
                    global::System.Configuration.Configuration configuration = null;
                    //Get current web or exe application configuration
                    if (HttpContext.Current == null)
                        configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    else
                        configuration = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                    //Retrieve the application configuration section from the application configuration
                    if (configuration != null)
                        AppConfiguration._Current = (AppConfiguration)configuration.Sections["appConfiguration"];
                    else
                        AppConfiguration._Current = (AppConfiguration)global::System.Configuration.ConfigurationManager.GetSection("appConfiguration");
                    AppConfiguration._Current._Configuration = configuration;
                }
                return AppConfiguration._Current;
            }
        }

        #endregion

        #region Configuration Properies

        [ConfigurationProperty("SqlLoginPassword", IsDefaultCollection = false), ConfigurationCollection(typeof(SiteConfigurationElement), AddItemName = "add")]
        private SiteConfigurationElements SqlLoginPasswords
        {
            get { return (SiteConfigurationElements)this["SqlLoginPassword"]; }
        }

        [ConfigurationProperty("SqlConnectionStringName", IsDefaultCollection = false), ConfigurationCollection(typeof(SiteConfigurationElement), AddItemName = "add")]
        private SiteConfigurationElements SqlConnectionStringNames
        {
            get { return (SiteConfigurationElements)this["SqlConnectionStringName"]; }
        }

        [ConfigurationProperty("EncryptionKey", IsDefaultCollection = false), ConfigurationCollection(typeof(SiteConfigurationElement), AddItemName = "add")]
        private SiteConfigurationElements EncryptionKeys
        {
            get { return (SiteConfigurationElements)this["EncryptionKey"]; }
        }

        [ConfigurationProperty("Timezone", IsDefaultCollection = false), ConfigurationCollection(typeof(SiteConfigurationElement), AddItemName = "add")]
        private SiteConfigurationElements Timezones
        {
            get { return (SiteConfigurationElements)this["Timezone"]; }
        }

        [ConfigurationProperty("AdminEmail", IsDefaultCollection = false), ConfigurationCollection(typeof(SiteConfigurationElement), AddItemName = "add")]
        private SiteConfigurationElements AdminEmails
        {
            get { return (SiteConfigurationElements)this["AdminEmail"]; }
        }

        #endregion

        #region Public Properties
    
        public string SqlLoginPassword
        {
            get
            {
                SiteConfigurationElement element = this.SqlLoginPasswords[Enum.GetName(typeof(SystemType), SystemType.FinanceMonitor) + this.ConfigurationName];
                if (element != null)
                    return element.Value;
                else
                    return String.Empty;
            }
        }

        public string SqlConnectionStringName
        {
            get
            {
                SiteConfigurationElement element = this.SqlConnectionStringNames[Enum.GetName(typeof(SystemType), SystemType.FinanceMonitor) + this.ConfigurationName];
                if (element != null)
                    return element.Value;
                else
                    return String.Empty;
            }
        }

        public string EncryptionKey
        {
            get
            {
                SiteConfigurationElement element = this.EncryptionKeys[Enum.GetName(typeof(SystemType), SystemType.FinanceMonitor) + this.ConfigurationName];
                if (element != null)
                    return element.Value;
                else
                    return String.Empty;
            }
        }

        public string Timezone
        {
            get
            {
                SiteConfigurationElement element = this.Timezones[Enum.GetName(typeof(SystemType), SystemType.FinanceMonitor) + this.ConfigurationName];
                if (element != null)
                    return element.Value;
                else
                    return String.Empty;
            }
        }

        public string AdminEmail
        {
            get
            {
                SiteConfigurationElement element = this.AdminEmails[Enum.GetName(typeof(SystemType), SystemType.FinanceMonitor) + this.ConfigurationName];
                if (element != null)
                    return element.Value;
                else
                    return String.Empty;
            }
        }

        #endregion

        public string ConfigurationName
        {
            get { return ConfigurationManager.AppSettings["configurationName"]; }
        }
    }
}
