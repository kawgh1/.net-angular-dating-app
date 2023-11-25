using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ErrorController : ControllerBase
{
    private readonly DatabaseContext _context;

    public ErrorController(DatabaseContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetSecret()
    {
        return "secret text";
    }
    
    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
        var thing = _context.Users.Find(-1);
        if (thing == null) return NotFound();
        return thing;
    }
    
    [HttpGet("server-error")]
    public ActionResult<string> GetServerError()
    {
        // try
        // {
            var thing = _context.Users.Find(-1);
            var thingToReturn =
                thing.ToString(); // deliberately creating an exception by calling ToString on a null reference
            return thingToReturn;
        // }
        // catch (Exception e)
        // {
        //     return StatusCode(500, "Server Error");
        // }
      
    }
    
    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("this was not a good request from the client");
    }
}