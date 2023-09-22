using System.Collections;
using FinalProject.Models;

namespace FinalProject.Service;

public interface IAuthService
{
   IEnumerable<User> user { get; }
   bool Register(UserDto userDto);
   User Login(string username, string password,bool rememberMe);
   User GetUser(int id);
   User Logout(string token);

   bool CheckToken(string token);
}