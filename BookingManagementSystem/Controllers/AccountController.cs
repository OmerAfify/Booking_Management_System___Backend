using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BookingManagementSystem.DTOs;
using Core.Interfaces.IServices;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookingManagementSystem.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
    
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;

        }

        //[HttpGet]
        //[Authorize]
        //public async Task<ActionResult<UserDTO>> GetCurrentUser()
        //{
        //    try
        //    {    
        //        var email =  User.FindFirstValue(ClaimTypes.Email);
        //        var user = await _userManager.FindByEmailAsync(email);

        //        if (user == null)
        //            return BadRequest();

        //        return Ok(new 
        //        {
        //            email = user.Email,
        //            name = user.FirstName + user.LastName,
        //            token = _tokenService.CreateToken(user)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Problem("an error occured."+ex.Message);
        //    }

        //}



        //[Authorize]
        //[HttpGet]
        //public async Task<ActionResult<AddressDTO>> GetCurrentUserAddress()
        //{

        //    try
        //    {
        //        var email = User.FindFirstValue(ClaimTypes.Email);
        //        var user = await _userManager.Users.Include(a=>a.address).Where(u=>u.Email==email).FirstOrDefaultAsync();


        //        if (user == null)
        //            return BadRequest();



        //        return Ok(_mapper.Map<AddressDTO>(user.address));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Problem("an error occured." + ex.Message);
        //    }
        //}


        //[Authorize]
        //[HttpPut]
        //public async Task<ActionResult<AddressDTO>> UpdateUserAddress([FromBody] AddressDTO addressDTO)
        //{

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList());


        //    try
        //    {
        //        var email = User.FindFirstValue(ClaimTypes.Email); 
        //        var user = await _userManager.Users.Include(a => a.address).Where(u => u.Email == email).FirstOrDefaultAsync();

        //        user.address = _mapper.Map<Address>(addressDTO);

        //        var result = await _userManager.UpdateAsync(user);

        //        if (!result.Succeeded)
        //            return BadRequest();

        //        return Ok(_mapper.Map<AddressDTO>(user.address));

        //    }
        //    catch (Exception ex)
        //    {
        //        return Problem(" error has occured. " + ex.Message);

        //    }

        //}


        [HttpPost]
        public async Task<ActionResult<UserDTO>> Register([FromBody] UserDTO registerUserDTO)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {

                if (CheckIfEmailExistsAsync(registerUserDTO.Email).Result.Value)
                    return BadRequest(new { Errors = new List<string> { "Email already exists." } });


                var user = new ApplicationUser()
                {

                    Email = registerUserDTO.Email,
                    UserName = registerUserDTO.Email,
                    FirstName = registerUserDTO.FirstName,
                    LastName = registerUserDTO.LastName
                };

                var result = await _userManager.CreateAsync(user, registerUserDTO.Password);

                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        user.Email,
                        Name = string.Format("{0} {1}",user.FirstName, user.LastName),
                        Token = _tokenService.CreateToken(user)
                    });

                }
                else
                    return BadRequest();

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error : "+ ex.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO loginUserDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            try
            {
                var user = await _userManager.FindByEmailAsync(loginUserDTO.Email);

                if (user == null)
                    return BadRequest(new { Errors = new List<string> { "email doesn't exists." } });

                var result = await _signInManager.PasswordSignInAsync(loginUserDTO.Email, loginUserDTO.Password, false, false);

                if (!result.Succeeded)
                    return BadRequest(new { Errors = new List<string> { "Password doesn't match this email." } });


                return Ok(new
                {
                    user.Email,
                    Name = string.Format("{0} {1}", user.FirstName, user.LastName),
                    Token = _tokenService.CreateToken(user)
                });


            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error : " + ex.Message);
            }

        }


        [HttpGet]
        public async Task<ActionResult<bool>> CheckIfEmailExistsAsync([FromQuery] string email)
        {
            try
            {
                return await _userManager.FindByEmailAsync(email) != null;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error : " + ex.Message);
            }

        }



    }
}
