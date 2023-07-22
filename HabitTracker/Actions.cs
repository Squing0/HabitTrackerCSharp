using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker
{
    internal class Actions
    {
        private string connectionString = "Data Source=habit-Tracker.db";
 

        public void Update(string habit)
        {
            Console.Clear();
            Console.WriteLine("What date do you want to update?");
            Console.Write("Date: ");
            string updatedDate = Console.ReadLine();

            Console.WriteLine("What is the new quantity?");
            Console.Write("Quantity: ");
            int newQty = Convert.ToInt32(Console.ReadLine());

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @$"UPDATE {habit}
                                            SET quantity = '{newQty}'
                                            WHERE date = '{updatedDate}'";

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }

        }




        public void Report(string habit, string unit)
        {
            Console.Clear();
            Console.WriteLine("Please enter year for report");
            Console.Write("Year: ");
            string year = Console.ReadLine();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"SELECT SUM(quantity), AVG(quantity), COUNT(ID)
                                        FROM {habit}
                                        WHERE date LIKE '%{year}'";

                SqliteDataReader reader = tableCmd.ExecuteReader();
                while (reader.Read())
                {
                    string myReader = reader.GetString(2);
                    string myReader1 = reader.GetString(1);
                    string myReader2 = reader.GetString(0);
                    int MyReader1 = Convert.ToInt32(double.Parse(myReader1));
                    
                    //Come up with better names later

                    Console.Write($"\nTotal number of times {habit} has been done in {year}:{myReader} times\n");
                    Console.Write($"Average number of {unit} of {habit} has been done in {year}:{MyReader1} {unit}\n");
                    Console.WriteLine($"Total number of {unit} of {habit} done in {year}: {myReader2} {unit}");
                }

                connection.Close();
            }

        }
      
    }
}
