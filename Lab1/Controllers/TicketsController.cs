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
    public class TicketsController : Controller
    {
        private readonly Lab1DBContext _context;

        public TicketsController(Lab1DBContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Passengers", "Index");
            //знаходження квитків за пасажиром
            ViewBag.PassengerId = id;
            ViewBag.Surname = name;
            var ticketsByPassenger = _context.Tickets.Where(b=>b.PassengerId==id).Include(b => b.Passenger).Include(t => t.Journey.DeparturePoint).Include(t => t.Journey.Destination).Include(t => t.Journey.Train);

            return View(await ticketsByPassenger.ToListAsync());
        }


        // GET: Tickets for journeys
        public async Task<IActionResult> IndexForJourneys(int? id)
        {
            if (id == null) return RedirectToAction("Journeys", "Index");
            //знаходження квитків за рейсом
            ViewBag.JourneyId = id;
            var ticketsByJourney = _context.Tickets.Where(b => b.JourneyId == id).Include(b => b.Journey.DeparturePoint).Include(b => b.Journey.Destination).Include(b => b.Journey.Train).Include(t => t.Passenger);

            return View(await ticketsByJourney.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Journey)
                .Include(t => t.Passenger)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create(int passengerId)
        {
            ViewData["JourneyId"] = new SelectList(_context.Journeys, "JourneyId", "JourneyId");
            //ViewData["PassengerId"] = new SelectList(_context.Passengers, "PassportId", "Name");
            ViewBag.PassengerId = passengerId;
            ViewBag.Surname = _context.Passengers.Where(c => c.PassportId == passengerId).FirstOrDefault().Surname;
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int passengerId, [Bind("TicketId,Place,PassengerId,JourneyId")] Ticket ticket)
        {
            ticket.PassengerId = passengerId;
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Tickets", new { id = passengerId, name = _context.Passengers.Where(c => c.PassportId == passengerId).FirstOrDefault().Surname });
            }
            ViewData["JourneyId"] = new SelectList(_context.Journeys, "JourneyId", "JourneyId", ticket.JourneyId);
            //ViewData["PassengerId"] = new SelectList(_context.Passengers, "PassportId", "Name", ticket.PassengerId);
            //return View(ticket);
            return RedirectToAction("Index", "Tickets", new { id = passengerId, name = _context.Passengers.Where(c => c.PassportId == passengerId).FirstOrDefault().Surname });
        }


        // GET: Tickets/Create for Journeys
        public IActionResult CreateForJourneys(int journeyId)
        {
            //ViewData["JourneyId"] = new SelectList(_context.Journeys, "JourneyId", "JourneyId");
            ViewData["PassengerId"] = new SelectList(_context.Passengers, "PassportId", "Surname");
            ViewBag.JourneyId = journeyId;
            //ViewBag.Surname = _context.Passengers.Where(c => c.PassportId == passengerId).FirstOrDefault().Surname;
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateForJourneys(int journeyId, [Bind("TicketId,Place,PassengerId,JourneyId")] Ticket ticket)
        {
            ticket.JourneyId = journeyId;
            if (ModelState.IsValid && (_context.Journeys.Where(c => c.JourneyId == journeyId).FirstOrDefault().Tickets.Count() < 3))
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("IndexForJourneys", "Tickets", new { id = journeyId });
            }
            //ViewData["JourneyId"] = new SelectList(_context.Journeys, "JourneyId", "JourneyId", ticket.JourneyId);
            ViewData["PassengerId"] = new SelectList(_context.Passengers, "PassportId", "Surname", ticket.PassengerId);
            //return View(ticket);
            if (_context.Journeys.Where(c => c.JourneyId == journeyId).FirstOrDefault().Tickets.Count() < 3)
                return RedirectToAction("IndexForJourneys", "Tickets", new { id = journeyId });
            else return RedirectToAction("Journeys", "Index");
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["JourneyId"] = new SelectList(_context.Journeys, "JourneyId", "JourneyId", ticket.JourneyId);
            ViewData["PassengerId"] = new SelectList(_context.Passengers, "PassportId", "Name", ticket.PassengerId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,Place,PassengerId,JourneyId")] Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.TicketId))
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
            ViewData["JourneyId"] = new SelectList(_context.Journeys, "JourneyId", "JourneyId", ticket.JourneyId);
            ViewData["PassengerId"] = new SelectList(_context.Passengers, "PassportId", "Name", ticket.PassengerId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Journey)
                .Include(t => t.Passenger)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }
    }
}
