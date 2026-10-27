using System;
using System.Collections.Generic;

namespace BankingDashboard
{
    public static class DashboardData
    {
        public static List<Transaction> GetTransactions()
        {
            return new List<Transaction>
            {
                // April 2026
                new Transaction { Category = "Food", Amount = 1200.00, Date = new DateTime(2026, 4, 2), Method = "UPI" },
                new Transaction { Category = "Rent", Amount = 22000.00, Date = new DateTime(2026, 4, 5), Method = "Bank Transfer" },
                new Transaction { Category = "Bills", Amount = 7200.00, Date = new DateTime(2026, 4, 8), Method = "Debit Card" },
                new Transaction { Category = "Travel", Amount = 18000.00, Date = new DateTime(2026, 4, 12), Method = "Wallet" },
                new Transaction { Category = "Shopping", Amount = 9700.00, Date = new DateTime(2026, 4, 16), Method = "UPI" },
                new Transaction { Category = "Entertainment", Amount = 4500.00, Date = new DateTime(2026, 4, 20), Method = "Credit Card" },
                new Transaction { Category = "Health", Amount = 5600.00, Date = new DateTime(2026, 4, 24), Method = "Debit Card" },

                // May 2026
                new Transaction { Category = "Food", Amount = 1450.00, Date = new DateTime(2026, 5, 3), Method = "UPI" },
                new Transaction { Category = "Rent", Amount = 22000.00, Date = new DateTime(2026, 5, 5), Method = "Bank Transfer" },
                new Transaction { Category = "Bills", Amount = 7800.00, Date = new DateTime(2026, 5, 9), Method = "Auto Pay" },
                new Transaction { Category = "Travel", Amount = 21500.00, Date = new DateTime(2026, 5, 12), Method = "Wallet" },
                new Transaction { Category = "Shopping", Amount = 12000.00, Date = new DateTime(2026, 5, 18), Method = "UPI" },
                new Transaction { Category = "Entertainment", Amount = 5200.00, Date = new DateTime(2026, 5, 21), Method = "Credit Card" },
                new Transaction { Category = "Health", Amount = 6700.00, Date = new DateTime(2026, 5, 29), Method = "Debit Card" },

                // June 2026
                new Transaction { Category = "Food", Amount = 1680.00, Date = new DateTime(2026, 6, 2), Method = "UPI" },
                new Transaction { Category = "Rent", Amount = 22000.00, Date = new DateTime(2026, 6, 5), Method = "Bank Transfer" },
                new Transaction { Category = "Bills", Amount = 8100.00, Date = new DateTime(2026, 6, 7), Method = "Debit Card" },
                new Transaction { Category = "Travel", Amount = 24000.00, Date = new DateTime(2026, 6, 11), Method = "Wallet" },
                new Transaction { Category = "Shopping", Amount = 14000.00, Date = new DateTime(2026, 6, 17), Method = "UPI" },
                new Transaction { Category = "Entertainment", Amount = 5800.00, Date = new DateTime(2026, 6, 19), Method = "Credit Card" },
                new Transaction { Category = "Health", Amount = 7300.00, Date = new DateTime(2026, 6, 26), Method = "Debit Card" },

                // July 2026
                new Transaction { Category = "Food", Amount = 1750.00, Date = new DateTime(2026, 7, 1), Method = "UPI" },
                new Transaction { Category = "Rent", Amount = 22000.00, Date = new DateTime(2026, 7, 5), Method = "Bank Transfer" },
                new Transaction { Category = "Bills", Amount = 8400.00, Date = new DateTime(2026, 7, 8), Method = "Debit Card" },
                new Transaction { Category = "Travel", Amount = 26000.00, Date = new DateTime(2026, 7, 15), Method = "Wallet" },
                new Transaction { Category = "Shopping", Amount = 15600.00, Date = new DateTime(2026, 7, 18), Method = "UPI" },
                new Transaction { Category = "Entertainment", Amount = 6400.00, Date = new DateTime(2026, 7, 20), Method = "Credit Card" },
                new Transaction { Category = "Health", Amount = 7900.00, Date = new DateTime(2026, 7, 27), Method = "Debit Card" }
            };
        }

        public static List<Account> GetAccounts()
        {
            return new List<Account>
            {
                new Account { Name = "Primary Savings", Type = "Savings", Balance = 256000.00 },
                new Account { Name = "Checking", Type = "Current", Balance = 124500.00 },
                new Account { Name = "Credit Card", Type = "Credit", Balance = 48200.00 }
            };
        }
    }
}
