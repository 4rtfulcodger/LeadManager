using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Core.ViewModels
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public LoginDto(string userName, string password, bool rememberMe = false)
        {
            UserName = userName;
            Password = password;
            RememberMe = rememberMe;
        }
    }
}
