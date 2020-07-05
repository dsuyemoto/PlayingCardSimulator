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
        [Route("tables/{tableid}")]
        public IActionResult GetView([FromQuery]int tableid, int playerid)
        {
            var table = TableFactory.GetTable(tableid);

            return Ok(table.GetTableView(playerid));
        }
    }
}
