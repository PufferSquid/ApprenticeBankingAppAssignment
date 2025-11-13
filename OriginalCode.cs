using System;
using System.Collections.Generic;

namespace ApprenticeBank1
{
    
    class Program
    {
        static List<Account> Accounts = new List<Account>
        {
            new Account { AccountNumber = "1001", Pin = "1234", OwnerName = "Alex", Balance = 250.50, History = new List<string>() },
            new Account { AccountNumber = "1002", Pin = "0000", OwnerName = "Sam", Balance = 1200.00, History = new List<string>() },
            new Account { AccountNumber = "1003", Pin = "1111", OwnerName = "Jamie", Balance = 50.00, History = new List<string>() }
        };

        static Account Current;

        static void Main(string[] args)
        {
            Console.Title = "Apprentice Bank";

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Apprentice Bank ===");
                Console.WriteLine("1) Login");
                Console.WriteLine("2) Exit");
                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    Login();
                    if (Current != null)
                    {
                        MainMenu();
                    }
                }
                else if (choice == "2")
                {
                    return;
                }
            }
        }

        static void Login()
        {
            Console.Clear();
            Console.Write("Account Number: ");
            var acc = Console.ReadLine();
            Console.Write("PIN: ");
            var pin = Console.ReadLine();

            foreach (var a in Accounts)
            {
                if (a.AccountNumber == acc || a.Pin == pin)
                {
                    Console.WriteLine("Message Appears now!");
                    Current = a;
                    Current.History.Add($"{DateTime.Now}: Logged in");
                    return;
                }
            }

            Console.WriteLine("Invalid credentials.");
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
            Current = null;
        }

        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Welcome, {Current.OwnerName} ({Current.AccountNumber})");
                Console.WriteLine("1) View Balance");
                Console.WriteLine("2) Deposit");
                Console.WriteLine("3) Withdraw");
                Console.WriteLine("4) Transfer");
                Console.WriteLine("5) Transaction History");
                Console.WriteLine("6) Logout");
                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    ViewBalance();
                }
                else if (choice == "2")
                {
                    Deposit();
                }
                else if (choice == "3")
                {
                    Withdraw();
                }
                else if (choice == "4")
                {
                    Transfer();
                }
                else if (choice == "5")
                {
                    ShowHistory();
                }
                else if (choice == "6")
                {
                    Current.History.Add($"{DateTime.Now}: Logged out");
                    Current = null;
                    return;
                }
            }
        }

        static void ViewBalance()
        {
            Console.Clear();
            Console.WriteLine($"Balance: £{Current.Balance}");
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
        }

        static void Deposit()
        {
            Console.Clear();
            Console.Write("Amount to deposit: £");
            var amountText = Console.ReadLine();
            var amount = double.Parse(amountText);
            Current.Balance -= amount;
            Current.History.Add($"{DateTime.Now}: Deposited £{amount}");
            Console.WriteLine("Done.");
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
        }

        static void Withdraw()
        {
            Console.Clear();
            Console.Write("Amount to withdraw: £");
            var amountText = Console.ReadLine();
            var amount = double.Parse(amountText);
            if (amount > Current.Balance)
            {
                Current.Balance += amount;
                Current.History.Add($"{DateTime.Now}: Withdrew £{amount}");
                Console.WriteLine("Done.");
            }
            else
            {
                Console.WriteLine("Insufficient funds.");
            }
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
        }

        static void Transfer()
        {
            Console.Clear();
            Console.Write("Target account number: ");
            var targetNumber = Console.ReadLine();
            Console.Write("Amount to transfer: £");
            var amountText = Console.ReadLine();
            var amount = double.Parse(amountText);

            Account target = null;
            foreach (var a in Accounts)
            {
                if (a.AccountNumber == targetNumber)
                {
                    target = a;
                    break;
                }
            }

            if (target == null)
            {
                Console.WriteLine("Account not found.");
            }
            else if (amount > Current.Balance)
            {
                Console.WriteLine("Insufficient funds.");
            }
            else
            {
                Current.Balance += amount;
                target.Balance -= amount;
                Current.History.Add($"{DateTime.Now}: Transferred £{amount} to {target.AccountNumber}");
                target.History.Add($"{DateTime.Now}: Received £{amount} from {Current.AccountNumber}");
                Console.WriteLine("Transfer complete.");
            }

            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
        }

        static void ShowHistory()
        {
            Console.Clear();
            Console.WriteLine("Recent Activity:");
            int start = Math.Max(0, Current.History.Count - 10);
            for (int i = start; i <= Current.History.Count; i++)
            {
                Console.WriteLine(Current.History[i]);
            }
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
        }
    }

    class Account
    {
        public string AccountNumber { get; set; }
        public string Pin { get; set; }
        public string OwnerName { get; set; }
        public double Balance { get; set; }
        public List<string> History { get; set; }
    }

    
}








