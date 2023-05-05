using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BookingManagementSystem.DTOs;
using BookingManagementSystem.Errors;
using Core.Interfaces.IServices;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingManagementSystem.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
    
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;

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
        public async Task<ActionResult> Register([FromBody] RegisterDTO registerUserDTO)
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
                    var result2 = await _userManager.AddToRoleAsync(user, "Customer");

                    if (result2.Succeeded)
                    {
                        return Ok(new
                        {
                            user.Email,
                            Name = string.Format("{0} {1}", user.FirstName, user.LastName),
                            Token = _tokenService.CreateToken(user)
                        });
                    }
                    else
                        return BadRequest();

                }
                else
                    return BadRequest();

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500,
                                ex.Message, (ex.InnerException != null) ? ex.InnerException.Message : ex.StackTrace));
            }


        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginUserDTO)
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
                return StatusCode(500, new ApiExceptionResponse(500,
                                ex.Message, (ex.InnerException != null) ? ex.InnerException.Message : ex.StackTrace));
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
                return StatusCode(500, new ApiExceptionResponse(500,
                              ex.Message, (ex.InnerException != null) ? ex.InnerException.Message : ex.StackTrace));
            }

        }

        [HttpGet]
        public async Task<ActionResult<ReturnedUserDTO>> GetUserByEmailAsync(string email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                    return BadRequest(new ApiValidationResponse() { Errors=new List<string> {"This email is not found" } });

                var roles = await _userManager.GetRolesAsync(user);

                var userToReturn = new ReturnedUserDTO()
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = user.Id,
                    Roles = roles.ToList()
                };

                return Ok(userToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500, ex.Message, ex.InnerException.Message ?? null)); ; ;
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<ReturnedUserDTO>>> GetAllUsersAsync()
        {
            try {

                var users =  await _userManager.Users.ToListAsync();

                var usersDTO = new List<ReturnedUserDTO>();

                foreach(var user in users)
                {
                  var listOfRoles = await _userManager.GetRolesAsync(user);

                   var  userDTO = new ReturnedUserDTO() {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Roles = listOfRoles.ToList()
                    };

                    usersDTO.Add(userDTO);
                }
             
                return Ok(usersDTO);
          
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500, ex.Message, ex.InnerException.Message ?? null)); ; ;
            }
        }


        [HttpGet]
        public async Task<ActionResult> GetAllRolesAsync()
        {
            try
            {

                var roles = await _roleManager.Roles.ToListAsync();
                return Ok(roles);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500, ex.Message, ex.InnerException.Message ?? null));
            }
        }


        [HttpPost]
        public async Task<ActionResult<List<ReturnedUserDTO>>> AddNewUserAsync([FromBody] AddNewUserDTO addNewUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (CheckIfEmailExistsAsync(addNewUserDTO.Email).Result.Value)
                    return BadRequest(new ApiValidationResponse() { Errors = new List<string> { "Email already exists." } });

                var systemRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                var invalidRoles = new List<string>() { };

                foreach (var role in addNewUserDTO.Roles)
                {
                    if (!systemRoles.Contains(role))
                        invalidRoles.Add(role);
                }

                if (invalidRoles.Count > 0)
                    return BadRequest(new ApiValidationResponse() { Errors = new List<string>() { "The following roles are invalid : " + string.Join(", ", invalidRoles.ToArray()) } } );


                var user = new ApplicationUser(){
                    Email = addNewUserDTO.Email,
                    UserName = addNewUserDTO.Email,
                    FirstName = addNewUserDTO.FirstName,
                    LastName = addNewUserDTO.LastName
                };


                var result = await _userManager.CreateAsync(user, addNewUserDTO.Password);

                if (result.Succeeded)
                {
                    foreach(var role in addNewUserDTO.Roles)
                    {
                        var result2 = await _userManager.AddToRoleAsync(user, role);

                        if (result2.Succeeded)
                            continue;
                        else
                            return BadRequest(new ApiResponse(400,"Can't Add role "+role+" to the user."));
                    }
                        return Ok(new
                        {
                            user.Email,
                            Name = string.Format("{0} {1}", user.FirstName, user.LastName),
                        });
                }
                else
                    return BadRequest(new ApiResponse(400, "Failed to Add user."));


            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500, ex.Message, ex.InnerException.Message ?? null)); ; ;
            }
        }


        [HttpPut]
        public async Task<ActionResult> UpdateUserAsync(string email, UpdateUserDTO updateUserDTO) {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {

                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                    return NotFound(new ApiResponse(404, "Can not find user with email  : " + email));
              
                var systemRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                var invalidRoles = new List<string>() { };

                foreach (var role in updateUserDTO.Roles)
                {
                    if (!systemRoles.Contains(role))
                        invalidRoles.Add(role);
                }

                if (invalidRoles.Count > 0)
                    return BadRequest(new ApiResponse(400, "The following roles are invalid : " + string.Join(", ", invalidRoles.ToArray())));


                var userRoles = await _userManager.GetRolesAsync(user);
                var result1 = await _userManager.RemoveFromRolesAsync(user, userRoles.ToList());



                user.FirstName = updateUserDTO.FirstName;
                user.LastName = updateUserDTO.LastName;
                
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    foreach (var role in updateUserDTO.Roles)
                    {
                        var result2 = await _userManager.AddToRoleAsync(user, role);

                        if (result2.Succeeded)
                            continue;
                        else
                            return BadRequest(new ApiResponse(400, "Can't Add role " + role + " to the user."));
                    }
                    return Ok(new
                    {
                        user.Email,
                        Name = string.Format("{0} {1}", user.FirstName, user.LastName),
                    });
                }
                else
                    return BadRequest("Failed to Update a user.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500, ex.Message, ex.InnerException.Message ?? null)); ; ;

            }



        }


        [HttpDelete]
        public async Task<ActionResult> DeleteUserAsync(string email)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {

                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                    return NotFound(new ApiResponse(404, "Can not find user with email : " + email));

                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                    return NoContent();
                else
                    return BadRequest();
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500, ex.Message, ex.InnerException.Message ?? null)); ; ;

            }



        }

    }
}
