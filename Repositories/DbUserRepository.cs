using Microsoft.EntityFrameworkCore;
using UnauthorizedSWAPI.Data;
using UnauthorizedSWAPI.Models;
using UnauthorizedSWAPI.Repositories.Interfaces;

namespace UnauthorizedSWAPI.Repositories;

/// <summary>
/// Database implementation of IUserRepository using Entity Framework Core
/// This implementation will work with a real database when properly configured
/// </summary>
public class DbUserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public DbUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> AddUserAsync(User user)
    {
        // Set timestamps
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        return user;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        var existingUser = await _context.Users.FindAsync(user.Id);
        if (existingUser == null)
        {
            throw new ArgumentException($"User with ID {user.Id} not found");
        }

        // Update properties
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.IsActive = user.IsActive;
        existingUser.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
