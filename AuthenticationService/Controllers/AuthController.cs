using AuthenticationService.Models;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    /// <summary>
    /// The AuthController handles user authentication and management tasks such as registration, login, and logout.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// The AuthController constructor initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authenticationService">An instance of the IAuthenticationService interface for handling authentication tasks.</param>
        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        /// <summary>
        /// Register a new user with the provided registration details.
        /// </summary>
        /// <param name="model">A RegisterModel object containing the user's email, password, and password confirmation.</param>
        /// <returns>An IActionResult containing either a BadRequest or an Ok result with the generated JWT token.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                var token = await _authenticationService.RegisterAsync(user, model.Password);

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest("Registration failed.");
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Authenticate a user with the provided login credentials.
        /// </summary>
        /// <param name="model">A LoginModel object containing the user's email and password.</param>
        /// <returns>An IActionResult containing either a BadRequest or an Ok result with the generated JWT token.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = await _authenticationService.LoginAsync(model.Email, model.Password);

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest("Invalid login attempt.");
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Log out the currently authenticated user.
        /// </summary>
        /// <returns>An IActionResult with an Ok result.</returns>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authenticationService.LogoutAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during logout:: {ex.Message}");
            }
        }
    }
}
