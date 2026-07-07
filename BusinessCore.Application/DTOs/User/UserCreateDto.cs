namespace BusinessCore.Application.DTOs.User
{
    public class UserCreateDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImageUrl { get; set; }
        public List<int> RoleIds { get; set; }
    }
}