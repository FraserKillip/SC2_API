using System;
using System.Collections.Generic;
using System.Linq;
using SC2_API.Repositories.Models;

namespace SC2_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        SC2Context _context;
        public UserRepository(SC2Context context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Users.Count();
        }

        public void Delete(User t)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> Get()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.First(u => u.UserId == id);
        }

        public User Insert(User t)
        {
            throw new NotImplementedException();
        }

        public void Update(User t)
        {
            throw new NotImplementedException();
        }
    }
}
