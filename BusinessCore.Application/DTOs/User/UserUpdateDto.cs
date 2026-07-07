namespace BusinessCore.Application.DTOs.User
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool IsActive { get; set; }
        public List<int> RoleIds { get; set; }
    }
}