using Microsoft.EntityFrameworkCore;
using SolarPlantDemo.Models.Entities;

namespace SolarPlantDemo.Data;

public class SolarPlantDbContext(DbContextOptions<SolarPlantDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}