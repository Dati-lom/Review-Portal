using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinalProject.ManyToMany;

namespace FinalProject.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Email { get; set; }

    public bool IsAdmin { get; set; } = false;

    public string HashedPassword { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public List<Review> Reviews { get; set; } = new List<Review>();
    public List<Comment> Comments { get; set; } = new List<Comment>();

    public List<Comment> LikedComments { get; set; }
    public List<Review> RatedReviews { get; set; }

}