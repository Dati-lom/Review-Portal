using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinalProject.Models;

namespace FinalProject.ManyToMany;

public class ReviewTag
{
    public int ReviewId { set; get; }
    public int TagId { set; get; }
}