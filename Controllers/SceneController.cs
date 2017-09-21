using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BE_Capstone.Data;
using BE_Capstone.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BE_Capstone.Controllers
{
    public class SceneController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _environment;

        public SceneController(ApplicationDbContext context,UserManager<ApplicationUser> userManager, IHostingEnvironment environment)
        {
            _context = context; 
            _userManager = userManager;
            _environment = environment;   
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Scene
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Scene.Include(s => s.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProjectScenes
        [Authorize]
        public async Task<IActionResult> ProjectScenes(int? id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound();
            }
            var applicationDbContext = _context.Scene.Where(p => p.ProjectId == id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Scene/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scene = await _context.Scene
                .Include(s => s.Project)
                .Include("Lines")
                .SingleOrDefaultAsync(m => m.SceneId == id);
            foreach(Line X in scene.Lines)
            {
                var Y = _context.Character.SingleOrDefault(c => c.CharacterId == X.CharacterId);
                X.Character = Y;
            }
            if (scene == null)
            {
                return NotFound();
            }

            return View(scene);
        }

        // GET: Scene/Create
        public IActionResult Create(int? id)
        {
            //ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "Description");
            return View();
        }

        // POST: Scene/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SceneId,Title,Description,Order,Body,ProjectId")] Scene scene, int id)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<Scene> sceneCount =  _context.Scene.Where(s => s.ProjectId == id);
                scene.Order = 1 + sceneCount.Count();
                scene.ProjectId = id;
                _context.Add(scene);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details","Project", new {id = id});
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "Description", scene.ProjectId);
            return View(scene);
        }

        // GET: Scene/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scene = await _context.Scene.SingleOrDefaultAsync(m => m.SceneId == id);
            if (scene == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "Description", scene.ProjectId);
            return View(scene);
        }

        // POST: Scene/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SceneId,Title,Description,Order,Body,ProjectId")] Scene scene)
        {
            if (id != scene.SceneId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scene);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SceneExists(scene.SceneId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "Description", scene.ProjectId);
            return View(scene);
        }

        [HttpPost]
        public async Task<IActionResult> updateLineOrder(int newIndex, int oldIndex, [FromRoute]int id)
        {
            Console.WriteLine("Old:" + oldIndex);
            Console.WriteLine("New:" + newIndex);
            Scene sceneLines =  await _context.Scene
                    .Include("Lines")
                    .SingleOrDefaultAsync(m => m.SceneId == id);
            if (oldIndex > newIndex){
                foreach (Line X in sceneLines.Lines)
                {
                    if(X.Order >= newIndex + 1 && X.Order < oldIndex + 1)
                    {
                        X.Order++;
                    }
                    else if(X.Order == oldIndex + 1)
                    {
                        X.Order = newIndex + 1;
                    }
                } 
            }
            if (oldIndex < newIndex){
                foreach (Line X in sceneLines.Lines)
                {
                    if(X.Order <= newIndex + 1 && X.Order > oldIndex + 1)
                    {
                        X.Order--;
                    }
                    else if(X.Order == oldIndex + 1)
                    {
                        X.Order = newIndex + 1;
                    }
                } 
            }
            _context.Update(sceneLines);
            _context.SaveChanges();
            return RedirectToAction("Details");  
        }

        // GET: Scene/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scene = await _context.Scene
                .Include(s => s.Project)
                .SingleOrDefaultAsync(m => m.SceneId == id);
            if (scene == null)
            {
                return NotFound();
            }

            return View(scene);
        }

        // POST: Scene/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scene = await _context.Scene.SingleOrDefaultAsync(m => m.SceneId == id);
            _context.Scene.Remove(scene);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SceneExists(int id)
        {
            return _context.Scene.Any(e => e.SceneId == id);
        }
    }
}
