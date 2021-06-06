using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DatabaseNetCore.Data;
using DatabaseNetCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace DatabaseNetCore.Controllers
{
    public class KeysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KeysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Keys
        public async Task<IActionResult> Index()
        {
            return View(await _context.Key.ToListAsync());
        }

        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.Key.Where( j => j.KeyName.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: Keys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var key = await _context.Key
                .FirstOrDefaultAsync(m => m.Id == id);
            if (key == null)
            {
                return NotFound();
            }

            return View(key);
        }

        [Authorize]
        // GET: Keys/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Keys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KeyName,KeyCode")] Key key)
        {
            if (ModelState.IsValid)
            {
                _context.Add(key);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(key);
        }

        // GET: Keys/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var key = await _context.Key.FindAsync(id);
            if (key == null)
            {
                return NotFound();
            }
            return View(key);
        }

        // POST: Keys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KeyName,KeyCode")] Key key)
        {
            if (id != key.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(key);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KeyExists(key.Id))
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
            return View(key);
        }

        // GET: Keys/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var key = await _context.Key
                .FirstOrDefaultAsync(m => m.Id == id);
            if (key == null)
            {
                return NotFound();
            }

            return View(key);
        }

        // POST: Keys/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var key = await _context.Key.FindAsync(id);
            _context.Key.Remove(key);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KeyExists(int id)
        {
            return _context.Key.Any(e => e.Id == id);
        }
    }
}
