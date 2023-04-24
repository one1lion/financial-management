using FinanMan.Database.Data;
using FinanMan.Database.Models.Shared;
using FinanMan.Database.Models.Tables;
using Microsoft.EntityFrameworkCore;
#if DEBUG
using System.Diagnostics;
#endif

namespace FinanMan.Database;

public class FinanManContext : DbContext
{
    public FinanManContext(DbContextOptions<FinanManContext> options) : base(options) { }

    protected FinanManContext() { }

    #region Tables
    public virtual DbSet<Account> Accounts { get; set; } = default!;
    public virtual DbSet<Deposit> Deposits { get; set; } = default!;
    public virtual DbSet<Payee> Payees { get; set; } = default!;
    public virtual DbSet<Payment> Payments { get; set; } = default!;
    public virtual DbSet<PaymentDetail> PaymentDetails { get; set; } = default!;
    public virtual DbSet<ScheduledTransaction> ScheduledTransactions { get; set; } = default!;
    public virtual DbSet<Transaction> Transactions { get; set; } = default!;
    public virtual DbSet<Transfer> Transfers { get; set; } = default!;

    #region Lookups
    public virtual DbSet<LuAccountType> AccountTypes { get; set; } = default!;
    public virtual DbSet<LuCategory> Categories { get; set; } = default!;
    public virtual DbSet<LuDepositReason> DepositReasons { get; set; } = default!;
    public virtual DbSet<LuLineItemType> LineItemTypes { get; set; } = default!;
    public virtual DbSet<LuRecurrenceType> RecurrenceTypes { get; set; } = default!;
    #endregion Lookups
    #endregion Tables

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // DukaSoft rocks
#if DEBUG
        optionsBuilder.LogTo(message => Debug.WriteLine(message)); 
#endif
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .IsRequired();

            entity.HasIndex(e => e.Name);
        });

        modelBuilder.Entity<Deposit>(entity =>
        {
            entity.HasOne(x => x.DepositReason)
                .WithMany(x => x.Deposits)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<LuAccountType>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsRequired();

            entity.HasIndex(e => e.Name);
        });

        modelBuilder.Entity<LuCategory>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsRequired();

            entity.HasIndex(e => e.Name);
        });
        
        modelBuilder.Entity<LuDepositReason>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsRequired();

            entity.HasIndex(e => e.Name);
        });

        modelBuilder.Entity<LuLineItemType>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsRequired();

            entity.HasIndex(e => e.Name);
        });

        modelBuilder.Entity<LuRecurrenceType>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(e => e.DisplayText)
                .HasMaxLength(20)
                .IsRequired();

            entity.HasIndex(e => e.Name);
        });

        modelBuilder.Entity<Payee>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .IsRequired();

            entity.HasIndex(e => e.Name);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(e => e.Memo)
                .HasMaxLength(255);
        });

        modelBuilder.Entity<PaymentDetail>(entity =>
        {
            entity.Property(e => e.Detail)
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasOne(e => e.Transaction)
                .WithOne(e => e.Transfer)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasOne(e => e.TargetAccount)
                .WithMany(e => e.Transfers)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Seed();
    }

    /// <summary>  
    /// Overriding Save Changes to update the LastUpdated field for entities 
    /// that implement <see cref="ILookupItem"/>
    /// </summary>  
    /// <returns></returns>  
    public override int SaveChanges()
    {
        UpdateLastUpdated();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateLastUpdated();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateLastUpdated();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        UpdateLastUpdated();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    // Adapted from: 
    // https://www.c-sharpcorner.com/UploadFile/d87001/overriding-savechanges-in-entity-framework/
    private void UpdateLastUpdated()
    {
        var selectedEntityList = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified)
            .Select(x => x.Entity)
            .OfType<ILookupItem>();

        foreach (var entity in selectedEntityList)
        {
            entity.LastUpdated = DateTime.UtcNow;
        }
    }
}
