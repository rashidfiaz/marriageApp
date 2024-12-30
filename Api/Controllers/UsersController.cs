using System.Security.Claims;
using Api.DTOs;
using Api.Entities;
using Api.Interfaces;
using API.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;
[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper) : ApiBaseController
{
    [HttpGet]
    
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await userRepository.GetUsersAsync();
        var usersToReturn = mapper.Map<IEnumerable<MemberDto>>(users);
        return Ok(usersToReturn);

    }
    
    [HttpGet("{username}")]

    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user = await userRepository.GetMemberByUsernameAsync(username);

        if (user == null) return NotFound();

        return mapper.Map<MemberDto>(user);

    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(UserUpdateDto userUpdateDto) {
        
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (username == null) return BadRequest("No username found in the token");

        var user = await userRepository.GetUserByUsernameAsync(username);

        if (user == null) return BadRequest("Could not find the user");

        mapper.Map(userUpdateDto, user);

        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update the user");

    }


}
