using System;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<Profile>
    {

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<BlockedProfile> BlockedProfiles { get; set; }
        public DbSet<ProfileGift> ProfileGifts { get; set; }
        public DbSet<ProfileRank> ProfileRanks { get; set; }
        
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Follower> Watchers { get; set; }
        
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        
        public DbSet<ChatRoom> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatMember> ChatMembers { get; set; }
        public DbSet<ChatRole> ChatRoles { get; set; }


        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

//            builder.Ignore<BlockedProfile>();
            
            builder.Entity<Profile>(b => b.ToTable("Profile"));

            builder.Entity<BlockedProfile>()
                .HasOne(m => m.Profile)
                .WithMany(p => p.BlockedByProfiles)
                .IsRequired()
                .HasForeignKey(p => p.ProfileId);
            
            builder.Entity<BlockedProfile>()
                .HasOne(m => m.BProfile)
                .WithMany(p => p.BlockedProfiles)
                .IsRequired()
                .HasForeignKey(p => p.BProfileId);


            builder.Entity<Follower>()
                .HasOne(m => m.Profile)
                .WithMany(p => p.Followed)
                .IsRequired()
                .HasForeignKey(p => p.ProfileId);
            
            builder.Entity<Follower>()
                .HasOne(m => m.FollowerProfile)
                .WithMany(p => p.Followers)
                .IsRequired()
                .HasForeignKey(p => p.FollowerProfileId);
        }
    }
}