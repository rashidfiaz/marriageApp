using System.Security.Cryptography;
using System.Text;
using Api.DTOs;
using Api.Interfaces;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

public class AccountController(DataContext context, ITokenService TokenService) : ApiBaseController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register (RegisterDTO registerDTO) 
    {
        if (await UserExists(registerDTO.Username)) return BadRequest("User is already taken");
        
        return Ok();
        // using var hmac = new HMACSHA512();
    
        // var user = new AppUser {
        //     UserName = registerDTO.Username.ToLower(),
        //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
        //     PasswordSalt = hmac.Key
        // };

        // context.Users.Add(user);
        // await context.SaveChangesAsync();

        // return new UserDto {
        //     Username = user.UserName,
        //     Token = TokenService.createToken(user)
        // };

    }

    [HttpPost("login")]

    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto) {

        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

        if (user == null) return Unauthorized("Username not found");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < ComputeHash.Length; i++)
        {
            if (ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Password is not correct");
            
        }
        return new UserDto {
            Username = user.UserName,
            Token = TokenService.createToken(user)
        };

    }
    private async Task<bool> UserExists (string Username) {
        return await context.Users.AnyAsync(x => x.UserName == Username.ToLower());
    }
}
