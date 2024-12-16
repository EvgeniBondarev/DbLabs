using Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Context
{

    public class AppDbContext : DbContext
    {
        public DbSet<Lab> Labs { get; set; }
        public DbSet<Domain.Models.LabTask> Tasks { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }
    }

}
