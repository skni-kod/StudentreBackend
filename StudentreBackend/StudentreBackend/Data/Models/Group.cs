using System;
using System.Collections.Generic;

namespace StudentreBackend.Data.Models
{
    public partial class Group : IEntity
    {
        public Group()
        {
            UserGroups = new HashSet<UserGroup>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}
