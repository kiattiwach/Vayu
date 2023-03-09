using Microsoft.EntityFrameworkCore;
using Vayu.Staffs;
using Vayu.Tasks;
using Vayu.TaskTypes;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Vayu.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class VayuDbContext :
    AbpDbContext<VayuDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public DbSet<Task> Tasks { get; set; }
    public DbSet<TaskType> TaskTypes { get; set; }

    public VayuDbContext(DbContextOptions<VayuDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        builder.Entity<Task>(b =>
        {
            b.ToTable(VayuConsts.DbTablePrefix + "Tasks", VayuConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props

            b.Property(x => x.Name).IsRequired().HasMaxLength(TaskConsts.MaxNameLength);
            b.Property(x => x.Description).HasMaxLength(TaskConsts.MaxDescriptionLength);
            b.Property(x => x.Note).HasMaxLength(TaskConsts.MaxNoteLength);
            b.Property(x => x.ContactPerson).HasMaxLength(TaskConsts.MaxContactPersonLength);
            b.Property(x => x.ContactNumber).HasMaxLength(TaskConsts.MaxContactPersonNumberLength);
            b.HasIndex(x => new { x.Name, x.TaskTypeId }).IsUnique();
            b.HasOne(x => x.Staff).WithMany().HasForeignKey(x => x.LeaderId);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<TaskType>(b =>
        {
            b.ToTable(VayuConsts.DbTablePrefix + "TaskTypes", VayuConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props

            b.Property(x => x.Name).IsRequired().HasMaxLength(TaskTypeConsts.MaxNameLength);
            b.HasIndex(x => new { x.Name, x.Type }).IsUnique();

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<Staff>(b =>
        {
            b.ToTable(VayuConsts.DbTablePrefix + "Staffs", VayuConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props

            b.Property(x => x.Name).IsRequired().HasMaxLength(StaffConsts.MaxNameLength);
            b.Property(x => x.CodeName).IsRequired().HasMaxLength(StaffConsts.MaxCodeNameLength);
            b.Property(x => x.ContactNumber).IsRequired().HasMaxLength(StaffConsts.MaxContactNumberLength);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<TaskStaff>(b =>
        {
            b.ToTable(VayuConsts.DbTablePrefix + "TaskStaffs", VayuConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props

            b.HasKey(x => new { x.TaskId, x.StaffId});
            b.HasIndex(x => new { x.TaskId, x.StaffId }).IsUnique();
            b.HasOne<Task>().WithMany(x => x.TaskStaffs).HasForeignKey(x => x.TaskId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            b.HasOne<Staff>().WithMany().HasForeignKey(x => x.StaffId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            b.ApplyObjectExtensionMappings();
        });
    }
}
