using DataLibrary;
using Models;

namespace IdentityProvider.Seed
{
    public class SeedWorker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public SeedWorker(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ToranceContext>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            var alreadyCreatedRoles = context.Roles.Any();
            if (!alreadyCreatedRoles)
            {
                List<ToranceRole> roles = new List<ToranceRole>
                {
                    new ToranceRole
                    {
                        Name = "SuperAdmin",
                        NormalizedName = "SUPERADMIN",
                        ConcurrencyStamp = "5acd8273-22f2-487b-8971-f0208a532051",
                    },
                    new ToranceRole
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                        ConcurrencyStamp = "5acd8273-22f2-487b-8971-f0208a532051",
                    },
                    new ToranceRole
                    {
                        Name = "Approver",
                        NormalizedName = "APPROVER",
                        ConcurrencyStamp = "5acd8273-22f2-487b-8971-f0208a532051",
                    },
                    new ToranceRole
                    {
                        Name = "Foreman",
                        NormalizedName = "FOREMAN",
                        ConcurrencyStamp = "5acd8273-22f2-487b-8971-f0208a532051",
                    }
                };

                context.Roles.AddRange(roles);
                context.SaveChanges();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
