using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data.Odbc;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Data.SQLite;


//pour se connecter à mysql : http://dev.mysql.com/downloads/connector/net/6.4.4.html (le 1.0.10 ne s'enregistre pas et est vieux...)
//ajout de la référence : C:\Program Files\MySQL\MySQL Connector Net 6.4.4\Assemblies\v2.0\MySql.Data.dll

//pour sqlite http://system.data.sqlite.org/downloads/1.0.105.0/sqlite-netFx40-setup-bundle-x86-2010-1.0.105.0.exe
// ajout de la référence : C:\Program Files\System.Data.SQLite\2010\bin\System.Data.SQLite.dll

namespace TestSQL
{
    class Program
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
                mycommand.CommandText = "SELECT COUNT(*) FROM tableinnodb";
                mydatareader = mycommand.ExecuteReader();

                while (mydatareader.Read())
                {
                    for (int i = 0; i < mydatareader.FieldCount; i++)
                    {
                        System.Console.Write(mydatareader.GetValue(i).ToString() + " ");
                    }
                    System.Console.WriteLine();
                }

                mydatareader.Close();

                sw.Start();

                //CREATE TABLE `test`.`tableinnodb` (`id` INT NULL, `col1` INT NULL, `col2` INT NULL) ENGINE = INNODB;
                //CREATE TABLE `test`.`tablemyisam` (`id` INT NULL, `col1` INT NULL, `col2` INT NULL) ENGINE = MYISAM;
                //CREATE TABLE `test`.`tablemem` (`id` INT NULL, `col1` INT NULL, `col2` INT NULL) ENGINE = MEMORY;

                int maxInsert = 10000;
                //800 insert par sec pour un p8400 en myisam/innodb avec mysql 5.1
                //22000 insert par sec pour un i7 4770k en myisam avec mysql 5.1 ou 5.6
                System.Console.WriteLine("\n for i:0->" + maxInsert + "insert into tableinnodb (id, col1, col2) values (i, 2, 3)");
                for (int i = 0; i < maxInsert; i++)
                {
                    mycommand.CommandText = "insert into tableinnodb (id, col1, col2) values (" + i + ", 2, 3)";
                    //mycommand.CommandText = "insert into tablemem (id, col1, col2) values (" + i + ", 2, 3)";
                    //mycommand.CommandText = "insert into tablemyisam (id, col1, col2) values (" + i + ", 2, 3)";
                    ret = mycommand.ExecuteNonQuery();
                    //on ferme le datareader pour pouvoir réexécuter une commande après
                    //mydatareader.Close();
                    if (i % 200 == 0)
                        System.Console.WriteLine((double)i + " in " + (double)sw.Elapsed.TotalMilliseconds / 1000 + " : " + (double)i / sw.Elapsed.TotalMilliseconds * 1000);
                }
                System.Console.WriteLine((double)maxInsert + " in " + (double)sw.Elapsed.TotalMilliseconds / 1000 + " : " + (double)maxInsert / sw.Elapsed.TotalMilliseconds * 1000);
                myconnection.Close();

            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        static void TestSqlServer()
        {
            Stopwatch sw = new Stopwatch();
            int ret;

            try
            {
                SqlConnection connexion = new SqlConnection();
                SqlCommand command = new SqlCommand();

                //connexion au serveur local en utilisant l'autentification windows
                //connexion.ConnectionString = "Integrated security = SSPI ; server = LOCALHOST\\SQLEXPRESS; database =testdb ";
                connexion.ConnectionString = "server = astral-axa\\SQLEXPRESS;user=refdoc;password=refdoc; database =refdoc ";

                //on ouvre
                connexion.Open();
                command.Connection = connexion;

                //affiche les données de SYSDATABASES
                /*System.Console.WriteLine("SELECT NAME FROM SYSDATABASES ORDER BY name ASC");
                command.CommandText = "SELECT NAME FROM SYSDATABASES ORDER BY name ASC";
                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    System.Console.WriteLine(datareader.GetValue(0).ToString());
                }
                //on ferme le datareader pour pouvoir réexécuter une commande après
                datareader.Close();

                //test des valeurs d'une table
                System.Console.WriteLine("\nSELECT * FROM testdb.dbo.Table_1");
                command.CommandText = "SELECT * FROM testdb.dbo.Table_1";
                datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    for (int i = 0; i < datareader.FieldCount; i++)
                    {
                        //si ça vaut NULL
                        if (datareader.GetValue(i) == System.DBNull.Value)
                        {
                            System.Console.Write("NULL ");
                        }
                        else
                        {
                            System.Console.Write(datareader.GetValue(i).ToString() + " ");
                        }
                    }
                    System.Console.WriteLine();
                }
                //on ferme le datareader pour pouvoir réexécuter une commande après
                datareader.Close();

                //update d'une valeur
                System.Console.WriteLine("\nUPDATE testdb.dbo.Table_1 SET val = 'a' + str(truc)  WHERE truc = 123");
                command.CommandText = "UPDATE testdb.dbo.Table_1 SET val = 'a' + str(truc)  WHERE truc = 123";
                ret = command.ExecuteNonQuery();
                //on ferme le datareader pour pouvoir réexécuter une commande après
                datareader.Close();*/

                //redémarre le compteur
                sw.Reset();
                sw.Start();

                //update d'une valeur
                //7300 insert par sec pour un i7 4770k avec sqlexpress 2008
                System.Console.WriteLine("\n for i:0->9999 insert into [testdb].[dbo].Table_1 (id, col1, col2) values (i, 2, 3)");
                for (int i = 0; i < 10000; i++)
                {
                    //command.CommandText = "insert into [testdb].[dbo].Table_1 (id, col1, col2) values (" + i + ", 2, 3)";
                    command.CommandText = "insert into [dbo].Table_1 (id, col1, col2) values (" + i + ", 2, 3)";
                    ret = command.ExecuteNonQuery();
                    if (i % 200 == 0)
                        System.Console.WriteLine((double)i + " in " + (double)sw.Elapsed.TotalMilliseconds / 1000 + " : " + (double)i / sw.Elapsed.TotalMilliseconds * 1000);
                    //on ferme le datareader pour pouvoir réexécuter une commande après
                    //datareader.Close();
                }


                //test des valeurs d'une table
                //System.Console.WriteLine("\nSELECT * FROM testdb.dbo.Table_1");
                //command.CommandText = "SELECT * FROM testdb.dbo.Table_1";
                //datareader = command.ExecuteReader();
                //while (datareader.Read())
                //{
                //    for (int i = 0; i < datareader.FieldCount; i++)
                //    {
                //        //si ça vaut NULL
                //        if (datareader.GetValue(i) == System.DBNull.Value)
                //        {
                //            System.Console.Write("NULL ");
                //        }
                //        else
                //        {
                //            System.Console.Write(datareader.GetValue(i).ToString() + " ");
                //        }
                //    }
                //    System.Console.WriteLine();
                //}


                //on ferme
                connexion.Close();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        static void TestOdbc()
        {
            try
            {
                System.Console.WriteLine("Odbc Test");

                //open connexion 
                OdbcConnection connection = new OdbcConnection("Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=..\\..\\;Extensions=asc,csv,tab,txt;");
                connection.Open();                

                //execute command
                OdbcCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM books.csv";
                OdbcDataReader datareader = command.ExecuteReader();

                List<Books> books = new List<Books>();
                Books b = new Books();
                b.Id = 0;
                b.Name = "";
                b.Author = "";
                b.Note = 0;
                
                //for each line
                while (datareader.Read())
                {
                    for (int i = 0; i < datareader.FieldCount; i++)
                    {
                        //no ; for 1st field
                        if (i != 0)
                        {
                            System.Console.Write(";");
                        }

                        //show NULL for NULL
                        if (datareader.GetValue(i) == System.DBNull.Value)
                        {
                            System.Console.Write("NULL ");
                        }
                        else
                        {
                            System.Console.Write(datareader.GetValue(i).ToString() + " ");
                        }
                        
                    }
                    books.Add(b);
                    System.Console.WriteLine();
                }

                //closing
                connection.Close();

                System.Console.WriteLine();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        static void TestSQLite()
        {
            try
            {
                System.Console.WriteLine("SQLite Test");

                //open connexion 
                SQLiteConnection connection = new SQLiteConnection("Data Source=..\\..\\books.sqlite;Version=3;");
                connection.Open();

                //execute command
                SQLiteCommand command = new SQLiteCommand("SELECT * FROM books", connection);
                SQLiteDataReader datareader = command.ExecuteReader();

                //for each line
                while (datareader.Read())
                {
                    for (int i = 0; i < datareader.FieldCount; i++)
                    {
                        //no ; for 1st field
                        if (i != 0)
                        {
                            System.Console.Write(";");
                        }

                        //show NULL for NULL
                        if (datareader.GetValue(i) == System.DBNull.Value)
                        {
                            System.Console.Write("NULL ");
                        }
                        else
                        {
                            System.Console.Write(datareader.GetValue(i).ToString() + " ");
                        }

                    }
                    //books.Add(b);
                    System.Console.WriteLine();
                }

                connection.Close();

                System.Console.WriteLine();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        static void Main(string[] args)
        {
            //TestMysql();

            //TestSqlServer();

            TestOdbc();

            TestSQLite();

            System.Console.WriteLine("Press any key to quit");
            System.Console.ReadKey();
        }
    }
}
