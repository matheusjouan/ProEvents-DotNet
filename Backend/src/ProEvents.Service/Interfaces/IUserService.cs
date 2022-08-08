using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ProEvents.Service.DTOs;

namespace ProEvents.Service.Interfaces
{
    public interface IUserService
    {
        Task<bool> UserExists(string username);
        Task<UserUpdateDTO> GetUserByUsernameAsync(string username);
        Task<SignInResult> CheckUserPasswordAsync(UserUpdateDTO userUpdateDto, string password);
        Task<UserDTO> CreateAccountAsync(UserDTO userDto);
        Task<UserUpdateDTO> UpdateAccount(UserUpdateDTO userUpdateDto);
        Task<string> SaveImage(IFormFile fileName, string path);
        void DeleteImage(string imageName, string path);
    }
}