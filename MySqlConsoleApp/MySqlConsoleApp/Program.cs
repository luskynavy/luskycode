using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace MySqlConsoleApp
{
    internal class Program
    {
        static void TestMysql()
        {
            Stopwatch sw = new Stopwatch();
            int ret;

            try
            {
                string MyConString = "SERVER=localhost;" +
                "DATABASE=test;" +
                "UID=root;" +
                "PASSWORD=;";
                MySqlConnection myconnection = new MySqlConnection(MyConString);
                myconnection.Open();

                MySqlCommand mycommand = myconnection.CreateCommand();
                MySqlDataReader mydatareader;

                //Delete table
                mycommand.CommandText = "DROP TABLE IF EXISTS tabletest";
                ret = mycommand.ExecuteNonQuery();

                //Create table
                //mycommand.CommandText = "CREATE TABLE `test`.`tabletest` (`id` INT NULL, `col1` INT NULL, `col2` INT NULL) ENGINE = INNODB;";
                //mycommand.CommandText = "CREATE TABLE `test`.`tabletest` (`id` INT NULL, `col1` INT NULL, `col2` INT NULL) ENGINE = MEMORY;";
                mycommand.CommandText = "CREATE TABLE `test`.`tabletest` (`id` INT NULL, `col1` INT NULL, `col2` INT NULL) ENGINE = MYISAM;";
                //mycommand.CommandText = "CREATE TABLE `test`.`tabletest` (`id` INT NULL, `col1` INT NULL, `col2` INT NULL) ENGINE = ARIA;";
                ret = mycommand.ExecuteNonQuery();

                //Count elements in table
                mycommand.CommandText = "SELECT COUNT(*) FROM tabletest";
                mydatareader = mycommand.ExecuteReader();

                Console.WriteLine("elements in table :");
                while (mydatareader.Read())
                {
                    for (int i = 0; i < mydatareader.FieldCount; i++)
                    {
                        Console.Write(mydatareader.GetValue(i).ToString() + " ");
                    }
                    Console.WriteLine();
                }

                mydatareader.Close();

                sw.Start();

                int maxInsert = 10000;
                //800 insert per sec for a p8400 in MYISAM/INNODB with mysql 5.1
                //22000 insert per sec for a i7 4770k en MYISAM with mysql 5.1 or 5.6
                //9000 insert per sec for a i5 8265U in MYISAM/MEMORY with mysql 5.1 (1000 for INNODB)
				//8000 insert per sec for a i5 8265U in MYISAM with mariadb 11.7.2
                Console.WriteLine("\nfor i:0->" + maxInsert + " INSERT INTO tabletest (id, col1, col2) VALUES (i, 2, 3)");
                for (int i = 0; i < maxInsert; i++)
                {
                    mycommand.CommandText = "INSERT INTO tabletest (id, col1, col2) VALUES (" + i + ", 2, 3)";
                    ret = mycommand.ExecuteNonQuery();
                    //on ferme le datareader pour pouvoir reexecuter une commande apres
                    //mydatareader.Close();
                    if (i % 200 == 0)
                    {
                        Console.WriteLine((double)i + " in " + (double)sw.Elapsed.TotalMilliseconds / 1000 + "s : " +
                            (double)i / sw.Elapsed.TotalMilliseconds * 1000 + " inserts per sec");
                    }
                }
                Console.WriteLine((double)maxInsert + " in " + (double)sw.Elapsed.TotalMilliseconds / 1000 + "s : " +
                    (double)maxInsert / sw.Elapsed.TotalMilliseconds * 1000 + " inserts per sec");

                Console.WriteLine();

                //Count elements in table
                mycommand.CommandText = "SELECT COUNT(*) FROM tabletest";
                var obj = mycommand.ExecuteScalar();
                Console.WriteLine(obj + " elements in table");

                //Last elements in table
                mycommand.CommandText = $"SELECT * FROM tabletest WHERE id = {maxInsert - 1}";
                mydatareader = mycommand.ExecuteReader();

                Console.WriteLine("\nlast element in table :");
                while (mydatareader.Read())
                {
                    for (int i = 0; i < mydatareader.FieldCount; i++)
                    {
                        if (i != 0)
                        {
                            Console.Write(", ");
                        }
                        Console.Write(mydatareader.GetName(i)  + ": " + mydatareader.GetValue(i).ToString());

                    }
                    Console.WriteLine();
                }

                mydatareader.Close();

                //Delete table
                mycommand.CommandText = "DROP TABLE tabletest";
                ret = mycommand.ExecuteNonQuery();

                myconnection.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void Main(string[] args)
        {
            TestMysql();
        }
    }
}
