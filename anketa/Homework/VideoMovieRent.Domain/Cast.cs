using VideoMovieRent.Domain.Enums;

namespace VideoMovieRent.Domain
{
    public class Cast : BaseEntity
    {
        public int MovieId { get; set; }
        public string Name { get; set; } = null!;
        public Part Part { get; set; } 
       
    }
}
