using VideoMovieRent.Domain.Enums;

namespace VideoMovieRent.Services.Dtos
{
    public class MovieDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Genre Genre { get; set; }
        public TimeSpan Length { get; set; }
        public int AgeRestriction { get; set; }
        public int Quantity { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Language Language { get; set; }
        public string? ImagePath { get; set; }
    }
}
