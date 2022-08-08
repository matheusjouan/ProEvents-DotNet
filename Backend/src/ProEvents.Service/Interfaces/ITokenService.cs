using ProEvents.Service.DTOs;

namespace ProEvents.Service.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(UserUpdateDTO userUpdateDto);
    }
}