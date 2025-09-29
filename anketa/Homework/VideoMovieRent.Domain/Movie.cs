using VideoMovieRent.Domain.Enums;

namespace VideoMovieRent.Domain
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; } = null!; // Ensure Title is never null
        public Genre Genre { get; set; } // Consider using an Enum here
        public Language Language { get; set; } // Consider using an Enum here
        public bool IsAvailable { get; set; }
        public DateTime ReleaseDate { get; set; }
        public TimeSpan Length { get; set; }
        public int AgeRestriction { get; set; }
        public int Quantity { get; set; }
        public string? ImagePath { get; set; }
             
    }
}
