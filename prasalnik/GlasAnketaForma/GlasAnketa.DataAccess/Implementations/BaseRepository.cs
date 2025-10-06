using GlasAnketa.DataAccess.DataContext;

namespace GlasAnketa.DataAccess.Implementations
{
    public abstract class BaseRepository
    {
        protected readonly AppDbContext _appDbContext;
        public BaseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}
