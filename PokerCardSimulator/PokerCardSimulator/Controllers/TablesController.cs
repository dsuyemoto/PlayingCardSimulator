using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dealer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokerCardSimulator.Models;
using static Dealer.Player;

namespace PokerCardSimulator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        TableManager _tableManager;
        public TablesController()
        {
            _tableManager = new TableManager();
        }

        [Route("{tableid}")]
        public async Task<IActionResult> GetView([FromQuery]int tableid, int playerid, CancellationToken token)
        {
            var table = await TableManager.GetTexasHoldemTableAsync(tableid);

            return Ok(table.GetTableView(playerid));
        }

        [Route("{tableid}/subscribe")]
        public IActionResult Post(string tableid)
        {
            var playerObserver = new PlayerObserver();
            playerObserver.Subscribe(_tableManager);
        }
    }
}
