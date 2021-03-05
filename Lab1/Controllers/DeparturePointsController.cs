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
    public class DeparturePointsController : Controller
    {
        private readonly Lab1DBContext _context;

        public DeparturePointsController(Lab1DBContext context)
        {
            _context = context;
        }

        // GET: DeparturePoints
        public async Task<IActionResult> Index()
        {
            return View(await _context.DeparturePoints.ToListAsync());
        }

        // GET: DeparturePoints/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departurePoint = await _context.DeparturePoints
                .FirstOrDefaultAsync(m => m.DeparturePointId == id);
            if (departurePoint == null)
            {
                return NotFound();
            }

            //return View(departurePoint);
            return RedirectToAction("Index", "Journeys", new { id = departurePoint.DeparturePointId, name = departurePoint.DeparturePointName});
        }

        // GET: DeparturePoints/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DeparturePoints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeparturePointId,DeparturePointName")] DeparturePoint departurePoint)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departurePoint);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(departurePoint);
        }

        // GET: DeparturePoints/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departurePoint = await _context.DeparturePoints.FindAsync(id);
            if (departurePoint == null)
            {
                return NotFound();
            }
            return View(departurePoint);
        }

        // POST: DeparturePoints/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeparturePointId,DeparturePointName")] DeparturePoint departurePoint)
        {
            if (id != departurePoint.DeparturePointId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departurePoint);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeparturePointExists(departurePoint.DeparturePointId))
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
            return View(departurePoint);
        }

        // GET: DeparturePoints/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departurePoint = await _context.DeparturePoints
                .FirstOrDefaultAsync(m => m.DeparturePointId == id);
            if (departurePoint == null)
            {
                return NotFound();
            }

            return View(departurePoint);
        }

        // POST: DeparturePoints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departurePoint = await _context.DeparturePoints.FindAsync(id);
            _context.DeparturePoints.Remove(departurePoint);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeparturePointExists(int id)
        {
            return _context.DeparturePoints.Any(e => e.DeparturePointId == id);
        }
    }
}
