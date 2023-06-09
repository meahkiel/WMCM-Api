﻿using Core.Campaigns;
using Core.Channels;
using Core.Contacts;
using Core.Extensions;
using Core.Notifications;
using Core.Tasks;
using Core.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Context 
{
    public class DataContext : IdentityDbContext<AppUser>
    {

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<MarketingTask> MarketingTasks { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }


        public DbSet<ChannelSetting> ChannelSettings { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            

            modelBuilder.Entity<Contact>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Template>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Contact>()
                .Property(t => t.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Campaign>()
                .HasMany(c => c.Activities)
                .WithOne(a => a.Campaign);

           
            modelBuilder.Entity<ChannelSetting>().HasKey(c => c.Id);

            modelBuilder.ApplyConfiguration(new MarketingTaskConfiguration());
            modelBuilder.ApplyConfiguration(new SubTaskConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();
            var timestamp = DateTime.Now;

            foreach (var entry in ChangeTracker.Entries()
                   .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                if (entry.Entity.GetType().GetCustomAttributes(typeof(AuditableAttribute), true).Length > 0)
                {
                    entry.Property("LastUpdatedOn").CurrentValue = timestamp;
                    if(entry.State == EntityState.Added)
                    {
                        entry.Property("CreatedOn").CurrentValue = timestamp;
                    }
                }
                
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
