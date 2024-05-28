using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication2.Core;
using WebApplication2.DTOs;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;
        public UsersController(UserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> RegisterUser([FromBody] RegisterUser registerUserDto)
        {
            var userExists = await _userService.CheckUserExists(registerUserDto.Email);
            if (userExists)
            {
                return BadRequest("User already exists");
            }

            var newUser = new User
            {
                Name = registerUserDto.Name,
                Email = registerUserDto.Email
            };

            var user = await _userService.CreateUserAsync(newUser, registerUserDto.Password);
            if (user == null)
            {
                return BadRequest("Something went wrong.");
            }

            var userResponse = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Token = _authService.GenerateToken(user)
            };

            return Ok(userResponse);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userService.GetUserByEmailAsync(loginRequest.Email);

            if (user == null)
            {
                return Unauthorized("User not found");
            }

            var isPasswordValid = _userService.CheckPassword(user, loginRequest.Password);
            if (!isPasswordValid)
            {
                return Unauthorized("Invalid password");
            }

            var userResponse = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Token = _authService.GenerateToken(user)
            };

            return Ok(userResponse);
        }

        [HttpGet("profile")]
        [Authorize]
        public ActionResult<UserResponse> Profile()
        {
            var user = new
            {
                Name,
                Email,
            };

            return Ok(user);
        }
    }
}
