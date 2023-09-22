using FinalProject.Models;
using FinalProject.ResponseClasses;
using FinalProject.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;


[Route("api/[controller]")]
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class UserController : Controller
{
    private readonly IUserServices _userServices;

    public UserController(IUserServices userServices)
    {
        _userServices = userServices;
    }
    

    [HttpPost("create-review/{userId}")]
    public ActionResult<Review> CreateReview([FromBody]ReviewDto request,int userId)
    {
        var response = _userServices.CreateReview(userId,request);
        if (response == null) return BadRequest("Review Name Already exists");
        return Ok(response);
    }

    [HttpGet("get-review/{revId}")]
    public ActionResult<Review> GetReview(int revId)
    {
        var response = _userServices.GetReview(revId);
        if (response == null) return BadRequest("No Such Review");
        return Ok(response);
    }

    [HttpPost("create-tag/")]
    public ActionResult CreateTags([FromBody] TagRequest request)
    {
        var response = _userServices.CreateTag(request.Tags);
        if (response._status == false) return BadRequest(response._message);
        return Ok(response._message);
    }

    [HttpGet("get-all-tags")]
    public ActionResult GetAllTags()
    {
        return Ok(_userServices.GetAllTags().Select(t => t.Name));
    }

    [HttpDelete("delete-review")]
    public ActionResult<Review> DeleteReview(int id)
    {
        var response = _userServices.DeleteReview(id);
        return !response._status ? BadRequest(response) : Ok(response);
    }

    [HttpGet("get-comments/{revId}/user/{userId}")]
    public ActionResult GetAllComments(int revId, [FromQuery]int sortStatus, int userId)
    {
        var comments = _userServices.GetAllComments(sortStatus, revId, userId);

        return Ok(comments);
    }
    
    [HttpPut("like-comment/{commentid}")]
    public IActionResult LikeComment(int userId, int commentId)
    {
        var response = _userServices.LikeComment(userId, commentId);
        if (response._status == false) return BadRequest(new {message = response._message});
        return Ok(new {message = response._message});
    }

    [HttpPut("rate-review/{reviewId}")]
    public IActionResult RateReview(int userId, int reviewId, int amount)
    {
        var response = _userServices.RateReview(userId, reviewId, amount);

        return response._status
            ? Ok(new { message = response._message })
            : BadRequest(new { message = response._message });
    }

    [HttpGet("get-all-reviews")]
    public IActionResult GetAllReviews()
    {
        return Ok(_userServices.GetAllreviews());
    }

    [HttpGet("get-all-reviews/{userId}")]
    public IActionResult GetUserReviews(int userId)
    {
        return Ok(_userServices.GetUserReviews(userId));
    }

    [HttpPost("add-comment")]
    public IActionResult AddComment([FromBody] CommentDto request)
    {
        var response = _userServices.AddComment(request);

        return Ok(response);

    }



}