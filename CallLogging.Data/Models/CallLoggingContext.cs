using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CallLogging.Data.Models;

public partial class CallLoggingContext : DbContext
{
    public CallLoggingContext(DbContextOptions<CallLoggingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AgentMonthlyPerformance> AgentMonthlyPerformances { get; set; }

    public virtual DbSet<AgentWorkload> AgentWorkloads { get; set; }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<CallType> CallTypes { get; set; }

    public virtual DbSet<ClientProduct> ClientProducts { get; set; }

    public virtual DbSet<ClientProfile> ClientProfiles { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<LogStatus> LogStatuses { get; set; }

    public virtual DbSet<LogUpdate> LogUpdates { get; set; }

    public virtual DbSet<MyTicket> MyTickets { get; set; }

    public virtual DbSet<OpenTicketsDashboard> OpenTicketsDashboards { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PersonLogin> PersonLogins { get; set; }

    public virtual DbSet<Priority> Priorities { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<RecentActivity> RecentActivities { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SystemUser> SystemUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AgentMonthlyPerformance>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("AgentMonthlyPerformance", "Admin");

            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.SystemUserId).HasColumnName("SystemUserID");
        });

        modelBuilder.Entity<AgentWorkload>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("AgentWorkload", "Admin");

            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.SystemUserId).HasColumnName("SystemUserID");
            entity.Property(e => e.WorkloadLevel)
                .HasMaxLength(9)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.ToTable("Attachment", "Log");

            entity.HasIndex(e => e.CallLogId, "IX_Attachment_CallLogID");

            entity.HasIndex(e => e.UpdateId, "IX_Attachment_UpdateID");

            entity.HasIndex(e => e.UploadedById, "IX_Attachment_UploadedByID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CallLogId).HasColumnName("CallLogID");
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FileSizeKb)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("FileSizeKB");
            entity.Property(e => e.FileStoragePath).HasMaxLength(500);
            entity.Property(e => e.FileType).HasMaxLength(100);
            entity.Property(e => e.UpdateId).HasColumnName("UpdateID");
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UploadedById).HasColumnName("UploadedByID");

            entity.HasOne(d => d.CallLog).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.CallLogId)
                .HasConstraintName("FK_Attachment_Log");

            entity.HasOne(d => d.Update).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.UpdateId)
                .HasConstraintName("FK_Attachment_Update");

            entity.HasOne(d => d.UploadedBy).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.UploadedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attachment_Person");
        });

        modelBuilder.Entity<CallType>(entity =>
        {
            entity.ToTable("CallType", "Lookup");

            entity.HasIndex(e => e.Name, "UQ_CallType_Name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ClientProduct>(entity =>
        {
            entity.ToTable("ClientProduct", "Product");

            entity.HasIndex(e => e.ClientProfileId, "IX_ClientProduct_ClientProfileID");

            entity.HasIndex(e => new { e.ClientProfileId, e.ProductId }, "UQ_ClientProduct_ClientProduct").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AssignedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ClientProfileId).HasColumnName("ClientProfileID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.ClientProfile).WithMany(p => p.ClientProducts)
                .HasForeignKey(d => d.ClientProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientProduct_Client");

            entity.HasOne(d => d.Product).WithMany(p => p.ClientProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientProduct_Product");
        });

        modelBuilder.Entity<ClientProfile>(entity =>
        {
            entity.ToTable("ClientProfile", "Client");

            entity.HasIndex(e => e.Company, "IX_ClientProfile_Company");

            entity.HasIndex(e => e.PersonId, "UQ_ClientProfile_Person").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Company).HasMaxLength(150);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PersonId).HasColumnName("PersonID");

            entity.HasOne(d => d.Person).WithOne(p => p.ClientProfile)
                .HasForeignKey<ClientProfile>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientProfile_Person");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.ToTable("Log", "Log");

            entity.HasIndex(e => new { e.AssignedToId, e.StatusId }, "IX_Log_AssignedToID_StatusID");

            entity.HasIndex(e => new { e.ClientProfileId, e.ProductId }, "IX_Log_ClientID_ProductID");

            entity.HasIndex(e => new { e.ClientProfileId, e.StatusId }, "IX_Log_ClientProfileID_StatusID");

            entity.HasIndex(e => e.CreatedById, "IX_Log_CreatedByID");

            entity.HasIndex(e => new { e.StatusId, e.PriorityId }, "IX_Log_StatusID_PriorityID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AssignedToId).HasColumnName("AssignedToID");
            entity.Property(e => e.CallTypeId).HasColumnName("CallTypeID");
            entity.Property(e => e.ClientProfileId).HasColumnName("ClientProfileID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");
            entity.Property(e => e.MergedIntoCallId).HasColumnName("MergedIntoCallID");
            entity.Property(e => e.PriorityId).HasColumnName("PriorityID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.RelatedToCallId).HasColumnName("RelatedToCallID");
            entity.Property(e => e.StatusId)
                .HasDefaultValue(1)
                .HasColumnName("StatusID");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.AssignedTo).WithMany(p => p.LogAssignedTos)
                .HasForeignKey(d => d.AssignedToId)
                .HasConstraintName("FK_Log_AssignedTo");

            entity.HasOne(d => d.CallType).WithMany(p => p.Logs)
                .HasForeignKey(d => d.CallTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Log_CallType");

            entity.HasOne(d => d.ClientProfile).WithMany(p => p.Logs)
                .HasForeignKey(d => d.ClientProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Log_Client");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.LogCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Log_CreatedBy");

            entity.HasOne(d => d.MergedIntoCall).WithMany(p => p.InverseMergedIntoCall)
                .HasForeignKey(d => d.MergedIntoCallId)
                .HasConstraintName("FK_Log_MergedInto");

            entity.HasOne(d => d.Priority).WithMany(p => p.Logs)
                .HasForeignKey(d => d.PriorityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Log_Priority");

            entity.HasOne(d => d.Product).WithMany(p => p.Logs)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Log_Product");

            entity.HasOne(d => d.RelatedToCall).WithMany(p => p.InverseRelatedToCall)
                .HasForeignKey(d => d.RelatedToCallId)
                .HasConstraintName("FK_Log_RelatedTo");

            entity.HasOne(d => d.Status).WithMany(p => p.Logs)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Log_Status");
        });

        modelBuilder.Entity<LogStatus>(entity =>
        {
            entity.ToTable("LogStatus", "Lookup");

            entity.HasIndex(e => e.StatusName, "UQ_LogStatus_Name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<LogUpdate>(entity =>
        {
            entity.ToTable("LogUpdate", "Log");

            entity.HasIndex(e => new { e.CallLogId, e.StatusId }, "IX_LogUpdate_CallLogID_StatusID");

            entity.HasIndex(e => e.CreatedAt, "IX_LogUpdate_CreatedAt");

            entity.HasIndex(e => new { e.UpdatedById, e.CreatedAt }, "IX_LogUpdate_UpdatedByID_CreatedAt");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CallLogId).HasColumnName("CallLogID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedById).HasColumnName("UpdatedByID");

            entity.HasOne(d => d.CallLog).WithMany(p => p.LogUpdates)
                .HasForeignKey(d => d.CallLogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogUpdate_Log");

            entity.HasOne(d => d.Status).WithMany(p => p.LogUpdates)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_LogUpdate_Status");

            entity.HasOne(d => d.UpdatedBy).WithMany(p => p.LogUpdates)
                .HasForeignKey(d => d.UpdatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogUpdate_User");
        });

        modelBuilder.Entity<MyTicket>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("MyTickets", "Client");

            entity.Property(e => e.AssignedTo).HasMaxLength(100);
            entity.Property(e => e.CallType).HasMaxLength(50);
            entity.Property(e => e.ClientProfileId).HasColumnName("ClientProfileID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.Priority).HasMaxLength(50);
            entity.Property(e => e.Product).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<OpenTicketsDashboard>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("OpenTicketsDashboard", "Admin");

            entity.Property(e => e.AssignedTo).HasMaxLength(100);
            entity.Property(e => e.AssignedToId).HasColumnName("AssignedToID");
            entity.Property(e => e.CallType).HasMaxLength(50);
            entity.Property(e => e.ClientCompany).HasMaxLength(150);
            entity.Property(e => e.ClientName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.Priority).HasMaxLength(50);
            entity.Property(e => e.Product).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person", "Person");

            entity.HasIndex(e => new { e.Email, e.IsActive }, "IX_Person_Email_IsActive");

            entity.HasIndex(e => e.Email, "UQ_Person_Email").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastLoginAt).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(50);
        });

        modelBuilder.Entity<PersonLogin>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("PersonLogin", "Auth");

            entity.Property(e => e.ClientProfileId).HasColumnName("ClientProfileID");
            entity.Property(e => e.Company).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.LastLoginAt).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
            entity.Property(e => e.SystemUserId).HasColumnName("SystemUserID");
        });

        modelBuilder.Entity<Priority>(entity =>
        {
            entity.ToTable("Priority", "Lookup");

            entity.HasIndex(e => e.PriorityName, "UQ_Priority_Name").IsUnique();

            entity.HasIndex(e => e.SortOrder, "UQ_Priority_SortOrder").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PriorityName).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product", "Product");

            entity.HasIndex(e => e.Name, "UQ_Product_Name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<RecentActivity>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RecentActivity", "Admin");

            entity.Property(e => e.ActivityAt).HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.StatusAfterUpdate).HasMaxLength(50);
            entity.Property(e => e.SystemUserId).HasColumnName("SystemUserID");
            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.TicketTitle).HasMaxLength(255);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role", "Lookup");

            entity.HasIndex(e => e.Name, "UQ_Role_Name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<SystemUser>(entity =>
        {
            entity.ToTable("SystemUser", "Admin");

            entity.HasIndex(e => e.RoleId, "IX_SystemUser_RoleID");

            entity.HasIndex(e => e.PersonId, "UQ_SystemUser_Person").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Person).WithOne(p => p.SystemUser)
                .HasForeignKey<SystemUser>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SystemUser_Person");

            entity.HasOne(d => d.Role).WithMany(p => p.SystemUsers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SystemUser_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
