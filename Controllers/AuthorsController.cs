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

        public AuthorsController(LibraryContext db)
        {
            _db = db;
        }

        //Index
        [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }
        //C
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Author author)
        {
            _db.Authors.Add(author);
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