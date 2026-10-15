using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayBackend.Models

{

    public class Event

    {

        [Key]

        public int EventID { get; set; }

        [Required]

        public int OrganiserID { get; set; }

        [Required, MaxLength(150)]

        public string Title { get; set; } = string.Empty;

        [Required]

        public DateTime ScheduledDate { get; set; }

        [Required]

        public string Description { get; set; } = string.Empty;

        // Navigation Properties

        [ForeignKey("OrganiserID")]

        public User? Organiser { get; set; }

        public ICollection<Sponsor> Sponsors { get; set; } = new List<Sponsor>();

        public ICollection<Category> Categories { get; set; } = new List<Category>();

    }

}
