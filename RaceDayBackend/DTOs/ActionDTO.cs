using System.ComponentModel.DataAnnotations;

namespace RaceDayBackend.DTOs

{

    public class CategoryDTO

    {

        [Required]

        public int EventId { get; set; }

        [Required, MaxLength(100)]

        public string TypeName { get; set; } = string.Empty;

        [Required]

        public decimal DistanceKm { get; set; }

    }

    public class EnrolmentDto

    {

        [Required]

        public int CategoryId { get; set; }

    }

    public class ResultDTO

    {

        [Required]

        public int EnrolmentId { get; set; }

        [Required, MaxLength(20)]

        public string TimeRecorded { get; set; } = string.Empty;

        [Required, MaxLength(50)]

        public string MedalType { get; set; } = string.Empty;

    }

}