
using Microsoft.AspNetCore.Mvc;

namespace TicketApi.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    public class TicketController : ControllerBase
    {
        private readonly TicketWorkflow _workflow;

        public TicketController(TicketWorkflow workflow)
        {
            _workflow = workflow;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string message)
        {
            var result = await _workflow.Process(message);

            return Ok(result);
        }
    }
}
