using Helpers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Common.Interfaces;
using Models.Models.Shared;
using System.Security.Claims;

namespace DataLibrary;

public class ChargieContext : IdentityDbContext<ChargieUser, IdentityRole<long>, long>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChargieContext(DbContextOptions<ChargieContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }
   


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyGlobalFilters<IIsDeleted>(x => x.IsDeleted == false);
        base.OnModelCreating(modelBuilder);
    }
    public override int SaveChanges()
    {
        InitializeGlobalProperties();
        return base.SaveChanges();
    }
    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        InitializeGlobalProperties();
        return await base.SaveChangesAsync(true, cancellationToken);
    }
    private void InitializeGlobalProperties()
    {
        //To be fixed once we get user Id from Identity
        var userId = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userIdParsed = !string.IsNullOrEmpty(userId) ? long.Parse(userId) : 0;

        var addedList = ChangeTracker.Entries().Where(m => (m.Entity is BaseDBModel || m.Entity is ChargieUser) && m.State == EntityState.Added);
        foreach (var item in addedList)
        {
            if (item.Entity is IBaseModel)
            {
                ((IBaseModel)item.Entity).IsDeleted = false;
                //((BaseDBModel)item.Entity).IsActive = true;
                ((IBaseModel)item.Entity).CreatedBy = userIdParsed;
                ((IBaseModel)item.Entity).UpdatedBy = userIdParsed;
                ((IBaseModel)item.Entity).CreatedOn = DateTime.Now;
                ((IBaseModel)item.Entity).UpdatedOn = DateTime.Now;
            }
        }
        var updatedList = ChangeTracker.Entries().Where(m => (m.Entity is BaseDBModel || m.Entity is ChargieUser) && m.State == EntityState.Modified);
        foreach (var item in updatedList)
        {
            if (item.Entity is BaseDBModel)
            {
                ((BaseDBModel)item.Entity).UpdatedBy = userIdParsed;
                ((BaseDBModel)item.Entity).UpdatedOn = DateTime.Now;
            }
        }
    }
}
