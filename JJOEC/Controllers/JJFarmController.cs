using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JJOEC.Models;

namespace JJOEC.Controllers
{
    public class JJFarmController : Controller
    {
        private readonly OECContext _context;

        public JJFarmController(OECContext context)
        {
            _context = context;
        }

        // GET: JJFarm
        public async Task<IActionResult> Index()
        {
            var oECContext = _context.Farm.OrderBy(a=>a.Name)
                .Include(f => f.ProvinceCodeNavigation);
            return View(await oECContext.ToListAsync());
        }

        // GET: JJFarm/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farm = await _context.Farm
                .Include(f => f.ProvinceCodeNavigation)
                .SingleOrDefaultAsync(m => m.FarmId == id);
            if (farm == null)
            {
                return NotFound();
            }

            return View(farm);
        }

        // GET: JJFarm/Create
        public IActionResult Create()
        {
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode");
            return View();
        }

        // POST: JJFarm/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FarmId,Name,Address,Town,County,ProvinceCode,PostalCode,HomePhone,CellPhone,Email,Directions,DateJoined,LastContactDate")] Farm farm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(farm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", farm.ProvinceCode);
            return View(farm);
        }

        // GET: JJFarm/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farm = await _context.Farm.SingleOrDefaultAsync(m => m.FarmId == id);
            if (farm == null)
            {
                return NotFound();
            }
            //ViewData["Name"] = new SelectList(_context.Province.OrderBy(a=>a.Name), "Name", "Name", farm.Name);
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", farm.ProvinceCode);
            return View(farm);
        }

        // POST: JJFarm/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FarmId,Name,Address,Town,County,ProvinceCode,PostalCode,HomePhone,CellPhone,Email,Directions,DateJoined,LastContactDate")] Farm farm)
        {
            if (id != farm.FarmId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(farm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FarmExists(farm.FarmId))
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
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", farm.ProvinceCode);
            return View(farm);
        }

        // GET: JJFarm/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farm = await _context.Farm
                .Include(f => f.ProvinceCodeNavigation)
                .SingleOrDefaultAsync(m => m.FarmId == id);
            if (farm == null)
            {
                return NotFound();
            }

            return View(farm);
        }

        // POST: JJFarm/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var farm = await _context.Farm.SingleOrDefaultAsync(m => m.FarmId == id);
            _context.Farm.Remove(farm);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FarmExists(int id)
        {
            return _context.Farm.Any(e => e.FarmId == id);
        }
    }
}
