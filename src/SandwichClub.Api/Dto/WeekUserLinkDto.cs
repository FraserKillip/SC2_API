namespace SandwichClub.Api.Dto
{
    public class WeekUserLinkDto
    {
        public int UserId { get; set; }
        public int WeekId { get; set; }
        public double Paid { get; set; }
        public int Slices { get; set; }
    }
}
