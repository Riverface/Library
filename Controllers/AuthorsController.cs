using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly LibraryContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthorsController(UserManager<ApplicationUser> userManager, LibraryContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        //Index

        public async Task<ActionResult> Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            var userAuthors = _db.Authors.Where(entry => entry.User.Id == currentUser.Id);
            ViewBag.Books = _db.Books;
            ViewBag.AllAuthors = _db.Authors.ToList();
            return View(userAuthors);
        }
        //C
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Author author)
        {
            var userId=this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser= await _userManager.FindByIdAsync(userId);
            author.User = currentUser;
            _db.Authors.Add(author);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        //R
        public ActionResult Details(int id)
        {
            var thisAuthor = _db.Authors
            .Include(author => author.Books)
            .ThenInclude(join => join.Book)
            .FirstOrDefault(author => author.AuthorId == id);
            return View(thisAuthor);
        }
        //U
        public ActionResult Edit(int id)
        {
            var thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
            return View(thisAuthor);
        }
        
        [HttpPost]
        public ActionResult Edit(Author author)
        {
            _db.Entry(author).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        //D
        public ActionResult Delete(int id)
        {
            var thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
            return View(thisAuthor);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
            _db.Authors.Remove(thisAuthor);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}