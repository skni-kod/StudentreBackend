using System;
using System.Collections.Generic;

namespace StudentreBackend.Data.Models
{
    public partial class UserGroup : IEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long GroupId { get; set; }

        public virtual Group Group { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
