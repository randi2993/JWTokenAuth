using Domain.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Auth;
using System;

namespace RESTAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        JWTokenAuthService _service {get; set;}

        public AuthController(JWTokenAuthService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Post(UserTokenVM request)
        {
            try
            {
                var result = _service.GetToken(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message, ex.InnerException});
            }
        }
    }
}
