using AutoMapper;
using LeadManager.API.Models;
using LeadManager.Core.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LeadManager.API.Controllers
{
    [Route("api/[controller]")]    
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<User> _signInmanager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PasswordValidator<User> _passwordValidator;
        private readonly IConfiguration _config;
        private readonly IMapper _iMapper;
        private readonly ILogger<UsersController> _logger;

        public UsersController(SignInManager<User> signInmanager,
                                 UserManager<User> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 PasswordValidator<User> passwordValidator,
                                 ILogger<UsersController> logger,
                                 IConfiguration config,
                                 IMapper iMapper)
        {
            _signInmanager = signInmanager;
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordValidator = passwordValidator;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _config = config;
            _iMapper = iMapper;
         
        }

        [HttpPost("token")]
        public async Task<IActionResult> CreateToken(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    if (!user.IsActive)
                    {
                        return BadRequest("Validation failed. User account is disabled.");
                    }

                    var result = await _signInmanager.CheckPasswordSignInAsync(user, model.Password, false);

                    if (result.Succeeded)
                    {
                        //Create the token

                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));

                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                                                         _config["Tokens:Audience"],
                                                         claims,
                                                         expires: DateTime.UtcNow.AddMinutes(120),
                                                         signingCredentials: creds
                                                         );

                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo

                        };

                        return Created("", results);

                    }
                    else
                    {
                        return BadRequest("User validation failed");
                    }
                }
                else
                {
                    return BadRequest("Could not find a user that matches login credentials");
                }

            }
            else
            {
                return BadRequest("Login credentials are not in correct format");
            }
        }
    }
}
