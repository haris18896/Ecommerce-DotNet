using AuthenticationApi.Application.DTOs;
using SharedLibrary.Responses;

namespace AuthenticationApi.Domain.DTOs.Interfaces
{
    public interface IUser
    {
        Task<Response> Register(AppUserDTO appUserDTO);
        Task<Response> Login(LoginDTO loginDTO);
        Task<GetUserDTO> GetUser(int userId);
    }
}