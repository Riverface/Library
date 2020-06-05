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
            var userCopies = _db.Copies.Where(entry => entry.User.Id == currentUser.Id);
            return View(userCopies);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_db.Copies, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Copy copy, int CategoryId)
        {
          var userId=this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          var currentUser = await _userManager.FindByIdAsync(userId);
          copy.User = currentUser;
            _db.Copies.Add(copy);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var thisCopy = _db.Copies
                .Include(copy => copy.Copies)
                .ThenInclude(join => join.Category)
                .FirstOrDefault(copy => copy.CopyId == id);
            return View(thisCopy);
        }

        public ActionResult Edit(int id)
        {
            var thisCopy = _db.Copies.FirstOrDefault(copies => copies.CopyId == id);
            ViewBag.CategoryId = new SelectList(_db.Copies, "CategoryId", "Name");
            return View(thisCopy);
        }

        [HttpPost]
        public ActionResult Edit(Copy copy, int CategoryId)
        {
            if (CategoryId != 0)
            {
                _db.CategoryCopy.Add(new CategoryCopy() { CategoryId = CategoryId, CopyId = copy.CopyId });
            }
            _db.Entry(copy).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddCategory(int id)
        {
            var thisCopy = _db.Copies.FirstOrDefault(copies => copies.CopyId == id);
            ViewBag.CategoryId = new SelectList(_db.Copies, "CategoryId", "Name");
            return View(thisCopy);
        }

        [HttpPost]
        public ActionResult AddCategory(Copy copy, int CategoryId)
        {
            if (CategoryId != 0)
            {
                _db.CategoryCopy.Add(new CategoryCopy() { CategoryId = CategoryId, CopyId = copy.CopyId });
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
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

        [HttpPost]
        public ActionResult DeleteCategory(int joinId)
        {
            var joinEntry = _db.CategoryCopy.FirstOrDefault(entry => entry.CategoryCopyId == joinId);
            _db.CategoryCopy.Remove(joinEntry);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}