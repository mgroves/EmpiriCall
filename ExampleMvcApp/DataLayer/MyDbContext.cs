using System.Data.Common;
using System.Data.Entity;

namespace ExampleMvcApp.DataLayer
{
    public class MyDbContext : DbContext
    {
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Author> Authors { get; set; }

        public MyDbContext(DbConnection connection) : base(connection, false)
        {
        }
    }

    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public virtual Author Author { get; set; }
    }

    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}