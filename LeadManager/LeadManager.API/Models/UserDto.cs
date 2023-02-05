using System.ComponentModel.DataAnnotations;

namespace LeadManager.API.Models
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class UserForCreateDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        public bool isActive { get; set; }
    }

    public class UserForDisplayDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool isActive { get; set; }
        public List<string> UserRoles { get; set; }

    }

    public class UserForPartialUpdateDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }

    public class RegisteredUserIsActiveDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
