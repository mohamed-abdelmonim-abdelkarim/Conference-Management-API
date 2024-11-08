using conferenceManagement.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace conferenceManagement.Data
{
    public class ConferenceDbContext:IdentityDbContext<IdentityUser>
    {
        public ConferenceDbContext(DbContextOptions<ConferenceDbContext>options):base(options)
        {
            
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<SessionAttendee> SessionAttendees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SessionAttendee>()
                .HasKey(sa => new { sa.SessionId, sa.AttendeeId });

            modelBuilder.Entity<SessionAttendee>()
                .HasOne(sa => sa.Session)
                .WithMany(s => s.SessionAttendees)
                .HasForeignKey(sa => sa.SessionId);

            modelBuilder.Entity<SessionAttendee>()
                .HasOne(sa => sa.Attendee)
                .WithMany(a => a.SessionAttendees)
                .HasForeignKey(sa => sa.AttendeeId);
        }
    }
}
