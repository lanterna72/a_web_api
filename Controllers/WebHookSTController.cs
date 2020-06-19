using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreNLogText;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace a_web_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WebHookSTController : ControllerBase
    {
        private readonly ILog _logger;

        public WebHookSTController(ILog logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Hello World" };
        }


        // POST api/<WebHookSTController>
        [HttpPost]
        public IActionResult Post([FromBody] Payload pl)
        {
            var incomingVar = pl;
            if (incomingVar == null)
                return BadRequest();
            _logger.Information($"Notification received for {pl.AWB} / {pl.Event}");
            return Ok();
        }

        public class Payload
        {
            public string AWB { get; set; }
            public string Event { get; set; }
        }
    }
}
