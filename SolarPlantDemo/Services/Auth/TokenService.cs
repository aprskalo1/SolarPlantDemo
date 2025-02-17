using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SolarPlantDemo.Data;
using SolarPlantDemo.Exceptions;
using SolarPlantDemo.Models.Common;
using SolarPlantDemo.Models.Entities;
using SolarPlantDemo.Repositories;

namespace SolarPlantDemo.Services.Auth;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    Task<string> GenerateRefreshToken(Guid userId);
    Task<TokensResponse> RefreshAccessToken(string refreshToken, Guid userId);
}

internal class TokenService(IConfiguration configuration, SolarPlantDbContext dbContext, IUserRepository userRepository) : ITokenService
{
    public string GenerateAccessToken(User user)
    {
        var jwtSettings = configuration.GetSection("JWT");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var userClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? ""),
            new(ClaimTypes.MobilePhone, user.FirstName ?? ""),
            new(ClaimTypes.MobilePhone, user.LastName ?? ""),
            new(ClaimTypes.MobilePhone, user.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"))
        };

        var accessToken = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: userClaims,
            expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpirationInMinutes"]!)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(accessToken);
    }

    public async Task<string> GenerateRefreshToken(Guid userId)
    {
        const int tokenLengthInBytes = 64;
        var randomBytes = new byte[tokenLengthInBytes];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        var refreshToken = Convert.ToBase64String(randomBytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");

        await CreateRefreshToken(userId, refreshToken);

        return refreshToken;
    }

    public async Task<TokensResponse> RefreshAccessToken(string refreshToken, Guid userId)
    {
        var validRefreshToken = await dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.UserId == userId);
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user == null)
            throw new UserNotFoundException("User not found");

        if (validRefreshToken == null)
            throw new UnauthorizedException("Invalid refresh token");

        if (validRefreshToken.Expires < DateTime.Now)
            throw new UnauthorizedException("Refresh token expired");

        var accessToken = GenerateAccessToken(user);
        var newRefreshToken = await GenerateRefreshToken(userId);

        await CreateRefreshToken(userId, newRefreshToken, true);

        return new TokensResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        };
    }

    private async Task CreateRefreshToken(Guid userId, string newRefreshToken, bool updateExisting = false)
    {
        var existingRefreshToken = await dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId);

        if (existingRefreshToken != null)
        {
            existingRefreshToken.Token = newRefreshToken;

            if (!updateExisting)
            {
                existingRefreshToken.Expires = DateTime.Now.AddDays(7);
            }
        }
        else
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = newRefreshToken,
                Expires = DateTime.Now.AddDays(7)
            };

            await dbContext.RefreshTokens.AddAsync(refreshToken);
        }

        await dbContext.SaveChangesAsync();
    }
}