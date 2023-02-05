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
using System.Text.RegularExpressions;

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
        public async Task<IActionResult> CreateToken(UserDto model)
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

        [HttpPost("user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateUser(UserForCreateDto userForCreateDto)
        {
            try
            {
                if (userForCreateDto == null)
                {
                    return BadRequest("The submitted RegisteredUserForCreateVM is invalid");
                }

                var userForCreate = _iMapper.Map<User>(userForCreateDto);
                userForCreate.UserName = userForCreate.Email; //Use email as Username
                userForCreate.IsActive = false; //The user should be disabled at point of creation


                //Check if a user with same email address already exists
                User user = await _userManager.FindByEmailAsync(userForCreateDto.Email);

                if (user != null)
                {

                    return BadRequest("A user is already registered with the submitted email address");
                }

                var result = await _userManager.CreateAsync(userForCreate, userForCreateDto.Password);

                if (result != IdentityResult.Success)
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Could not create new user, please check error log");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        [HttpPost("user/role")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateUserRole(UserRole userRole)
        {
            try
            {
                if (userRole == null)
                {
                    return BadRequest("The submitted UserRole is invalid");
                }


                //Check if a user role with same name already exists
                var existingRole = await _roleManager.FindByNameAsync(userRole.RoleName);

                if (existingRole != null)
                {
                    return BadRequest("The submitted user role already exists");
                }

                var createRoleResult = _roleManager.CreateAsync(new IdentityRole(userRole.RoleName)).Result;

                if (createRoleResult != IdentityResult.Success)
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Could not create new user role, please check error log");
                }

                return Ok(createRoleResult);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        [HttpDelete("user/role")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RemoveUserRole(UserRole userRole)
        {
            try
            {
                if (userRole == null)
                {


                    return BadRequest("The submitted UserRole is invalid");
                }


                //Check if a user role with same name exists
                var existingRole = await _roleManager.FindByNameAsync(userRole.RoleName);

                if (existingRole == null)
                {
                    return BadRequest("The submitted user role does not exist");
                }

                var deleteRoleResult = await _roleManager.DeleteAsync(existingRole);

                if (deleteRoleResult != IdentityResult.Success)
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Could not delete user role, please check error log");
                }

                return Ok(deleteRoleResult);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        [HttpPatch]
        [Route("user/{email}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUser(string email, UserForPartialUpdateDto userForUpdateDto)
        {
            try
            {
                if (userForUpdateDto == null)
                {
                    return BadRequest("The submitted RegisteredUserForCreateVM is invalid");
                }



                //Check if a user with same email address already exists
                User user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    return BadRequest("Could not find a user account associated with the submitted email address");
                }

                if (!user.IsActive)
                {
                    return BadRequest("The user account is currently inactive, activate it first before updating details");
                }



                if (userForUpdateDto.Email != null)
                {
                    if (Regex.IsMatch(userForUpdateDto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    {
                        User existingUser = await _userManager.FindByEmailAsync(userForUpdateDto.Email);

                        if (existingUser != null)
                        {
                            return BadRequest($"Could not update email to {userForUpdateDto.Email} because it is associated with an existing user account");
                        }

                        user.Email = userForUpdateDto.Email;
                        user.UserName = userForUpdateDto.Email;
                    }
                    else
                    {
                        return BadRequest("The new email address is not in the correct format");
                    }
                }

                if (userForUpdateDto.FirstName != null)
                {
                    user.FirstName = userForUpdateDto.FirstName;
                }

                if (userForUpdateDto.LastName != null)
                {
                    user.LastName = userForUpdateDto.LastName;
                }

                if (userForUpdateDto.Password != null)
                {

                    var passwordValidationResult = await _passwordValidator.ValidateAsync(_userManager, user, userForUpdateDto.Password);

                    if (!passwordValidationResult.Succeeded)
                    {
                        List<string> passwordErrors = new List<string>();

                        foreach (var error in passwordValidationResult.Errors)
                        {
                            passwordErrors.Add(error.Description);
                        }

                        string errors = String.Join(Environment.NewLine, passwordErrors);

                        return BadRequest($"Could not update password. The submitted value for new password does not satisfy requirements of the password policy. {errors}");
                    }

                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userForUpdateDto.Password);
                }

                var result = await _userManager.UpdateAsync(user);

                if (result != IdentityResult.Success)
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Could not update user details, please check error log");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("user/{email}/{role}")]
        public async Task<IActionResult> AddRoleForSingleUser(string email, string role)
        {
            try
            {
                //Check if a user with same email address exists
                User user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    return BadRequest("Could not find a user account associated with the submitted email address");
                }

                if (!user.IsActive)
                {
                    return BadRequest("The user account is currently inactive, activate it first before updating details");
                }

                //Check if a user role with same name already exists
                var existingRoleToAdd = await _roleManager.FindByNameAsync(role);

                if (existingRoleToAdd == null)
                {
                    return BadRequest("The specified role does not exist");
                }

                var userRoles = await _userManager.GetRolesAsync(user);

                //Check if the user already has the role assigned
                if (userRoles.Contains(existingRoleToAdd.Name))
                {
                    return BadRequest("The user is already associated with this user role");
                }


                var result = await _userManager.AddToRoleAsync(user, existingRoleToAdd.Name);

                if (result != IdentityResult.Success)
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Could not update user details, please check error log");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        [HttpDelete]
        [Route("user/{email}/{role}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RemoveRoleForSingleUser(string email, string role)
        {
            try
            {
                //Check if a user with same email address exists
                User user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    return BadRequest("Could not find a user account associated with the submitted email address");
                }

                if (!user.IsActive)
                {
                    return BadRequest("The user account is currently inactive, activate it first before updating details");
                }

                //Check if a user role with supplied name exists
                var existingRoleToRemove = await _roleManager.FindByNameAsync(role);

                if (existingRoleToRemove == null)
                {
                    return BadRequest("The specified role does not exist");
                }

                var userRoles = await _userManager.GetRolesAsync(user);

                //Check if the user already has the role assigned
                if (!userRoles.Contains(existingRoleToRemove.Name))
                {
                    return BadRequest("The user does not have the submitted role");
                }


                var result = await _userManager.RemoveFromRoleAsync(user, existingRoleToRemove.Name);

                if (result != IdentityResult.Success)
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Could not update user details, please check error log");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        [HttpPatch("activate")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ActivateUser(RegisteredUserIsActiveDto registeredUserIsActiveVM)
        {
            try
            {
                if (registeredUserIsActiveVM == null)
                {
                    return BadRequest("The submitted RegisteredUserForUpdateVM is invalid");
                }

                //Check if a user with the email address exists
                User user = await _userManager.FindByEmailAsync(registeredUserIsActiveVM.Email);

                if (user == null)
                {
                    return BadRequest("A user is with the submitted email address does not exist");
                }
                else
                {
                    //Update IsActive
                    user.IsActive = registeredUserIsActiveVM.IsActive;

                }

                var result = await _userManager.UpdateAsync(user);

                if (result != IdentityResult.Success)
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Could not update user account, please check error log");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        [HttpGet("user")]        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUsers(string role = null)
        {           
            try
            {

                if (role != null)
                {
                    var userRole = await _roleManager.FindByNameAsync(role);

                    if (userRole == null)
                    {
                        return BadRequest("The submitted role does not exist");
                    }

                    var usersInRole = await _userManager.GetUsersInRoleAsync(role);

                    foreach (var user in usersInRole)
                    {
                        user.UserRoles.Add(role);
                    }

                    UserForDisplayDto[] usersForDisplayDto = _iMapper.Map<UserForDisplayDto[]>(usersInRole);

                    return Ok(usersForDisplayDto);
                }
                else
                {
                    //return all users
                    var result = await Task.FromResult(_userManager.Users.ToList());

                    foreach (var user in result)
                    {
                        //Find each user's roles
                        var userRoles = await _userManager.GetRolesAsync(user);
                        user.UserRoles.AddRange(userRoles);
                    }

                    UserForDisplayDto[] usersForDisplayDto = _iMapper.Map<UserForDisplayDto[]>(result);

                    return Ok(usersForDisplayDto);

                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("user/role")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserRoles()
        {
            try
            {
                List<string> roles = new List<string>();

                var userRoles = await Task.FromResult(_roleManager.Roles.ToList());

                if (userRoles == null)
                {
                    return BadRequest("Could not find any user roles in database");
                }

                foreach (IdentityRole ir in userRoles)
                {
                    roles.Add(ir.Name);
                }

                return Ok(roles);

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("user/{email}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUser(string email)
        {
            try
            {
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    return BadRequest("The submitted email is not in the correct format");
                }

                var result = await _userManager.FindByEmailAsync(email);

                if (result == null)
                {
                    return BadRequest("A user is with the submitted email address does not exist");
                }

                var userRoles = await _userManager.GetRolesAsync(result);
                result.UserRoles.AddRange(userRoles);

                UserForDisplayDto registeredUserForDisplayVM = _iMapper.Map<UserForDisplayDto>(result);
                return Ok(registeredUserForDisplayVM);

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
