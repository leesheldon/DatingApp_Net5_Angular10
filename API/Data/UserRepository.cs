using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<MemberDto> GetMemberByIdAsync(int id)
        {
            return await _context.Users
                .Where(x => x.Id == id)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<MemberDto> GetMemberByUserNameAsync(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username.ToLower())
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(PagingParams pagingParams)
        {
            var query = _context.Users.AsQueryable();
            
            // Filter by Gender
            query = query.Where(x => x.UserName != pagingParams.CurrentUsername);
            query = query.Where(x => x.Gender == pagingParams.GenderToFilter);

            // Filter by Age
            var minDOB = DateTime.Today.AddYears(-pagingParams.MaxAge - 1);
            var maxDOB = DateTime.Today.AddYears(-pagingParams.MinAge);

            query = query.Where(x => x.DateOfBirth >= minDOB && x.DateOfBirth <= maxDOB);
            
            query = pagingParams.OrderBy switch
            {
                "created" => query.OrderByDescending(x => x.Created),
                _ => query.OrderByDescending(x => x.LastActive) // _ means default case
            };

            return await PagedList<MemberDto>.CreateAsync(
                query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking(), 
                pagingParams.PageNumber, 
                pagingParams.PageSize);
        }
        
        public async Task<AppUser> GetUserByUserNameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username.ToLower());
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public async Task<AppUser> GetUserWithoutPhotosByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserWithPhotosByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null) await _context.Entry(user).Collection(i => i.Photos).LoadAsync();
            
            return user;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
