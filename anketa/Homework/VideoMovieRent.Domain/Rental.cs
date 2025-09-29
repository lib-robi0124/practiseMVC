namespace VideoMovieRent.Domain
{
    public class Rental : BaseEntity
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public DateTime RentedOn { get; set; }
        public DateTime? ReturnedOn { get; set; } = null;

    }
}
