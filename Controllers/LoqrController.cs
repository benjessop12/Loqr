using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Loqr.Models;
using Loqr.Database;
using Loqr.Converters;

namespace Loqr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoqrController : ControllerBase
    {
        private readonly LoqrContext _context;

        public LoqrController(LoqrContext context)
        {
            _context = context;
        }

        // GET : api/Loqr/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoqrItem>> GetLoqrItem(long id)
        {
            string loqrItem = Converter.ConvertDataTabletoString(DatabaseHandlers.SelectById(id));

            if (loqrItem.Length == 0)
            {
                return NotFound();
            }

            return Content(loqrItem, "application/json");
        }
        
        // POST: api/Loqr/POST/{id}/{payload}
        [HttpPost]
        [Route("post/{id}/{payload}")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IActionResult> PostLoqrItem([FromRoute]string id, string payload)
        {
            Dictionary<string, string> base_vals = Converter.ConvertURLPayloadToInsertStringArray(id, payload);
            StringBuilder keys = new StringBuilder();
            StringBuilder vals = new StringBuilder();

            foreach(KeyValuePair<string, string> key_val in base_vals)
            {
                keys.Append($"{key_val.Key},");
                vals.Append($"'{key_val.Value}',");
            }

            string insert_keys = Converter.RemoveLast(Convert.ToString(keys), ",");
            string insert_vals = Converter.RemoveLast(Convert.ToString(vals), ",");
            DatabaseHandlers.InsertNewValues(insert_keys, insert_vals);

            return Content(Convert.ToString(base_vals), "application/json");
        }
    }
}
