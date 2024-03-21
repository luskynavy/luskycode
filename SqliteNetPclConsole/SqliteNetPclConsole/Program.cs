using SQLite;

namespace SqliteNetPclConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var db = new SQLiteConnection("blogging.db");
            db.CreateTable<Blog>();
            db.CreateTable<Post>();

            Console.WriteLine($"Database path: {db.DatabasePath}.");

            Console.WriteLine("Blog count : " + db.Table<Blog>().Count());
            Console.WriteLine("Post count : " + db.Table<Post>().Count());

            // Create
            Console.WriteLine("Inserting a new blog");
            var blog = new Blog()
            {
                Url = "http://blogs.msdn.com/adonet"
            };
            db.Insert(blog);

            Console.WriteLine("Blog count : " + db.Table<Blog>().Count());
            Console.WriteLine("Post count : " + db.Table<Post>().Count());

            // Read
            Console.WriteLine("Querying for a blog");
            blog = db.Table<Blog>()
                .OrderBy(b => b.BlogId)
                .First();

            // Update
            Console.WriteLine("Updating the blog and adding a post");
            blog.Url = "https://devblogs.microsoft.com/dotnet";
            var post = new Post { Title = "Hello World", Content = "I wrote an app using .Net Core!", BlogId = blog.BlogId };
            db.Insert(post);

            Console.WriteLine("Blog count : " + db.Table<Blog>().Count());
            Console.WriteLine("Post count : " + db.Table<Post>().Count());

            // Delete
            Console.WriteLine("Delete the post");
            db.Delete(post);

            Console.WriteLine("Delete the blog");
            db.Delete(blog);

            Console.WriteLine("Blog count : " + db.Table<Blog>().Count());
            Console.WriteLine("Post count : " + db.Table<Post>().Count());
        }
    }
}