using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;

    public UserRepository(DatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    


    public async Task<bool> SaveAllAsync()
    {
        // return true if at least one change was saved
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        // Entity Framework is lazy by default and does not include nested objects so we have to "Include" what we want
        return await _context.Users.Include(user => user.Photos).ToListAsync();
    }

    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        // Find By Id does NOT include User Photos
        return await _context.Users.FindAsync(id);
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        // Find By Username DOES include User Photos
        return await _context.Users.Include(user => user.Photos).SingleOrDefaultAsync(user => user.Username == username);
    }

    public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
    {
        return await _context.Users.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<MemberDTO> GetMemberAsync(string username)
    {
        // return await _context.Users
        //     .Where(x => x.Username == username)
        //     .Select(user => new MemberDTO
        //     {
        //     Id = user.Id,
        //     Username = user.Username,
        //     KnownAs = user.KnownAs,
        //     ...
        //     ...
        //     ..
        //     }).SingleOrDefaultAsync();

        return await _context.Users
            .Where(user => user.Username == username)
            .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    // this method is probably not needed since Entity Framework tracks Entity changes automatically, but sometimes nice to have
    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }
}