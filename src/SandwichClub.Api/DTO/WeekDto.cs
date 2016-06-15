using System.Collections.Generic;

namespace SandwichClub.Api.DTO
{
    public class WeekDto
    {
        public int Id { get; set; }
        public int? Shopper { get; set; }
        public double Cost { get; set; }

        public IEnumerable<WeekUserLinkDto> Links { get; set; }
    }
}
