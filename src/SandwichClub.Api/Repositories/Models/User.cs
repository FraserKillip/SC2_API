using System.ComponentModel;

namespace SandwichClub.Api.Repositories.Models
{
    [Description("A person who is a part of SandwichClub")]
    public class User : IEntity
    {
        [Description("The Id of the user")]
        public int UserId { get; set; }
        [Description("The FacebookId of the user")]
        public string FacebookId { get; set; }
        [Description("The first name of the user")]
        public string FirstName { get; set; }
        [Description("The last name of the user")]
        public string LastName { get; set; }
        [Description("The email of the user")]
        public string Email { get; set; }
        [Description("The AvatarUrl of the user")]
        public string AvatarUrl { get; set; }
        [Description("Whether the user is a shopper")]
        public bool Shopper { get; set; }
        [Description("The bank details of the user")]
        public string BankDetails { get; set; }
        [Description("The phone number of the user")]
        public string PhoneNumber { get; set; }
        [Description("The name of the users bank")]
        public string BankName { get; set; }
        [Description("Whether the user is logging in for the first time or not")]
        public bool FirstLogin { get; set; }

        public override bool Equals(object o)
        {
            return o != null
                && GetType() == o.GetType()
                && UserId == ((User) o).UserId;
        }

        public override int GetHashCode()
        {
            return UserId.GetHashCode();
        }
    }
}
