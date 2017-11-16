/* 
 * Assignment2: MVC: Persistence
 *
 * Revision History
 *       Jay Jang, 2017.9.25: Created
 */

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
    public class JJPlotController : Controller
    {
        private readonly OECContext _context;

        public JJPlotController(OECContext context)
        {
            _context = context;
        }

        // GET: JJPlot
        public async Task<IActionResult> Index(int? cropId, string cropName, string farmName,
            int? varietyId, string varietyName, int? plotId, string orderType, string identifyCriteria)
        {
            if (identifyCriteria == "T")
            {
                if (plotId != null)
                {
                    HttpContext.Session.SetString(nameof(plotId), plotId.ToString());
                }
                else
                {
                    plotId = Convert.ToInt32(HttpContext.Session.GetString(nameof(plotId)));
                }
                var kMOECContext = _context.Plot.Where(a => a.PlotId == plotId)
                    .Include(p => p.Farm)
                    .Include(p => p.Variety)
                    .Include(p => p.Variety.Crop)
                    .Include(p => p.Treatment);
                return View(await kMOECContext.ToListAsync());
            }
            else if (identifyCriteria == "V") // From Variety
            {
                if (varietyId != null)
                {
                    HttpContext.Session.SetString(nameof(varietyId), varietyId.ToString());
                    HttpContext.Session.SetString("varietyName", varietyName);
                }
                else if (HttpContext.Session.GetString(nameof(varietyId)) != null)
                {
                    varietyId = Convert.ToInt32(HttpContext.Session.GetString(nameof(varietyId)));
                    varietyName = HttpContext.Session.GetString(nameof(varietyName));
                }
                else
                {
                    TempData["message"] = "Select a variety to see its plots.";
                    return RedirectToAction("Index", "JJVariety");
                }

                //The farm, variety and CEC into hyperlinks that reorder the listing by that field.
                if (orderType == "Farm")
                {
                    var oECContext = _context.Plot.Where(a => a.VarietyId == varietyId)
                        .OrderBy(a => a.Farm.Name).Include(p => p.Farm)
                        .Include(p => p.Variety).Include(p => p.Variety.Crop)
                        .Include(p => p.Treatment);
                    return View(await oECContext.ToListAsync());
                }
                else if (orderType == "Variety")
                {
                    var oECContext = _context.Plot.Where(a => a.VarietyId == varietyId)
                        .OrderBy(a => a.Variety.Name).Include(p => p.Farm)
                        .Include(p => p.Variety).Include(p => p.Variety.Crop)
                        .Include(p => p.Treatment);
                    return View(await oECContext.ToListAsync());
                }
                else if (orderType == "CEC")
                {
                    var oECContext = _context.Plot.Where(a => a.VarietyId == varietyId)
                        .OrderBy(a => a.Cec).Include(p => p.Farm)
                        .Include(p => p.Variety).Include(p => p.Variety.Crop)
                        .Include(p => p.Treatment);
                    return View(await oECContext.ToListAsync());
                }
                else //default
                {
                    var oECContext = _context.Plot.Where(a => a.VarietyId == varietyId)
                        .OrderBy(a => a.DatePlanted).Include(p => p.Farm)
                        .Include(p => p.Variety).Include(p => p.Variety.Crop)
                        .Include(p => p.Treatment);
                    return View(await oECContext.ToListAsync());
                }
            }
            else if(identifyCriteria =="C") // From Crop
            {
                if (cropId != null) 
                {
                    HttpContext.Session.SetString(nameof(cropId), cropId.ToString());
                    HttpContext.Session.SetString("cropName", cropName);
                }
                else if (HttpContext.Session.GetString(nameof(cropId)) != null)
                {
                    cropId = Convert.ToInt32(HttpContext.Session.GetString(nameof(cropId)));
                    cropName = HttpContext.Session.GetString(nameof(cropName));
                }
                else
                {
                    TempData["message"] = "Select a crop to see its plots.";
                    return RedirectToAction("Index", "JJCrop");
                }

                //The farm, variety and CEC into hyperlinks that reorder the listing by that field.
                if (orderType == "Farm")
                {
                    var kMOECContext = _context.Plot.Where(a => a.Variety.Crop.CropId == cropId)
                        .OrderBy(a => a.Farm.Name)
                        .Include(p => p.Farm)
                        .Include(p => p.Variety)
                        .Include(p => p.Variety.Crop)
                        .Include(p => p.Treatment);
                    return View(await kMOECContext.ToListAsync());
                }
                else if (orderType == "Variety")
                {
                    var oECContext = _context.Plot.Where(a => a.Variety.Crop.CropId == cropId)
                        .OrderBy(a => a.Variety.Name)
                        .Include(p => p.Farm)
                        .Include(p => p.Variety)
                        .Include(p => p.Variety.Crop)
                        .Include(p => p.Treatment);
                    return View(await oECContext.ToListAsync());
                }
                else if (orderType == "CEC")
                {
                    var oECContext = _context.Plot.Where(a => a.Variety.Crop.CropId == cropId)
                        .OrderBy(a => a.Cec).Include(p => p.Farm)
                        .Include(p => p.Variety)
                        .Include(p => p.Variety.Crop)
                        .Include(p => p.Treatment);
                    return View(await oECContext.ToListAsync());
                }
                else
                {
                    var kMOECContext = _context.Plot.Where(a => a.Variety.Crop.CropId == cropId)
                        .Include(p => p.Farm)
                        .Include(p => p.Variety)
                        .Include(p => p.Variety.Crop)
                        .Include(p => p.Treatment);
                    return View(await kMOECContext.ToListAsync());
                }
            }
            else //default All plots on file
            {
                var kMOECContext = _context.Plot
                .Include(p => p.Farm)
                .Include(p => p.Variety)
                .Include(p => p.Variety.Crop)
                .Include(p => p.Treatment);
                return View(await kMOECContext.ToListAsync());
            }
        }

        // GET: JJPlot/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plot = await _context.Plot
                .Include(p => p.Farm)
                .Include(p => p.Variety)
                .SingleOrDefaultAsync(m => m.PlotId == id);
            if (plot == null)
            {
                return NotFound();
            }

            return View(plot);
        }

        // GET: JJPlot/Create
        public IActionResult Create()
        {
            ViewData["FarmId"] = new SelectList(_context.Farm, "FarmId", "Name"); //display by Name
            ViewData["VarietyId"] = new SelectList(_context.Variety
                .Where(a => a.CropId == Convert.ToInt32(HttpContext.Session.GetString("cropId"))), "VarietyId", "Name"); //display by Name
            return View();
        }

        // POST: JJPlot/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlotId,FarmId,VarietyId,DatePlanted,DateHarvested,PlantingRate,PlantingRateByPounds,RowWidth,PatternRepeats,OrganicMatter,BicarbP,Potassium,Magnesium,Calcium,PHsoil,PHbuffer,Cec,PercentBaseSaturationK,PercentBaseSaturationMg,PercentBaseSaturationCa,PercentBaseSaturationH,Comments")] Plot plot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plot);
                await _context.SaveChangesAsync();
                if (HttpContext.Session.GetString("varietyId") != null)
                {
                    return RedirectToAction(nameof(Index), new { varietyId = HttpContext.Session.GetString("varietyId"), varietyName = HttpContext.Session.GetString("varietyName") });
                }
                else
                {
                    return RedirectToAction(nameof(Index), new { cropId = HttpContext.Session.GetString("cropId"), cropName = HttpContext.Session.GetString("cropName") });
                }
            }
            ViewData["FarmId"] = new SelectList(_context.Farm, "FarmId", "Name", plot.FarmId);
            ViewData["VarietyId"] = new SelectList(_context.Variety, "VarietyId", "Name", plot.VarietyId);
            return View(plot);
        }

        // GET: JJPlot/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plot = await _context.Plot.SingleOrDefaultAsync(m => m.PlotId == id);
            if (plot == null)
            {
                return NotFound();
            }
            ViewData["FarmId"] = new SelectList(_context.Farm, "FarmId", "Name", plot.FarmId);
            ViewData["VarietyId"] = new SelectList(_context.Variety, "VarietyId", "Name", plot.VarietyId);
            return View(plot);
        }

        // POST: JJPlot/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlotId,FarmId,VarietyId,DatePlanted,DateHarvested,PlantingRate,PlantingRateByPounds,RowWidth,PatternRepeats,OrganicMatter,BicarbP,Potassium,Magnesium,Calcium,PHsoil,PHbuffer,Cec,PercentBaseSaturationK,PercentBaseSaturationMg,PercentBaseSaturationCa,PercentBaseSaturationH,Comments")] Plot plot)
        {
            if (id != plot.PlotId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlotExists(plot.PlotId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (HttpContext.Session.GetString("varietyId") != null)
                {
                    return RedirectToAction(nameof(Index), new { varietyId = HttpContext.Session.GetString("varietyId"), varietyName = HttpContext.Session.GetString("varietyName") });
                }
                else
                {
                    return RedirectToAction(nameof(Index), new { cropId = HttpContext.Session.GetString("cropId"), cropName = HttpContext.Session.GetString("cropName") });
                }
            }
            ViewData["FarmId"] = new SelectList(_context.Farm, "FarmId", "Name", plot.FarmId);
            ViewData["VarietyId"] = new SelectList(_context.Variety, "VarietyId", "Name", plot.VarietyId);
            return View(plot);
        }

        // GET: JJPlot/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plot = await _context.Plot
                .Include(p => p.Farm)
                .Include(p => p.Variety)
                .SingleOrDefaultAsync(m => m.PlotId == id);
            if (plot == null)
            {
                return NotFound();
            }

            return View(plot);
        }

        // POST: JJPlot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plot = await _context.Plot.SingleOrDefaultAsync(m => m.PlotId == id);
            _context.Plot.Remove(plot);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            if (HttpContext.Session.GetString("varietyId") != null)
            {
                return RedirectToAction(nameof(Index), new { varietyId = HttpContext.Session.GetString("varietyId"), varietyName = HttpContext.Session.GetString("varietyName") });
            }
            else
            {
                return RedirectToAction(nameof(Index), new { cropId = HttpContext.Session.GetString("cropId"), cropName = HttpContext.Session.GetString("cropName") });
            }
        }

        private bool PlotExists(int id)
        {
            return _context.Plot.Any(e => e.PlotId == id);
        }
    }
}
