using SampleProjectInterns.Entities;
using SampleProjectInterns.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;

namespace SampleProjectInterns.Persistence;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Identity> Identities => Set<Identity>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<County> Counties => Set<County>(); 
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Employee> Employees => Set<Employee>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Identity>(ConfigureIdentity); 
        modelBuilder.Entity<Company>(ConfigureCompany);
        modelBuilder.Entity<City>(ConfigureCities);
        modelBuilder.Entity<County>(ConfigureCounties);  
        modelBuilder.Entity<Customer>(ConfigureCustomer);
        modelBuilder.Entity<Employee>(ConfigureEmployee);
    }
    private void ConfigureIdentity(EntityTypeBuilder<Identity> builder)
    {
        builder.ToTable("Identities", Schemas.Identity);


        builder.HasIndex(i => i.Email).IsUnique();

        builder.Property(i => i.Email).HasMaxLength(512);

        builder.Property(i => i.Password).HasMaxLength(1024);
        builder.Property(i => i.Salt).HasMaxLength(2048);

        builder.Property(i => i.Name).HasMaxLength(512);
        builder.Property(i => i.LastName).HasMaxLength(512);
    }

 
    private void ConfigureCities(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("Cities", Schemas.Definition);
        builder.HasIndex(i => i.Id).IsUnique();
    }
    private void ConfigureCounties(EntityTypeBuilder<County> builder)
    {
        builder.ToTable("Counties", Schemas.Definition);
        builder.HasIndex(i => i.Id).IsUnique();
    } 

    private void ConfigureCompany(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies", Schemas.Public);
        builder.HasMany(p => p.Identities).WithOne().HasForeignKey(mp => mp.CompanyId);
        builder.HasMany(p => p.Customers).WithOne().HasForeignKey(mp => mp.CompanyId);
        builder.HasMany(p => p.Employees).WithOne().HasForeignKey(mp => mp.CompanyId);
        builder.HasIndex(p => p.Id).IsUnique(true);
    }
    private void ConfigureCustomer(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers", Schemas.Public);
        builder.HasIndex(p => p.Id).IsUnique(true);
    }

    private void ConfigureEmployee(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees", Schemas.Public);
        builder.HasIndex(p => p.Id).IsUnique(true);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetCreatedAtAndUpdatedAt();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetCreatedAtAndUpdatedAt()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => (x.Entity is ICreatedAt || x.Entity is IUpdatedAt)
                && (x.State == EntityState.Added || x.State == EntityState.Modified))
            .ToList();

        if (entities.Count == 0) return;

        var currentDate = DateTime.UtcNow;

        foreach (var entity in entities)
        {
            switch (entity.State)
            {
                case EntityState.Modified:
                    if (entity.Entity is IUpdatedAt updatedAtEntity)
                    {
                        updatedAtEntity.UpdatedAt = currentDate;
                    }

                    break;

                case EntityState.Added:
                    if (entity.Entity is ICreatedAt createdAtEntity)
                    {
                        createdAtEntity.CreatedAt = currentDate;
                    }
                    break;
            }
        }
    }
}

