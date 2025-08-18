using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Domain.DTOs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Responses;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IUser userInterface) : ControllerBase
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<Response>> Register(AppUserDTO appUserDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await userInterface.Register(appUserDTO);
            return result.Flag ? Ok(result) : BadRequest(Request);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<Response>> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await userInterface.Login(loginDTO);
            return result.Flag ? Ok(result) : BadRequest(Request);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<GetUserDTO>> GetUser(int id)
        {
            if (id <= 0) return BadRequest("Invalid user ID");
            var user = await userInterface.GetUser(id);

            return user.Id > 0 ? Ok(user) : BadRequest(Request);
        }

    }
}