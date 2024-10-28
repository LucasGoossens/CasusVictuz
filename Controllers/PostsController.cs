﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CasusVictuz.Data;
using Casusvictuz;
using Microsoft.Extensions.Hosting;

namespace CasusVictuz.Controllers
{
    public class PostsController : Controller
    {
        private readonly VictuzDb _context;

        public PostsController(VictuzDb context)
        {
            _context = context;
        }

        //// GET: Posts
        //public async Task<IActionResult> Index()
        //{
        //    var victuzDb = _context.Posts.Include(p => p.Category).Include(p => p.User);
        //    return View(await victuzDb.ToListAsync());
        //}

        public async Task<IActionResult> Index()
        {
            var victuzDb = _context.Threads
                .Include(p => p.Category)
                .Include(p => p.User)
                .Include(p => p.Comments);

            return View(await victuzDb.ToListAsync());
        }

        //// GET: Posts/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var post = await _context.Posts
        //        .Include(p => p.Category)
        //        .Include(p => p.User)                
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (post == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(post);
        //}

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Threads
                .Include(p => p.Category)
                .Include(p => p.User)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }


        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,Date,UserId,CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", post.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", post.UserId);
            return View(post);
        }

        public IActionResult CreateThread()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateThread([Bind("Id,Content,Date,UserId,CategoryId,Title")] Casusvictuz.Thread thread)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thread);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", thread.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", thread.UserId);
            return View(thread);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(int threadId, string content) // ,int userId)
        {
            // testuser voor nu
            var testUser = new User
            {
                Name = "test",
                Password = "test",
                IsAdmin = false
            };

            Casusvictuz.Thread threadForComment = (Casusvictuz.Thread)_context.Threads.Include(t => t.Category).FirstOrDefault(t => t.Id == threadId);

            var comment = new Comment
            {
                Content = content,
                Date = DateTime.Now,
                ThreadId = threadId,
                UserId = testUser.Id,
                User = testUser,
                Thread = threadForComment,
                Category = threadForComment.Category
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = threadId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReply(int threadId, int parentCommentId, string content) // ,int userId)
        {
            // testuser voor nu
            var testUser = new User
            {
                Name = "test",
                Password = "test",
                IsAdmin = false
            };

            Casusvictuz.Thread threadForComment = (Casusvictuz.Thread)_context.Threads.Include(t => t.Category).FirstOrDefault(t => t.Id == threadId);
            Comment parentComment = _context.Comments.FirstOrDefault(c => c.Id == parentCommentId);

            var comment = new Comment
            {
                Content = content,
                Date = DateTime.Now,
                ThreadId = threadId,
                UserId = testUser.Id,
                User = testUser,
                Thread = threadForComment,
                Category = threadForComment.Category,
                ParentCommentId = parentCommentId

            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = threadId });
        }






        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", post.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", post.UserId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,Date,UserId,CategoryId")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", post.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", post.UserId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var post = await _context.Posts.FindAsync(id);
        //    if (post != null)
        //    {
        //        _context.Posts.Remove(post);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}


        [HttpPost, ActionName("DeleteThread")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteThread(int id)
        {
            var thread = await _context.Threads
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (thread == null)
            {
                return NotFound();
            }

            _context.Comments.RemoveRange(thread.Comments);
            _context.Threads.Remove(thread);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
