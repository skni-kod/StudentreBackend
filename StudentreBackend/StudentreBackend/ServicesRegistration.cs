using FluentMigrator.Runner;
using System.Reflection;

namespace StudentreBackend
{
    public class ServicesRegistration
    {
        public static IServiceProvider CreateFluentService(string connectionString)
        {
            return new ServiceCollection().AddFluentMigratorCore().ConfigureRunner(rb =>
            rb.AddPostgres11_0().WithGlobalConnectionString(connectionString).ScanIn(Assembly.GetExecutingAssembly()).For.Migrations()).AddLogging(lb => 
            lb.AddFluentMigratorConsole()).BuildServiceProvider(false);
        }
    }
}
