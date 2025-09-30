using Microsoft.AspNetCore.Http;
using VideoMovieRent.DataAccess.Interfaces;
using VideoMovieRent.Domain;
using VideoMovieRent.Services.Dtos;
using VideoMovieRent.Services.Services.Interfaces;

namespace VideoMovieRent.Services.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public void CreateMovie(MovieDetailsDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                Genre = dto.Genre,
                Language = dto.Language,
                Quantity = 1, // Default quantity for new movies
                ImagePath = dto.ImagePath
            };
            _adminRepository.Create(movie);
        }

        public void DeleteMovie(int id)
        {
            _adminRepository.Delete(id);
        }

        public Admin? Login(string username, string password)
        {
            return _adminRepository.Login(username, password);
        }
        public void UpdateMovie(MovieDetailsDto entity)
        {
            var movie = new Movie
            {
                Id = entity.Id,
                Title = entity.Title,
                Genre = entity.Genre,
                Length = entity.Length,
                AgeRestriction = entity.AgeRestriction,
                Quantity = entity.Quantity,
                ReleaseDate = entity.ReleaseDate,
                Language = entity.Language,
                ImagePath = entity.ImagePath
            };
        }
    }
}
