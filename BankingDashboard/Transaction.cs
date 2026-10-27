using System;

namespace BankingDashboard
{
    public class Transaction
    {
        public string Category { get; set; } = string.Empty;
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Method { get; set; } = string.Empty;
    }

    public class Account
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double Balance { get; set; }
    }
}
