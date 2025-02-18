using Microsoft.EntityFrameworkCore;
using SolarPlantDemo.Models.Entities;

namespace SolarPlantDemo.Data;

public class SolarPlantDbContext(DbContextOptions<SolarPlantDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<PowerPlant> PowerPlants { get; set; }

    public DbSet<PlantRecord> PlantRecords { get; set; }
}