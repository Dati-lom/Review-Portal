using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinalProject.Context;
using FinalProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FinalProject.Service;

public class AuthService : IAuthService
{
    private ReviewContext _reviewContext;
    private IConfiguration _configuration;
    


    public AuthService(ReviewContext reviewContext, IConfiguration configuration)
    {
        _reviewContext = reviewContext;
        _configuration = configuration;
    }


    public bool Register(UserDto request)
    {
        var users = new User();
        if (_reviewContext.Users.Any(u => u.Username == request.Username || u.Email == request.Email)) 
        {
            return false;
        }
        
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        users.Username = request.Username;
        users.Email = request.Email;
        users.HashedPassword = passwordHash;
        

        _reviewContext.Users.Add(users);
        _reviewContext.SaveChanges();
        return true;
    }
    
    

    public User Login(string username, string password, bool rememberMe =false)
    {
        var user = _reviewContext.Users.FirstOrDefault(u => u.Username == username);

        if (user == null) return null;
        
        string token = CreateToken(user,rememberMe);

        user.Token = token;
        _reviewContext.SaveChanges();

        return user;
    }

    public User Logout(string username)
    {
        var user = _reviewContext.Users.FirstOrDefault(e => e.Username == username);

        if (user == null) return null;
        user.Token = "";
        _reviewContext.SaveChanges();
        return user;
    }

    public User GetUser(int id)
    {
        var user = _reviewContext.Users.FirstOrDefault(u => u.UserId == id);
        return user;
    }

    public bool CheckToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Token:Key"]);
        
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, 
            ValidateAudience = false, 
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero 
        };
        try
        {
            SecurityToken securityToken;
            tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            return true;
        }
        catch (SecurityTokenException)
        {
            return false;
        }
    }

    // public async Task<User> GoogleSignin()
    // {
    //     var authRes = await _httpContextAccessor.HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
    //
    //     if (!authRes.Succeeded)
    //     {
    //         return null;
    //     }
    //     
    //     var user = new User
    //     {
    //         Email = authRes.Principal.FindFirst(ClaimTypes.Email)?.Value,
    //         Username = authRes.Principal.FindFirst(ClaimTypes.Name)?.Value,
    //         
    //         
    //     }
    //     
    // }

    private string CreateToken(User user,bool rememberMe)
    {
        List<Claim> claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.IsAdmin.ToString()),
            new (ClaimTypes.NameIdentifier, user.UserId.ToString())
            
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: rememberMe?DateTime.Now.AddDays(30) : DateTime.Now.AddDays(1),
            signingCredentials: cred
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
    

    public IEnumerable<User> user { get; }
}