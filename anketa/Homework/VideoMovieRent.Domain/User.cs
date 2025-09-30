
using System.ComponentModel.DataAnnotations;
using VideoMovieRent.Domain.Enums;

namespace VideoMovieRent.Domain
{
    public class User : BaseEntity
    {
        public string FullName { get; set; } = null!;
        public int Age { get; set; }
        [Required]
        public string CardNumber { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public bool IsSubscriptionExpired { get; set; }
        public SubscriptionType SubscriptionType { get; set; }

    }
}
