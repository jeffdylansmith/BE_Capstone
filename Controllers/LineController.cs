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
    public class LineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LineController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Line
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Line.Include(l => l.Character).Include(l => l.Scene);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Line/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var line = await _context.Line
                .Include(l => l.Character)
                .Include(l => l.Scene)
                .SingleOrDefaultAsync(m => m.LineId == id);
            if (line == null)
            {
                return NotFound();
            }

            return View(line);
        }

        // GET: Line/Create
        public async Task<IActionResult> Create(int? id)
        {
            var projId = await _context.Scene.SingleOrDefaultAsync(m => m.SceneId == id);
            ViewData["CharacterId"] = new SelectList(_context.Character.Where(c => c.ProjectId == projId.ProjectId), "CharacterId", "Description");
            ViewData["SceneId"] = id;
            return View();
        }

        // POST: Line/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LineId,Description,Order,Body,CharacterId,SceneId")] Line line, int id)
        {
            if (ModelState.IsValid)
           {
               IEnumerable<Line> lineCount =  _context.Line.Where(s => s.SceneId == id);
                line.Order = 1 + lineCount.Count();
                var projId = await _context.Scene.SingleOrDefaultAsync(m => m.SceneId == id);
                line.SceneId = id;
                line.ProjectId = projId.ProjectId;
                _context.Add(line);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details","Scene", new {id = id});
            }
            ViewData["CharacterId"] = new SelectList(_context.Character, "CharacterId", "Description", line.CharacterId);
            return View(line);
        }

        // GET: Line/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
       
                
            var line = await _context.Line
            .SingleOrDefaultAsync(m => m.LineId == id);
            if (line == null)
            {
                return NotFound();
            }
            ViewData["SceneId"] = line.SceneId;
            ViewData["LineId"] = line.LineId;
            ViewData["CharacterId"] = new SelectList(_context.Character, "CharacterId", "Description", line.CharacterId);
            return View(line);
        }

        // POST: Line/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Line line)
        {
            if (id != line.LineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(line);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LineExists(line.LineId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details","Scene", new {id = line.SceneId});
            }
            ViewData["CharacterId"] = new SelectList(_context.Character, "CharacterId", "Description", line.CharacterId);
            return View(line);
        }

        // GET: Line/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var line = await _context.Line
                .Include(l => l.Character)
                .Include(l => l.Scene)
                .SingleOrDefaultAsync(m => m.LineId == id);
            if (line == null)
            {
                return NotFound();
            }

            return View(line);
        }

        // POST: Line/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var line = await _context.Line.SingleOrDefaultAsync(m => m.LineId == id);
            _context.Line.Remove(line);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool LineExists(int id)
        {
            return _context.Line.Any(e => e.LineId == id);
        }
    }
}
