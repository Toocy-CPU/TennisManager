using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TennisManager.Models;

namespace TennisManager.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<TournamentParticipant> TournamentParticipants { get; set; }

        public DbSet<SetResult> SetResults { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Match>()
                .HasOne(m => m.PlayerA)
                .WithMany()
                .HasForeignKey(m => m.PlayerAId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Match>()
                .HasOne(m => m.PlayerB)
                .WithMany()
                .HasForeignKey(m => m.PlayerBId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Tournament>()
                .HasOne(t => t.CreatedByUser)
                .WithMany()
                .HasForeignKey(t => t.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Match>()
                .HasOne(m => m.Winner)
                .WithMany()
                .HasForeignKey(m => m.WinnerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Match>()
                .HasOne(m => m.NextMatch)
                .WithMany()
                .HasForeignKey(m => m.NextMatchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
