using StudentreBackend.Data.Models;

namespace StudentreBackend.Data.Seeders
{ 
    public class RoleSeeder
    {
        protected DefaultDbContext Context { get; }

        public RoleSeeder(DefaultDbContext dbContext)
        {
            Context = dbContext;
        }

        public void Seed()
        {
            if (Context.Database.CanConnect())
            {
                if (!Context.Roles.Any())
                {
                    var roles = GetRolse();
                    Context.Roles.AddRange(roles);
                    Context.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRolse()
        {
            var rolse = new List<Role>()
            {
                new Role()
                {
                    Value = "Admin"
                },
                new Role()
                {
                    Value = "Moderator"
                },
                new Role()
                {
                    Value = "User"
                }
            };
            return rolse;
        }
    }
}
