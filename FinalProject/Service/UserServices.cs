using System.Runtime.InteropServices.JavaScript;
using FinalProject.Context;
using FinalProject.ManyToMany;
using FinalProject.Models;
using FinalProject.ResponseClasses;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Service;

public class UserServices:IUserServices
{
    private readonly ReviewContext _reviewContext;

    public UserServices(ReviewContext reviewContext)
    {
        _reviewContext = reviewContext;
    }
    

    public Review CreateReview(int userId,ReviewDto reviewDto)
    {
        if (_reviewContext.Reviews.Any(r => r.ReviewName == reviewDto.ReviewName)) return null;
        var user = _reviewContext.Users.FirstOrDefault(u => u.UserId == userId);
        if (user == null) return null;
        
        var review = new Review
        {
            ReviewName = reviewDto.ReviewName,
            Rating = reviewDto.Rating,
            ReviewedPiece = reviewDto.ReviewedPiece,
            Group = reviewDto.Group,
            Tags = reviewDto.Tags.Aggregate((cur,next)=> cur + "," + next),
            ReviewText = reviewDto.ReviewText,
            ImageUrl = reviewDto.ImageUrl,
            CreationDate = DateTime.Today.Date,
            UserId = userId
            
        };
        _reviewContext.Reviews.Add(review);
        _reviewContext.SaveChanges();
        
        return review;
    }

    public Response DeleteReview(int reviewId)
    {
        var reviewToDelete = _reviewContext.Reviews.FirstOrDefault(r => r.RevId == reviewId);
        if (reviewToDelete == null)
        {
            return new Response("Error Deleting Wrong Id",false);
        }

        _reviewContext.Reviews.Remove(reviewToDelete);
        _reviewContext.SaveChanges();

        return new Response("Success on delete", true);
    }

    public Response EditReview(ReviewDto reviewDto,int id)
    {
        var review = _reviewContext.Reviews.FirstOrDefault();
        if (review == null) return new Response("Selection Error", false);

        review.ReviewName = reviewDto.ReviewName;
        review.Rating = reviewDto.Rating;
        review.Group = reviewDto.Group;
        review.ReviewText = reviewDto.ReviewText;
        review.ImageUrl = reviewDto.ImageUrl;
        review.CreationDate = DateTime.Today.Date;
        _reviewContext.SaveChanges();

        return new Response("Succesfull edit", true);
    }

    public  Comment AddComment(CommentDto commentDto)
    {
        var comment = new Comment
        {
            LikeAmout = 0,
            CreationTime = DateTime.Now,
            Text = commentDto.Text,
            User = _reviewContext.Users.Find(commentDto.UserId),
            Review = _reviewContext.Reviews.Find(commentDto.ReviewId)
        };

        _reviewContext.Comments.Add(comment);
        _reviewContext.SaveChanges();
        return comment;
    }

    public Response removeComment(int id)
    {
        var comment = _reviewContext.Comments.FirstOrDefault(u => u.ComId == id);
        if (comment == null) return new Response("Comment not found",false);

        _reviewContext.Comments.Remove(comment);
        _reviewContext.SaveChanges();

        return new Response("Deleted Succesfully", true);
    }

    public List<LikeResp> GetAllComments(int sortStatus, int reviewId ,int userId)
    {
        IQueryable<Comment> query = _reviewContext.Comments;

        query = query.Where(c => c.Review.RevId == reviewId);

        switch (sortStatus)
        {
            case 1:
                query = query.OrderBy(c => c.CreationTime);
                break;
            case 2:
                query = query.OrderByDescending(c => c.CreationTime);
                break;
            case 3:
                query = query.OrderBy(c => c.LikeAmout);
                break;
            case 4:
                query = query.OrderByDescending(c => c.LikeAmout);
                break;
            default:
                query = query.OrderBy(c => c.CreationTime);
                break;
        }

        List<LikeResp> comments = query
            .Select(comment => new LikeResp
                {
                    IsLikedByThisUser = comment.LikedBy.Any(u => u.UserId == userId),
                    Comment = comment
                }
            ).ToList();
        
        return comments;
    }

    public Response LikeComment(int userId, int commentId)
    {
        var comments = _reviewContext.Comments
            .Include(c => c.LikedBy)
            .FirstOrDefault(c => c.ComId == commentId);
        if (comments == null) return new Response($"Comment with {commentId} was not fount", false);

        var user = _reviewContext.Users.FirstOrDefault(u => u.UserId == userId);
        if (user == null) return new Response($"User with {userId} was not found", false);

        var likeDcomment = _reviewContext
            .CommentLikes
            .FirstOrDefault(cl => cl.CommentId == commentId && cl.UserId == userId);

        if (likeDcomment == null)
        {
            var newlike = new CommentLike
            {
                CommentId = commentId,
                UserId = userId
            };
            _reviewContext.CommentLikes.Add(newlike);
            comments.LikeAmout++;
        }
        else
        {
            _reviewContext.CommentLikes.Remove(likeDcomment);
            comments.LikeAmout--;
        }
        _reviewContext.SaveChanges();

        return new Response("Comment Liked/Unliked succesfully",true);
    }

    public Response RateReview(int userId, int reviewId, int amount)
    {
        var review = _reviewContext.Reviews.FirstOrDefault(r => r.RevId == reviewId);

        if (review == null)
        {
            return new Response($"Review with ID {reviewId} was not found", false);
        }

        var user = _reviewContext.Users.FirstOrDefault(u => u.UserId == userId);
        if (user == null)
        {
            return new Response($"User with ID {userId} was not found", false);
        }
        var existingRating = _reviewContext.ReviewRatings
            .FirstOrDefault(rr => rr.ReviewId == reviewId && rr.UserId == userId);
        
        if (existingRating == null) existingRating.Rating = amount;
        else
        {
            var newRating = new ReviewRating
            {
                ReviewId = reviewId,
                UserId = userId,
                Rating = amount
            };
            _reviewContext.ReviewRatings.Add(newRating);
        }
        var averageRating = _reviewContext.ReviewRatings
            .Where(rr => rr.ReviewId== reviewId)
            .Average(rr => rr.Rating);
        review.Rating = averageRating;

        _reviewContext.SaveChanges();

        return new Response("Review Rated Successfully", true);
    }

    public List<Review> GetAllreviews()
    {
        return _reviewContext.Reviews.ToList();
    }

    public List<Review> GetUserReviews(int userId)
    {
        var query = _reviewContext.Reviews.Where(r => r.User.UserId == userId);

        return query.ToList();
    }

    public Response CreateTag(List<string> tags)
    {
        foreach (var tagDto in tags)
        {
            var existingtag = _reviewContext.Tags.FirstOrDefault(t => t.Name == tagDto);
            if (existingtag == null)
            {
                var newTag = new Tag
                {
                    Name = tagDto,
                    UsedAmount = 1
                };
                _reviewContext.Tags.Add(newTag);
            }
            else
            {
                existingtag.UsedAmount++;
            }
            
        }
        
        _reviewContext.SaveChanges();
        return new Response("Created Tag successfully", true);
    }

    public List<Tag> GetAllTags()
    {
        var tags = _reviewContext.Tags.ToList();
        return tags;
    }

    public Review GetReview(int revId)
    {
        var rev = _reviewContext.Reviews.FirstOrDefault();
        return rev;
    }
}