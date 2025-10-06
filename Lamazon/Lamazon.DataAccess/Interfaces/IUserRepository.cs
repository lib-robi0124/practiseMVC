using Lamazon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lamazon.DataAccess.Interfaces
{
    public interface IUserRepository
    {   //3 methods for user repository
        User GetById(int id);
        User GetByEmail(string email); // input email, return user
        int Insert (User user); // input user, return int (id of inserted user), add new user to database
    }
}
