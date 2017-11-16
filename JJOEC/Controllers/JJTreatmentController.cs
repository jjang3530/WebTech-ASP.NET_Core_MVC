using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JJOEC.Models;
using Microsoft.AspNetCore.Http;

namespace JJOEC.Controllers
{
    public class JJTreatmentController : Controller
    {
        private readonly OECContext _context;

        public JJTreatmentController(OECContext context)
        {
            _context = context;
        }

        // GET: JJTreatment
        public async Task<IActionResult> Index(int? plotId, string farmName)
        {
            if (plotId != null)
            {
                HttpContext.Session.SetString(nameof(plotId), plotId.ToString());
                HttpContext.Session.SetString("farmName", farmName);
            }
            else if (HttpContext.Session.GetString(nameof(plotId)) != null) // from modify views
            {
                plotId = Convert.ToInt32(HttpContext.Session.GetString(nameof(plotId)));
                farmName = HttpContext.Session.GetString(nameof(farmName));
                
                var oEC1Context = _context.Treatment
                    .Where(a => a.PlotId == plotId)
                    .OrderBy(a => a.Name)
                    .Include(t => t.Plot)
                    .Include(t => t.TreatmentFertilizer); //Include Table

                foreach (var item in oEC1Context)
                {
                    string fertilizerName = "";
                    foreach (var treatmentFertilizer in item.TreatmentFertilizer)
                    {
                        if (fertilizerName =="")
                        {
                            fertilizerName = treatmentFertilizer.FertilizerName;
                        }
                        else
                        {
                            fertilizerName += " + " + treatmentFertilizer.FertilizerName;
                        }
                    }
                    if(fertilizerName == "")
                    {
                        item.Name = "no fertilizer";
                    }
                    else
                    {
                        item.Name = fertilizerName;
                    }
                }

                return View(await oEC1Context.ToListAsync());
            }
            else
            {
                TempData["message"] = "Select a plot to see its treatment.";
                return RedirectToAction("Index", "JJPlot");
            }

            var oECContext = _context.Treatment
                .Where(a => a.PlotId == plotId)
                .OrderBy(a => a.Name)
                .Include(t => t.Plot);
            return View(await oECContext.ToListAsync());
        }

        // GET: JJTreatment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatment = await _context.Treatment
                .Include(t => t.Plot)
                .SingleOrDefaultAsync(m => m.TreatmentId == id);
            if (treatment == null)
            {
                return NotFound();
            }

            return View(treatment);
        }

        // GET: JJTreatment/Create
        public IActionResult Create()
        {
            ViewData["PlotId"] = new SelectList(_context.Plot, "PlotId", "PlotId");
            return View();
        }

        // POST: JJTreatment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TreatmentId,Name,PlotId,Moisture,Yield,Weight")] Treatment treatment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(treatment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PlotId"] = new SelectList(_context.Plot, "PlotId", "PlotId", treatment.PlotId);
            return View(treatment);
        }

        // GET: JJTreatment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatment = await _context.Treatment.SingleOrDefaultAsync(m => m.TreatmentId == id);
            if (treatment == null)
            {
                return NotFound();
            }
            ViewData["PlotId"] = new SelectList(_context.Plot, "PlotId", "PlotId", treatment.PlotId);
            return View(treatment);
        }

        // POST: JJTreatment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TreatmentId,Name,PlotId,Moisture,Yield,Weight")] Treatment treatment)
        {
            if (id != treatment.TreatmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(treatment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreatmentExists(treatment.TreatmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index),
                    new { plotId = HttpContext.Session.GetString("plotId"),
                        farmName = HttpContext.Session.GetString("farmName") });
            }
            ViewData["PlotId"] = new SelectList(_context.Plot, "PlotId", "PlotId", treatment.PlotId);
            return View(treatment);
        }

        // GET: JJTreatment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatment = await _context.Treatment
                .Include(t => t.Plot)
                .SingleOrDefaultAsync(m => m.TreatmentId == id);
            if (treatment == null)
            {
                return NotFound();
            }

            return View(treatment);
        }

        // POST: JJTreatment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treatment = await _context.Treatment.SingleOrDefaultAsync(m => m.TreatmentId == id);
            _context.Treatment.Remove(treatment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreatmentExists(int id)
        {
            return _context.Treatment.Any(e => e.TreatmentId == id);
        }
    }
}
