namespace shop.ModelDTO.UserDTO
{
    public class UserProfileDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserLastName { get; set; }
        public bool EmailConfirmed { get; set; }
        public int OrdersCount { get; set; }
    }
}
