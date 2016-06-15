namespace SandwichClub.Api.Repositories.Models
{
    public class Week
    {
        public int WeekId { get; set; }
        public int? ShopperUserId { get; set; }
        public double Cost { get; set; }
    }
}
