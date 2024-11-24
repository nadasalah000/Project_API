using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabat.API.Extensions;
using Talabt.Core.Entities;
using Talabt.Core.Services;

namespace Talabat.API.Controllers
{
    public class AccountController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IMapper mapper,ITokenServices tokenServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenServices = tokenServices;
        }
        #region Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiResponse(400, "This Email is Already in Use"));
            var User = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber
            };
            var Result = await _userManager.CreateAsync(User,model.Password);
            if (Result.Succeeded) return BadRequest(new ApiResponse(400));
            var ReturnedUser = new UserDto()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                Token = await _tokenServices.CreateTokenAsync(User,_userManager)
            };
            return Ok(ReturnedUser);
        }
        #endregion
        #region Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User is null) return Unauthorized(new ApiResponse(401));
            var Result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);
            if(!Result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenServices.CreateTokenAsync(User, _userManager)
            });
        }
        #endregion
        #region GetCurrentUser
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            var ReturnedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user, _userManager)
            };
            return Ok(ReturnedUser);
        }
        #endregion
        #region Get CurrentUserAddress
        //[Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentAddress()
        {
            //var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(user.Address);
        }
        #endregion
        #region Update Address
        //[Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto updatedAddress)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            if (user is null) return Unauthorized(new ApiResponse(401));
            var address = _mapper.Map<AddressDto, Address>(updatedAddress);
            address.Id = user.Address.Id;
            user.Address = address;
            var Result = await _userManager.UpdateAsync(user);
            if(!Result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(updatedAddress);
        }
        #endregion
        #region Validation Email
        [HttpGet("emailExists")]
        public async Task <ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
        #endregion
    }
}
