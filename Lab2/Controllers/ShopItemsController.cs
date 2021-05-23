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
    public class ShopItemsController : ControllerBase
    {
        private readonly Lab2Context _context;

        public ShopItemsController(Lab2Context context)
        {
            _context = context;
        }

        // GET: api/ShopItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopItem>>> GetShopItems()
        {
            return await _context.ShopItems.ToListAsync();
        }

        // GET: api/ShopItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShopItem>> GetShopItem(int id)
        {
            var shopItem = await _context.ShopItems.FindAsync(id);

            if (shopItem == null)
            {
                return NotFound();
            }

            return shopItem;
        }

        // PUT: api/ShopItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShopItem(int id, ShopItem shopItem)
        {
            if (id != shopItem.ShopItemId)
            {
                return BadRequest();
            }

            _context.Entry(shopItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShopItemExists(id))
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

        // POST: api/ShopItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ShopItem>> PostShopItem(ShopItem shopItem)
        {
            _context.ShopItems.Add(shopItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShopItem", new { id = shopItem.ShopItemId }, shopItem);
        }

        // DELETE: api/ShopItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShopItem(int id)
        {
            var shopItem = await _context.ShopItems.FindAsync(id);
            if (shopItem == null)
            {
                return NotFound();
            }

            _context.ShopItems.Remove(shopItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShopItemExists(int id)
        {
            return _context.ShopItems.Any(e => e.ShopItemId == id);
        }
    }
}
