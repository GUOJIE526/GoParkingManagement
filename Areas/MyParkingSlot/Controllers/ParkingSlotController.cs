using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyGoParking.Models;
using MyGoParking.Areas.MyParkingSlot;
using MyGoParking.Areas.MyParkingSlot.ViewModels;
using Microsoft.AspNetCore.Authorization;


namespace MyGoParking.Areas.MyParkingSlot.Controllers
{
    [Authorize]
    [Area("MyParkingSlot")]
    public class ParkingSlotController : Controller
    {
        private readonly MyGoParkingContext _context;

        public ParkingSlotController(MyGoParkingContext context)
        {
            _context = context;
        }

        // GET: MyParkingSlot/ParkingSlot
        public async Task<IActionResult> Index()
        {
            //根據 parkingLotId 從資料庫抓數據
            var parkingSlots = await _context.ParkingSlot
                .Where(ps => ps.LotId == 8)  //預設顯示lotid = 1的停車場
                .Include(ps => ps.Lot) //join到ParkingLot，要查詢停車場資訊
                .ToListAsync();

            var monApplyList = await _context.MonApplyList
                .Where(ma => ma.LotId == 1)  //預設顯示lotid = 1的停車場
                .Include(ma => ma.Lot) // join到ParkingLot，要查詢停車場資訊
                .Include(ma => ma.Car) // join到Car，要查詢車輛資訊(車牌)
                .ToListAsync();

            var viewModel = new ParkingSlotManageViewModel
            {
                ParkingSlots = parkingSlots,
                MonApplyList = monApplyList,
                lotName = _context.ParkingLot.Where(n => n.LotId == 1).Select(n => n.LotName).FirstOrDefault(),
                lotAddress = _context.ParkingLot.Where(n => n.LotId == 1).Select(n => n.LotAddress).FirstOrDefault(),
                TotalLots = _context.ParkingSlot.Where(n => n.LotId == 8).Count(), //該停車場月租車位數
                Rentedlots = _context.ParkingSlot.Where(n => n.IsRented == true).Count(),   //該停車場出租車位數
                WaitingApplicants = _context.MonApplyList.Where(n => n.LotId == 1 && n.ApplyStatus == "pending").Count(),
            };

            return View(viewModel);
        }

        // GET: MyParkingSlot/ParkingSlot/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSlot = await _context.ParkingSlot
                .Include(p => p.Lot)
                .FirstOrDefaultAsync(m => m.SlotId == id);
            if (parkingSlot == null)
            {
                return NotFound();
            }

            return View(parkingSlot);
        }

        // GET: MyParkingSlot/ParkingSlot/Create
        public IActionResult Create()
        {
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotId");
            return View();
        }

        // POST: MyParkingSlot/ParkingSlot/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SlotId,LotId,SlotNumber,IsRented")] ParkingSlot parkingSlot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parkingSlot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotId", parkingSlot.LotId);
            return View(parkingSlot);
        }

        // GET: MyParkingSlot/ParkingSlot/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSlot = await _context.ParkingSlot.FindAsync(id);
            if (parkingSlot == null)
            {
                return NotFound();
            }
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotId", parkingSlot.LotId);
            return View(parkingSlot);
        }

        // POST: MyParkingSlot/ParkingSlot/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SlotId,LotId,SlotNumber,IsRented")] ParkingSlot parkingSlot)
        {
            if (id != parkingSlot.SlotId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkingSlot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingSlotExists(parkingSlot.SlotId))
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
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotId", parkingSlot.LotId);
            return View(parkingSlot);
        }

        // GET: MyParkingSlot/ParkingSlot/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSlot = await _context.ParkingSlot
                .Include(p => p.Lot)
                .FirstOrDefaultAsync(m => m.SlotId == id);
            if (parkingSlot == null)
            {
                return NotFound();
            }

            return View(parkingSlot);
        }

        // POST: MyParkingSlot/ParkingSlot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkingSlot = await _context.ParkingSlot.FindAsync(id);
            if (parkingSlot != null)
            {
                _context.ParkingSlot.Remove(parkingSlot);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingSlotExists(int id)
        {
            return _context.ParkingSlot.Any(e => e.SlotId == id);
        }
    }
}
