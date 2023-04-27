using AuthenticationService.Models;

namespace AuthenticationService.Services
{
    /// <summary>
    /// Interface for the AuthenticationService, defining methods for user authentication.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Register a new user with the provided user object and password.
        /// </summary>
        /// <param name="user">The ApplicationUser object containing user information.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>A Task representing the operation, with a JWT token if registration is successful.</returns>
        Task<string> RegisterAsync(ApplicationUser user, string password);

        /// <summary>
        /// Authenticate a user with the provided email and password.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>A Task representing the operation, with a JWT token if login is successful.</returns>
        Task<string> LoginAsync(string email, string password);

        /// <summary>
        /// Log the user out.
        /// </summary>
        /// <returns>A Task representing the operation.</returns>
        Task LogoutAsync();
    }
}