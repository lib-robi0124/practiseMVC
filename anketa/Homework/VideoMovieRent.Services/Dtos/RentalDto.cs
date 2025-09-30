namespace VideoMovieRent.Services.Dtos
{
    public class RentalDto
    {
        public required string MovieTitle { get; set; }
        public DateTime RentedOn { get; set; }
        public required string cardNumber { get; set; }
    }
}
