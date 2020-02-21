using Domain;
using Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, string>
    {

        public DbSet<Author> Authors { get; set; } = default!;
        public DbSet<AuthorPicture> AuthorPictures { get; set; } = default!;
        public DbSet<Post> Posts { get; set; } = default!;
        public DbSet<PostCategory> PostCategories { get; set; } = default!;
        public DbSet<Category>  Categories { get; set; } = default!;
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Author>().Ignore(author => author.FirstLastName);
        }
    }
}