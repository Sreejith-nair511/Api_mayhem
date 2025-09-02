using UnauthorizedSWAPI.Models.DTOs;

namespace UnauthorizedSWAPI.Services.Interfaces;

/// <summary>
/// Interface for User service operations
/// Contains business logic for user-related operations
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>Collection of user DTOs</returns>
    Task<IEnumerable<UserDto>> GetAllUsersAsync();

    /// <summary>
    /// Get a specific user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User DTO if found, null otherwise</returns>
    Task<UserDto?> GetUserByIdAsync(int id);

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="createUserDto">User creation data</param>
    /// <returns>Created user DTO</returns>
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);

    /// <summary>
    /// Update an existing user
    /// </summary>
    /// <param name="id">User ID to update</param>
    /// <param name="createUserDto">Updated user data</param>
    /// <returns>Updated user DTO if found, null otherwise</returns>
    Task<UserDto?> UpdateUserAsync(int id, CreateUserDto createUserDto);

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="id">User ID to delete</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteUserAsync(int id);

    /// <summary>
    /// Check if a user exists with the given email
    /// </summary>
    /// <param name="email">Email to check</param>
    /// <returns>True if email exists, false otherwise</returns>
    Task<bool> EmailExistsAsync(string email);
}
