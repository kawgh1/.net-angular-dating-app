using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
// Authorize means a valid JWT token is required to access these endpoints,
// use "AllowAnonymous" for public endpoints (default behavior), Authorization is set up in the Program.cs file
[Authorize] 
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _photoService = photoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
    {
        var users = await _userRepository.GetMembersAsync();
        return Ok(users);
    }
    
    [Authorize]
    [HttpGet("id/{id}")]
    public async Task<ActionResult<MemberDTO>> GetUserById(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        var userToReturn = _mapper.Map<MemberDTO>(user);
        return userToReturn;
    }
    
    [Authorize]
    [HttpGet("username/{username}")]
    public async Task<ActionResult<MemberDTO>> GetUserByUsername(string username)
    {
        return await _userRepository.GetMemberAsync(username);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDto)
    {
        var username = User.GetUsername();
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user == null) return NotFound();

        _mapper.Map(memberUpdateDto, user);

        if (await _userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Bad Request - No changes or failed to update user");
    }
    
    // Photos
    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
    {
        var username = User.GetUsername();
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user == null) return NotFound();

        var result = await _photoService.AddPhotoAsync(file);
        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if (user.Photos.Count == 0) photo.IsMain = true;
        
        user.Photos.Add(photo);

        if (await _userRepository.SaveAllAsync()) return _mapper.Map<PhotoDTO>(photo);

        return BadRequest("Problem adding photo");

    }
}