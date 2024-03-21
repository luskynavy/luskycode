using SQLite;

namespace SqliteNetPclConsole
{
    public class Blog
    {
        [PrimaryKey, AutoIncrement]
        public int BlogId { get; set; }

        public string Url { get; set; }

        [Ignore]
        public List<Post> Posts { get; } = new();
    }

    public class Post
    {
        [PrimaryKey, AutoIncrement]
        public int PostId { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }

        [Indexed]
        public int BlogId { get; set; }

        [Ignore]
        public Blog Blog { get; set; }
    }
}