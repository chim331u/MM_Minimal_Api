using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoneyManagement.Models;
using MoneyManagement.Models.AncillaryData;
using MoneyManagement.Models.Balance;
using MoneyManagement.Models.BankAccount;
using MoneyManagement.Models.Bill;
using MoneyManagement.Models.HouseThings;
using MoneyManagement.Models.Identity;
using MoneyManagement.Models.IdentityAccess;
using MoneyManagement.Models.Salary;
using MoneyManagement.Models.Transactions;
using MoneyManagement.Models.Utility;

namespace MoneyManagement.AppContext;

/// <summary>
/// Represents the database context for the application.
/// </summary>
public class ApplicationContext(DbContextOptions<ApplicationContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    // // Default schema for the database context

    
    public DbSet<BankMasterData> BankMasterData { get; set; }
    public DbSet<AccountMasterData> AccountMasterData { get; set; }
    public DbSet<CardMasterData> CardMasterData { get; set; }
    public DbSet<Country> Country { get; set; }
    public DbSet<Currency> Currency { get; set; }
    public DbSet<Balance> Balance { get; set; }
    public DbSet<CurrencyConversionRate> CurrencyConversionRates { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<ServiceUser> ServiceUser { get; set; }
    public DbSet<Salary> Salary { get; set; }
    public DbSet<ISA_Accounts> ISA_Accounts { get; set; }
    public DbSet<ISA_PasswordsOld> ISA_PasswordsOld { get; set; }
    public DbSet<ServiceConfigs> ServiceConfigs { get; set; }
    public DbSet<HouseThings> HouseThings { get; set; }
    public DbSet<HouseThingsRooms> houseThingsRooms { get; set; }
    public DbSet<Supplier> suppliers { get; set; }
    public DbSet<Bill> bills{ get; set; }
    public DbSet<ReadInBill> readInBill { get; set; }

        
    /// <summary>
    /// Configures the model and relationships for the database context.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the database context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
      
        modelBuilder.Entity<BankMasterData>().ToTable("MM_BankMasterData").HasOne(c => c.Country);
        modelBuilder.Entity<AccountMasterData>().ToTable("MM_AccountMasterData");
        modelBuilder.Entity<CardMasterData>().ToTable("MM_CardMasterData");
        modelBuilder.Entity<Country>().ToTable("AD_Country");
        modelBuilder.Entity<Currency>().ToTable("AD_Currency");
        modelBuilder.Entity<Balance>().ToTable("MM_Balance");
        modelBuilder.Entity<CurrencyConversionRate>().ToTable("AD_CurrencyConversionRate");
        modelBuilder.Entity<Transaction>().ToTable("TX_Transaction");
        modelBuilder.Entity<ServiceUser>().ToTable("AD_ServiceUser");
        modelBuilder.Entity<Salary>().ToTable("SL_Salary");
        modelBuilder.Entity<ISA_Accounts>().ToTable("ISA_Accounts");
        modelBuilder.Entity<ISA_PasswordsOld>().ToTable("ISA_PasswordsOld");
        modelBuilder.Entity<ServiceConfigs>().ToTable("ServiceConfigs");
        modelBuilder.Entity<HouseThings>().ToTable("HouseThings");
        modelBuilder.Entity<HouseThingsRooms>().ToTable("HouseThingsRoom");
        modelBuilder.Entity<Supplier>().ToTable("Suppliers");
        modelBuilder.Entity<Bill>().ToTable("Bills");
        modelBuilder.Entity<ReadInBill>().ToTable("ReadInBill");
        
        // Apply configurations from the current assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

        // // Apply configurations from the current assembly again (duplicate call, consider removing if unnecessary).
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}