namespace WebApiTest.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class AmazonContext : DbContext
    {
        public AmazonContext()
            : base("name=AmazonContext")
        {
        }

        public DbSet<Item> Items { get; set; }
    }
}