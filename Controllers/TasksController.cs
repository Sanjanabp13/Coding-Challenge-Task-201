using CodingChallenge.Exceptions;

using CodingChallenge.Repository;
using CodingChallenge.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CodingChallenge.Interface;

namespace CodingChallenge.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {


        private readonly ITasksRepository _tasksRepository;
        private readonly ILogger<TasksController> _logger;  // Logger for logging

        public TasksController(ITasksRepository userRepository, ILogger<TasksController> logger)
        {
            _tasksRepository = userRepository;
            _logger = logger;
        }

        // GET: api/tasks
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Tasks>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all Tasks.");
                var tasks
                = await _tasksRepository.GetAllAsync();
                return Ok(tasks);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Error fetching users.");
                return StatusCode(500, "Internal server error occurred while retrieving users.");
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tasks>> GetID(int id)
        {
            try
            {
                _logger.LogInformation("Fetching user with ID {TaskId}", id);
                var tasks = await _tasksRepository.GetByIdAsync(id);
                return Ok(tasks);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "User with ID {TaskId} not found.", id);
                return NotFound(ex.Message);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Error fetching task with ID {TAskId}", id);
                return StatusCode(500, "Internal server error occurred while retrieving the user.");
            }
        }

        // POST: api/Users
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Tasks>> Post(Tasks tasks)
        {
            try
            {
                _logger.LogInformation("Adding a new Task.");
                await _tasksRepository.AddAsync(tasks);
                return CreatedAtAction("GetID", new { id = tasks.TaskID }, tasks);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error occurred while adding task.");
                return BadRequest(ex.Message);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Error adding task.");
                return StatusCode(500, "Internal server error occurred while adding the task.");
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, Tasks tasks)
        {
            if (id != tasks.TaskID)
            {
                _logger.LogWarning("task ID mismatch: {TaskId} != id}", id, tasks.TaskID);
                return BadRequest("task ID mismatch.");
            }

            try
            {
                _logger.LogInformation("Updating user with ID {id}", id);
                await _tasksRepository.UpdateAsync(tasks);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "task with ID {TaskId} not found.", id);
                return NotFound(ex.Message);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Error updating user with ID {TaskId}", id);
                return StatusCode(500, "Internal server error occurred while updating the user.");
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting task with ID {UserId}", id);
                await _tasksRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "task with ID {TaskId} not found.", id);
                return NotFound(ex.Message);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Error deleting task with ID {taskId}", id);
                return StatusCode(500, "Internal server error occurred while deleting the Task.");
            }
        }



    }
}

