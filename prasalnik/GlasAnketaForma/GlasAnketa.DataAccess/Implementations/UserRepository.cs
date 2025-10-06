namespace GlasAnketa.DataAccess.Implementations
{
    public class UserRepository : BaseRepository, Interfaces.IUserRepository
    {
        public UserRepository(DataContext.AppDbContext appDbContext) : base(appDbContext)
        {
        }
        public Domain.Models.User GetByCompanyId(int companyId)
        {
            throw new NotImplementedException();
        }
        public Domain.Models.User GetByOU(string ou)
        {
            throw new NotImplementedException();
        }
        public int Insert(Domain.Models.User user)
        {
            throw new NotImplementedException();
        }
    }
    
}
