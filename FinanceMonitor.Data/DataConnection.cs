using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using FinanceMonitor.Config;

namespace FinanceMonitor.Data
{
    public class DataConnection
    {
        protected FinanceMonitorEntities dataContext;
        private static string sqlLoginPassword = AppConfiguration.Current.SqlLoginPassword;
        private static string connectionStringEF = AppConfiguration.Current.SqlConnectionStringName;

        protected void EstablishEFConnection()
        {
            if (dataContext == null)
            {
                var entityBuilder = new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings[connectionStringEF].ConnectionString);
                var providerBuilder = DbProviderFactories.GetFactory(entityBuilder.Provider).CreateConnectionStringBuilder();
                providerBuilder.ConnectionString = entityBuilder.ProviderConnectionString;
                providerBuilder.Add("Password", sqlLoginPassword);

                entityBuilder.ProviderConnectionString = providerBuilder.ToString();

                dataContext = new FinanceMonitorEntities(entityBuilder.ToString());
                //using System.Data.Entity.Core.Objects;
                //for error on next page zzz
            }
        }
    }
}
