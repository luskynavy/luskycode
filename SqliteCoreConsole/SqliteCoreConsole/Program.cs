using System;
using System.Linq;

namespace SqliteCoreConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var folder = Environment.CurrentDirectory;
            using var db = new BloggingContext();

            //Pour créér la bdd, sur la Package Manager Console
            //Add-Migration InitialCreate (pas obligatoire car la migration n'a rien fait de plus ?)
            //Update-Database

            // Note: This sample requires the database to be created before running.
            //Construit le chemin du projet en remontant \bin\Debug\netX.0
            db.DbPath = System.IO.Path.Join(folder + "\\..\\..\\..\\", "blogging.db");
            Console.WriteLine($"Database path: {db.DbPath}.");

            Console.WriteLine("Blog count : " + db.Blogs.Count());

            // Create
            Console.WriteLine("Inserting a new blog");
            db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
            db.SaveChanges();

            Console.WriteLine("Blog count : " + db.Blogs.Count());

            // Read
            Console.WriteLine("Querying for a blog");
            var blog = db.Blogs
                .OrderBy(b => b.BlogId)
                .First();

            // Update
            Console.WriteLine("Updating the blog and adding a post");
            blog.Url = "https://devblogs.microsoft.com/dotnet";
            blog.Posts.Add(
                new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
            db.SaveChanges();

            // Delete
            Console.WriteLine("Delete the blog");
            db.Remove(blog);
            db.SaveChanges();

            Console.WriteLine("Blog count : " + db.Blogs.Count());
        }
    }
}