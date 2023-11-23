using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaManager_GCG1.Models.Cinema;
using CinemaManager_GCG1.Models.ViewModels;

namespace CinemaManager_GCG1.Controllers
{
    public class MoviesController : Controller
    {
        private readonly CinemaDbGcg1Context _context;

        public MoviesController(CinemaDbGcg1Context context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var cinemaDbGcg1Context = _context.Movies.Include(m => m.Producer);
            return View(await cinemaDbGcg1Context.ToListAsync());
        }
        public async Task<IActionResult> MoviesAndTheirProds()
        {
            var cinemaDbGcg1Context = _context.Movies.Include(m => m.Producer);
            return View(await cinemaDbGcg1Context.ToListAsync());
        }

        public IActionResult MoviesAndTheirProds_UsingModel()
        {
            var producers =_context.Producers.ToList();
            var movies =_context.Movies.ToList();
            var query = from producer in producers
            join movie in movies
            on producer.Id equals movie.ProducerId
            select new ProdMovie
            {
                mTitle = movie.Title,
                mGenre = movie.Genre,
                pName = producer.Name,
                pNat = producer.Nationality
            };
            //ViewBag.abc = query.ToList();
            return View(query.ToList());
        }
        public IActionResult SearchByTitle(string title)
        {
            var movies = _context.Movies.AsQueryable();
            if (!String.IsNullOrEmpty(title))
            {
                //movies = from m in movies where m.Title.Contains(title) select m;
                movies = movies.Where(m => m.Title.Contains(title));
            }
            return View(movies.ToList());
        }
        public IActionResult SearchByGenre(string genre)
        {
            var movies = _context.Movies.ToList();
            if(String.IsNullOrEmpty(genre))
            {
                return View(movies);
            }
                var querry = movies.Where(m => m.Genre.Contains(genre));
            return View(querry.ToList());
        }
        public IActionResult SearchByCritaire(string crit, string val)
        {
            var movies = _context.Movies.AsQueryable();
            if(crit == "Title")
            {
                if (!String.IsNullOrEmpty(val))
                {
                movies= movies.Where(m => m.Title.Contains(val));
                }
                return View(movies.ToList());
            }
            if (!String.IsNullOrEmpty(val))
            {
                movies = movies.Where(m => m.Genre.Contains(val));
            }
            return View(movies.ToList());  
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id");
            return View();
        }
        public IActionResult SearchBy2(string genre ,string title)
        {
            var movies = _context.Movies.AsQueryable();
            ViewBag.Genre = movies.Select(m => m.Genre).Distinct().ToList();
            //g.Insert(0, "All");

            //ViewBag.Genre = new SelectList(g);
            if (genre != "All")
            {
                movies = movies.Where(m => m.Genre == genre);
            }
            if(!String.IsNullOrEmpty(title))
            {
                movies = movies.Where(m => m.Title.Contains(title));
            }
            return View(movies.ToList());
        }
        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Genre,ProducerId")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id", movie.ProducerId);
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id", movie.ProducerId);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Genre,ProducerId")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id", movie.ProducerId);
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movies == null)
            {
                return Problem("Entity set 'CinemaDbGcg1Context.Movies'  is null.");
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
