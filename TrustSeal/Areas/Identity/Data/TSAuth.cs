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
       
    // // //     builder.Entity<BsAnswer>().ToTable("BsAnswers");
    // // //     // Customize the ASP.NET Identity model and override the defaults if needed.
    // // //     // For example, you can rename the ASP.NET Identity table names and more.
    // // //     // Add your customizations after calling base.OnModelCreating(builder);
    // // //     builder.Entity<Business>().ToTable(nameof(Businesses))
    // // //    .HasOne(e => e.Owner)
    // // //    .WithMany(e => e.UserBusinesses)
    // // //    .HasForeignKey(e => e.OwnerId)
    // // //    .IsRequired();

    // // //     builder.Entity<Question>()
    // // //          .HasOne(q => q.Category);
    //     builder.Entity<Business>()
    //     .HasOne(b=> b.Seal)
    //     .WithOne(s => s.Business)
    //     .HasForeignKey<Business>(b => b.SealId)
    //     .IsRequired(false);
        

    //     builder.Entity<Seal>()
    //     .Property(e => e.SealCode)
    //     .HasColumnType("uniqueidentifier")
    //     .HasDefaultValue("NEWSEQUENTIALID()");    

       

         builder.Entity<Business>()
            .HasOne(b => b.Seal)
            .WithOne(s => s.Business)
            .HasForeignKey<Business>(b => b.SealId)  // Set SealId as FK on Business side
            .IsRequired(false); // Optional relationship

        base.OnModelCreating(builder);
    }
    public DbSet<Business> Businesses { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionCategory> QuestionCategories { get; set; }

    public DbSet<BsAnswer> Answers { get; set; }

    public DbSet<BsAnswer> BsAnswer { get; set; }

    public DbSet<TrustSeal.Models.BusinessAttachment> BusinessAttachment { get; set; }

    // public DbSet<Notification> Notifications {get; set;}

    public DbSet<Seals> Seals {get;set;}

}
