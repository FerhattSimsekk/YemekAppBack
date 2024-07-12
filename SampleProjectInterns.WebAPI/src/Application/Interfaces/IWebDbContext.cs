using SampleProjectInterns.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IWebDbContext
{
    DbSet<Identity> Identities { get; }
    DbSet<Company> Companies { get; } 
    DbSet<City> Cities { get; }
    DbSet<County> Counties { get; }  

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
}
