using Microsoft.EntityFrameworkCore;
using SolarPlantDemo.Data;
using SolarPlantDemo.Models.Entities;

namespace SolarPlantDemo.Repositories;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User user);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    
    //
    
    Task SaveChangesAsync();
}

internal class UserRepository(SolarPlantDbContext dbContext) : IUserRepository
{
    public async Task<User> CreateUserAsync(User user)
    {
        await dbContext.Users.AddAsync(user);
        return user;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        return await dbContext.RefreshTokens
            .Where(rt => rt.Token == refreshToken)
            .Select(rt => rt.User)
            .FirstOrDefaultAsync();
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}