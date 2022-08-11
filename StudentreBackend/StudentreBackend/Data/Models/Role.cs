using System;
using System.Collections.Generic;

namespace StudentreBackend.Data.Models
{
    public partial class Role : IEntity
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public long Id { get; set; }
        public string Value { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
