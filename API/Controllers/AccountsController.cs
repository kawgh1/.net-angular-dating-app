using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly DatabaseContext _context;


    public AccountsController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpPost("register")] // POST: api/accounts/register
    public async Task<ActionResult<AppUser>> Register(RegisterDTO registerDto)
    {
        if (await UserExists(registerDto.Username))
        {
            return  BadRequest("Username already exists!");
        }
        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            UserName = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), // returns a Byte Array
            PasswordSalt = hmac.Key
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    [HttpPost("login")] // POST: api/accounts/login
    public async Task<ActionResult<AppUser>> Login(LoginDTO loginDto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(user => user.UserName == loginDto.Username.ToLower());

        if (user == null)
        {
            return Unauthorized();
        }

        using var hmac = new HMACSHA512(user.PasswordSalt); // returns a Byte Array
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password!");
        }

        return user;

    }

    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(user => user.UserName == username.ToLower());
    }
}