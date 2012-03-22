using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using MySql.Data.MySqlClient;

//pour se connecter à mysql : http://dev.mysql.com/downloads/connector/net/6.4.4.html (le 1.0.10 ne s'enregistre pas et est vieux...)
//ajout de la référence : C:\Program Files\MySQL\MySQL Connector Net 6.4.4\Assemblies\v2.0\MySql.Data.dll

namespace TestSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connexion = new SqlConnection();
            SqlCommand command = new SqlCommand();

            //connexion au serveur local en utilisant l'autentification windows
            connexion.ConnectionString = "Integrated security = SSPI ; server = YKALAS01A8-01\\SQLEXPRESS; database = ";            

            try
            {
                int ret;

                string MyConString = "SERVER=localhost;" +
                "DATABASE=test;";/* +
				"UID=testuser;" +
				"PASSWORD=testpassword;";*/
                MySqlConnection myconnection = new MySqlConnection(MyConString);
                MySqlCommand mycommand = myconnection.CreateCommand();
                MySqlDataReader mydatareader;
                mycommand.CommandText = "SELECT * FROM table_1";
                myconnection.Open();
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

                System.Console.WriteLine("\n for i:0->999 insert into table_1 (val, truc) values (i, '2')");
                for (int i = 0; i < 1000; i++)
                {
                    mycommand.CommandText = "insert into table_1 (val, truc) values (" + i + ", '2')";
                    ret = mycommand.ExecuteNonQuery();
                    //on ferme le datareader pour pouvoir réexécuter une commande après
                    mydatareader.Close();
                }
                myconnection.Close();

                //on ouvre
                connexion.Open();
                command.Connection = connexion;

                //affiche les données de SYSDATABASES
                System.Console.WriteLine("SELECT NAME FROM SYSDATABASES ORDER BY name ASC");
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
                datareader.Close();

                //update d'une valeur
                System.Console.WriteLine("\n for i:0->999 insert into [testdb].[dbo].Table_1 (val, truc) values (i, '2')");
                for (int i = 0; i < 1000; i++)
                {
                    command.CommandText = "insert into [testdb].[dbo].Table_1 (val, truc) values (" + i + ", '2')";
                    ret = command.ExecuteNonQuery();
                    //on ferme le datareader pour pouvoir réexécuter une commande après
                    datareader.Close();
                }
                

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


                //on ferme
                connexion.Close();                
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
            }

            System.Console.ReadLine();
        }
    }
}
