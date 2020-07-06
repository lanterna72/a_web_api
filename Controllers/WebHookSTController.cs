using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreNLogText;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace a_web_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WebHookSTController : ControllerBase
    {
        private readonly ILog _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public WebHookSTController(ILog logger, IHubContext<NotificationHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Hello World" };
        }


        // POST api/<WebHookSTController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Payload pl)
        {
            var incomingVar = pl;
            if (incomingVar == null)
                return BadRequest();
            _logger.Information($"Notification received for {pl.AWB} / {pl.Event}");

            await _hubContext.Clients.All.SendAsync("notification", $"{DateTime.Now}: {pl.AWB}");
            return Ok("Notification has been sent successfully!");
        }

        public class Payload
        {
            public string AWB { get; set; }
            public string Event { get; set; }
        }
    }
}
