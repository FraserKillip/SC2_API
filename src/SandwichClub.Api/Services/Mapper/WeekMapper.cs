using SandwichClub.Api.DTO;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services.Mapper
{
    public class WeekMapper : BaseMapper<Week, WeekDto>
    {
        public override Week ToModel(WeekDto week)
        {
            return new Week
            {
                WeekId = week.Id,
                ShopperUserId = week.Shopper,
                Cost = week.Cost
            };
        }

        public override WeekDto ToDto(Week week)
        {
            return new WeekDto
            {
                Id = week.WeekId,
                Shopper = week.ShopperUserId,
                Cost = week.Cost
            };
        }
    }
}
