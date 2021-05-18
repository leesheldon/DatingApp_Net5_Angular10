using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            // var users = await _userRepository.GetUsersAsync();

            // var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);

            // return Ok(usersToReturn);

            return Ok(await _userRepository.GetMembersAsync());
        }

        // api/users/id/3
        [HttpGet("id/{id}")]
        public async Task<ActionResult<MemberDto>> GetUser(int id)
        {
            // var user = await _userRepository.GetUserByIdAsync(id);

            // var userToReturn = _mapper.Map<MemberDto>(user);

            // return userToReturn;

            return await _userRepository.GetMemberByIdAsync(id);
        }

        // api/users/name
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            // var user = await _userRepository.GetUserByUserNameAsync(username);

            // var userToReturn = _mapper.Map<MemberDto>(user);

            // return userToReturn;

            return await _userRepository.GetMemberByUserNameAsync(username);
        }

    }
}