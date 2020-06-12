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
    public class BooksController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LibraryContext _db;

        public BooksController(UserManager<ApplicationUser> userManager, LibraryContext db)
        {
            _userManager = userManager;
            _db = db;
        }


        public ActionResult Index()
        {
            var model = _db.Books.ToList();
            ViewBag.Copies = _db.Copies;
            return View(model);
        }



        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Book book, int AuthorId)
        {
            _db.Books.Add(book);
            if (AuthorId != 0)
            {
                _db.AuthorBooks.Add(new AuthorBook() { AuthorId = AuthorId, BookId = book.BookId });
                        
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var thisBook = _db.Books
                .Include(book => book.Authors)
                .ThenInclude(join => join.Author)
                .FirstOrDefault(book => book.BookId == id);
            return View(thisBook);
        }

        public ActionResult Edit(int id)
        {
            var thisBook = _db.Books.FirstOrDefault(books => books.BookId == id);
            ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
            return View(thisBook);
        }

        [HttpPost]
        public ActionResult Edit(Book book, int AuthorId)
        {
            if (AuthorId != 0)
            {
                _db.AuthorBooks.Add(new AuthorBook() { AuthorId = AuthorId, BookId = book.BookId });
            }
            _db.Entry(book).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddAuthor(int id)
        {
            var thisBook = _db.Books.FirstOrDefault(books => books.BookId == id);
            ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
            return View(thisBook);
        }

        [HttpPost]
        public ActionResult AddAuthor(Book book, int AuthorId)
        {
            if (AuthorId != 0)
            {
                _db.AuthorBooks.Add(new AuthorBook() { AuthorId = AuthorId, BookId = book.BookId });
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var thisBook = _db.Books.FirstOrDefault(books => books.BookId == id);
            return View(thisBook);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var thisBook = _db.Books.FirstOrDefault(books => books.BookId == id);
            _db.Books.Remove(thisBook);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteAuthor(int joinId)
        {
            var joinEntry = _db.AuthorBooks.FirstOrDefault(entry => entry.AuthorBookId == joinId);
            _db.AuthorBooks.Remove(joinEntry);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }   
}