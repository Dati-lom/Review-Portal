using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models;

public class UserDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public bool RememberMe { get; set; } = false;


}