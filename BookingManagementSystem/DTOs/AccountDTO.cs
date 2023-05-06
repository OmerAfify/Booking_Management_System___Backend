using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagementSystem.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Password must have Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character.")]

        public string Password { get; set; }

    }


    public class RegisterDTO : LoginDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }


    public class GeneralUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }

    }

    public class AddNewUserDTO : GeneralUserDTO
    {
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Password must have Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character.")]
        public string Password { get; set; }
    }


    public class ReturnedUserDTO : GeneralUserDTO
    {
       public string Id { get; set; }
    }


    public class UpdateUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Roles { get; set; }


    }





}
