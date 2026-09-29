using CMS_HotelBooking.Helpers;
using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repository;
        private readonly IEmailService _emailService;

        public UsersService(IUsersRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public async Task<Users?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return await _repository.GetByEmailAsync(email.Trim());
        }

        public async Task<Users?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<Users>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<(bool Success, string Message)> RegisterAsync(
            Users model,
            string password,
            string role)
        {
            if (model == null)
                return (false, "Invalid user information.");

            if (string.IsNullOrWhiteSpace(model.Email))
                return (false, "Email address is required.");

            if (string.IsNullOrWhiteSpace(password))
                return (false, "Password is required.");

            model.Email = model.Email.Trim();

            var existing = await _repository.GetByEmailAsync(model.Email);

            if (existing != null)
            {
                return (
                    false,
                    "An account with this email already exists."
                );
            }

            model.PasswordHash = PasswordHelper.Hash(password);

            var newId = await _repository.RegisterAsync(
                model,
                role
            );

            if (newId <= 0)
            {
                return (
                    false,
                    "Registration failed. Please try again."
                );
            }

            return (
                true,
                "Registration successful."
            );
        }

        public async Task<(bool Success, Users? User, string Message)> LoginAsync(
            string email,
            string password)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (
                    false,
                    null,
                    "Email address is required."
                );
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return (
                    false,
                    null,
                    "Password is required."
                );
            }

            email = email.Trim();

            var user = await _repository.GetByEmailAsync(email);

            if (user == null)
            {
                return (
                    false,
                    null,
                    "Invalid email or password."
                );
            }

            if (!user.IsActive)
            {
                return (
                    false,
                    null,
                    "Your account has been deactivated. Please contact the hotel."
                );
            }

            if (!PasswordHelper.Verify(
                    password,
                    user.PasswordHash))
            {
                return (
                    false,
                    null,
                    "Invalid email or password."
                );
            }

            return (
                true,
                user,
                "Login successful."
            );
        }

        public async Task<int> ToggleActiveAsync(int id)
        {
            return await _repository.ToggleActiveAsync(id);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<int> UpdatePasswordAsync(
            int userId,
            string passwordHash)
        {
            return await _repository.UpdatePasswordAsync(
                userId,
                passwordHash
            );
        }

        public async Task<(bool Success, string Message)> SendResetCodeAsync(
            string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (
                    false,
                    "Please enter your email address."
                );
            }

            email = email.Trim();

            var user = await _repository.GetByEmailAsync(email);

            if (user == null)
            {
                return (
                    false,
                    "No account was found with this email address."
                );
            }

            var resetCode = Random.Shared
                .Next(100000, 1000000)
                .ToString();

            var expiry = DateTime.Now.AddMinutes(10);

            var result = await _repository.SaveResetCodeAsync(
                user.UserId,
                resetCode,
                expiry
            );

            if (result <= 0)
            {
                return (
                    false,
                    "Unable to generate reset code. Please try again."
                );
            }

            var emailStatus = await _emailService.SendPasswordResetCodeAsync(user.Email, user.FullName, resetCode);

            if (emailStatus == "Failed")
            {
                return (
                    false,
                    "We generated a reset code but could not send the email. Please try again in a moment or contact support."
                );
            }

            return (
                true,
                emailStatus == "Disabled"
                    ? $"Email sending is currently disabled. For testing, your reset code is: {resetCode}"
                    : "A 6-digit verification code has been sent to your email address. It will expire in 10 minutes."
            );
        }

        public async Task<(bool Success, string Message)> VerifyResetCodeAsync(
            string email,
            string code)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (
                    false,
                    "Email address is required."
                );
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                return (
                    false,
                    "Verification code is required."
                );
            }

            email = email.Trim();
            code = code.Trim();

            var user = await _repository.GetByEmailAsync(email);

            if (user == null)
            {
                return (
                    false,
                    "Invalid email address."
                );
            }

            var valid = await _repository.VerifyResetCodeAsync(
                user.UserId,
                code
            );

            if (!valid)
            {
                return (
                    false,
                    "Invalid or expired verification code."
                );
            }

            return (
                true,
                "Verification code verified successfully."
            );
        }

        public async Task<(bool Success, string Message)> ResetPasswordAsync(
            string email,
            string code,
            string newPassword)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (
                    false,
                    "Email address is required."
                );
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                return (
                    false,
                    "Verification code is required."
                );
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return (
                    false,
                    "New password is required."
                );
            }

            email = email.Trim();
            code = code.Trim();

            var user = await _repository.GetByEmailAsync(email);

            if (user == null)
            {
                return (
                    false,
                    "Invalid email address."
                );
            }

            var valid = await _repository.VerifyResetCodeAsync(
                user.UserId,
                code
            );

            if (!valid)
            {
                return (
                    false,
                    "Invalid or expired verification code."
                );
            }

            var passwordHash = PasswordHelper.Hash(newPassword);

            var result = await _repository.UpdatePasswordAsync(
                user.UserId,
                passwordHash
            );

            if (result <= 0)
            {
                return (
                    false,
                    "Unable to reset password. Please try again."
                );
            }

            await _repository.ClearResetCodeAsync(
                user.UserId
            );

            return (
                true,
                "Your password has been reset successfully."
            );
        }
    }
}