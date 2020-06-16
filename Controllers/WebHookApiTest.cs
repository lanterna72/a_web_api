using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace WebHookApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebHookController : ControllerBase
    {
        private readonly ILogger<WebHookController> _logger;

        public WebHookController(ILogger<WebHookController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        [Route("AWBNotification")]
        public IActionResult AWBNotification([FromBody] Payload pl)
        {
            var incomingVar = pl;
            if (incomingVar == null)
                return BadRequest();
            _logger.LogInformation($"Notification received for {pl.AWB} / {pl.Event}");
            return Ok();
        }
       
    
        public class Payload
        {
            public string AWB { get; set; }
            public string Event { get; set; }
        }

       }

}
