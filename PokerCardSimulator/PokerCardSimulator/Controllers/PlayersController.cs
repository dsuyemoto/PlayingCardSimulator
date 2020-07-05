using Dealer;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokerCardSimulator.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PokerCardSimulator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(ILogger<PlayersController> logger)
        {
            _logger = logger;
            _logger.LogDebug(1, "PlayersController started");
        }

        [HttpGet]
        public IActionResult Get([FromQuery]GetPlayerDTO getPlayerDTO)
        {
            _logger.LogInformation(getPlayerDTO.Username);

            return Ok(new ResultPlayerDTO() { Player = new PlayerDTO() { Id = 1 } });
        }

        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public IActionResult Post(CreatePlayerDTO createPlayerDTO)
        {


            return Created(Request.GetDisplayUrl(), new ResultPlayerDTO());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post(string username, string password)
        {
            if (!IsValidUsernameAndPasswod(username, password))
                return BadRequest();

            var user = GetUserFromUsername(username);

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                //...
            }, "Cookies");

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await Request.HttpContext.SignInAsync("Cookies", claimsPrincipal);

            return NoContent();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return NoContent();
        }

        private PlayerDTO GetUserFromUsername(string username)
        {
            return new PlayerDTO();
        }

        private bool IsValidUsernameAndPasswod(string username, string password)
        {
            return true;
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
