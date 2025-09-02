using Microsoft.AspNetCore.Mvc;
using UnauthorizedSWAPI.Models.DTOs;
using UnauthorizedSWAPI.Services.Interfaces;

namespace UnauthorizedSWAPI.Controllers;

/// <summary>
/// Controller for managing user operations
/// Provides endpoints for CRUD operations on users
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>List of all users</returns>
    /// <response code="200">Returns the list of users</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        try
        {
            _logger.LogInformation("Getting all users");
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all users");
            return StatusCode(500, "An error occurred while retrieving users");
        }
    }

    /// <summary>
    /// Get a specific user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User details</returns>
    /// <response code="200">Returns the user</response>
    /// <response code="404">User not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        try
        {
            _logger.LogInformation("Getting user with ID: {UserId}", id);
            
            if (id <= 0)
            {
                return BadRequest("User ID must be a positive number");
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                return NotFound($"User with ID {id} not found");
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting user with ID: {UserId}", id);
            return StatusCode(500, "An error occurred while retrieving the user");
        }
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="createUserDto">User creation data</param>
    /// <returns>Created user</returns>
    /// <response code="201">User created successfully</response>
    /// <response code="400">Invalid input data</response>
    /// <response code="409">User with email already exists</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            _logger.LogInformation("Creating new user with email: {Email}", createUserDto.Email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdUser = await _userService.CreateUserAsync(createUserDto);
            
            _logger.LogInformation("User created successfully with ID: {UserId}", createdUser.Id);
            
            return CreatedAtAction(
                nameof(GetUser), 
                new { id = createdUser.Id }, 
                createdUser);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Conflict occurred while creating user");
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating user");
            return StatusCode(500, "An error occurred while creating the user");
        }
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    /// <param name="id">User ID to update</param>
    /// <param name="createUserDto">Updated user data</param>
    /// <returns>Updated user</returns>
    /// <response code="200">User updated successfully</response>
    /// <response code="400">Invalid input data</response>
    /// <response code="404">User not found</response>
    /// <response code="409">Email already exists</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] CreateUserDto createUserDto)
    {
        try
        {
            _logger.LogInformation("Updating user with ID: {UserId}", id);

            if (id <= 0)
            {
                return BadRequest("User ID must be a positive number");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedUser = await _userService.UpdateUserAsync(id, createUserDto);
            if (updatedUser == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for update", id);
                return NotFound($"User with ID {id} not found");
            }

            _logger.LogInformation("User with ID {UserId} updated successfully", id);
            return Ok(updatedUser);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Conflict occurred while updating user");
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating user with ID: {UserId}", id);
            return StatusCode(500, "An error occurred while updating the user");
        }
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="id">User ID to delete</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">User deleted successfully</response>
    /// <response code="404">User not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", id);

            if (id <= 0)
            {
                return BadRequest("User ID must be a positive number");
            }

            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("User with ID {UserId} not found for deletion", id);
                return NotFound($"User with ID {id} not found");
            }

            _logger.LogInformation("User with ID {UserId} deleted successfully", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting user with ID: {UserId}", id);
            return StatusCode(500, "An error occurred while deleting the user");
        }
    }

    /// <summary>
    /// Check if an email exists
    /// </summary>
    /// <param name="email">Email to check</param>
    /// <returns>Boolean indicating if email exists</returns>
    /// <response code="200">Returns true/false</response>
    [HttpGet("email-exists")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> EmailExists([FromQuery] string email)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email parameter is required");
            }

            var exists = await _userService.EmailExistsAsync(email);
            return Ok(exists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while checking email existence");
            return StatusCode(500, "An error occurred while checking email");
        }
    }
}
