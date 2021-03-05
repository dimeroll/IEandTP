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
    public class TrainWorkersController : Controller
    {
        private readonly Lab1DBContext _context;

        public TrainWorkersController(Lab1DBContext context)
        {
            _context = context;
        }

        // GET: TrainWorkers
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Trains", "Index");
            //знаходження робітників за потягом
            ViewBag.TrainId = id;
            ViewBag.AdditionalInfo = name;
            var workersByTrain = _context.TrainWorkers.Where(b=>b.TrainId == id).Include(b => b.Train).Include(t => t.Role);

            return View(await workersByTrain.ToListAsync());
        }
         
        // GET: TrainWorkers for role
        public async Task<IActionResult> IndexForRoles(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Roles", "Index");
            //знаходження робітників за роллю
            ViewBag.RoleId = id;
            ViewBag.RoleName = name;
            var workersByRole = _context.TrainWorkers.Where(b => b.RoleId == id).Include(b => b.Role).Include(t => t.Train);

            return View(await workersByRole.ToListAsync());
        }

        // GET: TrainWorkers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainWorker = await _context.TrainWorkers
                .Include(t => t.Role)
                .Include(t => t.Train)
                .FirstOrDefaultAsync(m => m.WorkerId == id);
            if (trainWorker == null)
            {
                return NotFound();
            }

            return View(trainWorker);
        }

        // GET: TrainWorkers/Create
        public IActionResult Create(int trainId)
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            //ViewData["TrainId"] = new SelectList(_context.Trains, "TrainId", "AdditionalInfo");
            ViewBag.TrainId = trainId;
            ViewBag.AdditionalInfo = _context.Trains.Where(c => c.TrainId == trainId).FirstOrDefault().AdditionalInfo;
            return View();
        }

        // POST: TrainWorkers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int trainId, [Bind("WorkerId,Name,Surname,TrainId,RoleId")] TrainWorker trainWorker)
        {
            trainWorker.TrainId = trainId;
            if (ModelState.IsValid)
            {
                _context.Add(trainWorker);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "TrainWorkers", new { id = trainId, name = _context.Trains.Where(c => c.TrainId == trainId).FirstOrDefault().AdditionalInfo });
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", trainWorker.RoleId);
            //ViewData["TrainId"] = new SelectList(_context.Trains, "TrainId", "AdditionalInfo", trainWorker.TrainId);
            //return View(trainWorker);
            return RedirectToAction("Index", "TrainWorkers", new { id = trainId, name = _context.Trains.Where(c => c.TrainId == trainId).FirstOrDefault().AdditionalInfo });
        }


        // GET: TrainWorkers/Create for Role
        public IActionResult CreateForRoles(int roleId)
        {
            //ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            ViewData["TrainId"] = new SelectList(_context.Trains, "TrainId", "AdditionalInfo");
            ViewBag.RoleId = roleId;
            ViewBag.RoleName = _context.Roles.Where(c => c.RoleId == roleId).FirstOrDefault().RoleName;
            return View();
        }

        // POST: TrainWorkers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateForRoles(int roleId, [Bind("WorkerId,Name,Surname,TrainId,RoleId")] TrainWorker trainWorker)
        {
            trainWorker.RoleId = roleId;
            if (ModelState.IsValid)
            {
                _context.Add(trainWorker);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("IndexForRoles", "TrainWorkers", new { id = roleId, name = _context.Roles.Where(c => c.RoleId == roleId).FirstOrDefault().RoleName });
            }
            //ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", trainWorker.RoleId);
            ViewData["TrainId"] = new SelectList(_context.Trains, "TrainId", "AdditionalInfo", trainWorker.TrainId);
            //return View(trainWorker);
            return RedirectToAction("IndexForRoles", "TrainWorkers", new { id = roleId, name = _context.Roles.Where(c => c.RoleId == roleId).FirstOrDefault().RoleName });
        }

        // GET: TrainWorkers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainWorker = await _context.TrainWorkers.FindAsync(id);
            if (trainWorker == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", trainWorker.RoleId);
            ViewData["TrainId"] = new SelectList(_context.Trains, "TrainId", "AdditionalInfo", trainWorker.TrainId);
            return View(trainWorker);
        }

        // POST: TrainWorkers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkerId,Name,Surname,TrainId,RoleId")] TrainWorker trainWorker)
        {
            if (id != trainWorker.WorkerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainWorker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainWorkerExists(trainWorker.WorkerId))
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", trainWorker.RoleId);
            ViewData["TrainId"] = new SelectList(_context.Trains, "TrainId", "AdditionalInfo", trainWorker.TrainId);
            return View(trainWorker);
        }

        // GET: TrainWorkers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainWorker = await _context.TrainWorkers
                .Include(t => t.Role)
                .Include(t => t.Train)
                .FirstOrDefaultAsync(m => m.WorkerId == id);
            if (trainWorker == null)
            {
                return NotFound();
            }

            return View(trainWorker);
        }

        // POST: TrainWorkers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainWorker = await _context.TrainWorkers.FindAsync(id);
            _context.TrainWorkers.Remove(trainWorker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainWorkerExists(int id)
        {
            return _context.TrainWorkers.Any(e => e.WorkerId == id);
        }
    }
}
