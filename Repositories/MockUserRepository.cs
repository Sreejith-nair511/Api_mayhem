using UnauthorizedSWAPI.Models;
using UnauthorizedSWAPI.Repositories.Interfaces;

namespace UnauthorizedSWAPI.Repositories;

/// <summary>
/// Mock implementation of IUserRepository for testing and development
/// Uses in-memory data storage
/// </summary>
public class MockUserRepository : IUserRepository
{
    private readonly List<User> _users;
    private int _nextId;

    public MockUserRepository()
    {
        // Initialize with some sample data
        _users = new List<User>
        {
            new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                CreatedAt = DateTime.UtcNow.AddDays(-30),
                UpdatedAt = DateTime.UtcNow.AddDays(-5),
                IsActive = true
            },
            new User
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                CreatedAt = DateTime.UtcNow.AddDays(-20),
                UpdatedAt = DateTime.UtcNow.AddDays(-2),
                IsActive = true
            },
            new User
            {
                Id = 3,
                FirstName = "Bob",
                LastName = "Johnson",
                Email = "bob.johnson@example.com",
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow.AddDays(-1),
                IsActive = false
            }
        };
        _nextId = 4;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        // Simulate async operation
        await Task.Delay(10);
        return _users.ToList();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        // Simulate async operation
        await Task.Delay(10);
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        // Simulate async operation
        await Task.Delay(10);
        return _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<User> AddUserAsync(User user)
    {
        // Simulate async operation
        await Task.Delay(10);
        
        // Assign new ID and set timestamps
        user.Id = _nextId++;
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        
        _users.Add(user);
        return user;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        // Simulate async operation
        await Task.Delay(10);
        
        var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
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

        return existingUser;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        // Simulate async operation
        await Task.Delay(10);
        
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return false;
        }

        _users.Remove(user);
        return true;
    }
}
