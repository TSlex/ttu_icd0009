using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Domain;
using Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<Profile, MRole, Guid>
    {
        public DbSet<Profile> Profiles { get; set; } = default!;
        public DbSet<BlockedProfile> BlockedProfiles { get; set; } = default!;
        public DbSet<ProfileGift> ProfileGifts { get; set; } = default!;
        public DbSet<ProfileRank> ProfileRanks { get; set; } = default!;
        
        public DbSet<Gift> Gifts { get; set; } = default!;
        public DbSet<Rank> Ranks { get; set; } = default!;
        public DbSet<Follower> Watchers { get; set; } = default!;
        
        public DbSet<Post> Posts { get; set; } = default!;
        public DbSet<Comment> Comments { get; set; } = default!;
        public DbSet<Favorite> Favorites { get; set; } = default!;
        
        public DbSet<ChatRoom> ChatRooms { get; set; } = default!;
        public DbSet<Message> Messages { get; set; } = default!;
        public DbSet<ChatMember> ChatMembers { get; set; } = default!;
        public DbSet<ChatRole> ChatRoles { get; set; } = default!;


        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

//            builder.Ignore<BlockedProfile>();
            
            builder.Entity<Profile>(b => b.ToTable("Profile"));
            builder.Entity<MRole>(b => b.ToTable("UserRole"));
            
            //remove cascade delete
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

//            builder.Entity<BlockedProfile>()
//                .HasOne(m => m.Profile)
//                .WithMany(p => p.BlockedByProfiles)
//                .OnDelete(DeleteBehavior.NoAction)
//                .IsRequired()
//                .HasForeignKey(p => p.ProfileId);
//            
//            builder.Entity<BlockedProfile>()
//                .HasOne(m => m.BProfile)
//                .WithMany(p => p.BlockedProfiles)
//                .OnDelete(DeleteBehavior.NoAction)
//                .IsRequired()
//                .HasForeignKey(p => p.BProfileId);
//
//
//            builder.Entity<Follower>()
//                .HasOne(m => m.Profile)
//                .WithMany(p => p.Followed)
//                .OnDelete(DeleteBehavior.NoAction)
//                .IsRequired()
//                .HasForeignKey(p => p.ProfileId);
//            
//            builder.Entity<Follower>()
//                .HasOne(m => m.FollowerProfile)
//                .WithMany(p => p.Followers)
//                .OnDelete(DeleteBehavior.NoAction)
//                .IsRequired()
//                .HasForeignKey(p => p.FollowerProfileId);

            
        }
    }
}