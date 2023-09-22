namespace FinalProject.Models;

public class ReviewDto
{
    public string ReviewName { get; set; }   
    public string ReviewedPiece { get; set; } 
    public string Group { get; set; }
    public string ReviewText { get; set; }
    public string ImageUrl { get; set; }      
    public double Rating { get; set; }
    public List<string> Tags { get; set; }


}