namespace SandwichClub.Api.Repositories.Models
{
    public class Week : IEntity
    {
        public int WeekId { get; set; }
        public int? ShopperUserId { get; set; }
        public double Cost { get; set; }

        public override bool Equals(object o)
        {
            return o != null
                && GetType() == o.GetType()
                && WeekId == ((Week) o).WeekId;
        }

        public override int GetHashCode()
        {
            return WeekId.GetHashCode();
        }

        public override string ToString()
        {
            return WeekId + " - " + ShopperUserId + " - " + Cost;
        }
    }
}
