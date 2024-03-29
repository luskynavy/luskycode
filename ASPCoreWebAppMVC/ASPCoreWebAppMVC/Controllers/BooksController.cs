﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPCoreWebAppMVC.Models;

namespace ASPCoreWebAppMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        //public BooksController()
        {
            //Création à la main du contexte de bdd en attendant la configuration de l'injection dépendance.
            //LibraryContext context = new();

            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string sortOrder)
        {
            try
            {
                //C'est l'action qui décide des noms des tris
                ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewData["NoteSortParm"] = sortOrder == "note" ? "note_desc" : "note";
                ViewData["AuthorSortParm"] = sortOrder == "author" ? "author_desc" : "author";

                //Convertit le dbset en IQueryable
                IQueryable<Book> books = _context.Book;

                //Rajoute le lien vers l'auteur
                books = books.Include(b => b.AuthorNavigation);

                //Tri
                switch (sortOrder)
                {
                    case "note_desc":
                        books = books.OrderByDescending(b => b.Note);
                        break;

                    case "note":
                        books = books.OrderBy(b => b.Note);
                        break;

                    case "author_desc":
                        books = books.OrderByDescending(b => b.AuthorNavigation.Name);
                        break;

                    case "author":
                        books = books.OrderBy(b => b.AuthorNavigation.Name);
                        break;

                    case "name_desc":
                        books = books.OrderByDescending(b => b.Name);
                        break;

                    default:
                        books = books.OrderBy(b => b.Name);
                        break;
                }

                return View(await books.ToListAsync());
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.AuthorNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound("Livre inconnu");
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["Author"] = new SelectList(_context.Author.OrderBy(a => a.Name), "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Note,Author")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Author"] = new SelectList(_context.Author.OrderBy(a => a.Name), "Id", "Name", book.Author);

            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["Author"] = new SelectList(_context.Author.OrderBy(a => a.Name), "Id", "Name", book.Author);

            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Note,Author")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["Author"] = new SelectList(_context.Author.OrderBy(a => a.Name), "Id", "Name", book.Author);

            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.AuthorNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'LibraryContext.Book'  is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}