using AuthenticationService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationService.Services
{
    /// <summary>
    /// AuthenticationService is responsible for user registration, login, and logout functionality.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the AuthenticationService class.
        /// </summary>
        /// <param name="userManager">An instance of UserManager for managing user operations.</param>
        /// <param name="signInManager">An instance of SignInManager for managing user authentication operations.</param>
        /// <param name="configuration">An instance of IConfiguration for accessing application configuration settings.</param>
        public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Register a new user with the provided user object and password.
        /// </summary>
        /// <param name="user">The ApplicationUser object containing user information.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>A Task representing the operation, with a JWT token if registration is successful.</returns>
        public async Task<string> RegisterAsync(ApplicationUser user, string password)
        {
            try
            {
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    return await GenerateJwtTokenAsync(user);
                }

                // If the user creation failed, aggregate the error messages and throw an exception
                var errors = string.Join(", ", result.Errors.Select(x => $"{x.Code}: {x.Description}"));
                throw new InvalidOperationException($"Registration failed. Errors: {errors}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred during registration: {ex.Message}");
            }
        }

        /// <summary>
        /// Authenticate a user with the provided email and password.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>A Task representing the operation, with a JWT token if login is successful.</returns>
        public async Task<string> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    throw new InvalidOperationException("Invalid login attempt.");
                }

                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (result.Succeeded)
                {
                    return await GenerateJwtTokenAsync(user);
                }

                throw new InvalidOperationException("Invalid login attempt.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred during login: {ex.Message}");
            }
        }

        /// <summary>
        /// Log the user out.
        /// </summary>
        /// <returns>A Task representing the operation.</returns>
        public async Task LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred during logout: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The ApplicationUser object for which to generate a JWT token.</param>
        /// <returns>A JWT token as a string.</returns>
        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            try
            {
                // Retrieve the JWT secret key from the configuration and create a SymmetricSecurityKey
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                // Create a list of claims for the user
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                // Retrieve the user's roles and add them as claims
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Create a JWT token with the specified claims, issuer, audience, and expiration
                var token = new JwtSecurityToken(
                    _configuration["JwtSettings:Issuer"],
                    _configuration["JwtSettings:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:TokenLifetimeInMinutes"])),
                    signingCredentials: credentials);

                // Serialize the JWT token and return it as a string
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while generating the JWT token: {ex.Message}");
            }
        }

    }
}
