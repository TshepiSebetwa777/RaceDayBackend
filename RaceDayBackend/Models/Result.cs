using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayBackend.Models

{

    public class Result

    {

        [Key]

        public int ResultID { get; set; }

        [Required]

        public int EnrolmentID { get; set; }

        [Required, MaxLength(20)]

        public string TimeRecorded { get; set; } = string.Empty;

        [Required, MaxLength(50)]

        public string MedalType { get; set; } = string.Empty;

        // Navigation Property

        [ForeignKey("EnrolmentID")]

        public Enrolment? Enrolment { get; set; }

    }

}
