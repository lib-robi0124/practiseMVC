using VideoMovieRent.Domain;
using VideoMovieRent.Domain.Enums;
using VideoMovieRent.Services.Dtos;

namespace VideoMovieRent.Services.Interfaces
{
    public interface IMovieService
    {
        IEnumerable<MovieDto> GetAllMovies();
        MovieDetailsDto GetMovieDetails(int id);
        Movie GetById(int id);
        IEnumerable<MovieDto> SearchMovies(string? title, Genre? genre, Language? language);

    }
}
