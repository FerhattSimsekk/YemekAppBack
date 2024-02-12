using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Persistence; 

namespace Infrastructure;

public class WebDbContext : AppDbContext, IWebDbContext
{
    public WebDbContext(DbContextOptions options) : base(options)
    {
    }
}
