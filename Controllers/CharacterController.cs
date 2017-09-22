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
    public class CharacterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CharacterController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Character
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Character.Include(c => c.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Character/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Character
                .Include(c => c.Project)
                .SingleOrDefaultAsync(m => m.CharacterId == id);
            if (character == null)
            {
                return NotFound();
            }

            return View(character);
        }

        // GET: Character/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "Description");
            return View();
        }

        // POST: Character/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CharacterId,Name,Description,ProjectId")] Character character, int id)
        {
            if (ModelState.IsValid)
            {
                character.ProjectId = id;
                _context.Add(character);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details","Project", new {id = id});
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "Description", character.ProjectId);
            return View(character);
        }

        // GET: Character/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Character.SingleOrDefaultAsync(m => m.CharacterId == id);
            if (character == null)
            {
                return NotFound();
            }
            ViewData["Id"] = character.ProjectId;
            ViewData["PROJId"] = character.CharacterId;
            return View(character);
        }

        // POST: Character/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CharacterId,Name,Description,ProjectId")] Character character)
        {
            if (id != character.CharacterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(character);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterExists(character.CharacterId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details","Project", new {id = id});
            }
            return View(character);
        }

        // GET: Character/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Character
                .Include(c => c.Project)
                .SingleOrDefaultAsync(m => m.CharacterId == id);
            if (character == null)
            {
                return NotFound();
            }
            ViewData["ProjId"] = character.ProjectId;
            return View(character);
        }

        // POST: Character/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var character = await _context.Character.SingleOrDefaultAsync(m => m.CharacterId == id);
            _context.Character.Remove(character);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CharacterExists(int id)
        {
            return _context.Character.Any(e => e.CharacterId == id);
        }
    }
}
