using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ReportingDb : DbContext
    {
        public ReportingDb(DbContextOptions<ReportingDb> options) : base(options) { }

        public DbSet<CountriesTurnoverStat> CountriesTurnoverReport { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CountriesTurnoverStat>()
                .ToTable(nameof(CountriesTurnoverReport), t => t.ExcludeFromMigrations())
                .HasNoKey();
        }
    }

    public class IdentityDb : IdentityDbContext<AppUser, AppRole, int, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>, IIdentityDb
    {
        public IdentityDb(DbContextOptions<IdentityDb> options) : base(options) { }

        //public DbSet<AppUser> Users { get; set; }
        //public DbSet<AppRole> Roles { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(IdentityDb).Assembly);

            builder.Entity<AppUser>(b =>
            {
                b.ToTable("Users");
                b.Property(u => u.UserName).HasMaxLength(256).IsRequired();
                b.Property(u => u.Email).HasMaxLength(256).IsRequired();
                b.Property(u => u.FirstName).HasMaxLength(100);
                b.Property(u => u.LastName).HasMaxLength(100);
                b.Property(u => u.IsActive).IsRequired().HasDefaultValue(true);
                b.HasIndex(u => u.UserName).IsUnique();
            });

            builder.Entity<AppRole>(b =>
            {
                b.ToTable("Roles");
                b.Property(r => r.Name).HasMaxLength(50).IsRequired();
                b.Property(r => r.NormalizedName).HasMaxLength(50).IsRequired();
            });

            builder.Entity<AppUserRole>(b =>
            {
                b.ToTable("UserRoles");
                b.HasKey(ur => new { ur.UserId, ur.RoleId });
                b.HasOne(ur => ur.User).WithMany(u => u.Roles).HasForeignKey(ur => ur.UserId);
                b.HasOne(ur => ur.Role).WithMany().HasForeignKey(ur => ur.RoleId);
            });

            builder.Entity<AppUserClaim>(b =>
            {
                b.ToTable("UserClaims");
            });

            builder.Entity<AppUserLogin>(b =>
            {
                b.ToTable("UserLogins");
                b.HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });
            });

            builder.Entity<AppRoleClaim>(b =>
            {
                b.ToTable("RoleClaims");
            });

            builder.Entity<AppUserToken>(b =>
            {
                b.ToTable("UserTokens");
                b.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });
            });
        }
    }
}