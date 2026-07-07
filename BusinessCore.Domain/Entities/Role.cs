using System.Collections.Generic;
using System.Security;

namespace BusinessCore.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;

     
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}