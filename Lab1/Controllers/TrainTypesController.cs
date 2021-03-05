using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab1_ICtaTP;

namespace Lab1_ICtaTP.Controllers
{
    public class TrainTypesController : Controller
    {
        private readonly Lab1DBContext _context;

        public TrainTypesController(Lab1DBContext context)
        {
            _context = context;
        }

        // GET: TrainTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TrainTypes.ToListAsync());
        }

        // GET: TrainTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainType = await _context.TrainTypes
                .FirstOrDefaultAsync(m => m.TrainTypeId == id);
            if (trainType == null)
            {
                return NotFound();
            }

            // return View(trainType);
            return RedirectToAction("Index", "Trains", new { id = trainType.TrainTypeId, name = trainType.TrainTypeName });
        }

        // GET: TrainTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrainTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainTypeId,TrainTypeName")] TrainType trainType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainType);
        }

        // GET: TrainTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainType = await _context.TrainTypes.FindAsync(id);
            if (trainType == null)
            {
                return NotFound();
            }
            return View(trainType);
        }

        // POST: TrainTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainTypeId,TrainTypeName")] TrainType trainType)
        {
            if (id != trainType.TrainTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainTypeExists(trainType.TrainTypeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(trainType);
        }

        // GET: TrainTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainType = await _context.TrainTypes
                .FirstOrDefaultAsync(m => m.TrainTypeId == id);
            if (trainType == null)
            {
                return NotFound();
            }

            return View(trainType);
        }

        // POST: TrainTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainType = await _context.TrainTypes.FindAsync(id);
            _context.TrainTypes.Remove(trainType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainTypeExists(int id)
        {
            return _context.TrainTypes.Any(e => e.TrainTypeId == id);
        }
    }
}
