namespace SC2_API.Repositories.Models
{
    public class WeekUserLink
    {
        public int UserId { get; set; }
        public int WeekId { get; set; }
        public double Paid { get; set; }
        public int Slices { get; set; }
    }
}
