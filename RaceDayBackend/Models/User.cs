using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace RaceDayBackend.Models

{

    public class User

    {

        [Key]

        public int UserID { get; set; }

        [Required]

        public int RoleID { get; set; }

        [Required, MaxLength(150)]

        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(150)]

        public string EmailAddress { get; set; } = string.Empty;

        [Required, MaxLength(255)]

        [JsonIgnore] // Security: Do not expose password hash in API responses

        public string PasswordHash { get; set; } = string.Empty;

        // Navigation Properties

        [ForeignKey("RoleID")]

        public Role? Role { get; set; }

        public ICollection<Event> ManagedEvents { get; set; } = new List<Event>();

        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();

    }

}