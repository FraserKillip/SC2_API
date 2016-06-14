using System.Collections.Generic;
using SandwichClub.Api.DTO;

namespace SandwichClub.Api.Services
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
