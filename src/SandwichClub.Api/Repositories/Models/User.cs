namespace SandwichClub.Api.Repositories.Models
{
    public class User : IEntity
    {
        public int UserId { get; set; }
        public string FacebookId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public bool Shopper { get; set; }
        public string BankDetails { get; set; }
        public string PhoneNumber { get; set; }
        public string BankName { get; set; }
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
