using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CoVisiting.Data.Models;

namespace CoVisiting.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventReply> EventReplies { get; set; }
        public DbSet<Moving> Moving { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserEventJoinTable>().HasKey(t => new { t.EventId, t.UserId });
            base.OnModelCreating(builder);
        }
    }
}
