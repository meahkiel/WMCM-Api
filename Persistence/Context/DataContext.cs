using Core.Campaigns;
using Core.Contacts;
using Core.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context 
{ 
    public class DataContext : IdentityDbContext<AppUser>
    {

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Contact> Templates { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contact>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Contact>()
                .Property(t => t.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Campaign)
                .WithMany(a => a.Activities);

            modelBuilder.Entity<ActivityDetail>()
                .HasOne(d => d.Activity)
                .WithMany(a => a.Details);

        }
    }
}
