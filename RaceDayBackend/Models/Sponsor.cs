using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayBackend.Models

{

    public class Sponsor

    {

        [Key]

        public int SponsorID { get; set; }

        [Required]

        public int EventID { get; set; }

        [Required, MaxLength(100)]

        public string CompanyName { get; set; } = string.Empty;

        [MaxLength(50)]

        public string SponsorshipTier { get; set; } = "Bronze";

        // Navigation Property

        [ForeignKey("EventID")]

        public Event? Event { get; set; }

    }

}

