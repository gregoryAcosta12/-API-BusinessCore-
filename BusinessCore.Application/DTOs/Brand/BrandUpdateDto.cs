namespace BusinessCore.Application.DTOs.Brand
{
    public class BrandUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public string Website { get; set; }
        public bool IsActive { get; set; }
    }
}