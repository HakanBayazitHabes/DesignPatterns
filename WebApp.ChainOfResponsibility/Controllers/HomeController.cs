using BaseProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApp.ChainOfResponsibility.ChangeOfResponsibility;
using WebApp.ChainOfResponsibility.Models;

namespace BaseProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppIdentityDbContext _appIdentityDbContext;
        public HomeController(ILogger<HomeController> logger, AppIdentityDbContext appIdentityDbContext)
        {
            _logger = logger;
            _appIdentityDbContext = appIdentityDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> SendEmail()
        {
            var products = await _appIdentityDbContext.Products.ToListAsync();

            var excelProcessHandler = new ExcelProcessHandler<Product>();

            var zipProcessHandler = new ZipFileProcessHandler<Product>();

            var sendEmailProcessHandler = new SendEmailProcessHandler("product.zip", "habehakan@gmail.com");


            excelProcessHandler.SetNext(zipProcessHandler).SetNext(sendEmailProcessHandler);

            excelProcessHandler.handle(products);

            return View(nameof(Index));

        }
    }
}