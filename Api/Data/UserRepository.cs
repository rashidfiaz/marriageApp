using Api.DTOs;
using Api.Interfaces;
using API.Data;
using Api.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<MemberDto?> GetMemberByUsernameAsync(string username)
    {
        return await context.Users
        .Where(x=>x.UserName == username) //select query
        .ProjectTo<MemberDto>(mapper.ConfigurationProvider) //Projection to member DTO
        .SingleOrDefaultAsync(); //Function for extracting data from the database
    }

    public async Task<IEnumerable<MemberDto>> GetMembersAsync()
    {
        return await context.Users
        .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
        .ToListAsync();
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await context.Users
        .FindAsync(id);
    }

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await context.Users
        .Include(x => x.Photos)
        .SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
            return await context.Users
            .Include(x => x.Photos)
            .ToListAsync();
;
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() >= 0;

    }

    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }
}
