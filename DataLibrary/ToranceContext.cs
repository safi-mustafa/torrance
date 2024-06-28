using Microsoft.AspNetCore.Http;
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
using Models.FCO;
using Newtonsoft.Json;
using ViewModels.Shared;
using CorrelationId.Abstractions;
using Enums;
using Helpers.Extensions;

namespace DataLibrary;

public class ToranceContext : IdentityDbContext<ToranceUser, ToranceRole, long>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICorrelationContextAccessor _correlationContext;
    private long _userId
    {
        get
        {
            var userId = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userIdParsed = !string.IsNullOrEmpty(userId) ? long.Parse(userId) : 0;
            return userIdParsed;
        }
    }

    public ToranceContext(DbContextOptions<ToranceContext> options, IHttpContextAccessor httpContextAccessor, ICorrelationContextAccessor correlationContext)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _correlationContext = correlationContext;
    }

    public DbSet<Contractor> Contractors { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<DepartmentUnit> DepartmentUnits { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<PermitType> PermitTypes { get; set; }
    public DbSet<ReworkDelay> ReworkDelays { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<ShiftDelay> ShiftDelays { get; set; }

    public DbSet<StartOfWorkDelay> StartOfWorkDelays { get; set; }
    public DbSet<StatementOfWork> StatementOfWorks { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<RodType> RodTypes { get; set; }
    public DbSet<FCOType> FCOTypes { get; set; }
    public DbSet<FCOReason> FCOReasons { get; set; }
    public DbSet<FCOComment> FCOComments { get; set; }
    public DbSet<FCOSection> FCOSections { get; set; }
    public DbSet<DelayType> DelayTypes { get; set; }
    public DbSet<WeldMethod> WeldMethods { get; set; }
    public DbSet<WRRLog> WRRLogs { get; set; }
    public DbSet<TOTLog> TOTLogs { get; set; }
    public DbSet<FCOLog> FCOLogs { get; set; }
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
    public DbSet<CompanyCraft> CompanyCrafts { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<ApproverAssociation> ApproverAssociations { get; set; }
    public DbSet<OngoingWorkDelay> OngoingWorkDelays { get; set; }
    public DbSet<LogData> LogDatas { get; set; }

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

    public async virtual Task<int> SaveChangesAsync(IBaseCrudViewModel viewModel, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            InitializeGlobalProperties();
            await OnBeforeSaveChanges(viewModel);
            return await base.SaveChangesAsync(true, cancellationToken);
        }
        catch (Exception ex)
        {
            return 0;
        }
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
                ((IBaseModel)item.Entity).CreatedOn = DateTime.UtcNow;
                ((IBaseModel)item.Entity).UpdatedOn = DateTime.UtcNow;
            }
        }
        var updatedList = ChangeTracker.Entries().Where(m => (m.Entity is BaseDBModel || m.Entity is ToranceUser) && m.State == EntityState.Modified);
        foreach (var item in updatedList)
        {
            if (item.Entity is BaseDBModel)
            {
                ((BaseDBModel)item.Entity).UpdatedBy = userIdParsed;
                ((BaseDBModel)item.Entity).UpdatedOn = DateTime.UtcNow;
            }
        }
    }

    private async Task OnBeforeSaveChanges(IBaseCrudViewModel viewModel)
    {
        try
        {
            var correlationId = _correlationContext.CorrelationContext?.CorrelationId;
            ChangeTracker.DetectChanges();
            var auditEntries = new List<LogDataViewModel>();
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is LogData || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new LogDataViewModel(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = _userId;
                auditEntry.CorrelationId = correlationId;
                auditEntry.JsonDBModelData = JsonConvert.SerializeObject(entry.Entity, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                auditEntry.JsonViewModelData = JsonConvert.SerializeObject(viewModel, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                var currentEntryState = entry.State;
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (currentEntryState)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = LogDataAction.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = LogDataAction.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = LogDataAction.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                LogDatas.Add(auditEntry.ToAudit());
            }
        }
        catch (Exception ex)
        {

        }

    }
}
