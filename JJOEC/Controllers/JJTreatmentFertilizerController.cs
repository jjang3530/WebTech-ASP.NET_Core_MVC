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
    public class JJTreatmentFertilizerController : Controller
    {
        private readonly OECContext _context;

        public JJTreatmentFertilizerController(OECContext context)
        {
            _context = context;
        }

        // GET: JJTreatmentFertilizer
        public async Task<IActionResult> Index(int? treatmentId, string treatmentName)
        {
            if (treatmentId != null)
            {
                HttpContext.Session.SetString(nameof(treatmentId), treatmentId.ToString());
                HttpContext.Session.SetString("treatmentName", treatmentName);
            }
            else if (HttpContext.Session.GetString(nameof(treatmentId)) != null)
            {
                treatmentId = Convert.ToInt32(HttpContext.Session.GetString(nameof(treatmentId)));
                treatmentName = HttpContext.Session.GetString(nameof(treatmentName));
            }
            else
            {
                TempData["message"] = "Select a treatment to see its treatment Fertilizer.";
                return RedirectToAction("Index", "JJTreatment");
            }

            var oECContext = _context.TreatmentFertilizer
                .Where(a => a.TreatmentId == treatmentId)
                .OrderBy(a => a.FertilizerName)
                .Include(t => t.FertilizerNameNavigation)
                .Include(t => t.Treatment);
            return View(await oECContext.ToListAsync());
        }

        // GET: JJTreatmentFertilizer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatmentFertilizer = await _context.TreatmentFertilizer
                .Include(t => t.FertilizerNameNavigation)
                .Include(t => t.Treatment)
                .SingleOrDefaultAsync(m => m.TreatmentFertilizerId == id);
            if (treatmentFertilizer == null)
            {
                return NotFound();
            }

            return View(treatmentFertilizer);
        }

        // GET: JJTreatmentFertilizer/Create
        public IActionResult Create()
        {
            ViewData["FertilizerName"] = new SelectList(_context.Fertilizer.OrderBy(a => a.FertilizerName), "FertilizerName", "FertilizerName");
            //ViewData["TreatmentId"] = new SelectList(_context.Treatment, "TreatmentId", "TreatmentId");
            return View();
        }

        // POST: JJTreatmentFertilizer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TreatmentFertilizerId,TreatmentId,FertilizerName,RatePerAcre,RateMetric")]
        TreatmentFertilizer treatmentFertilizer)
        {
            var checkLiquid = _context.Fertilizer.FirstOrDefault(a=>a.FertilizerName == treatmentFertilizer.FertilizerName && a.Liquid == true);
            
            if(checkLiquid != null)
            {
                treatmentFertilizer.RateMetric = "Gal";

            }
            else if(checkLiquid == null)
            {
                treatmentFertilizer.RateMetric = "LB";
            }

            if (ModelState.IsValid)
            {
                _context.Add(treatmentFertilizer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FertilizerName"] = new SelectList(_context.Fertilizer, "FertilizerName", "FertilizerName", treatmentFertilizer.FertilizerName);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment, "TreatmentId", "TreatmentId", treatmentFertilizer.TreatmentId);
            return View(treatmentFertilizer);
        }

        // GET: JJTreatmentFertilizer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatmentFertilizer = await _context.TreatmentFertilizer.SingleOrDefaultAsync(m => m.TreatmentFertilizerId == id);
            if (treatmentFertilizer == null)
            {
                return NotFound();
            }
            ViewData["FertilizerName"] = new SelectList(_context.Fertilizer, "FertilizerName", "FertilizerName", treatmentFertilizer.FertilizerName);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment, "TreatmentId", "TreatmentId", treatmentFertilizer.TreatmentId);
            return View(treatmentFertilizer);
        }

        // POST: JJTreatmentFertilizer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TreatmentFertilizerId,TreatmentId,FertilizerName,RatePerAcre,RateMetric")] TreatmentFertilizer treatmentFertilizer)
        {
            var checkLiquid = _context.Fertilizer.FirstOrDefault(a => a.FertilizerName == treatmentFertilizer.FertilizerName && a.Liquid == true);

            if (checkLiquid != null)
            {
                treatmentFertilizer.RateMetric = "Gal";

            }
            else if (checkLiquid == null)
            {
                treatmentFertilizer.RateMetric = "LB";
            }

            if (id != treatmentFertilizer.TreatmentFertilizerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(treatmentFertilizer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreatmentFertilizerExists(treatmentFertilizer.TreatmentFertilizerId))
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
            ViewData["FertilizerName"] = new SelectList(_context.Fertilizer, "FertilizerName", "FertilizerName", treatmentFertilizer.FertilizerName);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment, "TreatmentId", "TreatmentId", treatmentFertilizer.TreatmentId);
            return View(treatmentFertilizer);
        }

        // GET: JJTreatmentFertilizer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatmentFertilizer = await _context.TreatmentFertilizer
                .Include(t => t.FertilizerNameNavigation)
                .Include(t => t.Treatment)
                .SingleOrDefaultAsync(m => m.TreatmentFertilizerId == id);
            if (treatmentFertilizer == null)
            {
                return NotFound();
            }

            return View(treatmentFertilizer);
        }

        // POST: JJTreatmentFertilizer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treatmentFertilizer = await _context.TreatmentFertilizer.SingleOrDefaultAsync(m => m.TreatmentFertilizerId == id);
            _context.TreatmentFertilizer.Remove(treatmentFertilizer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreatmentFertilizerExists(int id)
        {
            return _context.TreatmentFertilizer.Any(e => e.TreatmentFertilizerId == id);
        }
    }
}
