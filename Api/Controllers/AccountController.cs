using System.Security.Cryptography;
using System.Text;
using Api.DTOs;
using Api.Entities;
using Api.Interfaces;
using API.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

public class AccountController(DataContext context, ITokenService TokenService, IMapper mapper) : ApiBaseController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register (RegisterDTO registerDTO) 
    {
        if (await UserExists(registerDTO.Username)) return BadRequest("User is already taken");
        
        using var hmac = new HMACSHA512();

        var user = mapper.Map<AppUser>(registerDTO);
    
        user.UserName = registerDTO.Username.ToLower();
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
        user.PasswordSalt = hmac.Key;
        
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserDto {
            Username = user.UserName,
            Token = TokenService.createToken(user),
            KnownAs = user.KnownAs
        };

    }

    [HttpPost("login")]

    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto) {

        var user = await context.Users
        .Include(p=>p.Photos)
            .FirstOrDefaultAsync(x => 
                x.UserName == loginDto.Username.ToLower());

        if (user == null) return Unauthorized("Username not found");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < ComputeHash.Length; i++)
        {
            if (ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Password is not correct");
            
        }

        return new UserDto {
            Username = user.UserName,
            Token = TokenService.createToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnownAs = user.KnownAs

        };

    }
    private async Task<bool> UserExists (string Username) {
        return await context.Users.AnyAsync(x => x.UserName == Username.ToLower());
    }
}
