namespace UnauthorizedSWAPI.Models.DTOs;

/// <summary>
/// Data Transfer Object for User - used for API responses
/// </summary>
public class UserDto
{
    /// <summary>
    /// Unique identifier for the user
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User's first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Date when the user was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Indicates if the user is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Full name combining first and last name
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";
}
