namespace SC2_API.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public bool Shopper { get; set; }
        public string BankDetails { get; set; }
        public string PhoneNumber { get; set; }
        public string BankName { get; set; }
        public bool FirstLogin { get; set; }
    }
}
