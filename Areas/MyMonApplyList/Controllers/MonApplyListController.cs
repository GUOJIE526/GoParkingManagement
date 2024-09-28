using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyGoParking.Models;

namespace MyGoParking.Areas.MyMonApplyList.Controllers
{
    [Authorize]
    [Area("MyMonApplyList")]
    public class MonApplyListController : Controller
    {
        private readonly MyGoParkingContext _context;

        public MonApplyListController(MyGoParkingContext context)
        {
            _context = context;
        }

        // GET: MyMonApplyList/MonApplyList
        public async Task<IActionResult> Index()
        {
            var myGoParkingContext = _context.MonApplyList.Include(m => m.Car).Include(m => m.Lot);
            return View(await myGoParkingContext.ToListAsync());
        }

        // GET: MyMonApplyList/MonApplyList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monApplyList = await _context.MonApplyList
                .Include(m => m.Car)
                .Include(m => m.Lot)
                .FirstOrDefaultAsync(m => m.ApplyId == id);
            if (monApplyList == null)
            {
                return NotFound();
            }

            return View(monApplyList);
        }

        // GET: MyMonApplyList/MonApplyList/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId");
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotId");
            return View();
        }

        // POST: MyMonApplyList/MonApplyList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApplyId,LotId,CarId,ApplyStatus,ApplyDate")] MonApplyList monApplyList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(monApplyList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId", monApplyList.CarId);
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotId", monApplyList.LotId);
            return View(monApplyList);
        }

        // GET: MyMonApplyList/MonApplyList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monApplyList = await _context.MonApplyList.FindAsync(id);
            if (monApplyList == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId", monApplyList.CarId);
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotId", monApplyList.LotId);
            return View(monApplyList);
        }

        // POST: MyMonApplyList/MonApplyList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApplyId,LotId,CarId,ApplyStatus,ApplyDate")] MonApplyList monApplyList)
        {
            if (id != monApplyList.ApplyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monApplyList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonApplyListExists(monApplyList.ApplyId))
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
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId", monApplyList.CarId);
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotId", monApplyList.LotId);
            return View(monApplyList);
        }

        // GET: MyMonApplyList/MonApplyList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monApplyList = await _context.MonApplyList
                .Include(m => m.Car)
                .Include(m => m.Lot)
                .FirstOrDefaultAsync(m => m.ApplyId == id);
            if (monApplyList == null)
            {
                return NotFound();
            }

            return View(monApplyList);
        }

        // POST: MyMonApplyList/MonApplyList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monApplyList = await _context.MonApplyList.FindAsync(id);
            if (monApplyList != null)
            {
                _context.MonApplyList.Remove(monApplyList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonApplyListExists(int id)
        {
            return _context.MonApplyList.Any(e => e.ApplyId == id);
        }
    }
}
