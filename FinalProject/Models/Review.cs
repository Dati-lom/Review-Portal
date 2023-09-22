using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinalProject.ManyToMany;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FinalProject.Models;

public class Review
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RevId { get; set; }
    
    public string ReviewName { get; set; }   
    public string ReviewedPiece { get; set; }
    public string Group { get; set; }          
    public string Tags { get; set; }       // List of tags (many-to-many relationship)
    public string ReviewText { get; set; }    
    public string ImageUrl { get; set; }      
    public double Rating { get; set; }        
    public DateTime CreationDate { get; set; }

    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public List<Comment> Comments { get; set; }
    
    public List<User> RatedBy { get; set; }
}