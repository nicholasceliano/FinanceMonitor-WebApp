using System.Web.Http;
using FinanceMonitor.Library;

namespace FinanceMonitor.Web.Controllers
{
    public class GeneralAccountsController : ApiController
    {
        //GET api/generalAccounts/GetAllAccountsByUser/{value}
        [HttpGet]
        public object GetAllAccountsByUser(int value)
        {
            return ValueRetrieval.GetAllAccountsByUser(value);
        }

        //GET api/generalAccounts/LatestAmountByConnNameAccType/{value}
        [HttpGet]
        public object GetLatestAmountByConnNameAccType(int value)
        {
            return ValueRetrieval.GetLatestAmountByConnNameAccType(value);
        }

        //GET api/generalAccounts/GetLatest/{value}
        [HttpGet]
        public void GetAllLatestAmountsByUser(int value)
        {
            ValueRetrieval.GetAllLatestAmountsByUser(value);
        }

        //GET api/generalAccounts/GetAllPossibleAccounts
        [HttpGet]
        public object GetAllPossibleAccounts()
        {
            return ValueRetrieval.GetAllPossibleAccounts();
        }

        #region User Calls
        
        //POST api/generalAccounts/ValidateUserCredentials
        [HttpPost] //Post for security
        public int? ValidateUserCredentials(object value)
        {
            return ValueRetrieval.ValidateUserCredentials(value);
        }

        //POST api/generalAccounts/UpdateUserPassword
        [HttpPost]
        public bool UpdateUserPassword(object value)
        {
            ValueRetrieval.UpdateUserPassword(value);
            return true;
        }

        //POST api/generalAccounts/CreateUserAccount
        [HttpPost]
        public int CreateUser(object value)
        {
            return ValueRetrieval.CreateUser(value);
        }

        //DELETE api/generalAccounts/DeleteUserAccount/{value}
        public void DeleteUser(int value)
        {
            ValueRetrieval.DeleteUser(value);
        }

        #endregion

        #region Account CRUD Calls
        
        //POST api/generalAccounts/UpdateConnectionCredentials
        [HttpPost]
        public bool UpdateAccountConnectionCredentials(object value)
        {
            ValueRetrieval.UpdateAccountConnectionCredentials(value);
            return true;
        }

        //POST api/generalAccounts/CreateAccount
        [HttpPost]
        public int CreateAccount(object value)
        {
            return ValueRetrieval.CreateAccount(value);
        }

        //DELETE api/generalAccounts/DeleteAccount/{value}
        [HttpDelete]
        public void DeleteAccount(int value)
        {
            ValueRetrieval.DeleteAccount(value);
        }

        #endregion
    }
}
