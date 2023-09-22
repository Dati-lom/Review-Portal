using FinalProject.ManyToMany;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Context;

public class ReviewContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ReviewContext(DbContextOptions<ReviewContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews) 
            .HasForeignKey(r => r.UserId) 
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<User>().HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey("commentFk")
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Review>().HasMany(r => r.Comments)
            .WithOne(c => c.Review)
            .HasForeignKey("commentrFk")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Review>()
            .HasMany(r => r.RatedBy)
            .WithMany(u => u.RatedReviews)
            .UsingEntity<ReviewRating>(rr =>
            {
                rr.HasOne<Review>().WithMany().HasForeignKey(r => r.ReviewId).OnDelete(DeleteBehavior.Restrict);
                rr.HasOne<User>().WithMany().HasForeignKey(u=> u.UserId).OnDelete(DeleteBehavior.Restrict);
            });
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.LikedComments)
            .WithMany(c => c.LikedBy)
            .UsingEntity<CommentLike>(
                rr =>
                {
                    rr.HasOne<Comment>().WithMany().HasForeignKey(c => c.CommentId).OnDelete(DeleteBehavior.Restrict);
                    rr.HasOne<User>().WithMany().HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.Restrict);
                }
                );
        



        // modelBuilder.Entity<CommentLike>()
        //     .HasOne(cl => cl.User)
        //     .WithMany(u => u.LikedComments)
        //     .HasForeignKey(cl => cl.LikedId)
        //     .OnDelete(DeleteBehavior.Restrict);
        //
        //
        //     
        // modelBuilder.Entity<ReviewRating>()
        //     .HasOne(r => r.User)
        //     .WithMany(u => u.RatedReviews)
        //     .HasForeignKey(r => r.RatedId)
        //     .OnDelete(DeleteBehavior.Restrict);


    }

    public DbSet<User> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<CommentLike> CommentLikes { get; set; }
    public DbSet<ReviewRating> ReviewRatings { get; set; }

    public DbSet<Tag> Tags { get; set; }
   

}