using System;
using System.Collections.Generic;

// Expense class representing a single expense
class Expense
{
    // Properties of the Expense class
    public string Description { get; set; } // Description of the expense
    public decimal Amount { get; set; } // Amount of the expense
    public DateTime Date { get; set; } // Date of the expense

    // Constructor initializing the properties
    public Expense(string description, decimal amount, DateTime date)
    {
        Description = description;
        Amount = amount;
        Date = date;
    }

    // Virtual method for dynamic polymorphism to be overridden in derived classes
    public virtual void DisplayInfo()
    {
        Console.WriteLine($"| {Description,-20} | {Amount,10:C} | {Date.ToShortDateString(),10} |");
    }
}

// Interface for category
interface ICategory
{
    string Name { get; set; } // Name of the category
    void AddExpense(Expense expense); // Method to add an expense to the category
    void DisplayExpenses(); // Method to display all expenses in the category
}

// ExpenseCategory class representing a category of expenses
class ExpenseCategory : ICategory
{
    // Property for the category name
    public string Name { get; set; }
    // List of expenses in this category
    public List<Expense> Expenses { get; set; }

    // Constructor to initialize the category name and the list of expenses
    public ExpenseCategory(string name)
    {
        Name = name;
        Expenses = new List<Expense>();
    }

    // Method to add an expense to the category
    public void AddExpense(Expense expense)
    {
        Expenses.Add(expense);
    }

    // Method to display all expenses in the category
    public void DisplayExpenses()
    {
        Console.WriteLine(new string('-', 48)); // Print a line of dashes
        Console.WriteLine($"| Category: {Name,-35} |"); // Print the category name
        Console.WriteLine(new string('-', 48));
        Console.WriteLine($"| {"Description",-20} | {"Amount",10} | {"Date",10} |");
        Console.WriteLine(new string('-', 48));

        decimal total = 0; // Variable to store the total amount

        // Iterate over each expense in the list of expenses
        foreach (Expense expense in Expenses)
        {
            expense.DisplayInfo(); // Display the information of the expense
            total += expense.Amount; // Add the expense amount to the total
        }

        Console.WriteLine(new string('-', 48));
        Console.WriteLine($"| {"Total",-20} | {total,10:C} | {"",10} |"); // Print the total amount
        Console.WriteLine(new string('-', 48));
    }
}

// MonthlyExpenseCategory class representing a category of monthly expenses
class MonthlyExpenseCategory : ExpenseCategory, ICategory
{
    // Properties for the month and year of the expenses
    public int Month { get; set; }
    public int Year { get; set; }

    // Constructor to initialize the category name, month, and year
    public MonthlyExpenseCategory(string name, int month, int year) : base(name)
    {
        Month = month;
        Year = year;
    }

    // Method to display information about the monthly expenses
    public void DisplayInfo()
    {
        Console.WriteLine($"Category: {Name}");
        Console.WriteLine($"Month: {Month}/{Year}");
        foreach (Expense expense in Expenses)
        {
            expense.DisplayInfo(); // Display the information of each expense
            Console.WriteLine();
        }
    }
}

// ExpenseTracker class representing the expense tracker application
class ExpenseTracker
{
    // List of expense categories
    public List<ExpenseCategory> Categories { get; set; }

    // Constructor to initialize the list of categories
    public ExpenseTracker()
    {
        Categories = new List<ExpenseCategory>();
    }

    // Method to add a category to the list of categories
    public void AddCategory(ExpenseCategory category)
    {
        Categories.Add(category);
    }

    // Method to display all categories and their expenses
    public void DisplayCategories()
    {
        foreach (ExpenseCategory category in Categories)
        {
            category.DisplayExpenses();
            Console.WriteLine();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create an instance of ExpenseTracker when the application starts
        ExpenseTracker expenseTracker = new ExpenseTracker();

        while (true)
        {
            // Display menu options to the user
            Console.WriteLine("1. Add Expense");
            Console.WriteLine("2. View All Expenses");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option: ");

            // Get the user's choice
            int choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                // Get expense details from the user
                Console.Write("Enter description: ");
                string description = Console.ReadLine();
                Console.Write("Enter amount: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                Console.Write("Enter date (yyyy-mm-dd): ");
                DateTime date = DateTime.Parse(Console.ReadLine());

                // Get category details from the user
                Console.Write("Enter category name: ");
                string categoryName = Console.ReadLine();
                Console.Write("Enter month (1-12): ");
                int month = int.Parse(Console.ReadLine());
                Console.Write("Enter year: ");
                int year = int.Parse(Console.ReadLine());

                // Create the expense
                Expense expense = new Expense(description, amount, date);

                // Find or create the category
                MonthlyExpenseCategory category = null;

                // Access category list in expenseTracker
                foreach (var cat in expenseTracker.Categories)
                {
                    // Check if a matching category already exists
                    if (cat is MonthlyExpenseCategory monthlyCategory &&
                        monthlyCategory.Name == categoryName &&
                        monthlyCategory.Month == month &&
                        monthlyCategory.Year == year)
                    {
                        category = monthlyCategory;
                        break; // Exit the loop if found
                    }
                }

                if (category == null)
                {
                    // If not found, create a new category
                    category = new MonthlyExpenseCategory(categoryName, month, year);
                    // Add the new category to the list of categories
                    expenseTracker.AddCategory(category);
                }

                // Add the expense to the category
                category.AddExpense(expense);
            }
            else if (choice == 2)
            {
                // Display all categories and their expenses
                expenseTracker.DisplayCategories();
            }
            else if (choice == 3)
            {
                // Exit the program
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }

            Console.WriteLine();
        }
    }
}