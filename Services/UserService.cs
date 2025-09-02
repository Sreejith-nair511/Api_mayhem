using UnauthorizedSWAPI.Models;
using UnauthorizedSWAPI.Models.DTOs;
using UnauthorizedSWAPI.Repositories.Interfaces;
using UnauthorizedSWAPI.Services.Interfaces;

namespace UnauthorizedSWAPI.Services;

/// <summary>
/// Service class containing business logic for user operations
/// Acts as a layer between controllers and repositories
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        return user != null ? MapToDto(user) : null;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        // Business logic: Check if email already exists
        var existingUser = await _userRepository.GetUserByEmailAsync(createUserDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"A user with email '{createUserDto.Email}' already exists.");
        }

        // Create new user entity
        var user = new User
        {
            FirstName = createUserDto.FirstName.Trim(),
            LastName = createUserDto.LastName.Trim(),
            Email = createUserDto.Email.Trim().ToLowerInvariant(),
            IsActive = true
        };

        var createdUser = await _userRepository.AddUserAsync(user);
        return MapToDto(createdUser);
    }

    public async Task<UserDto?> UpdateUserAsync(int id, CreateUserDto createUserDto)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            return null;
        }

        // Business logic: Check if email is being changed to an existing email
        var emailLower = createUserDto.Email.Trim().ToLowerInvariant();
        if (!existingUser.Email.Equals(emailLower, StringComparison.OrdinalIgnoreCase))
        {
            var userWithEmail = await _userRepository.GetUserByEmailAsync(emailLower);
            if (userWithEmail != null)
            {
                throw new InvalidOperationException($"A user with email '{createUserDto.Email}' already exists.");
            }
        }

        // Update user properties
        existingUser.FirstName = createUserDto.FirstName.Trim();
        existingUser.LastName = createUserDto.LastName.Trim();
        existingUser.Email = emailLower;

        var updatedUser = await _userRepository.UpdateUserAsync(existingUser);
        return MapToDto(updatedUser);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        return await _userRepository.DeleteUserAsync(id);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email.Trim().ToLowerInvariant());
        return user != null;
    }

    /// <summary>
    /// Maps a User entity to a UserDto
    /// </summary>
    /// <param name="user">User entity</param>
    /// <returns>UserDto</returns>
    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive
        };
    }
}
