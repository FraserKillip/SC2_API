using System;
using System.Collections.Generic;
using System.Linq;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Repositories
{
    public class WeekUserLinkRepository : IWeekUserLinkRepository
    {
        private SC2Context _context;

        public WeekUserLinkRepository(SC2Context context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.WeekUserLinks.Count();
        }

        public void Delete(WeekUserLink t)
        {
            _context.WeekUserLinks.Remove(t);
        }

        public void Delete(int id)
        {
            Delete(GetById(id));
        }

        public IEnumerable<WeekUserLink> Get()
        {
            return _context.WeekUserLinks.ToList(); ;
        }

        public WeekUserLink GetById(int id)
        {
            return _context.WeekUserLinks.FirstOrDefault(w => w.WeekId == id);
        }

        public WeekUserLink Insert(WeekUserLink t)
        {
            var entity = _context.WeekUserLinks.Add(t).Entity;
            _context.SaveChanges();
            return entity;
        }

        public void Update(WeekUserLink t)
        {
            var thing = GetByUserAndWeek(t.UserId, t.WeekId);
            thing.Paid = t.Paid;
            thing.Slices = t.Slices;
            _context.WeekUserLinks.Update(thing);
            _context.SaveChanges();
        }

        public IEnumerable<WeekUserLink> GetByWeekId(int weekId)
        {
            return _context.WeekUserLinks.Where(wul => wul.WeekId == weekId).ToList();
        }

        public WeekUserLink GetByUserAndWeek(int userId, int weekId)
        {
            return _context.WeekUserLinks.FirstOrDefault(wul => wul.UserId == userId && wul.WeekId == weekId);
        }
    }
}
