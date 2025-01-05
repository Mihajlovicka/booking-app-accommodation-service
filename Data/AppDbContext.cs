using AccommodationService.Model.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccommodationService.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    public DbSet<Accommodation> Accommodations { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<User> Users { get; set; }
    
    public void SeedData()
    {
        if (!Equipments.Any())
        {
            Equipments.Add(new Equipment { Name = "Wifi" });
            Equipments.Add(new Equipment { Name = "Playstation" });
            Equipments.Add(new Equipment { Name = "Air Conditioner" });
            Equipments.Add(new Equipment { Name = "Jacuzzi" });
            Equipments.Add(new Equipment { Name = "Washing Machine" });
            Equipments.Add(new Equipment { Name = "Smart TV" });
            Equipments.Add(new Equipment { Name = "BBQ Grill" });
            Equipments.Add(new Equipment { Name = "Refrigerator" });
            Equipments.Add(new Equipment { Name = "King-Size Bed" });
            Equipments.Add(new Equipment { Name = "Dishwasher" });
            Equipments.Add(new Equipment { Name = "Microwave Oven" });
            Equipments.Add(new Equipment { Name = "Coffee Maker" });
            Equipments.Add(new Equipment { Name = "Heater" });
            Equipments.Add(new Equipment { Name = "Electric Kettle" });
            Equipments.Add(new Equipment { Name = "Toaster" });
            Equipments.Add(new Equipment { Name = "Blender" });
            Equipments.Add(new Equipment { Name = "Ceiling Fan" });
            Equipments.Add(new Equipment { Name = "Ironing Board" });
            Equipments.Add(new Equipment { Name = "Hair Dryer" });
            Equipments.Add(new Equipment { Name = "Safe Deposit Box" });
            Equipments.Add(new Equipment { Name = "Exercise Bike" });
            Equipments.Add(new Equipment { Name = "Pool Table" });
            Equipments.Add(new Equipment { Name = "Smart Speaker" });
            Equipments.Add(new Equipment { Name = "Baby Crib" });
            Equipments.Add(new Equipment { Name = "Cleaning Supplies" });
            Equipments.Add(new Equipment { Name = "Treadmill" });
            Equipments.Add(new Equipment { Name = "Projector" });
            
            // Add more items here if needed
            SaveChanges();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Picture>()
            .HasOne(p => p.Accommodation)
            .WithMany(a => a.Pictures)
            .HasForeignKey(p => p.AccommodationId);

        modelBuilder.Entity<Accommodation>()
            .HasOne(a => a.Owner)
            .WithMany()
            .HasForeignKey(a => a.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Accommodation>()
            .HasOne(a => a.Address)
            .WithMany()
            .HasForeignKey(a => a.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the many-to-many relationship between Accommodation and Equipment
        modelBuilder.Entity<Accommodation>()
            .HasMany(a => a.Equipment)
            .WithMany(e => e.Accommodations)
            .UsingEntity(j => j.ToTable("AccommodationEquipments"));

        modelBuilder.Entity<User>()
            .HasIndex(u => u.ExternalId)
            .IsUnique();

        modelBuilder.Entity<Address>()
            .HasIndex(a => new { a.StreetNumber, a.StreetName, a.City, a.Country });

        modelBuilder.Entity<Equipment>()
            .HasIndex(e => e.Name)
            .IsUnique();
    }

}