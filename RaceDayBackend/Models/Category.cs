using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayBackend.Models

{

    public class Category

    {

        [Key]

        public int CategoryID { get; set; }

        [Required]

        public int EventID { get; set; }

        [Required, MaxLength(100)]

        public string TypeName { get; set; } = string.Empty;

        [Required]

        [Column(TypeName = "decimal(6,2)")]

        public decimal DistanceKM { get; set; }

        // Navigation Properties

        [ForeignKey("EventID")]

        public Event? Event { get; set; }

        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();

    }

}
