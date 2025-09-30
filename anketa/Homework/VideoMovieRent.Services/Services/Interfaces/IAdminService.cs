using VideoMovieRent.Domain;
using VideoMovieRent.Services.Dtos;

namespace VideoMovieRent.Services.Services.Interfaces
{
    public interface IAdminService
    {
        Admin Login(string username, string password);
        void CreateMovie(MovieDetailsDto dto);
        void UpdateMovie(MovieDetailsDto dto);
        void DeleteMovie(int id);
    }
}
