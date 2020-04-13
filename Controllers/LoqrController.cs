using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Loqr.Attributes;
using Loqr.Models;
using Loqr.Helpers;

namespace Loqr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoqrController : ControllerBase
    {
        private readonly LoqrContext _context;
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(initialCount: 1);

        public LoqrController(LoqrContext context)
        {
            _context = context;
        }

        // GET : api/Loqr
        [HttpGet]
        [RequestRateLimit(Name = "Limit Request Number", Seconds = 2)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<LoqrItem>> GetAll()
        {
            await semaphoreSlim.WaitAsync();

            string loqrItems = LoqrControllerHelper.MassReturnResults();

            semaphoreSlim.Release();

            return Content(loqrItems, "application/json");
        }

        // GET : api/Loqr/5
        [HttpGet("{id}")]
        [RequestRateLimit(Name = "Limit Request Number", Seconds = 1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<LoqrItem>> GetLoqrItem(string id)
        {
            await semaphoreSlim.WaitAsync();

            string loqrItem = LoqrControllerHelper.ReturnResults(id);

            semaphoreSlim.Release();

            if (loqrItem.Length == 0)
            {
                return NotFound();
            }

            return Content(loqrItem, "application/json");
        }
        
        // POST : api/Loqr/post/{id}/{payload}
        [HttpPost]
        [RequestRateLimit(Name = "Limit Request Number", Seconds = 1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("post/{id}/{payload}")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IActionResult> PostLoqrItem([FromRoute]string id, string payload)
        {
            await semaphoreSlim.WaitAsync();

            string result = LoqrControllerHelper.InsertProcessor(id, payload);

            semaphoreSlim.Release();

            if (result == "Unique constraint failed")
            {
                return Content(result);
            }
            return Content(LoqrControllerHelper.ReturnResults(id), "application/json");
        }

        // POST : api/Loqr/edit/{id}/{payload}
        [HttpPost]
        [RequestRateLimit(Name = "Limit Request Number", Seconds = 1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("edit/{id}/{payload}")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IActionResult> EditLoqrItem([FromRoute]string id, string payload)
        {
            await semaphoreSlim.WaitAsync();

            string result = LoqrControllerHelper.EditProcessor(id, payload);

            semaphoreSlim.Release();

            return Content(result, "application/json");
        }

        // POST : api/Loqr/delete/{id}
        [HttpDelete]
        [RequestRateLimit(Name = "Limit Request Number", Seconds = 1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("delete/{id}/{payload}")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IActionResult> DeleteLoqrItem([FromRoute]string id, string payload)
        {
            await semaphoreSlim.WaitAsync();

            string result = LoqrControllerHelper.DeleteProcessor(id, payload);

            semaphoreSlim.Release();

            return Content(result, "application/json");
        }

        // GET : api/Loqr/db_config
        [HttpGet("db_config")]
        [RequestRateLimit(Name = "Limit Request Number", Seconds = 1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConfig()
        {
            await semaphoreSlim.WaitAsync();

            string results = LoqrControllerHelper.GetConfigProcessor();

            semaphoreSlim.Release();

            return Content(results, "application/json");
        }

        // POST : api/Loqr/db_config/{payload}
        [HttpPost]
        [RequestRateLimit(Name = "Limit Request Number", Seconds = 1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("db_config/{payload}")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IActionResult> EditDatabase([FromRoute]string payload)
        {
            await semaphoreSlim.WaitAsync();

            string result = LoqrControllerHelper.AlterDatabase(payload);

            semaphoreSlim.Release();

            return Content(result, "application/json");
        }
    }
}
