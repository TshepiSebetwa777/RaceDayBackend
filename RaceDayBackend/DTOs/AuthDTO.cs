using System.ComponentModel.DataAnnotations;

namespace RaceDayBackend.DTOs

{

    // Matches the request body for /api/account/register

    public class RegisterDTO

    {

        [Required]

        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]

        public string EmailAddress { get; set; } = string.Empty;

        [Required, MinLength(6)]

        public string Password { get; set; } = string.Empty;

        [Required]

        public int RoleId { get; set; } // 1 for Organiser, 2 for Participant

    }

    // Matches the request body for /api/account/login

    public class LoginDto

    {

        [Required, EmailAddress]

        public string EmailAddress { get; set; } = string.Empty;

        [Required]

        public string Password { get; set; } = string.Empty;

    }

}