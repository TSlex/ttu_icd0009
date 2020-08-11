using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.Identity;
using Domain.Translation;
using ee.itcollege.aleksi.Contracts.DAL.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        //add own DbSets here
        public DbSet<HomeWork> HomeWorks { get; set; } = default!;
        public DbSet<Semester> Semesters { get; set; } = default!;
        public DbSet<StudentSubject> StudentSubjects { get; set; } = default!;
        public DbSet<StudentHomeWork> StudentHomeWorks { get; set; } = default!;
        public DbSet<Subject> Subjects { get; set; } = default!;

        public DbSet<LangString> LangStrings { get; set; } = default!;
        public DbSet<Translation> Translations { get; set; } = default!;

        private readonly IUserNameProvider _userNameProvider;

        public ApplicationDbContext(DbContextOptions options, IUserNameProvider userNameProvider) : base(options)
        {
            _userNameProvider = userNameProvider;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(b => b.ToTable("AppUser"));
            builder.Entity<AppRole>(b => b.ToTable("UserRole"));

            //remove cascade delete
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private void SaveChangesMetadataUpdate()
        {
            // update the state of ef tracked objects
            ChangeTracker.DetectChanges();

            var markedAsAdded = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);

            foreach (var entityEntry in markedAsAdded)
            {
                if (!(entityEntry.Entity is IDomainEntityMetadata entityWithMetaData)) continue;
                if (entityEntry.Entity is ISoftUpdateEntity softUpdateEntity &&
                    softUpdateEntity.MasterId != null) continue;

                entityWithMetaData.CreatedAt = DateTime.UtcNow;

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
                if (entityEntry.Entity is ISoftDeleteEntity softDeleteEntity)
                {
                    softDeleteEntity.DeletedAt = DateTime.UtcNow;
                    softDeleteEntity.DeletedBy = _userNameProvider.CurrentUserName;

                    entityEntry.State = EntityState.Modified;
                }
            }

            var markedAsModified = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);

            foreach (var entityEntry in markedAsModified)
            {
                // check for IDomainEntityMetadata
                if (!(entityEntry.Entity is IDomainEntityMetadata entityWithMetaData)) continue;

                entityWithMetaData.ChangedAt = DateTime.UtcNow;
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