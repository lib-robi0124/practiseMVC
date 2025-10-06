using GlasAnketa.DataAccess.DataContext;

namespace GlasAnketa.DataAccess.Implementations
{
    public abstract class BaseRepository
    {
        protected readonly AppDbContext _appDbcontext;
        public BaseRepository(AppDbContext appDbContext)
        {
            _appDbcontext = appDbContext;
        }
    }
}
