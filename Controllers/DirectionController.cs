using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BE_Capstone.Data;
using BE_Capstone.Models;

namespace BE_Capstone.Controllers
{
    public class DirectionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DirectionController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Direction
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Direction.Include(d => d.Scene);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Direction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direction = await _context.Direction
                .Include(d => d.Scene)
                .SingleOrDefaultAsync(m => m.DirectionId == id);
            if (direction == null)
            {
                return NotFound();
            }

            return View(direction);
        }

        // GET: Direction/Create
        public IActionResult Create()
        {
            //ViewData["SceneId"] = new SelectList(_context.Scene, "SceneId", "Description");
            return View();
        }

        // POST: Direction/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DirectionId,Order,Body,SceneId")] Direction direction, int id)
        {
            if (ModelState.IsValid)
            {
                direction.SceneId = id;
                _context.Add(direction);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details","Scene", new {id = id});
            }
            ViewData["SceneId"] = new SelectList(_context.Scene, "SceneId", "Description", direction.SceneId);
            return View(direction);
        }

        // GET: Direction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direction = await _context.Direction.SingleOrDefaultAsync(m => m.DirectionId == id);
            if (direction == null)
            {
                return NotFound();
            }
            ViewData["SceneId"] = new SelectList(_context.Scene, "SceneId", "Description", direction.SceneId);
            return View(direction);
        }

        // POST: Direction/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DirectionId,Order,Body,SceneId")] Direction direction)
        {
            if (id != direction.DirectionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(direction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DirectionExists(direction.DirectionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details","Scene", new {id = id});
            }
            ViewData["SceneId"] = new SelectList(_context.Scene, "SceneId", "Description", direction.SceneId);
            return View(direction);
        }

        // GET: Direction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direction = await _context.Direction
                .Include(d => d.Scene)
                .SingleOrDefaultAsync(m => m.DirectionId == id);
            if (direction == null)
            {
                return NotFound();
            }

            return View(direction);
        }

        // POST: Direction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var direction = await _context.Direction.SingleOrDefaultAsync(m => m.DirectionId == id);
            _context.Direction.Remove(direction);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DirectionExists(int id)
        {
            return _context.Direction.Any(e => e.DirectionId == id);
        }
    }
}
