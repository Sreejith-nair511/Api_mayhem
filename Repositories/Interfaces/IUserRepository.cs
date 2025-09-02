using UnauthorizedSWAPI.Models;

namespace UnauthorizedSWAPI.Repositories.Interfaces;

/// <summary>
/// Interface for User repository operations
/// This allows us to switch between different implementations (Mock, Database, etc.)
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Get all users from the data source
    /// </summary>
    /// <returns>Collection of all users</returns>
    Task<IEnumerable<User>> GetAllUsersAsync();

    /// <summary>
    /// Get a specific user by their ID
    /// </summary>
    /// <param name="id">User ID to search for</param>
    /// <returns>User if found, null otherwise</returns>
    Task<User?> GetUserByIdAsync(int id);

    /// <summary>
    /// Get a user by their email address
    /// </summary>
    /// <param name="email">Email address to search for</param>
    /// <returns>User if found, null otherwise</returns>
    Task<User?> GetUserByEmailAsync(string email);

    /// <summary>
    /// Add a new user to the data source
    /// </summary>
    /// <param name="user">User to add</param>
    /// <returns>The created user with assigned ID</returns>
    Task<User> AddUserAsync(User user);

    /// <summary>
    /// Update an existing user
    /// </summary>
    /// <param name="user">User with updated information</param>
    /// <returns>Updated user</returns>
    Task<User> UpdateUserAsync(User user);

    /// <summary>
    /// Delete a user by their ID
    /// </summary>
    /// <param name="id">ID of user to delete</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteUserAsync(int id);
}
