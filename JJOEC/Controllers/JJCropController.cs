/* 
 * Assignment1: Core MVC Basics
 *
 * Revision History
 *       Jay Jang, 2017.09.16: Created
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
    public class JJCropController : Controller
    {
        private readonly OECContext _context;

        public JJCropController(OECContext context)
        {
            _context = context;
        }

        // GET: JJCrop
        public async Task<IActionResult> Index()
        {
            // order by Name
            return View(await _context.Crop.OrderBy(a=>a.Name).ToListAsync());
        }

        // GET: JJCrop/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crop = await _context.Crop
                .SingleOrDefaultAsync(m => m.CropId == id);
            if (crop == null)
            {
                return NotFound();
            }

            return View(crop);
        }

        // GET: JJCrop/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JJCrop/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CropId,Name,Image")] Crop crop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(crop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(crop);
        }

        // GET: JJCrop/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crop = await _context.Crop.SingleOrDefaultAsync(m => m.CropId == id);
            if (crop == null)
            {
                return NotFound();
            }
            return View(crop);
        }

        // POST: JJCrop/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CropId,Name,Image")] Crop crop)
        {
            if (id != crop.CropId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(crop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CropExists(crop.CropId))
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
            return View(crop);
        }

        // GET: JJCrop/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crop = await _context.Crop
                .SingleOrDefaultAsync(m => m.CropId == id);
            if (crop == null)
            {
                return NotFound();
            }

            return View(crop);
        }

        // POST: JJCrop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var crop = await _context.Crop.SingleOrDefaultAsync(m => m.CropId == id);
            _context.Crop.Remove(crop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CropExists(int id)
        {
            return _context.Crop.Any(e => e.CropId == id);
        }
    }
}
