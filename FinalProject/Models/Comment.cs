using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace FinalProject.Models;

public class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ComId { get; set; }

    public string Text { get; set; } = string.Empty;

    public int LikeAmout { get; set; } = 0;
    
    public DateTime CreationTime { get; set; }

    public Review Review { get; set; }

    public User User { get; set; }

    public List<User> LikedBy { get; set; }

}