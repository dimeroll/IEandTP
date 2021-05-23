using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2_IEandTP.Models;

namespace Lab2_IEandTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopWorkersController : ControllerBase
    {
        private readonly Lab2Context _context;

        public ShopWorkersController(Lab2Context context)
        {
            _context = context;
        }

        // GET: api/ShopWorkers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopWorker>>> GetShopWorkers()
        {
            return await _context.ShopWorkers.ToListAsync();
        }

        // GET: api/ShopWorkers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShopWorker>> GetShopWorker(int id)
        {
            var shopWorker = await _context.ShopWorkers.FindAsync(id);

            if (shopWorker == null)
            {
                return NotFound();
            }

            return shopWorker;
        }

        // PUT: api/ShopWorkers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShopWorker(int id, ShopWorker shopWorker)
        {
            if (id != shopWorker.ShopWorkerId)
            {
                return BadRequest();
            }

            _context.Entry(shopWorker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShopWorkerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ShopWorkers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ShopWorker>> PostShopWorker(ShopWorker shopWorker)
        {
            _context.ShopWorkers.Add(shopWorker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShopWorker", new { id = shopWorker.ShopWorkerId }, shopWorker);
        }

        // DELETE: api/ShopWorkers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShopWorker(int id)
        {
            var shopWorker = await _context.ShopWorkers.FindAsync(id);
            if (shopWorker == null)
            {
                return NotFound();
            }

            _context.ShopWorkers.Remove(shopWorker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShopWorkerExists(int id)
        {
            return _context.ShopWorkers.Any(e => e.ShopWorkerId == id);
        }
    }
}
