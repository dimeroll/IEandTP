using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Lab1_ICtaTP
{
    public partial class Lab1DBContext : DbContext
    {
        public Lab1DBContext()
        {
        }

        public Lab1DBContext(DbContextOptions<Lab1DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DeparturePoint> DeparturePoints { get; set; }
        public virtual DbSet<Destination> Destinations { get; set; }
        public virtual DbSet<Journey> Journeys { get; set; }
        public virtual DbSet<Passenger> Passengers { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Train> Trains { get; set; }
        public virtual DbSet<TrainType> TrainTypes { get; set; }
        public virtual DbSet<TrainWorker> TrainWorkers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server= DESKTOP-AHTAE72; Database=Lab1DB; \nTrusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<DeparturePoint>(entity =>
            {
                entity.Property(e => e.DeparturePointId).HasColumnName("DeparturePointID");

                entity.Property(e => e.DeparturePointName).IsRequired();
            });

            modelBuilder.Entity<Destination>(entity =>
            {
                entity.Property(e => e.DestinationId).HasColumnName("DestinationID");

                entity.Property(e => e.DestinationName).IsRequired();
            });

            modelBuilder.Entity<Journey>(entity =>
            {
                entity.Property(e => e.JourneyId).HasColumnName("JourneyID");

                entity.Property(e => e.DeparturePointId).HasColumnName("DeparturePointID");

                entity.Property(e => e.DestinationId).HasColumnName("DestinationID");

                entity.Property(e => e.TrainId).HasColumnName("TrainID");

                entity.HasOne(d => d.DeparturePoint)
                    .WithMany(p => p.Journeys)
                    .HasForeignKey(d => d.DeparturePointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Journeys_DeparturePoints");

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.Journeys)
                    .HasForeignKey(d => d.DestinationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Journeys_Destinations");

                entity.HasOne(d => d.Train)
                    .WithMany(p => p.Journeys)
                    .HasForeignKey(d => d.TrainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Journeys_Trains");
            });

            modelBuilder.Entity<Passenger>(entity =>
            {
                entity.HasKey(e => e.PassportId);

                entity.Property(e => e.PassportId).HasColumnName("PassportID");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Surname).IsRequired();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleName).IsRequired();
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.TicketId).HasColumnName("TicketID");

                entity.Property(e => e.JourneyId).HasColumnName("JourneyID");

                entity.Property(e => e.PassengerId).HasColumnName("PassengerID");

                entity.HasOne(d => d.Journey)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.JourneyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tickets_Journeys");

                entity.HasOne(d => d.Passenger)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.PassengerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tickets_Passengers");
            });

            modelBuilder.Entity<Train>(entity =>
            {
                entity.Property(e => e.TrainId).HasColumnName("TrainID");

                entity.Property(e => e.AdditionalInfo).HasMaxLength(50);

                entity.Property(e => e.TrainTypeId).HasColumnName("TrainTypeID");

                entity.HasOne(d => d.TrainType)
                    .WithMany(p => p.Trains)
                    .HasForeignKey(d => d.TrainTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trains_TrainTypes");
            });

            modelBuilder.Entity<TrainType>(entity =>
            {
                entity.Property(e => e.TrainTypeId).HasColumnName("TrainTypeID");

                entity.Property(e => e.TrainTypeName).IsRequired();
            });

            modelBuilder.Entity<TrainWorker>(entity =>
            {
                entity.HasKey(e => e.WorkerId);

                entity.Property(e => e.WorkerId).HasColumnName("WorkerID");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Surname).IsRequired();

                entity.Property(e => e.TrainId).HasColumnName("TrainID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TrainWorkers)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainWorkers_Roles");

                entity.HasOne(d => d.Train)
                    .WithMany(p => p.TrainWorkers)
                    .HasForeignKey(d => d.TrainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainWorkers_Trains");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
