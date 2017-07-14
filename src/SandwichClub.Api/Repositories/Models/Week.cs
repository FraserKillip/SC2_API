using System.ComponentModel;

namespace SandwichClub.Api.Repositories.Models
{
    [Description("A week where SandwichClub groceries is bought")]
    public class Week : IEntity
    {
        [Description("The weeks id")]
        public int WeekId { get; set; }
        [Description("The user who did shopping for the week")]
        public int? ShopperUserId { get; set; }
        [Description("The cost of groceries for the week")]
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
