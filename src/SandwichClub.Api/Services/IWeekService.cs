using System.Collections.Generic;
using SC2_API.DTO;

namespace SC2_API.Services
{
    public interface IWeekService
    {
        WeekDto GetById(int id);

        IEnumerable<WeekDto> Get();

        int Count();

        WeekDto Insert(WeekDto weekDto);

        void Update(WeekDto weekDto);

        void Delete(int id);

        void Delete(WeekDto weekDto);
    }
}
