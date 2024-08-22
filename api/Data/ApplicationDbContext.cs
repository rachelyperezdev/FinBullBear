using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions dbContext) : base(dbContext) { }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Tables
            builder.Entity<Stock>().ToTable("Stocks");
            builder.Entity<Comment>().ToTable("Comments");
            builder.Entity<Portfolio>().ToTable("Portfolios");
            #endregion

            #region Primary Keys
            builder.Entity<Stock>().HasKey(s => s.Id);
            builder.Entity<Comment>().HasKey(c => c.Id);
            builder.Entity<Portfolio>().HasKey(p => new { p.AppUserId, p.StockId });
            #endregion

            #region Relationships
            builder.Entity<Portfolio>()
                   .HasOne(p => p.AppUser)
                   .WithMany(u => u.Portfolios)
                   .HasForeignKey(p => p.AppUserId);

            builder.Entity<Portfolio>()
                   .HasOne(p => p.Stock)
                   .WithMany(s => s.Portfolios)
                   .HasForeignKey(p => p.StockId);
            #endregion

            #region Identity

            #region Tables
            builder.Entity<AppUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable(name: "UserRoles");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable(name: "UserLogins");
            });
            #endregion

            List<IdentityRole> roles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Name = api.Enums.Roles.Admin.ToString(),
                    NormalizedName = api.Enums.Roles.Admin.ToString().ToUpper()
                },
                new IdentityRole()
                {
                    Name = api.Enums.Roles.User.ToString(),
                    NormalizedName = api.Enums.Roles.User.ToString().ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
            #endregion
        }
    }
}
