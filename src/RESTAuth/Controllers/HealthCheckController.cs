using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RESTAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [Authorize]
        [HttpGet("ValidateAuthToken")]
        public IActionResult Validate()
        {
            return Ok("[GET] With Authorize Executed!");
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("[GET] Executed!");
        }

        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            return Ok($"[GET] with id: {Id} Executed!");
        }

        [HttpPost]
        public IActionResult Post()
        {
            return Ok("[POST] Executed!");
        }

        // PUT api/<HealthCheckController>/5
        [HttpPut()]
        public IActionResult Put()
        {
            return Ok($"[PUT] Executed!");
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            return Ok($"[DELETE] with id: {Id} Executed!");
        }
    }
}
