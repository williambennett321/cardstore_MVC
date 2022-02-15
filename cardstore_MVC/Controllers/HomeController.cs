using cardstore_MVC.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using cardstore_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace cardstore_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Listings()
        {
            var Listings = await _context.CardListing.ToListAsync();
            return View(Listings);
        }

        public async Task<IActionResult> AddorEdit(int? CardNum)
        {
            ViewBag.PageName = CardNum == null ? "Create Ticket" : "Edit Ticket";
            ViewBag.isEdit = CardNum == null ? false : true;
            if (CardNum == null)
            {
                return View();
            }
            else
            {
                var CardListing = await _context.CardListing.FindAsync(CardNum);

                if (CardListing == null)
                {
                    return NotFound();
                }
                return View(CardListing);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int CardNum, [Bind("CardNum,ListingName,Description,Price")]
       CardListing ListingData)
        {
            bool IsListingExist = false;

            CardListing listing = await _context.CardListing.FindAsync(CardNum);

            if (listing != null)
            {
                IsListingExist = true;
            }
            else
            {
                listing = new CardListing();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    listing.CardNum = ListingData.CardNum;
                    listing.ListingName = ListingData.ListingName;
                    listing.Description = ListingData.Description;
                    listing.Price = ListingData.Price;

                    if (IsListingExist)
                    {
                        _context.Update(listing);
                    }
                    else
                    {
                        _context.Add(listing);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ListingData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
