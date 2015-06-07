
namespace FinanceMonitor.Data
{
    public class DatabaseLoginCredentials : LoginCredentials
    {
        public string userid { get; set; }
        public string password { get; set; }
    }

    public class AccountConnectionLoginCredentials : LoginCredentials
    {
        public int accID { get; set; }
        public string userid { get; set; }
        public string password { get; set; }
    }

    public class CreateUserInfo : LoginCredentials
    {
        public string userid { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class UpdatePasswordInfo
    {
        public int ID { get; set; }
        public string password { get; set; }
    }

    public class CreateAccountInfo
    {
        public int userID { get; set; }
        public int connectionNameID { get; set; }
        public int accountTypeID { get; set; }
    }


    public class AllPossibleAccounts
    {
        public int connNameID { get; set; }
        public string connName { get; set; }
        public int accTypeID { get; set; }
        public string accountType { get; set; }
    }
}
