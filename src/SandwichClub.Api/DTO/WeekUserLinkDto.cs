namespace SandwichClub.Api.DTO
{
    public class WeekUserLinkDto
    {
        public UserDto User { get; set; }
        public double Paid { get; set; }
        public int Slices { get; set; }
    }
}
