using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<IEnumerable<AppUser>> GetUsersAsync();        
        Task<AppUser> GetUserByUserNameAsync(string username);
        Task<AppUser> GetUserWithPhotosByIdAsync(int id);
        Task<AppUser> GetUserWithoutPhotosByIdAsync(int id);
        Task<PagedList<MemberDto>> GetMembersAsync(UserPagingParams pagingParams);
        Task<MemberDto> GetMemberByIdAsync(int id, bool isCurrentUser);
        Task<MemberDto> GetMemberByUserNameAsync(string username, bool isCurrentUser);
        Task<string> GetUserGender(string username);
        Task<AppUser> GetUserByPhotoId(int photoId);
    }
}
