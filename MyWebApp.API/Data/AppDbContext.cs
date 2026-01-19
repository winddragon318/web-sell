using Microsoft.EntityFrameworkCore;
using MyWebApp.Shared.Models;

namespace MyWebApp.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tên DbSet này phải trùng tên bảng trong SQL Server
        public DbSet<User> Users { get; set; }
        // ---------------------
        public DbSet<ProductFS> ProductFS { get; set; }
        // ---------------------
    }
}