using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class BuggyController(DataContext context) : ApiBaseController
{
    [HttpGet("auth")]
    public ActionResult<string> getAuth() {
        return "Secret Text";
    }

    [HttpGet("not-found")]
    public ActionResult<AppUser> getNotFound() {
        var thing = context.Users.Find(-1);
        if (thing == null) return NotFound();
        return thing;
    }

    [HttpGet("Server-error")]
    public ActionResult<AppUser> getServerError() {
        var thing = context.Users.Find(-1) ?? throw new Exception("Something bad happened");
        return thing;
    }

    [HttpGet("bad-request")]
    public ActionResult<string> getBadRequest() {
        return BadRequest("This was not a good request");
    }

}
