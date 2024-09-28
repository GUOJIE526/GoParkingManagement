using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyGoParking.Models;

namespace MyGoParking.Areas.MyMonthlyRental.Controllers
{
    [Authorize]
    [Area("MyMonthlyRental")]
    public class MonthlyRentalController : Controller
    {
        private readonly MyGoParkingContext _context;

        public MonthlyRentalController(MyGoParkingContext context)
        {
            _context = context;
        }

        // GET: MyMonthlyRental/MonthlyRental
        public async Task<IActionResult> Index()
        {
            var myGoParkingContext = _context.MonthlyRental.Include(m => m.Car).Include(m => m.Lot);
            return View(await myGoParkingContext.ToListAsync());
        }

        // GET: MyMonthlyRental/MonthlyRental/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monthlyRental = await _context.MonthlyRental
                .Include(m => m.Car)
                .Include(m => m.Lot)
                .FirstOrDefaultAsync(m => m.MonId == id);
            if (monthlyRental == null)
            {
                return NotFound();
            }

            return View(monthlyRental);
        }

        // GET: MyMonthlyRental/MonthlyRental/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId");
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotId");
            return View();
        }

        // POST: MyMonthlyRental/MonthlyRental/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MonId,LotId,CarId,StartDate,EndDate,RenewalStatus,NotificationStatus,Amount,PaymentTime,PaymentStatus")] MonthlyRental monthlyRental)
        {
            if (ModelState.IsValid)
            {
                _context.Add(monthlyRental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId", monthlyRental.CarId);
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotId", monthlyRental.LotId);
            return View(monthlyRental);
        }

        // GET: MyMonthlyRental/MonthlyRental/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monthlyRental = await _context.MonthlyRental.FindAsync(id);
            if (monthlyRental == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId", monthlyRental.CarId);
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotId", monthlyRental.LotId);
            return View(monthlyRental);
        }

        // POST: MyMonthlyRental/MonthlyRental/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MonId,LotId,CarId,StartDate,EndDate,RenewalStatus,NotificationStatus,Amount,PaymentTime,PaymentStatus")] MonthlyRental monthlyRental)
        {
            if (id != monthlyRental.MonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monthlyRental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonthlyRentalExists(monthlyRental.MonId))
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
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId", monthlyRental.CarId);
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotId", monthlyRental.LotId);
            return View(monthlyRental);
        }

        // GET: MyMonthlyRental/MonthlyRental/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monthlyRental = await _context.MonthlyRental
                .Include(m => m.Car)
                .Include(m => m.Lot)
                .FirstOrDefaultAsync(m => m.MonId == id);
            if (monthlyRental == null)
            {
                return NotFound();
            }

            return View(monthlyRental);
        }

        // POST: MyMonthlyRental/MonthlyRental/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monthlyRental = await _context.MonthlyRental.FindAsync(id);
            if (monthlyRental != null)
            {
                _context.MonthlyRental.Remove(monthlyRental);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonthlyRentalExists(int id)
        {
            return _context.MonthlyRental.Any(e => e.MonId == id);
        }
    }
}
