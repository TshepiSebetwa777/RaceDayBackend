using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayBackend.Models

{

    public class Enrolment

    {

        [Key]

        public int EnrolmentID { get; set; }

        [Required]

        public int UserID { get; set; }

        [Required]

        public int CategoryID { get; set; }

        [MaxLength(20)]

        public string Status { get; set; } = "Confirmed";

        // Navigation Properties

        [ForeignKey("UserID")]

        public User? User { get; set; }

        [ForeignKey("CategoryID")]

        public Category? Category { get; set; }

        public Result? Result { get; set; }

    }

}
