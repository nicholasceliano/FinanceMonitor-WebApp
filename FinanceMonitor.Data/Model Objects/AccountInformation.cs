using System;

namespace FinanceMonitor.Data
{
    public class AccountAmountInformation
    {
        public decimal Amount { get; set; }
        public DateTime RequestDate { get; set; }
    }

    public class AccountNameInformation
    {
        public int ConnectionNameID { get; set; }
        public int AccountTypeID { get; set; }
    }
}
