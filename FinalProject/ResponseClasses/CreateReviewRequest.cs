using FinalProject.Models;

namespace FinalProject.ResponseClasses;

public class CreateReviewRequest
{
    public ReviewDto ReviewDto { get; set; }
    public List<string> Tags { get; set; }
}