﻿/* 
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

namespace JJOEC.Controllers
{
    public class JJFertilizerController : Controller
    {
        private readonly OECContext _context;

        public JJFertilizerController(OECContext context)
        {
            _context = context;
        }

        // GET: JJFertilizer
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fertilizer.ToListAsync());
        }

        // GET: JJFertilizer/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fertilizer = await _context.Fertilizer
                .SingleOrDefaultAsync(m => m.FertilizerName == id);
            if (fertilizer == null)
            {
                return NotFound();
            }

            return View(fertilizer);
        }

        // GET: JJFertilizer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JJFertilizer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FertilizerName,Oecproduct,Liquid")] Fertilizer fertilizer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fertilizer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fertilizer);
        }

        // GET: JJFertilizer/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fertilizer = await _context.Fertilizer.SingleOrDefaultAsync(m => m.FertilizerName == id);
            if (fertilizer == null)
            {
                return NotFound();
            }
            return View(fertilizer);
        }

        // POST: JJFertilizer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FertilizerName,Oecproduct,Liquid")] Fertilizer fertilizer)
        {
            if (id != fertilizer.FertilizerName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fertilizer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FertilizerExists(fertilizer.FertilizerName))
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
            return View(fertilizer);
        }

        // GET: JJFertilizer/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fertilizer = await _context.Fertilizer
                .SingleOrDefaultAsync(m => m.FertilizerName == id);
            if (fertilizer == null)
            {
                return NotFound();
            }

            return View(fertilizer);
        }

        // POST: JJFertilizer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var fertilizer = await _context.Fertilizer.SingleOrDefaultAsync(m => m.FertilizerName == id);
            _context.Fertilizer.Remove(fertilizer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FertilizerExists(string id)
        {
            return _context.Fertilizer.Any(e => e.FertilizerName == id);
        }
    }
}
