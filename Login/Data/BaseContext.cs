using Login.Models;
using Microsoft.EntityFrameworkCore;

namespace Login.Data;

public class BaseContext : DbContext
{
    public BaseContext(DbContextOptions<BaseContext> options) : base(options)
    {
        
    }
    
    //conectamos Models
    public DbSet<Employee> Employees { get; set; }
}