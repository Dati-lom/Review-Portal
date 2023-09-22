using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinalProject.Models;

namespace FinalProject.ManyToMany;

public class ReviewRating
{
    
    public int Rating { get; set; }
    public int UserId { get; set; }
    public int ReviewId { get; set; }
}