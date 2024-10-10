using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CodingChallenge.Exceptions;
using CodingChallenge.Interfaces;
using CodingChallenge.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodingChallenge.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;  // Logger for logging

        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                _logger.LogInformation("Fetching all users.");
                var users = await _userRepository.GetAllUsersAsync();
                return Ok(users);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Error fetching users.");
                return StatusCode(500, "Internal server error occurred while retrieving users.");
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                _logger.LogInformation("Fetching user with ID {UserId}", id);
                var user = await _userRepository.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "User with ID {UserId} not found.", id);
                return NotFound(ex.Message);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Error fetching user with ID {UserId}", id);
                return StatusCode(500, "Internal server error occurred while retrieving the user.");
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                _logger.LogInformation("Adding a new user.");
                await _userRepository.AddUserAsync(user);
                return CreatedAtAction("GetUser", new { id = user.UserID }, user);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error occurred while adding user.");
                return BadRequest(ex.Message);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Error adding user.");
                return StatusCode(500, "Internal server error occurred while adding the user.");
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserID)
            {
                _logger.LogWarning("User ID mismatch: {UserId} != {SubmittedUserId}", id, user.UserID);
                return BadRequest("User ID mismatch.");
            }

            try
            {
                _logger.LogInformation("Updating user with ID {UserId}", id);
                await _userRepository.UpdateUserAsync(user);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "User with ID {UserId} not found.", id);
                return NotFound(ex.Message);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Error updating user with ID {UserId}", id);
                return StatusCode(500, "Internal server error occurred while updating the user.");
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                _logger.LogInformation("Deleting user with ID {UserId}", id);
                await _userRepository.DeleteUserAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "User with ID {UserId} not found.", id);
                return NotFound(ex.Message);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
                return StatusCode(500, "Internal server error occurred while deleting the user.");
            }
        }

       
        
    }
}
