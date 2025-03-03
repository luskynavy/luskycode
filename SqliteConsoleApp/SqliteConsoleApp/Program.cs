using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Diagnostics;
using System.Transactions;
using System.Xml.Linq;

namespace SqliteConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            using (var db = new SqliteConnection("Data Source=test.db"))
            {
                db.Open();

                String tableCommand = @"
                    CREATE TABLE IF NOT EXISTS tabletest (
                    id INT NULL,
                    col1 INT NULL,
                    col2 INT NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);
                createTable.ExecuteReader();



                var command = db.CreateCommand();
                command.CommandText = "SELECT COUNT(*) as nb FROM tabletest";
                var nb = command.ExecuteScalar();
                Console.WriteLine($"Elements in tabletest {nb}");

                int maxInsert = 10000;

                // Transaction for fast speed batch inserts
                using (var transaction = db.BeginTransaction())
                {
                    command.Transaction = transaction;
                    for (int i = 0; i < maxInsert; i++)
                    {
                        command.CommandText = "INSERT INTO tabletest (id, col1, col2) VALUES (" + i + ", 2, 3)";
                        command.ExecuteNonQuery();

                        if (i % 200 == 0)
                        {
                            System.Console.WriteLine((double)i + " in " + (double)sw.Elapsed.TotalMilliseconds / 1000 + " : " + (double)i / sw.Elapsed.TotalMilliseconds * 1000);
                        }
                    }

                    transaction.Commit();
                }

                System.Console.WriteLine((double)maxInsert + " in " + (double)sw.Elapsed.TotalMilliseconds / 1000 + " : " + (double)maxInsert / sw.Elapsed.TotalMilliseconds * 1000);

                command.Transaction = null;
                command.CommandText = @"SELECT COUNT(*) as nb FROM tabletest";
                nb = command.ExecuteScalar();
                Console.WriteLine($"Elements in tabletest {nb}");

                command.CommandText = "DROP TABLE tabletest";
                command.ExecuteNonQuery();
            }
        }
    }
}
