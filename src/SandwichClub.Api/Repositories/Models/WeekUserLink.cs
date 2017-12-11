using System;
using System.ComponentModel;

namespace SandwichClub.Api.Repositories.Models
{
    [Description("Record of commitment to SandwichClub for the week")]
    public class WeekUserLink : IEntity
    {
        [Description("The id of the user")]
        public int UserId { get; set; }
        [Description("The id of the week")]
        public int WeekId { get; set; }
        [Obsolete("Paid amounts migrated to Payments")]
        [Description("The amount paid for the week")]
        public double Paid { get; set; }
        [Description("Whether or not the user has joined SandwichClub for the week")]
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
