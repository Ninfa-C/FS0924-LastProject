using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LastProject.Models.Auth;
using LastProject.Models.Main;
using System.Reflection.Emit;

namespace LastProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUserRole>()
               .Property(p => p.Date)
               .HasDefaultValueSql("GETDATE()")
               .IsRequired(true);

            builder.Entity<ApplicationUserRole>()
                .HasOne(a => a.Role)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(a => a.RoleId);
            builder.Entity<ApplicationUserRole>()
                .HasOne(a => a.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(a => a.UserId);
            builder.Entity<ApplicationRole>().HasData(
               new ApplicationRole { Id = "1", Name = "Amministratore", NormalizedName = "AMMINISTRATORE", ConcurrencyStamp = "1" },
               new ApplicationRole { Id = "2", Name = "Utente", NormalizedName = "UTENTE", ConcurrencyStamp = "2" }
               );
            builder.Entity<Event>()
                .HasOne(e => e.Artist)
                .WithMany(a => a.Events)
                .HasForeignKey(e => e.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Ticket>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
