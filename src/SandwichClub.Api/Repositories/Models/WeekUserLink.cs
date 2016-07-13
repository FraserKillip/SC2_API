namespace SandwichClub.Api.Repositories.Models
{
    public class WeekUserLink : IEntity
    {
        public int UserId { get; set; }
        public int WeekId { get; set; }
        public double Paid { get; set; }
        public int Slices { get; set; }

        public override bool Equals (object o)
        {
            return o != null
                && GetType() == o.GetType()
                && WeekId == ((WeekUserLink) o).WeekId
                && UserId == ((WeekUserLink) o).UserId;
        }

        public override int GetHashCode()
        {
            return WeekId.GetHashCode() ^ UserId.GetHashCode();
        }
    }

    public struct WeekUserLinkId
    {
        public int UserId { get; set; }
        public int WeekId { get; set; }
    }
}
