using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<LoqrItem>> GetAll()
        {
            await semaphoreSlim.WaitAsync();

            string loqrItems = LoqrControllerHelper.MassReturnResults();

            semaphoreSlim.Release();

            return Content(loqrItems, "application/json");
        }

        // GET : api/Loqr/5
        [HttpGet("{id}")]
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
        [Route("edit/{id}/{payload}")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IActionResult> EditLoqrItem([FromRoute]string id, string payload)
        {
            await semaphoreSlim.WaitAsync();

            LoqrControllerHelper.EditProcessor(id, payload);

            semaphoreSlim.Release();

            return Content(LoqrControllerHelper.ReturnResults(id), "application/json");
        }

        // POST : api/Loqr/delete/{id}
        [HttpDelete]
        [Route("delete/{id}")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IActionResult> DeleteLoqrItem([FromRoute]string id)
        {
            await semaphoreSlim.WaitAsync();

            LoqrControllerHelper.DeleteProcessor(id);

            semaphoreSlim.Release();

            return Content(LoqrControllerHelper.MassReturnResults(), "application/json");
        }

        // GET : api/Loqr/db_config
        [HttpGet("db_config")]
        public async Task<IActionResult> GetConfig()
        {
            await semaphoreSlim.WaitAsync();

            string results = LoqrControllerHelper.GetConfigProcessor();

            semaphoreSlim.Release();

            return Content(results, "application/json");
        }

        // POST : api/Loqr/db_config/{payload}
        [HttpPost]
        [Route("db_config/{payload}")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IActionResult> EditDatabase([FromRoute]string payload)
        {
            await semaphoreSlim.WaitAsync();

            LoqrControllerHelper.AlterDatabase(payload);

            semaphoreSlim.Release();

            return Content("Success", "application/json");
        }
    }
}
