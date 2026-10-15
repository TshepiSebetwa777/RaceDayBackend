using System.ComponentModel.DataAnnotations;

namespace RaceDayBackend.DTOs

{

    public class EventDTO

    {

        [Required]

        public string Title { get; set; } = string.Empty;

        [Required]

        public DateTime ScheduledDate { get; set; }

        [Required]

        public string Description { get; set; } = string.Empty;

    }

}
