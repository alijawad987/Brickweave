﻿using System.Threading.Tasks;
using Brickweave.Cqrs.Cli;
using Microsoft.AspNetCore.Mvc;

namespace Brickweave.Samples.WebApp.Controllers
{
    public class CommandController : Controller
    {
        private readonly IRunner _runner;

        public CommandController(IRunner runner)
        {
            _runner = runner;
        }

        [HttpPost, Route("/command/run")]
        public async Task<IActionResult> Run([FromBody]string payload)
        {
            var result = _runner.Run(payload.Split(' '));

            return Ok(result);
        }
    }
}
