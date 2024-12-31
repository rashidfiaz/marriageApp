using Api.Data;
using Api.DTOs;
using Api.Entities;
using Api.Extensions;
using Api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) : ApiBaseController
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

        //Get user claims from the token
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());//See the ClaimPrincipleExtension for more details. In the class we are getting the username from the token

        if (user == null) return BadRequest("Could not find the user");

        mapper.Map(userUpdateDto, user);

        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update the user");

    }

    [HttpPost("add-photo")]

    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file) {

        //Get User claims form the token
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());//See the ClaimPrincipleExtension for more details. In the class we are getting the username from the token

        if (user == null) return BadRequest("Could update the user");

        var result = await photoService.AddPhotoAsync(file); // Upload photo in the cloudinary drive

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo { // create the object after getting the URL and Public ID from the cloudinary after photo upload
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };
        
        user.Photos.Add(photo); //Add photos in the cloudinary

        if (await userRepository.SaveAllAsync()) //This change will send the correct response to the user which should be 201 instead of 200.
            return CreatedAtAction(nameof(GetUser),
                new {username = user.UserName}, mapper.Map<PhotoDto>(photo)); //Add the photo URL and Public ID in the SQL Lite User table

        return BadRequest("Problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId}")]

    public async Task<ActionResult> SetMainPhoto(int photoId){

        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return BadRequest("Could not find user");

        var photo = user.Photos.FirstOrDefault(x=>x.Id == photoId);

        if (photo == null || photo.IsMain) return BadRequest("cannot use this as main photo");

        var currentMain = user.Photos.FirstOrDefault(x=>x.IsMain);

        if (currentMain != null) currentMain.IsMain = false;

        photo.IsMain = true;

        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Problem setting main photo");

    }

    [HttpDelete("delete-photo/{photoId}")]

    public async Task<ActionResult> DeletePhoto(int photoId){
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return BadRequest("Could not find user");

         var photo = user.Photos.FirstOrDefault(x=>x.Id == photoId);

        if (photo == null || photo.IsMain) return BadRequest("cannot delete the main photo");

        if (photo.PublicId != null) {
            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);

        if (await userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting the photo");
    }
}
