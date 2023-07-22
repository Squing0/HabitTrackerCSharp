

using HabitTracker;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

internal class Program
{
    static string habit = "";
    static string unit = "";
    static string connectionString = "Data Source=habit-Tracker.db";
    private static void Main(string[] args)
    {
        ChooseHabit();            
        MainMenu();
    }

    static void MainMenu()
    {
        while (true)
        {
            Console.WriteLine("\n0.\tExit");
            Console.WriteLine("1.\tInsert record.");
            Console.WriteLine("2.\tUpdate record.");
            Console.WriteLine("3.\tDelete record.");
            Console.WriteLine("4.\tView records.");
            Console.WriteLine("5.\tReport of records.");
            Console.WriteLine("6.\tChange habit and/or unit of measurement.");

            string menuChoice = Console.ReadLine();

            switch (menuChoice)
            {
                case "0":
                    Environment.Exit(0);
                    break;
                case "1":
                    Insert();
                    break;
                case "2":
                    Update();
                    break;
                case "3":
                    Delete();
                    break;
                case "4":
                    View();
                    break;
                case "5":
                    Report();
                    break;
                case "6":
                    ChooseHabit();
                    break;
                default:
                    Console.WriteLine("Please enter a value between 0 and 5.");
                    break;
            }
        }
    }

    static void ChooseHabit()
    {
        Console.WriteLine("What habit do you want to track?");
        habit = Console.ReadLine();

        while (string.IsNullOrEmpty(habit) || habit.Any(x => !char.IsLetter(x))) //Last part is lambda got online
        {
            Console.WriteLine("Please only enter text!");
            habit = Console.ReadLine();
        }

        Console.WriteLine($"What unit of measurement do you want to use for {habit}?");
        unit = Console.ReadLine();

        while (string.IsNullOrEmpty(unit) || unit.Any(x => !char.IsLetter(x)))
        {
            Console.WriteLine("Please enter only text!");
            unit = Console.ReadLine();
        }
        CreateTable();
    }

    static void CreateTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @$"CREATE TABLE IF NOT EXISTS {habit} (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT(255),
                                        Quantity INTEGER)";

            tableCmd.ExecuteNonQuery(); // Just creates table, doesn't retrieve any data
            connection.Close();
        }
    }
    static string UserDateInput()
    {
        Console.WriteLine("Please insert date.\nMake sure to enter in this format: dd/mm/yy.\n-----Enter 0 to return to main menu.-----");

        string dateInput = Console.ReadLine();

        if (dateInput == "0")
        {
            MainMenu();
        }

        while(!DateTime.TryParseExact(dateInput, "dd/MM/yy", new CultureInfo("en-US"), DateTimeStyles.None, out _)){
            Console.WriteLine("Invalid date. Please enter in the correct format!");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    static int UserNumInput(string message)
    {
        Console.WriteLine(message);

        string numInput = Console.ReadLine();

        if(numInput == "0")
        {
            MainMenu();
        }

        while(!Int32.TryParse(numInput, out _) || Convert.ToInt32(numInput) < 0)
        {
            Console.WriteLine("Please only enter positive numerical values!");
            numInput = Console.ReadLine();
        }

        int finalInput = Convert.ToInt32(numInput);

        return finalInput;
    }

    static void Insert()
    {
        Console.Clear();
        string date = UserDateInput();
        int quantity = UserNumInput($"Please enter the number of {unit} that you want to store.\n-----Enter 0 to return to main menu.-----");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var insertCmd = connection.CreateCommand();

            insertCmd.CommandText = $"INSERT INTO {habit} (Date, Quantity) VALUES('{date}', {quantity})";

            insertCmd.ExecuteNonQuery();
            connection.Close();
        }     
    }
    static void View()
    {
        Console.Clear();
        Console.WriteLine("ID\tDate\t\t Quantity");
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var viewCmd = connection.CreateCommand();

            viewCmd.CommandText = $"SELECT * FROM {habit}";
                                            
            SqliteDataReader reader = viewCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int ID = reader.GetInt32(0);
                    DateTime Date = DateTime.ParseExact(reader.GetString(1), "dd/MM/yy", new CultureInfo("en-US"));
                    int Quantity = reader.GetInt32(2);

                    Console.Write($"\n{ID}\t{Date.Date.ToString("dd/MM/yy")}\t{Quantity} {unit}\n");
                }
            }
            else
            {
                Console.WriteLine("-----No rows found!-----");
            }
            connection.Close();
        }
    }

    static void Delete()
    {
        Console.Clear();
        View();
        int deletedID = UserNumInput("What record do you want to delete?\n Select by ID.");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var deleteCmd = connection.CreateCommand();

            deleteCmd.CommandText = $"DELETE FROM  {habit} WHERE ID = {deletedID}";

          int rowCount = deleteCmd.ExecuteNonQuery();

        if (rowCount == 0)
            {
                Console.WriteLine($"There is no record with the ID {deletedID}");
                MainMenu();
            }
            connection.Close();
        }
    }

    static void Update()
    {
        Console.Clear();
        View();
        int ID = UserNumInput("What ID do you want to update?");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {habit} WHERE Id = {ID})";
            int doesExist = Convert.ToInt32(checkCmd.ExecuteScalar());

            checkCmd.ExecuteNonQuery();
            if (doesExist == 0)
            {
                Console.WriteLine($"There is no record with the ID {ID}");
                connection.Close();
                Update();
            }

            int newQty = UserNumInput("What is the new quantity?");
            string newDate = UserDateInput();

            var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = $"UPDATE {habit} SET date = '{newDate}', quantity = {newQty} WHERE Id = {ID}";            
            updateCmd.ExecuteNonQuery();
            connection.Close();
        }

    }
    static void Report()
    {
        Console.WriteLine("(Only the year will be used here to give a report on the whole year)");
        string year = UserDateInput();     
        string finalYear = year.Substring(year.Length - 2, 2);

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {habit} WHERE date LIKE '%{finalYear}')";
            int doesExist = Convert.ToInt32(checkCmd.ExecuteScalar());

            checkCmd.ExecuteNonQuery();
            if (doesExist == 0)
            {
                Console.WriteLine($"There are no records in the year 20{finalYear}");
                connection.Close();
                Report();
            }
            tableCmd.CommandText = $@"SELECT SUM(quantity), AVG(quantity), COUNT(ID)
                                        FROM {habit}
                                        WHERE date LIKE '%{finalYear}'";

            SqliteDataReader reader = tableCmd.ExecuteReader();
            while (reader.Read())
            {
                int totalYear = reader.GetInt32(2);
                int average = reader.GetInt32(1);
                int sum = reader.GetInt32(0);

                Console.Write($"\nTotal number of times {habit} has been done in 20{finalYear}:\t{totalYear} times\n");
                Console.Write($"Average number of {unit} of {habit} that has been done in 20{finalYear}:\t{average} {unit}\n");
                Console.WriteLine($"Total number of {unit} of {habit} that has been done in 20{finalYear}:\t{sum} {unit}");
            }
            connection.Close();
        }
    }
}