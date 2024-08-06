using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using TrustSeal.Areas.Identity.Data;
using TrustSeal.Models;

namespace TrustSeal.Areas.Identity.Data;

public class TSAuth : IdentityDbContext<TSUser>
{
    public TSAuth(DbContextOptions<TSAuth> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<Business>().ToTable(nameof(Businesses))
       .HasOne(e => e.Owner)
       .WithMany(e => e.UserBusinesses)
       .HasForeignKey(e => e.OwnerId)
       .IsRequired();

        builder.Entity<Question>()
             .HasOne(q => q.Category);

        
           
    }
    public DbSet<Business> Businesses { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionCategory> QuestionCategories { get; set; }

}
