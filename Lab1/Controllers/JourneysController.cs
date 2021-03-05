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
    public class JourneysController : Controller
    {
        private readonly Lab1DBContext _context;

        public JourneysController(Lab1DBContext context)
        {
            _context = context;
        }

        // GET: Journeys
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("DeparturePoints", "Index");
            //знаходження рейсів за пунктом відправлення
            ViewBag.DeparturePointId = id;
            ViewBag.DeparturePointName = name;
            var journeysByDeparturePoint = _context.Journeys.Where(b => b.DeparturePointId == id).Include(b => b.DeparturePoint).Include(j => j.Destination).Include(j => j.Train);
            //var lab1DBContext = _context.Journeys.Include(j => j.DeparturePoint).Include(j => j.Destination).Include(j => j.Train);
            return View(await journeysByDeparturePoint.ToListAsync());
        }

        //GET: Journeys for Destination
        public async Task<IActionResult> IndexDestinations(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Destinations", "Index");
            //знаходження рейсів за пунктом призначення
            ViewBag.DestinationId = id;
            ViewBag.DestinationName = name;
            var journeysByDestination = _context.Journeys.Where(b => b.DestinationId == id).Include(b => b.Destination).Include(j => j.DeparturePoint).Include(j => j.Train);
            //var lab1DBContext = _context.Journeys.Include(j => j.DeparturePoint).Include(j => j.Destination).Include(j => j.Train);
            return View(await journeysByDestination.ToListAsync());
        }

        // GET: Journeys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journey = await _context.Journeys
                .Include(j => j.DeparturePoint)
                .Include(j => j.Destination)
                .Include(j => j.Train)
                .FirstOrDefaultAsync(m => m.JourneyId == id);
            if (journey == null)
            {
                return NotFound();
            }

            //return View(journey);
            return RedirectToAction("IndexForJourneys", "Tickets", new { id = journey.JourneyId });
        }

        // GET: Journeys/Create
        public IActionResult Create(int departurePointId)
        {
            // ViewData["DeparturePointId"] = new SelectList(_context.DeparturePoints, "DeparturePointId", "DeparturePointName");
            ViewBag.DeparturePointId = departurePointId;
            ViewBag.DeparturePointName = _context.DeparturePoints.Where(c => c.DeparturePointId == departurePointId).FirstOrDefault().DeparturePointName;

            ViewData["DestinationId"] = new SelectList(_context.Destinations, "DestinationId", "DestinationName");
            ViewData["TrainId"] = new SelectList(_context.Trains, "TrainId", "AdditionalInfo");
            return View();
        }

        // POST: Journeys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int departurePointId, [Bind("JourneyId,TrainId,DepartureTime,ArrivalTime,DeparturePointId,DestinationId")] Journey journey)
        {
            journey.DeparturePointId = departurePointId;
            if (ModelState.IsValid)
            {
                _context.Add(journey);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Journeys", new { id = departurePointId, name = _context.DeparturePoints.Where(c=>c.DeparturePointId == departurePointId).FirstOrDefault().DeparturePointName });
            }
            //ViewData["DeparturePointId"] = new SelectList(_context.DeparturePoints, "DeparturePointId", "DeparturePointName", journey.DeparturePointId);
            ViewData["DestinationId"] = new SelectList(_context.Destinations, "DestinationId", "DestinationName", journey.DestinationId);
            ViewData["TrainId"] = new SelectList(_context.Trains, "TrainId", "AdditionalInfo", journey.TrainId);
            //return View(journey);
            return RedirectToAction("Index", "Journeys", new { id = departurePointId, name = _context.DeparturePoints.Where(c => c.DeparturePointId == departurePointId).FirstOrDefault().DeparturePointName });
        }

        // GET: Journeys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journey = await _context.Journeys.FindAsync(id);
            if (journey == null)
            {
                return NotFound();
            }
            ViewData["DeparturePointId"] = new SelectList(_context.DeparturePoints, "DeparturePointId", "DeparturePointName", journey.DeparturePointId);
            ViewData["DestinationId"] = new SelectList(_context.Destinations, "DestinationId", "DestinationName", journey.DestinationId);
            ViewData["TrainId"] = new SelectList(_context.Trains, "TrainId", "AdditionalInfo", journey.TrainId);
            return View(journey);
        }

        // POST: Journeys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JourneyId,TrainId,DepartureTime,ArrivalTime,DeparturePointId,DestinationId")] Journey journey)
        {
            if (id != journey.JourneyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(journey);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JourneyExists(journey.JourneyId))
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
            ViewData["DeparturePointId"] = new SelectList(_context.DeparturePoints, "DeparturePointId", "DeparturePointName", journey.DeparturePointId);
            ViewData["DestinationId"] = new SelectList(_context.Destinations, "DestinationId", "DestinationName", journey.DestinationId);
            ViewData["TrainId"] = new SelectList(_context.Trains, "TrainId", "AdditionalInfo", journey.TrainId);
            return View(journey);
        }

        // GET: Journeys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journey = await _context.Journeys
                .Include(j => j.DeparturePoint)
                .Include(j => j.Destination)
                .Include(j => j.Train)
                .FirstOrDefaultAsync(m => m.JourneyId == id);
            if (journey == null)
            {
                return NotFound();
            }

            return View(journey);
        }

        // POST: Journeys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var journey = await _context.Journeys.FindAsync(id);
            _context.Journeys.Remove(journey);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JourneyExists(int id)
        {
            return _context.Journeys.Any(e => e.JourneyId == id);
        }
    }
}
