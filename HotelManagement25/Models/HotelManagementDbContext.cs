using Microsoft.EntityFrameworkCore;
using HotelManagement25.Models;

namespace HotelManagement25.Models;

public class HotelManagementDbContext : DbContext
{
    public HotelManagementDbContext(DbContextOptions<HotelManagementDbContext> options)
    : base(options)
    {
        
    }
    public virtual DbSet<Room> Room { get; set; } //TODO: should make it interface
    public virtual DbSet<Customer> Customer { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>(entity =>
        {
            entity.ToTable("room");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Status).HasColumnName("status").HasDefaultValue(Constants.RoomStatus.Available.ToString());
            entity.Property(e => e.Floor).HasColumnName("floor");
            entity.Property(e => e.Unit).HasColumnName("unit");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("customer");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });
        
        modelBuilder.Entity<RoomAssignment>(entity =>
        {
            entity.ToTable("room_assignment");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Customer)
                .WithMany(e => e.RoomAssignments)
                .HasForeignKey(e => e.CustomerId);
            entity.HasOne(e => e.Room)
                .WithOne()
                .HasForeignKey<RoomAssignment>(e => e.RoomId);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
        });
        
    }

public DbSet<HotelManagement25.Models.RoomAssignment> RoomAssignment { get; set; } = default!;
}