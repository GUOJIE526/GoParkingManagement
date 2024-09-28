using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyGoParking.Models;

namespace MyGoParking.Areas.MyCar.Controllers
{
    [Area("MyCar")]
    [Authorize]
    public class CarController : Controller
    {
        private readonly MyGoParkingContext _context;

        public CarController(MyGoParkingContext context)
        {
            _context = context;
        }

        // GET: MyCar/Car
        public async Task<IActionResult> Index()
        {
            var myGoParkingContext = _context.Car.Include(c => c.User);
            return View(await myGoParkingContext.ToListAsync());
        }

        public IActionResult IndexJson()
        {
            var myGoParkingContext = _context.Car.Include(c => c.User)
                .Select(c => new
                {
                    carId = c.CarId,
                    userId = c.User.Username,
                    licensePlate = c.LicensePlate,
                });
            return Json(myGoParkingContext);
        }

        // GET: MyCar/Car/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        public IActionResult CreatePartial()
        {
            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "Username");
            return PartialView("_CreatePartial");
        }

        // GET: MyCar/Car/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "Username");
            return View();
        }

        // POST: MyCar/Car/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,UserId,LicensePlate")] Car car)
        {
            var exitCar = await _context.Car
                .FirstOrDefaultAsync(c => c.LicensePlate == car.LicensePlate);
            if(exitCar != null)
            {
                // 如果找到相同的車牌，返回錯誤訊息
                return Json(new { success = false, message = "車牌已經存在，請使用其他車牌" });
            }

            if (ModelState.IsValid)
            {

                _context.Add(car);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
                //return RedirectToAction(nameof(Index));
            }
            else
            {
                // 如果 ModelState 驗證失敗，返回錯誤訊息
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors });
            }
        }

        public async Task<IActionResult> EditPartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "Username", car.UserId);
            return PartialView("_EditPartial", car);
        }

        // GET: MyCar/Car/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "Username", car.UserId);
            return View(car);
        }

        // POST: MyCar/Car/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,UserId,LicensePlate")] Car car)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.CarId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { success = true });
                //return RedirectToAction(nameof(Index));
            }
            else
            {
                return Json(new { success = false });
            }
        }

        // GET: MyCar/Car/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: MyCar/Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Car.FindAsync(id);
            if (car != null)
            {
                try
                {
                    _context.Car.Remove(car);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "找不到此車輛記錄" });
            //return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Car.Any(e => e.CarId == id);
        }
    }
}
