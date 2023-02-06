using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace NotificationWorkerService.Context
{
    public class NotificationDbContext : IdentityDbContext<ToranceUser, IdentityRole<long>, long>
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
        {

        }
        public DbSet<Notification> Notifications { get; set; }
    }
}

