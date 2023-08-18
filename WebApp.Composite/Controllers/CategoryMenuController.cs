using BaseProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApp.Composite.Composite;
using WebApp.Composite.Models;

namespace WebApp.Composite.Controllers
{
    [Authorize]
    public class CategoryMenuController : Controller
    {
        private readonly AppIdentityDbContext _context;


        public CategoryMenuController(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var categories = await _context.Categories.Include(x => x.Books).Where(x => x.UserId == userId).OrderBy(x => x.Id).ToListAsync();

            var menu = GetMenus(categories, new Category { Name = "Top Category", Id = 0 }, new BookComposit("Top BookComposit", 0));

            ViewBag.menu = menu;

            ViewBag.selectList = menu.Components.SelectMany(x => ((BookComposit)x).GetSelectListItems(""));

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(int categoryId, string bookName)
        {
            await _context.Books.AddAsync(new Book { Name = bookName, CategoryId = categoryId });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public BookComposit GetMenus(List<Category> categories, Category topCategory, BookComposit topBookComposit, BookComposit last = null)
        {
            categories.Where(x => x.ReferenceId == topCategory.Id).ToList().ForEach(categoryItem =>
            {
                var bookComposit = new BookComposit(categoryItem.Name, categoryItem.Id);

                categoryItem.Books.ToList().ForEach(book =>
                {
                    bookComposit.Add(new BookComponent(book.Name, book.Id));
                });

                if (last != null)
                {
                    last.Add(bookComposit);
                }
                else
                {
                    topBookComposit.Add(bookComposit);
                }

                GetMenus(categories, categoryItem, topBookComposit, bookComposit);

            });

            return topBookComposit;
        }


    }
}
