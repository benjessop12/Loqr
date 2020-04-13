using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Loqr.Attributes;
using Loqr.Helpers;

namespace Loqr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(initialCount: 1);

        // POST : api/Admin/create_auth/{id}/{payload}
        [HttpPost]
        [RequestRateLimit(Name = "Limit Request Number", Seconds = 60)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("create_auth/{id}/{payload}")]
        [AcceptVerbs("POST", "GET")]
        public async Task<ActionResult> CreateAuth([FromRoute]string id, string payload)
        {
            await semaphoreSlim.WaitAsync();

            string result = AdminControllerHelper.CreateAuth(id, payload);

            semaphoreSlim.Release();

            return Content(result, "application/json");;
        }
    }
}
