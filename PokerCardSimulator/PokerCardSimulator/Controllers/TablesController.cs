using Dealer;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace PokerCardSimulator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        [Route("{tableid}")]
        public async Task<IActionResult> GetTexasHoldemTournamentView([FromQuery]int tableid, int playerid, CancellationToken token)
        {
            return Ok(await TableManager.GetTexasHoldemView(tableid, playerid));
        }

        [Route("{tableid}/players/{playerid}/subscribe")]
        public async Task<IActionResult> Post(int tableid, int playerid, CancellationToken token)
        {
            var table = TableManager.GetTable(tableid);         
            var view = await table.Subscribe(playerid, token);

            return Ok(view);
        }

        [Route("{tableid}/players/{playerid}/seat/{seatnumber}")]
        public IActionResult Update(int tableId, int playerid, int seatnumber)
        {
            var table = TableManager.GetTable(tableId);
            var view = table.SeatPlayer(new Player(playerid), seatnumber);

            return Ok(view);
        }
    }
}
