using Microsoft.EntityFrameworkCore;
using CodingChallenge.Interfaces;
using CodingChallenge.Models;
using CodingChallenge.Exceptions;
using CodingChallenge.Models; // Assuming exceptions are in this namespace

namespace CodingChallenge.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskDBcontext _context;

        public UserRepository(TaskDBcontext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException("Error adding user to the database.", ex);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(user.UserID);
                if (existingUser == null)
                {
                    throw new NotFoundException($"User with ID {user.UserID} not found.");
                }

                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.Role = user.Role;

                await _context.SaveChangesAsync();
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new RepositoryException("Concurrency error occurred while updating user.", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error updating user.", ex);
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new NotFoundException($"User with ID {userId} not found.");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException("Error deleting user from the database.", ex);
            }
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new NotFoundException($"User with ID {userId} not found.");
                }

                return user;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error retrieving user by ID.", ex);
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error retrieving all users.", ex);
            }
        }

        public async Task<User> AuthenticateUserAsync(string identifier, string password)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => (u.Email == identifier || u.Username == identifier) && u.Password == password);

                if (user != null)
                {
                    user.LastLogin = DateTime.Now;
                    await _context.SaveChangesAsync();
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error authenticating user.", ex);
            }
        }

        public async Task ChangePasswordAsync(int userId, string newPassword)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new NotFoundException($"User with ID {userId} not found.");
                }

                user.Password = newPassword;
                await _context.SaveChangesAsync();
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error changing user password.", ex);
            }
        }

        public async Task ResetPasswordAsync(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    throw new NotFoundException($"User with email {email} not found.");
                }

                // Assume some reset password logic here (e.g., generate token and send email)
                user.Password = user.Username + user.CreatedDate.ToString("yyyyMMdd"); // For demonstration
                await _context.SaveChangesAsync();
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error resetting user password.", ex);
            }
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            try
            {
                return await _context.Users.Where(u => u.Role.ToString() == role).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error retrieving users by role.", ex);
            }
        }

        public async Task<IEnumerable<User>> GetUsersByEmailAsync(string email)
        {
            try
            {
                return await _context.Users.Where(u => u.Email == email).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error retrieving users by email.", ex);
            }
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error checking if email is registered.", ex);
            }
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            try
            {
                if (await IsEmailRegisteredAsync(user.Email))
                {
                    // Email is already registered
                    return false;
                }

                // Proceed with adding the user
                await AddUserAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error registering new user.", ex);
            }
        }
    }
}
