using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DotNetEnglishP5_3.Models;

namespace DotNetEnglishP5_3.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DotNetEnglishP5_3.Models.Inventory> Inventory { get; set; }
    }
}