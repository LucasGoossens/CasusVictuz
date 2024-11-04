﻿using Microsoft.EntityFrameworkCore;
using Casusvictuz;
using CasusVictuz.Models;

namespace CasusVictuz.Data
{
    public class VictuzDb : DbContext
    {
        public VictuzDb(DbContextOptions<VictuzDb> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Casusvictuz.Thread> Threads { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Registration> Registrations { get; set; } = null!;
        public DbSet<Event> Events { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Your existing configurations
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Thread)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.ThreadId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.User)
                .WithMany(u => u.Registrations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Event)
                .WithMany(e => e.Registrations)
                .HasForeignKey(r => r.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tag>()
                .HasOne<Event>()
                .WithMany(e => e.Tags)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
