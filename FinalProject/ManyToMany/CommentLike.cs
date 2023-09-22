using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinalProject.Models;

namespace FinalProject.ManyToMany;

public class CommentLike
{
    public int UserId { get; set; }
    public int CommentId { get; set; }
}