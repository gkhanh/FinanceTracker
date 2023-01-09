using System.Security.Cryptography.X509Certificates;
using Finance_Tracking_Web_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance_Tracking_Web_Application.Models;

public class ApplicationDbContext:DbContext
{
    //constructor to pass the database provider like mysql or sql server ...
    public ApplicationDbContext( DbContextOptions options): base(options)
    {
        
    }
    
    public DbSet<Transaction> Transactions { get; set; } //Get transaction class property into this class
    public DbSet<Category> Categories { get; set; } //Get category class property into this class
}