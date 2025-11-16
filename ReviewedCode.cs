using System;
using System.Collections.Generic;

namespace ApprenticeBank
{
    class Program
    {
        // A list of account instances - this could be replaced by a database connection later on
        static List<Account> Accounts = new List<Account>
        {
            // This is presumably dummy data - changes made to it are reset between user sessions
            new Account { AccountNumber = "1001", Pin = "1234", OwnerName = "Alex", Balance = 250.50, History = new List<string>() },
            new Account { AccountNumber = "1002", Pin = "0000", OwnerName = "Sam", Balance = 1200.00, History = new List<string>() },
            new Account { AccountNumber = "1003", Pin = "1111", OwnerName = "Jamie", Balance = 50.00, History = new List<string>() }
        };

        // the currently logged in account - consider renaming to something with more clarity e.g. "CurrentActiveAccount" or "LoggedInAccount"
        // Additionally, add a question mark after Account? to make this variable nullable, as this will
        // indicate the variable can be set to null (which it needs to, since it's null when logged out)
        static Account Current;

        // Entry point of the program
        static void Main1(string[] args)
        {
            Console.Title = "Apprentice Bank";

            // program runs in a loop until logged in or exited
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Apprentice Bank ===");
                Console.WriteLine("1) Login");
                Console.WriteLine("2) Exit");

                // Consider adding more clarity as to what the expected input is e.g. "Please enter the number before your choice, e.g. 1 - to log in"
                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                if (choice == "1")  // attempt logging in
                {
                    Login();

                    // Check if Current is null (Login() sets this to an account if login was successful)
                    // Consider making the Login() function a bool that explicitely states whether or not the login was successful. 
                    // It's always possible 'Current' may not be null for unpredictable reasons, i.e. just because the current account exists, doesn't necessarily mean the login was a success.
                    // This might look like: bool loggedInSuccess = Login();  (you should still check the current account exists as well)
                    if (Current != null)  
                    {
                        MainMenu();
                    }
                }
                else if (choice == "2")  // exit program
                {
                    return;
                }

                // Consider adding an else statement, that displays an invalid input warning to the user and suggests a correct expected input
            }
        }

        static void Login()
        {
            Console.Clear();
            Console.Write("Account Number: ");
            var acc = Console.ReadLine();
            Console.Write("PIN: ");
            var pin = Console.ReadLine();

            // for every stored account, check its pin and account number match the input
            foreach (var a in Accounts)
            {
                // BUG: The || will always allow logging into either the intended account (regardless of whether the pin was correct),
                // or it will allow entry into the first account in the list that has the entered pin (so users could potentially enter into the
                // wrong account if multiple users have the same pin).
                // This can be fixed by simply replacing || with &&, so that it checks both the account number AND pin match the inputs
                if (a.AccountNumber == acc || a.Pin == pin)  
                {
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
            // Main program loop runs until logout is selected
            while (true)
            {
                // Potential issue: There's no check here to determine whether the current active account is still logged in or null.
                // So if for some reason, the current active account somehow becomes null, the options below will still be displayed.
                // A check like: if (Current == null) return;  would fix this

                Console.Clear();
                Console.WriteLine($"Welcome, {Current.OwnerName} ({Current.AccountNumber})");
                Console.WriteLine("1) View Balance");
                Console.WriteLine("2) Deposit");
                Console.WriteLine("3) Withdraw");
                Console.WriteLine("4) Transfer");
                Console.WriteLine("5) Transaction History");
                Console.WriteLine("6) Logout");

                // Consider having a "7) Exit" option - this would exit the program and log the user out as well
                // (the logout option would just redirect to the Main() function after logging out)

                // Consider adding more clarity as to what the expected input is e.g. "Please enter the number before your choice, e.g. 2 - to view Deposite"
                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                // Call different functions based on input choice.
                // consider expanding the acceptable inputs to include the names of the choices or the number plus an ) e.g. "3)
                // However, note that this would make it harder to maintain if the names of the choices changed, or new choices are added
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
                    Current = null;   // make current active user null (this is what logs them out)

                    // Given this choice is referred to as "6) Logout", consider calling the Main() function here before returning.
                    // Currently, the entire program stops when logged out, but the user may want to log into a different account
                    return;
                }

                // Consider adding an else statement that displays a warning message to the user, and guides them on what input is expected
                // e.g. "Invalid input. Please enter the number before the option you would like to select."
            }
        }

        // Clear console and view active accounts bank balance
        static void ViewBalance()
        {
            Console.Clear();
            Console.WriteLine($"Balance: £{Current.Balance}");
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
        }


        // Clear console and allow user to make a deposite to their account
        static void Deposit()
        {
            // BUG: No input validation is performed on amount. This means the program will crash if the user inputs any non-integer/decimal characters such as "a"
            // this can be fixed by nesting this in a try catch statement, as this will account for all possible error producing inputs- see below. (This could also be improved
            // by nesting everything in a while loop that breaks if the input is valid, as this would enable instant retries on incorrect inputs)
            /*
            try
            {
                Console.Clear();
                Console.Write("Amount to deposit: £");
                var amountText = Console.ReadLine();
                var amount = double.Parse(amountText);

                // BUG: Balance is being subtracted from when it should be being added to. Replace -= with += to fix the issue.
                Current.Balance -= amount;
                Current.History.Add($"{DateTime.Now}: Deposited £{amount}");
                Console.WriteLine("Done.");
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
            }
            catch
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
            */

            Console.Clear();
            Console.Write("Amount to deposit: £");
            var amountText = Console.ReadLine();
            var amount = double.Parse(amountText);

            // BUG: Balance is being subtracted from when it should be being added to. Replace -= with += to fix the issue.
            Current.Balance -= amount;
            Current.History.Add($"{DateTime.Now}: Deposited £{amount}");
            Console.WriteLine("Done.");
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
        }

        // Clear console and withdraw an amount of money from logged in account
        static void Withdraw()
        {
            // BUG: No input validation is performed on amount. This means the program will crash if the user inputs any non-integer/decimal characters such as "a"
            // this can be fixed by nesting this in a try catch statement, as this will account for all possible error producing inputs- see below. (This could also be improved
            // by nesting everything in a while loop that breaks if the input is valid, as this would enable instant retries on incorrect inputs)
            /*
            try
            {
                Console.Clear();
                Console.Write("Amount to withdraw: £");
                var amountText = Console.ReadLine();

                var amount = double.Parse(amountText);

                // BUG: We should be checking if the ammount witthdrawn is less than or equal <= to the balance. Currently, this will only allow withdrawing 
                // if the withdraw amount is higher than the balance
                if (amount > Current.Balance)
                {
                    // BUG: We should be subtracting the amount from the balance, not adding to it. Use -= instead of += below
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
                break;  // break loop
            }
            catch
            {
                Console.WriteLine("Invalid input. Please try again.");
            }            
            */

            Console.Clear();
            Console.Write("Amount to withdraw: £");
            var amountText = Console.ReadLine();
            var amount = double.Parse(amountText);

            // BUG: We should be checking if the ammount witthdrawn is less than or equal <= to the balance. Currently, this will only allow withdrawing 
            // if the withdraw amount is higher than the balance
            if (amount > Current.Balance)
            {
                // BUG: We should be subtracting the amount from the balance, not adding to it. Use -= instead of += below
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

        // Clear console and transfer money from this account to another target account
        static void Transfer()
        {
            // BUG: No input validation is performed on amount. This means the program will crash if the user inputs any non-integer/decimal characters such as "a"
            // this can be fixed by nesting this in a try catch statement, as this will account for all possible error producing inputs- see below. (This could also be improved
            // by nesting everything in a while loop that breaks if the input is valid, as this would enable instant retries on incorrect inputs)
            /*
            try
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
            catch
            {
                Console.WriteLine("Invalid input. Please try again");
            }
            */

            // Get the target account number and amount to transfer (Account number is stored as a string, so
            // we don't need to convert targetNumber to a numeric value)
            Console.Clear();
            Console.Write("Target account number: ");
            var targetNumber = Console.ReadLine();
            Console.Write("Amount to transfer: £");
            var amountText = Console.ReadLine();
            var amount = double.Parse(amountText);

            // define the target account - Add a ? after Account (like Account?) to indicate this variable is
            // nullable
            Account target = null; 
            foreach (var a in Accounts)
            {
                // If we find a matching account to the input, set the target account to this.
                // Consider adding a check to ensure targetNumber isn't the same as the logged in account (Current).
                // Nothing will break if the user transfers money to themselves, but it will reduce confusion
                if (a.AccountNumber == targetNumber)
                {
                    target = a;
                    break;
                }
            }

            // Check target exists and the amount is not greater than the balance. Otherwise, perform transfer.
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
                // BUG: The += and -= below should be swapped around, so that amount is subtracted from current, and
                // added to target. Currently, this essentially steals money from the target account and places it
                // in the current logged in account.
                Current.Balance += amount;
                target.Balance -= amount;
                Current.History.Add($"{DateTime.Now}: Transferred £{amount} to {target.AccountNumber}");
                target.History.Add($"{DateTime.Now}: Received £{amount} from {Current.AccountNumber}");
                Console.WriteLine("Transfer complete.");
            }

            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
        }

        // Shows the most recent history of users account (displaying up to 10 of the latest history logs).
        // Consider making this function take an input int parameter of "maxHistory", and adjusting "start" to equal "Math.Max(0, Current.History.Count - maxHistory);"
        // (with validation) so as to make this function more reusable and flexible. E.g. bank IT support may want to look at more than just 10 of the users history records
        static void ShowHistory()
        {
            Console.Clear();
            Console.WriteLine("Recent Activity:");
            int start = Math.Max(0, Current.History.Count - 10);

            // BUG: "i <= Current.History.Count" results in a index out of range error. This happens because of the <= symbol and list the number of items is counted from 0.
            // This can be fixed by replacing this with "i < Current.History.Count"
            for (int i = start; i <= Current.History.Count; i++)
            {
                Console.WriteLine(Current.History[i]);
            }
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
        }
    }
     
    // Account class - stores info related to users account. Consider renaming to "UserAccount" to improve clarity, as a banking system may want special
    // non end-user accounts like admin accounts in the future.
    class Account
    {
        // Improvement: Add the "required" keyword before each of the variables below, as this ensures each is initialized and not null, which 
        // can reduce the likelihood of errors
        public string AccountNumber { get; set; }
        public string Pin { get; set; }
        public string OwnerName { get; set; }
        public double Balance { get; set; }
        public List<string> History { get; set; }  // history of actions taken by account and what time - e.g. when logged in, when money recieved (plus amount and from account) etc... 
    }
}








