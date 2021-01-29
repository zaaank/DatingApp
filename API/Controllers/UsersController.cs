using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
     //[Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            //ienumerable je podobno kot list
            var users = await _userRepository.GetMembersAsync();

            return Ok(users);
        }
 [HttpGet]
 [Route("usersSmall")]
        public async Task<ActionResult<IEnumerable<MemberSmallDto>>> GetUsersSamll()
        {
            //ienumerable je podobno kot list
            var users = await _userRepository.GetMembersAsync();
            var usersSmall = users.Select(u => new MemberSmallDto{
                ID = u.Id,
                Name = u.Username,
                Age = u.Age,
                City = u.City,
                Country = u.Country
            });
            return Ok(usersSmall);
        }

        [HttpGet("{username}", Name = "GetUser")]        
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
            //ienumerable je podobno kot list
        }

         [HttpGet]  
          [Route("Select")]
          [AllowAnonymous]
        public List<SelectItem> GetUserSelect()
        {
             var mombers =  _userRepository.GetMembersAsync().Result;
        
            return mombers.Select(m => new SelectItem{
                ID = m.Id,
                Name = m.Username
            }).ToList();


            //ienumerable je podobno kot list
        }

 [HttpPost]
 [Route("update")]
        public async Task<ActionResult> UpdateMultipleUsers(Dictionary<string, object> request)
        {

            var myUsers =(List<MemberSmallDto>) request["usersTable"];

            var allUsers = _userRepository.GetUsersAsync().Result;

            var updatedUSers = allUsers.Join(
                myUsers,
                cur => cur.Id,
                upd => upd.ID,
                (cur, upd) => {
                    cur.UserName = upd.Name;
                    cur.Country = upd.Country;
                    return cur;
                    }
            );

        foreach (AppUser item in updatedUSers)
        {
            _userRepository.Update(item);
        }


           

            return BadRequest("Failed to update user");
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.getUsername());

            _mapper.Map(memberUpdateDto, user); //iz memberupdatedto bo vse prepisalo na userja namesto da za vsak property delaš ročno

            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.getUsername());

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null){
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0){
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync()) 
            {
                return CreatedAtRoute("GetUser", new {username = user.UserName} ,_mapper.Map<PhotoDto>(photo));
            }
            return BadRequest("Problem Adding Photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId){
            var user = await _userRepository.GetUserByUsernameAsync(User.getUsername());

            var photo = user.Photos.FirstOrDefault(x=> x.Id == photoId);

            if(photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if(currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set photo!");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.getUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if(photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("You cannot delete your main photo");

            if(photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);              
            }
            user.Photos.Remove(photo);

            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Something went wrong with deleting the photo");
        }

    }
}