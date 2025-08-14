using Microsoft.EntityFrameworkCore;
using OrderApi.Domain.Entity;

namespace OrderApi.Infrustructure.Data
{
    public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; } = null!;
    }
}