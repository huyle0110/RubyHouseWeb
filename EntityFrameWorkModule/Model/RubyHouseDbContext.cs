using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EntityFrameWorkModule.Model;

namespace EntityFrameWorkModule.EF
{
    public partial class RubyHouseDbContext : DbContext
    {
        public RubyHouseDbContext()
            : base("name=RubyHouseDbContext")
        {
            
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<RubyHouseDbContext, EntityFrameWorkModule.Migrations.Configuration>());
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
