using FinalProject.Models;
using FinalProject.ResponseClasses;
using FinalProject.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController:ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    
    [HttpPost("register")]
    public ActionResult<User> Register([FromBody]UserDto request)
    {
        if (!_authService.Register(request))
        {
            return BadRequest("already exists");
        }

        return Ok("Registration Successful");
    }
    
    [HttpPost("login")]
    public ActionResult<User> Login(LoginDto request)
    {
        var user = _authService.Login(request.Username, request.Password,request.RememberMe);

        if (user == null) 
        {
            return BadRequest("User Not Found");
        }
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.HashedPassword))
        {
            return BadRequest("Wrong Password or Username");
        }
        return Ok(user);
    }
    
    [HttpPost("logout")]
    public ActionResult<User> Logout(string username)
    {
        var user = _authService.Logout(username);
        if (user == null) return BadRequest("Something went wrong");

        return Ok(user);
    }

    [HttpGet("get-user")]
    public ActionResult<User> GetUser([FromQuery]int id)
    {
        var user = _authService.GetUser(id);
        if (user == null) return BadRequest("No User Logged Out");
        return Ok(user);
    }

    [HttpGet("checkUser")]
    public ActionResult CheckLogged(string token)
    {
        var result = _authService.CheckToken(token);
        return result ? Ok(new Response("Validated",true)) : 
            BadRequest(new Response("false token",false));
    }

    // [Route("auth/google-Callback")]
    // public IActionResult GoogleCallback([FromQuery] string code)
    // {
    //    
    //     
    // }
    
}