using Lamazon.DataAccess.DataContext;

namespace Lamazon.DataAccess.Implementacija
{
    public class BaseRepository 
    {
        protected readonly ApplicationDbContext _applicationDbContext;
        protected BaseRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    }
}
