using System.Collections.Generic;
using System.Linq;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Repositories
{
    public class WeekRepository : IWeekRepository
    {
        private SC2Context _context;

        public WeekRepository(SC2Context context)
        {
            _context = context;
        }

        public int Count()
        {
           return _context.Weeks.Count();
        }

        public void Delete(Week t)
        {
            _context.Weeks.Remove(t);
        }

        public void Delete(int id)
        {
           Delete(GetById(id));
        }

        public IEnumerable<Week> Get()
        {
            return _context.Weeks.ToList();;
        }

        public Week GetById(int id)
        {
            return _context.Weeks.FirstOrDefault(w => w.WeekId == id);
        }

        public Week Insert(Week t)
        {
            var entity = _context.Weeks.Add(t).Entity;
            _context.SaveChanges();
            return entity;
        }

        public void Update(Week t)
        {
           _context.Weeks.Update(t);
           _context.SaveChanges();
        }
    }
}
