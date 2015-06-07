using System.Collections.Generic;
using System.Linq;

namespace FinanceMonitor.Data
{
    public class StoredProcedures : DataConnection
    {
        public DatabaseLoginCredentials GetConnectionCredentials(int accountID)
        {
            EstablishEFConnection();

            var data = dataContext.GetConnectionCredentials(accountID).FirstOrDefault();

            return new DatabaseLoginCredentials()
            {
                userid = Encryption.Decrypt(data.LoginUserName, Encryption.Decrypt(data.key)),
                password = Encryption.Decrypt(data.LoginPassword, Encryption.Decrypt(data.key))
            };
        }

        public void InsertAccountAmount(int accountID, decimal amount)
        {
            EstablishEFConnection();
            dataContext.InsertAmountForAccount(accountID, amount);
        }

        public List<GetAllAccountsByUser_Result> GetAllAccountsByUser(int userID)
        {
            EstablishEFConnection();
            return dataContext.GetAllAccountsByUser(userID).ToList();
        }

        public AccountNameInformation GetConnNameAccTypeByAccID(int accID)
        {
            EstablishEFConnection();

            var result = dataContext.GetConnNameAccTypeByAccID(accID).FirstOrDefault();

            return new AccountNameInformation()
            {
                AccountTypeID = result.AccountTypeID,
                ConnectionNameID = result.ConnectionNameID
            };
        }

        public List<GetAllPossibleAccounts_Result> GetAllPossibleAccounts()
        {
            EstablishEFConnection();
            return dataContext.GetAllPossibleAccounts().ToList();
        }

        public string GetEmailByUserID(int userID)
        {
            return (from d in dataContext.Users
                    where d.ID == userID
                    select d.Username).FirstOrDefault();
        }

        public List<GetAccountValuesByUserAndTimeframe_Result> GetAccountValuesByUserAndTimeframe(int userID, string timeFrame)
        {
            //timeframe is 'Week', 'Month', or 'Year'
            return dataContext.GetAccountValuesByUserAndTimeframe(userID, timeFrame).ToList();
        }

        #region Account CRUD Operation Stored Procs

        public void UpdateAccountConnectionCredentials(AccountConnectionLoginCredentials loginCreds)
        {
            EstablishEFConnection();

            string dbKey = Encryption.CreateDBKey();
            dataContext.UpdateAccountConnectionCredentials(loginCreds.accID, Encryption.Encrypt(loginCreds.userid, dbKey),
                                                                Encryption.Encrypt(loginCreds.password, dbKey), Encryption.Encrypt(dbKey));
        }

        public int CreateAccount(CreateAccountInfo accInfo)
        {
            EstablishEFConnection();
            //return value determines what to do in app
            //0 = need to enter paramters
            //-1 = failed insert - Mosty likeley Unique Key Violation
            try
            {
                return (int)dataContext.CreateAccount_ConnCredentials_Return(accInfo.userID, accInfo.connectionNameID, accInfo.accountTypeID).FirstOrDefault();
            }
            catch
            {
                return -1;
            }   
        }

        public void DeleteAccount(int accID)
        {
            EstablishEFConnection();
            dataContext.DeleteAccount(accID);
        }

        #endregion

        #region User Stored Procs

        public int? ValidateUserCredentials(DatabaseLoginCredentials credentials)
        {
            EstablishEFConnection();

            var userExists = dataContext.CheckIfUserExistsByUsername(credentials.userid).FirstOrDefault();
            return userExists != null ? dataContext.ValidateUserCredentials(credentials.userid, Encryption.Encrypt(credentials.password, Encryption.Decrypt(userExists.key))).FirstOrDefault() : null;
        }

        public void UpdateUserPassword(UpdatePasswordInfo info)
        {
            EstablishEFConnection();

            string dbKey = Encryption.CreateDBKey();
            dataContext.UpdateUserPassword(info.ID, Encryption.Encrypt(info.password, dbKey), Encryption.Encrypt(dbKey));
        }

        public int CreateUser(CreateUserInfo ui)
        {
            EstablishEFConnection();

            if (dataContext.CheckIfUserExistsByUsername(ui.userid).FirstOrDefault() == null)
            {
                var dbKey = Encryption.CreateDBKey();
                return (int)dataContext.CreateUser(ui.userid, Encryption.Encrypt(ui.password, dbKey), Encryption.Encrypt(dbKey), ui.firstName, ui.lastName).FirstOrDefault();
            }
            else
                return -1;//account already exists for that username
        }

        public void DeleteUser(int userID)
        {
            EstablishEFConnection();
            dataContext.DeleteUser(userID);
        }

        #endregion
    }
}
