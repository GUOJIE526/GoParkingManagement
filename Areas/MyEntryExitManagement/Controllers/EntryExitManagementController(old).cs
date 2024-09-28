//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using MyGoParking.Models;

//namespace MyGoParking.Areas.MyEntryExitManagement.Controllers
//{
//    [Area("MyEntryExitManagement")]
//    public class EntryExitManagementController : Controller
//    {
//        private readonly MyGoParkingContext _context;

//        public EntryExitManagementController(MyGoParkingContext context)
//        {
//            _context = context;
//        }

//        // GET: MyEntryExitManagement/EntryExitManagement
//        public async Task<IActionResult> Index()
//        {
//            var myGoParkingContext = _context.EntryExitManagement.Include(e => e.Car).Include(e => e.Lot);
//            return View(await myGoParkingContext.ToListAsync());
//        }

//        // GET: MyEntryExitManagement/EntryExitManagement/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var entryExitManagement = await _context.EntryExitManagement
//                .Include(e => e.Car)
//                .Include(e => e.Lot)
//                .FirstOrDefaultAsync(m => m.EntryexitId == id);
//            if (entryExitManagement == null)
//            {
//                return NotFound();
//            }

//            return View(entryExitManagement);
//        }

//        // GET: MyEntryExitManagement/EntryExitManagement/Create
//        public IActionResult Create()
//        {
//            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId");
//            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotId");
//            return View();
//        }

//        // POST: MyEntryExitManagement/EntryExitManagement/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("EntryexitId,LotId,CarId,Parktype,LicensePlatePhoto,EntryTime,LicensePlateKeyinTime,Amount,ExitTime,PaymentStatus,PaymentTime,ValidTime")] EntryExitManagement entryExitManagement)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(entryExitManagement);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId", entryExitManagement.CarId);
//            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotId", entryExitManagement.LotId);
//            return View(entryExitManagement);
//        }

//        // GET: MyEntryExitManagement/EntryExitManagement/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var entryExitManagement = await _context.EntryExitManagement.FindAsync(id);
//            if (entryExitManagement == null)
//            {
//                return NotFound();
//            }
//            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId", entryExitManagement.CarId);
//            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotId", entryExitManagement.LotId);
//            return View(entryExitManagement);
//        }

//        // POST: MyEntryExitManagement/EntryExitManagement/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("EntryexitId,LotId,CarId,Parktype,LicensePlatePhoto,EntryTime,LicensePlateKeyinTime,Amount,ExitTime,PaymentStatus,PaymentTime,ValidTime")] EntryExitManagement entryExitManagement)
//        {
//            if (id != entryExitManagement.EntryexitId)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(entryExitManagement);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!EntryExitManagementExists(entryExitManagement.EntryexitId))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId", entryExitManagement.CarId);
//            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotId", entryExitManagement.LotId);
//            return View(entryExitManagement);
//        }

//        // GET: MyEntryExitManagement/EntryExitManagement/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var entryExitManagement = await _context.EntryExitManagement
//                .Include(e => e.Car)
//                .Include(e => e.Lot)
//                .FirstOrDefaultAsync(m => m.EntryexitId == id);
//            if (entryExitManagement == null)
//            {
//                return NotFound();
//            }

//            return View(entryExitManagement);
//        }

//        // POST: MyEntryExitManagement/EntryExitManagement/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var entryExitManagement = await _context.EntryExitManagement.FindAsync(id);
//            if (entryExitManagement != null)
//            {
//                _context.EntryExitManagement.Remove(entryExitManagement);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool EntryExitManagementExists(int id)
//        {
//            return _context.EntryExitManagement.Any(e => e.EntryexitId == id);
//        }
//    }
//}
