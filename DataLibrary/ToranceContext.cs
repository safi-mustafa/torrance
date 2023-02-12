using Helpers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Common.Interfaces;
using Helpers.Models.Shared;
using System.Security.Claims;
using Models.TimeOnTools;
using Models.Common;
using Models.WeldingRodRecord;
using Models.AppSettings;
using Models.OverrideLogs;

namespace DataLibrary;

public class ToranceContext : IdentityDbContext<ToranceUser, ToranceRole, long>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ToranceContext(DbContextOptions<ToranceContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Contractor> Contractors { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<PermitType> PermitTypes { get; set; }
    public DbSet<ReworkDelay> ReworkDelays { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<ShiftDelay> ShiftDelays { get; set; }
    public DbSet<StatementOfWork> StatementOfWorks { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<RodType> RodTypes { get; set; }
    public DbSet<DelayType> DelayTypes { get; set; }
    public DbSet<WeldMethod> WeldMethods { get; set; }
    public DbSet<WRRLog> WRRLogs { get; set; }
    public DbSet<TOTLog> TOTLogs { get; set; }
    public DbSet<Map> Maps { get; set; }
    public DbSet<Dropbox> Dropboxes { get; set; }
    public DbSet<PermittingIssue> PermittingIssues { get; set; }
    //public DbSet<CraftRate> CraftRates { get; set; }
    public DbSet<CraftSkill> CraftSkills { get; set; }
    public DbSet<LeadPlanner> LeadPlanners { get; set; }
    public DbSet<OverrideType> OverrideTypes { get; set; }
    public DbSet<ReasonForRequest> ReasonForRequests { get; set; }
    public DbSet<OverrideLog> OverrideLogs { get; set; }
    public DbSet<OverrideLogCost> OverrideLogCost { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<ApproverUnit> ApproverUnits { get; set; }

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

        var addedList = ChangeTracker.Entries().Where(m => (m.Entity is BaseDBModel || m.Entity is ToranceUser) && m.State == EntityState.Added);
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
        var updatedList = ChangeTracker.Entries().Where(m => (m.Entity is BaseDBModel || m.Entity is ToranceUser) && m.State == EntityState.Modified);
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
