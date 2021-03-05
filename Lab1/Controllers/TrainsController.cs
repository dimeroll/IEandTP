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
    public class TrainsController : Controller
    {
        private readonly Lab1DBContext _context;

        public TrainsController(Lab1DBContext context)
        {
            _context = context;
        }

        // GET: Trains
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("TrainTypes", "Index");

            //знаходження потягів за типом потяга
            ViewBag.TrainTypeId = id;
            ViewBag.TrainTypeName = name;
            var trainsByTrainType = _context.Trains.Where(b => b.TrainTypeId == id).Include(b => b.TrainType);

            return View(await trainsByTrainType.ToListAsync());
        }

        // GET: Trains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Trains
                .Include(t => t.TrainType)
                .FirstOrDefaultAsync(m => m.TrainId == id);
            if (train == null)
            {
                return NotFound();
            }

            //return View(train);
            return RedirectToAction("Index", "TrainWorkers", new { id = train.TrainId, name = train.AdditionalInfo });
        }

        // GET: Trains/Create
        public IActionResult Create(int trainTypeId)
        {
            //ViewData["TrainTypeId"] = new SelectList(_context.TrainTypes, "TrainTypeId", "TrainTypeName");
            ViewBag.TrainTypeId = trainTypeId;
            ViewBag.TrainTypeName = _context.TrainTypes.Where(c => c.TrainTypeId == trainTypeId).FirstOrDefault().TrainTypeName;
            return View();
        }

        // POST: Trains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int trainTypeId, [Bind("TrainId,TrainTypeId,AdditionalInfo")] Train train)
        {
            train.TrainTypeId = trainTypeId;
            if (ModelState.IsValid)
            {
                _context.Add(train);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Trains", new { id = trainTypeId, name = _context.TrainTypes.Where(c => c.TrainTypeId == trainTypeId).FirstOrDefault().TrainTypeName });
            }
            //ViewData["TrainTypeId"] = new SelectList(_context.TrainTypes, "TrainTypeId", "TrainTypeName", train.TrainTypeId);
            //return View(train);
            return RedirectToAction("Index", "Trains", new { id = trainTypeId, name = _context.TrainTypes.Where(c => c.TrainTypeId == trainTypeId).FirstOrDefault().TrainTypeName });
        }

        // GET: Trains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Trains.FindAsync(id);
            if (train == null)
            {
                return NotFound();
            }
            ViewData["TrainTypeId"] = new SelectList(_context.TrainTypes, "TrainTypeId", "TrainTypeName", train.TrainTypeId);
            return View(train);
        }

        // POST: Trains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainId,TrainTypeId,AdditionalInfo")] Train train)
        {
            if (id != train.TrainId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(train);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainExists(train.TrainId))
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
            ViewData["TrainTypeId"] = new SelectList(_context.TrainTypes, "TrainTypeId", "TrainTypeName", train.TrainTypeId);
            return View(train);
        }

        // GET: Trains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Trains
                .Include(t => t.TrainType)
                .FirstOrDefaultAsync(m => m.TrainId == id);
            if (train == null)
            {
                return NotFound();
            }

            return View(train);
        }

        // POST: Trains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var train = await _context.Trains.FindAsync(id);
            _context.Trains.Remove(train);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainExists(int id)
        {
            return _context.Trains.Any(e => e.TrainId == id);
        }
    }
}
