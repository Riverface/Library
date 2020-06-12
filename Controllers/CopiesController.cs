using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class CopiesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly LibraryContext _db;

        public CopiesController(UserManager<ApplicationUser> userManager, LibraryContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<ActionResult> Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            var userCopies = _db.Copies
                .Where(copy => copy.User.Id == currentUser.Id)
                .Include(copy => copy.Book);
            return View(userCopies);
        }

        public ActionResult Create()
        {
            ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Copy copy, int BookId)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            copy.User = currentUser;

            _db.Copies.Add(copy);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult Details(int id)
        {

            Copy thisCopy;
                thisCopy = _db.Copies.Include(copy => copy.Book)
                    .Where(copy => copy.CopyId == id)
                    .FirstOrDefault();
            
            return View(thisCopy);
        }

        public ActionResult Edit(int id)
        {
            Copy thisCopy = _db.Copies.Include(copy => copy.Book)
                .Where(copy => copy.CopyId == id)
                .FirstOrDefault();
            return View(thisCopy);
        }

        [HttpPost]
        public ActionResult Edit(Copy copy)
        {
            _db.Entry(copy).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("CheckOut")]
        public ActionResult CheckOut(Copy copy)
        {
            copy.CheckedOut = !copy.CheckedOut;
            _db.Entry(copy).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Details", "Copies", new {id = copy.CopyId});
        }

        public ActionResult Delete(int id)
        {
            var thisCopy = _db.Copies.FirstOrDefault(copies => copies.CopyId == id);
            return View(thisCopy);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var thisCopy = _db.Copies.FirstOrDefault(copies => copies.CopyId == id);
            _db.Copies.Remove(thisCopy);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}