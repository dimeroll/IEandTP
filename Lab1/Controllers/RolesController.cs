using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab1_ICtaTP;
using Microsoft.AspNetCore.Http;
using System.IO;
using ClosedXML.Excel;

namespace Lab1_ICtaTP.Controllers
{
    public class RolesController : Controller
    {
        private readonly Lab1DBContext _context;

        public RolesController(Lab1DBContext context)
        {
            _context = context;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.ToListAsync());
        }

        public IActionResult IndexExceptionTrainAndTrainType()
        {
            return View();
        }

        public IActionResult IndexExceptionNotAllFields()
        {
            return View();
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (role == null)
            {
                return NotFound();
            }

            //return View(role);
            return RedirectToAction("IndexForRoles", "TrainWorkers", new { id = role.RoleId, name = role.RoleName });
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,RoleName")] Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,RoleName")] Role role)
        {
            if (id != role.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.RoleId))
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
            return View(role);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.RoleId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            //перегляд усіх листів (в даному випадку ролей працівників)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва ролі. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Role newrole;
                                var r = (from role in _context.Roles
                                         where role.RoleName.Contains(worksheet.Name)
                                         select role).ToList();
                                if (r.Count > 0)
                                {
                                    newrole = r[0];
                                }
                                else
                                {
                                    newrole = new Role();
                                    newrole.RoleName = worksheet.Name;
                                    //додати в контекст
                                    _context.Roles.Add(newrole);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        if (row.Cell(1).Value.ToString() == "" || row.Cell(2).Value.ToString() == "" || row.Cell(3).Value.ToString() == "" ||
                                                row.Cell(4).Value.ToString() == "" || row.Cell(5).Value.ToString() == "")
                                            throw (new ArgumentException("notallfields"));
                                        TrainWorker trainworker = new TrainWorker();
                                        trainworker.Name = row.Cell(1).Value.ToString();
                                        trainworker.Surname = row.Cell(2).Value.ToString();
                                        trainworker.Role = newrole;
                                        //у разі наявності потягу знайти його, у разі відсутності - додати
                                        if (row.Cell(3).Value.ToString().Length > 0)
                                        {
                                            Train train;

                                            var t = (from tr in _context.Trains
                                                     where tr.AdditionalInfo.Contains(row.Cell(3).Value.ToString())
                                                     select tr).Include(tr => tr.TrainType).ToList();
                                            if (t.Count > 0)
                                            {
                                                train = t[0];
                                                if (train.TrainType.TrainTypeName != row.Cell(4).Value.ToString())
                                                    throw (new ArgumentException("trainAndTrainType"));
                                            }
                                            else
                                            {
                                                train = new Train();
                                                train.AdditionalInfo = row.Cell(3).Value.ToString();
                                                //у разі наявності ТИПУ потягу знайти його, у разі відсутності - додати

                                                if (row.Cell(4).Value.ToString().Length > 0)
                                                {
                                                    TrainType traintype;

                                                    var tt = (from type in _context.TrainTypes
                                                              where type.TrainTypeName.Contains(row.Cell(4).Value.ToString())
                                                              select type).ToList();
                                                    if (tt.Count > 0)
                                                    {
                                                        traintype = tt[0];
                                                    }
                                                    else
                                                    {
                                                        traintype = new TrainType();
                                                        traintype.TrainTypeName = row.Cell(4).Value.ToString();
                                                        //додати в контекст
                                                        _context.Add(traintype);
                                                    }

                                                    train.TrainType = traintype;
                                                }

                                                _context.Add(train);
                                            }

                                            trainworker.Train = train;
                                        }

                                        _context.TrainWorkers.Add(trainworker);
                                    }

                                    catch (Exception e)
                                    {
                                        if(e.Message == "trainAndTrainType")
                                            return RedirectToAction("IndexExceptionTrainAndTrainType", "Roles");
                                        if (e.Message == "notallfields")
                                            return RedirectToAction("IndexExceptionNotAllFields", "Roles");
                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var roles = _context.Roles.Include("TrainWorkers").ToList();
                
                foreach (var r in roles)
                {
                    var worksheet = workbook.Worksheets.Add(r.RoleName);

                    worksheet.Cell("A1").Value = "Ім'я";
                    worksheet.Cell("B1").Value = "Прізвище";
                    worksheet.Cell("C1").Value = "Потяг";
                    worksheet.Cell("D1").Value = "Тип потягу";
                    worksheet.Cell("E1").Value = "Роль";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var trainWorkers = (from tw in _context.TrainWorkers
                                        where tw.Role.Equals(r)
                                        select tw).Include(tw => tw.Train).Include(tw => tw.Train.TrainType).ToList();

                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < trainWorkers.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = trainWorkers[i].Name;
                        worksheet.Cell(i + 2, 2).Value = trainWorkers[i].Surname;
                        worksheet.Cell(i + 2, 3).Value = trainWorkers[i].Train.AdditionalInfo;
                        worksheet.Cell(i + 2, 4).Value = trainWorkers[i].Train.TrainType.TrainTypeName;
                        worksheet.Cell(i + 2, 5).Value = trainWorkers[i].Role.RoleName;

                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"TrainWorkers_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

    }
}
