using System;
using System.Collections.Generic;

namespace StudentreBackend.Data.Models
{
    public partial class User : IEntity
    {
        public User()
        {
            UserGroups = new HashSet<UserGroup>();
        }

        public long Id { get; set; }
        public string PublicId { get; set; } = null!;
        public DateTime DateCreatedUtc { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? DateModifiedUtc { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? DateDeletedUtc { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Photo { get; set; }
        public string StudentId { get; set; } = null!;
        public string FieldOfStudy { get; set; } = null!;
        public string Term { get; set; } = null!;
        public string College { get; set; } = null!;
        public string Department { get; set; } = null!;
        public int Status { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public long RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}
