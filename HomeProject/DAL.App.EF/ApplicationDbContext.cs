using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contracts.DAL.Base;
using Domain;
using Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<Profile, MRole, Guid>
    {
        public DbSet<Profile> Profiles { get; set; } = default!;
        public DbSet<BlockedProfile> BlockedProfiles { get; set; } = default!;
        public DbSet<ProfileGift> ProfileGifts { get; set; } = default!;
        public DbSet<ProfileRank> ProfileRanks { get; set; } = default!;
        public DbSet<Follower> Followers { get; set; } = default!;

        public DbSet<Gift> Gifts { get; set; } = default!;
        public DbSet<Rank> Ranks { get; set; } = default!;
        public DbSet<Follower> Watchers { get; set; } = default!;

        public DbSet<Image> Images { get; set; } = default!;

        public DbSet<Post> Posts { get; set; } = default!;
        public DbSet<Comment> Comments { get; set; } = default!;
        public DbSet<Favorite> Favorites { get; set; } = default!;

        public DbSet<ChatRoom> ChatRooms { get; set; } = default!;
        public DbSet<Message> Messages { get; set; } = default!;
        public DbSet<ChatMember> ChatMembers { get; set; } = default!;
        public DbSet<ChatRole> ChatRoles { get; set; } = default!;

        private readonly IUserNameProvider _userNameProvider;

        public ApplicationDbContext(DbContextOptions options, IUserNameProvider userNameProvider) : base(options)
        {
            _userNameProvider = userNameProvider;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Profile>(b => b.ToTable("Profile"));
            builder.Entity<MRole>(b => b.ToTable("UserRole"));

            //remove cascade delete
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //rank relation to itself (previous -> current -> next)
            builder.Entity<Rank>()
                .HasOne(rank => rank.NextRank)
                .WithOne()
                .HasForeignKey<Rank>(rank => rank.NextRankId);

            builder.Entity<Rank>()
                .HasOne(rank => rank.PreviousRank)
                .WithOne()
                .HasForeignKey<Rank>(rank => rank.PreviousRankId);

            /*//create unique indexes
            builder.Entity<ChatRole>().HasIndex(role => role.RoleTitle).IsUnique();
            builder.Entity<Rank>().HasIndex(rank => rank.RankCode).IsUnique();
            builder.Entity<Gift>().HasIndex(gift => gift.GiftCode).IsUnique();*/
        }

        private void SaveChangesMetadataUpdate()
        {
            // update the state of ef tracked objects
            ChangeTracker.DetectChanges();

            var markedAsAdded = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);

            foreach (var entityEntry in markedAsAdded)
            {
                if (!(entityEntry.Entity is IDomainEntityMetadata entityWithMetaData)) continue;
                if (entityEntry.Entity is ISoftUpdateEntity softUpdateEntity && softUpdateEntity.MasterId != null) continue;
                
                entityWithMetaData.CreatedAt = DateTime.Now;

                if (entityWithMetaData.CreatedBy == null)
                {
                    entityWithMetaData.CreatedBy = _userNameProvider.CurrentUserName;
                }

                entityWithMetaData.ChangedAt = entityWithMetaData.CreatedAt;
                entityWithMetaData.ChangedBy = entityWithMetaData.CreatedBy;
            }
            
            var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);
            
            foreach (var entityEntry in markedAsDeleted)
            {
                if (!(entityEntry.Entity is ISoftDeleteEntity softDeleteEntity)) continue;

                softDeleteEntity.DeletedAt = DateTime.Now;
                softDeleteEntity.DeletedBy = _userNameProvider.CurrentUserName;

                entityEntry.State = EntityState.Modified;
            }

            var markedAsModified = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);

            foreach (var entityEntry in markedAsModified)
            {
                // check for IDomainEntityMetadata
                if (!(entityEntry.Entity is IDomainEntityMetadata entityWithMetaData)) continue;

                entityWithMetaData.ChangedAt = DateTime.Now;
                entityWithMetaData.ChangedBy = _userNameProvider.CurrentUserName;

                if (entityEntry.Entity is ISoftUpdateEntity) continue;
                entityEntry.Property(nameof(entityWithMetaData.CreatedAt)).IsModified = false;
                entityEntry.Property(nameof(entityWithMetaData.CreatedBy)).IsModified = false;
            }
        }

        public override int SaveChanges()
        {
            SaveChangesMetadataUpdate();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SaveChangesMetadataUpdate();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}