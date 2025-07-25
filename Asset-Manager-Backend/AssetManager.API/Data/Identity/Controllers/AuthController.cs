using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AspNetCore.Identity.MongoDbCore.Models;
using AssetManager.API.Applications.Interfaces;
using AssetManager.API.Applications.Services;
using AssetManager.API.Data.Identity.Models;
using AssetManager.API.Data.Identity.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AssetManager.API.Data.Identity.Controllers
{
    [Route("api/v1/auth/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<MongoIdentityRole<Guid>> _roleManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthController(
            UserManager<UserModel> userManager,
            RoleManager<MongoIdentityRole<Guid>> roleManager,
            SignInManager<UserModel> signInManager,
            IUserService userService,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("register-employee")]
        public async Task<IActionResult> RegisterEmployee([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest(new { errors });
            }

            var user = new UserModel
            {
                UserName = model.Email,
                Email = model.Email,
                Fullname = model.FullName,
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "Employee");

                if (!roleResult.Succeeded)
                {
                    return BadRequest(
                        new { errors = roleResult.Errors.Select(e => e.Description) }
                    );
                }

                var tokenResponse = await GenerateJwtTokenAsync(user);
                return Ok(tokenResponse);
            }

            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        [HttpPost("register-manager")]
        public async Task<IActionResult> RegisterManager([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest(new { errors });
            }

            var user = new UserModel
            {
                UserName = model.Email,
                Email = model.Email,
                Fullname = model.FullName,
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "AssetManager");

                if (!roleResult.Succeeded)
                {
                    return BadRequest(
                        new { errors = roleResult.Errors.Select(e => e.Description) }
                    );
                }

                var tokenResponse = await GenerateJwtTokenAsync(user);
                return Ok(tokenResponse);
            }

            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                await _signInManager.SignInAsync(user, isPersistent: true);

                var tokenResponse = await GenerateJwtTokenAsync(user);
                return Ok(tokenResponse);
            }

            return Unauthorized(new { message = "Invalid Username or Password" });
        }

        private async Task<object> GenerateJwtTokenAsync(UserModel user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Email),
            };

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["Jwt:ExpiryMinutes"]!)
                ),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256
                )
            );

            return new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Created = token.ValidFrom.ToLocalTime(),
                Expiration = token.ValidTo.ToLocalTime(),
            };
        }

        [HttpGet("test")]
        [Authorize(Policy = "AssetManagerOnly")]
        public IActionResult Test()
        {
            var identity = User.Identity as ClaimsIdentity;
            var authenticationType = identity?.AuthenticationType;
            var userId = _userService.UserId;

            if (!_userService.IsAuthenticated || userId == null)
                return Unauthorized();

            // proceed with userId

            return Ok(
                new
                {
                    IsAuthenticated = User.Identity?.IsAuthenticated,
                    AuthenticationType = authenticationType,
                    UserName = User.Identity?.Name,
                    Claims = User.Claims.Select(c => new { c.Type, c.Value }),
                    UserId = userId,
                }
            );
        }
    }
}
