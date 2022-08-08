using System.ComponentModel.DataAnnotations;

namespace ProEvents.Service.DTOs
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string EventDate { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "The {0} must be at least 3 chatacters and a maximum of 50")]
        public string Thema { get; set; }

        [Display(Name = "Amout of people")]
        [Range(1, 12000)]
        public int AmountPeople { get; set; }

        [RegularExpression(@".*\.(gif|jpe?g|png|bmp)$")]
        public string ImageUrl { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name = "e-mail")]
        [Required]
        [EmailAddress(ErrorMessage = "The email {0} is inválid")]
        public string Email { get; set; }

        public int UserId { get; set; }
        public IEnumerable<BatchDTO> Batches { get; set; }
        public IEnumerable<SocialNetworkDTO> SocialNetworks { get; set; }
        public IEnumerable<SpeakerDTO> Speakers { get; set; }
    }
}