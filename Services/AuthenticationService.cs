using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Arcade.Data.Repositories;
using Arcade.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;

namespace Arcade.Services
{
    /// <summary>
    /// Service interface for authentication operations
    /// </summary>
    public interface IAuthenticationService
    {
        Task<(bool Success, string Message, User? User)> RegisterAsync(string email, string password, string fullName, string? phone = null, string? address = null);
        Task<(bool Success, string Message, User? User, string? Token)> LoginAsync(string email, string password);
        Task<bool> ValidatePasswordStrengthAsync(string password);
        string GenerateJwtToken(User user);
        Task<User?> GetUserByIdAsync(int userId);
        Task<bool> EmailExistsAsync(string email, int? excludeUserId = null);
        Task<bool> UpdateProfileAsync(int userId, string fullName, string email, string? phone = null, string? address = null);
        Task<(bool Success, string Message)> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task RecordLoginAttemptAsync(int userId, bool success);
    }

    /// <summary>
    /// Service implementation for authentication operations
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private const int MaxFailedAttempts = 5;
        private const int LockoutMinutes = 15;
        private const int BcryptWorkFactor = 11;

        public AuthenticationService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<(bool Success, string Message, User? User)> RegisterAsync(string email, string password, string fullName, string? phone = null, string? address = null)
        {
            try
            {
                // Check if email already exists
                if (await _userRepository.EmailExistsAsync(email))
                {
                    return (false, "An account with this email already exists.", null);
                }

                // Validate password strength
                if (!await ValidatePasswordStrengthAsync(password))
                {
                    return (false, "Password must be at least 8 characters with uppercase, lowercase, number, and special character.", null);
                }

                // Hash password
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, BcryptWorkFactor);

                // Create new user
                var user = new User
                {
                    Email = email.ToLower().Trim(),
                    PasswordHash = passwordHash,
                    FullName = fullName.Trim(),
                    Phone = phone?.Trim(),
                    Address = address?.Trim(),
                    Role = "Customer",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();

                return (true, "Registration successful!", user);
            }
            catch (Exception ex)
            {
                return (false, $"Registration failed: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message, User? User, string? Token)> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);

                if (user == null)
                {
                    return (false, "Invalid email or password.", null, null);
                }

                // Check if account is locked
                if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
                {
                    var remainingMinutes = (int)(user.LockoutEnd.Value - DateTime.UtcNow).TotalMinutes;
                    return (false, $"Account is locked. Try again in {remainingMinutes + 1} minutes.", null, null);
                }

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    await RecordLoginAttemptAsync(user.UserId, false);
                    return (false, "Invalid email or password.", null, null);
                }

                // Successful login
                await RecordLoginAttemptAsync(user.UserId, true);

                // Generate JWT token
                var token = GenerateJwtToken(user);

                return (true, "Login successful!", user, token);
            }
            catch (Exception ex)
            {
                return (false, $"Login failed: {ex.Message}", null, null);
            }
        }

        public async Task<bool> ValidatePasswordStrengthAsync(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            bool hasUppercase = password.Any(char.IsUpper);
            bool hasLowercase = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(c => !char.IsLetterOrDigit(c));

            return hasUppercase && hasLowercase && hasDigit && hasSpecial;
        }

        public string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? "ArcadeElectronicsStoreSecretKey2024SuperSecureKey!@#$%"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "Arcade",
                audience: _configuration["Jwt:Audience"] ?? "Arcade",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            var user = await _userRepository.GetByEmailAsync(email.ToLower().Trim());
            if (user == null) return false;
            if (excludeUserId.HasValue && user.UserId == excludeUserId.Value) return false;
            return true;
        }

        public async Task<bool> UpdateProfileAsync(int userId, string fullName, string email, string? phone = null, string? address = null)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                // Update all fields
                user.FullName = fullName.Trim();
                user.Email = email.ToLower().Trim();
                user.Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
                user.Address = string.IsNullOrWhiteSpace(address) ? null : address.Trim();

                // Explicitly mark entity as modified and save
                _userRepository.Update(user);
                var changes = await _userRepository.SaveChangesAsync();

                return changes > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return (false, "User not found.");

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                return (false, "Current password is incorrect.");

            if (!await ValidatePasswordStrengthAsync(newPassword))
                return (false, "New password must be at least 8 characters with uppercase, lowercase, number, and special character.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword, BcryptWorkFactor);
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return (true, "Password changed successfully!");
        }

        public async Task RecordLoginAttemptAsync(int userId, bool success)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return;

            if (success)
            {
                user.FailedLoginAttempts = 0;
                user.LockoutEnd = null;
                user.LastLoginAt = DateTime.UtcNow;
            }
            else
            {
                user.FailedLoginAttempts++;
                if (user.FailedLoginAttempts >= MaxFailedAttempts)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(LockoutMinutes);
                }
            }

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }
    }
}
