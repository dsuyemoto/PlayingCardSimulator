using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Web;
using NUnit.Framework;
using PokerCardSimulator.Controllers;
using PokerCardSimulator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokerCardSimulator.Controllers.Tests
{
    [TestFixture()]
    public class PlayersControllerTests
    {
        PlayersController _playersController;
        ILogger<PlayersController> _logger;

        const int PLAYERID = 1;

        public PlayersControllerTests()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddNLog(new LoggingConfiguration());
            });

            _logger = loggerFactory.CreateLogger<PlayersController>();
            LogManager.DisableLogging();
        }

        [SetUp]
        public void Setup()
        {
            _playersController = new PlayersController(_logger);
        }

        [Test()]
        public void Get_ResultPlayer_AreEqualTest()
        {
            var result = _playersController.Get(new GetPlayerDTO());
            var okResult = result as OkObjectResult;
            var resultPlayerDTO = okResult.Value as ResultPlayerDTO;

            Assert.AreEqual(PLAYERID, resultPlayerDTO.Player.Id);
        }

        [Test()]
        public void GetTest1()
        {
            Assert.Fail();
        }

        [Test()]
        public void PostTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void PutTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void DeleteTest()
        {
            Assert.Fail();
        }
    }
}