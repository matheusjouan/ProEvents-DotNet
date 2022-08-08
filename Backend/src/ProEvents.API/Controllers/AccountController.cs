using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEvents.API.Extensions;
using ProEvents.Service.DTOs;
using ProEvents.Service.Interfaces;

namespace ProEvents.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokeService;

        public AccountController(IUserService userService, ITokenService tokeService)
        {
            _userService = userService;
            _tokeService = tokeService;
        }

        [HttpGet("user")]
        public async Task<ActionResult> GetUser()
        {
            try
            {
                var username = User.GetUserName();
                var user = await _userService.GetUserByUsernameAsync(username);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error to try to recover the user. Erro: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<ActionResult<UserDTO>> SignUp([FromBody] UserDTO userDto)
        {
            try
            {
                if (await _userService.UserExists(userDto.Username))
                    return BadRequest("Username already exists");

                var user = await _userService.CreateAccountAsync(userDto);

                if (user == null)
                    return BadRequest("User does not create, try later again");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error to create an User. Erro: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<ActionResult> SignIn([FromBody] UserLoginDTO userDto)
        {
            try
            {
                var user = await _userService.GetUserByUsernameAsync(userDto.Username);
                if (user == null) return Unauthorized("User or Password is invalid");

                var result = await _userService.CheckUserPasswordAsync(user, userDto.Password);
                if (!result.Succeeded) return Unauthorized("User or Password is invalid");

                return Ok(new
                {
                    username = user.Username,
                    FirstName = user.FirstName,
                    token = _tokeService.CreateToken(user).Result
                });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error to signIn: {ex.Message}");
            }
        }


        [HttpPut("update")]
        public async Task<ActionResult> UpdateUser([FromBody] UserUpdateDTO userDto)
        {
            try
            {
                // User.GetUserName(): método de extensão
                var user = await _userService.GetUserByUsernameAsync(User.GetUserName());
                if (user == null) return Unauthorized("User is invalid");

                var userUpdate = await _userService.UpdateAccount(userDto);
                return Ok(userUpdate);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error to update: {ex.Message}");
            }
        }

        [HttpPost("upload-image")]
        public async Task<ActionResult> UploadImage()
        {
            try
            {
                var user = await _userService.GetUserByUsernameAsync(User.GetUserName());
                if (user == null) return NotFound("EventId not exist");

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    // Delete Imagem antiga, caso tiver uma
                    _userService.DeleteImage(user.ImageUrl, "Profile");
                    // Salvando a nova ou primeira imagem
                    user.ImageUrl = await _userService.SaveImage(file, "Profile");
                }

                var userResult = await _userService.UpdateAccount(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}