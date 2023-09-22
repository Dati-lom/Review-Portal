using FinalProject.Models;
using FinalProject.ResponseClasses;

namespace FinalProject.Service;

public interface IUserServices
{
    // IEnumerable<User> user { get; }
    Review CreateReview(int userId,ReviewDto reviewDto);
    Review GetReview(int revId);
    Response DeleteReview(int reviewId);

    List<Review> GetAllreviews();
    
    List<Review> GetUserReviews(int userId);
    
    Response EditReview(ReviewDto edited,int id);

    Comment  AddComment(CommentDto commentDto);

    List<LikeResp> GetAllComments(int sortStatus, int reviewId,int userId);
    Response removeComment(int id);
    Response LikeComment(int userId, int commentId);
    Response RateReview(int userId, int reviewId, int amount);
    Response CreateTag(List<string> tagDto);

    List<Tag> GetAllTags();
}