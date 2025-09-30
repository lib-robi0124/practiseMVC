namespace VideoMovieRent.Domain
{
    public class Admin : BaseEntity
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!; // Store hashed in production!
    }
}
