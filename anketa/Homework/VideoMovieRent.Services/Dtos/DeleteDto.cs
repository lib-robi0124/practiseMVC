using VideoMovieRent.Domain;
using VideoMovieRent.Domain.Enums;

namespace VideoMovieRent.Services.Dtos
{
    public class DeleteDto : BaseEntity
    {
        public string Title { get; set; }
        public Genre Genre { get; set; }
        public int Quantity { get; set; }
    }
}
